package service

import (
	"sync"

	"github.com/gnolang/gno/gno.land/pkg/gnoclient"
	rpcclient "github.com/gnolang/gno/tm2/pkg/bft/rpc/client"
	"github.com/gnolang/gno/tm2/pkg/crypto/keys"
	"go.uber.org/zap"
)

type UserAccount struct {
	KeyInfo  keys.Info
	Password string
}

type gnoNativeService struct {
	Logger *zap.Logger
	Client *gnoclient.Client
	Lock   sync.RWMutex
	// The remote node address used to create client.RPCClient. We need to save this
	// here because the remote is a private member of the HTTP struct.
	Remote string

	// Map of key name to userAccount.
	UserAccounts map[string]*UserAccount
	// The active account in userAccounts, or nil if none
	ActiveAccount *UserAccount

	CloseFunc func()
}

func NewGnoNativeService(opts ...GnoNativeOption) (*gnoNativeService, error) {
	cfg := &Config{}
	if err := cfg.applyOptions(append(opts, WithFallbackDefaults)...); err != nil {
		return nil, err
	}

	svc, err := initService(cfg)
	if err != nil {
		return nil, err
	}

	return svc, nil
}

func initService(cfg *Config) (*gnoNativeService, error) {
	svc := &gnoNativeService{
		Logger:       cfg.Logger,
		UserAccounts: make(map[string]*UserAccount),
		CloseFunc:    func() {},
	}

	if err := cfg.checkDirs(); err != nil {
		return nil, err
	}

	kb, _ := keys.NewKeyBaseFromDir(cfg.RootDir)
	signer := &gnoclient.SignerFromKeybase{
		Keybase: kb,
		ChainID: cfg.ChainID,
	}

	rpcClient := rpcclient.NewHTTP(cfg.Remote, "/websocket")
	svc.Remote = cfg.Remote

	svc.Client = &gnoclient.Client{
		Signer:    signer,
		RPCClient: rpcClient,
	}

	return svc, nil
}

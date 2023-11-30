package main

import "C"

import (
	"fmt"

	rpcclient "github.com/gnolang/gno/tm2/pkg/bft/rpc/client"
	"github.com/gnolang/gno/tm2/pkg/crypto/bip39"
	"github.com/gnolang/gno/tm2/pkg/crypto/keys"
	"github.com/gnolang/gno/tm2/pkg/crypto/keys/client"
	"github.com/gnolang/gno/tm2/pkg/errors"
)

const (
	mnemonicEntropySize = 256
)

type baseCfg struct {
	client.BaseOptions
}

type queryCfg struct {
	rootCfg *baseCfg

	data   string
	height int64
	prove  bool

	// internal
	path string
}

var _baseCfg baseCfg

//export Generate
func Generate(customEntropy bool, inputEntropy *C.char, result *C.int) *C.char {
	var entropySeed []byte
	if customEntropy {
		// 	if len(inputEntropy) < 43 {
		// 		error := fmt.Errorf("256-bits is 43 characters in Base-64, and 100 in Base-6. You entered %v, and probably want more", len(inputEntropy))
		// 	}
		// 	if err != nil {
		// 		return err
		// 	}
		// 	if !conf {
		// 		return nil
		// 	}

		// 	// hash input entropy to get entropy seed
		// 	hashedEntropy := sha256.Sum256([]byte(inputEntropy))
		// 	entropySeed = hashedEntropy[:]
	} else {
		// read entropy seed straight from crypto.Rand
		var err error
		entropySeed, err = bip39.NewEntropy(mnemonicEntropySize)
		if err != nil {
			*result = -1
			return C.CString(err.Error())
		}
	}

	mnemonic, err := bip39.NewMnemonic(entropySeed[:])
	if err != nil {
		*result = -1
		return C.CString(err.Error())
	}

	*result = 0
	return C.CString(mnemonic)
}

//export AddAccount
func AddAccount(cname *C.char, cmnemonic *C.char, index C.int, result *C.int) *C.char {
	var (
		kb              keys.Keybase
		err             error
		encryptPassword string
	)

	kb, err = keys.NewKeyBaseFromDir(_baseCfg.Home)
	if err != nil {
		*result = 1
		return C.CString(err.Error())
	}

	name := C.GoString(cname)

	_, err = kb.GetByName(name)
	if err == nil {
		// account exists, ask for user confirmation
		err = errors.New("account exists")
		*result = 1
		return C.CString(err.Error())
	}

	// Get bip39 mnemonic
	mnemonic := C.GoString(cmnemonic)
	const bip39Passphrase string = "" // XXX research.
	var account uint64

	if len(mnemonic) == 0 {
		mnemonic, err = client.GenerateMnemonic(mnemonicEntropySize)
		if err != nil {
			*result = 1
			return C.CString(err.Error())
		}
	}

	info, err := kb.CreateAccount(name, mnemonic, bip39Passphrase, encryptPassword, uint32(account), uint32(index))
	if err != nil {
		*result = 1
		return C.CString(err.Error())
	}
	*result = 0
	return C.CString(printNewInfo(info))
}

func printNewInfo(info keys.Info) string {
	keyname := info.GetName()
	keytype := info.GetType()
	keypub := info.GetPubKey()
	keyaddr := info.GetAddress()
	keypath, _ := info.GetPath()

	result := fmt.Sprintf("* %s (%s) - addr: %v pub: %v, path: %v",
		keyname, keytype, keyaddr, keypub, keypath)
	return result
}

//export ListKeys
func ListKeys(result *C.int) *C.char {
	kb, err := keys.NewKeyBaseFromDir(_baseCfg.Home)
	if err != nil {
		*result = 1
		return C.CString(err.Error())
	}

	infos, err := kb.List()
	if err != nil {
		*result = 1
		return C.CString(err.Error())
	}
	*result = 0
	return C.CString(printInfos(infos))
}

func printInfos(infos []keys.Info) string {
	result := ""
	for i, info := range infos {
		keyname := info.GetName()
		keytype := info.GetType()
		keypub := info.GetPubKey()
		keyaddr := info.GetAddress()
		keypath, _ := info.GetPath()

		result += fmt.Sprintf("%d. %s (%s) - addr: %v pub: %v, path: %v",
			i, keyname, keytype, keyaddr, keypub, keypath)
	}
	return result
}

//export queryHandler
func queryHandler(cRemoteUrl *C.char, cPath *C.char, result *C.int) *C.char {
	cfg := &queryCfg{
		rootCfg: &_baseCfg,
	}
	remote := C.GoString(cRemoteUrl)
	if remote == "" || remote == "y" {
		*result = 1
		return C.CString("missing remote url")
	}
	cfg.path = C.GoString(cPath)

	data := []byte(cfg.data)
	opts2 := rpcclient.ABCIQueryOptions{
		// Height: height, XXX
		// Prove: false, XXX
	}
	cli := rpcclient.NewHTTP(remote, "/websocket")
	qres, err := cli.ABCIQueryWithOptions(
		cfg.path, data, opts2)
	if err != nil {
		*result = 1
		return C.CString(errors.Wrap(err, "querying").Error())
	}

	if err != nil {
		*result = 1
		return C.CString(err.Error())
	}

	if qres.Response.Error != nil {
		return C.CString(qres.Response.Error.Error())
	}

	resdata := qres.Response.Data
	// XXX in general, how do we know what to show?
	// proof := qres.Response.Proof
	height := qres.Response.Height
	response := fmt.Sprintf("height: %d\ndata: %s\n",
		height,
		string(resdata))

	*result = 0
	return C.CString(response)
}

func main() {}

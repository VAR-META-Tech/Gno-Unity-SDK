package main

import "C"

import (
	"errors"
	"fmt"

	"github.com/gnolang/gno/tm2/pkg/crypto/bip39"
	"github.com/gnolang/gno/tm2/pkg/crypto/keys"
	"github.com/gnolang/gno/tm2/pkg/crypto/keys/client"
)

const (
	mnemonicEntropySize = 256
)

type baseCfg struct {
	client.BaseOptions
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
func AddAccount(cname *C.char, cmnemonic *C.char, result *C.int) *C.char {
	var (
		kb              keys.Keybase
		err             error
		encryptPassword string
	)

	kb, err = keys.NewKeyBaseFromDir(_baseCfg.Home)
	if err != nil {
		return C.CString(err.Error())
	}

	name := C.GoString(cname)

	_, err = kb.GetByName(name)
	if err == nil {
		// account exists, ask for user confirmation
		err = errors.New("account exists")
		// return C.CString(err.Error())
	}

	// Get bip39 mnemonic
	mnemonic := C.GoString(cmnemonic)
	const bip39Passphrase string = "" // XXX research.
	var account uint64
	var index uint64

	if len(mnemonic) == 0 {
		mnemonic, err = client.GenerateMnemonic(mnemonicEntropySize)
		if err != nil {
			return C.CString(err.Error())
		}
	}

	info, err := kb.CreateAccount(name, mnemonic, bip39Passphrase, encryptPassword, uint32(account), uint32(index))
	if err != nil {
		return C.CString(err.Error())
	}
	printNewInfo(info)

	return C.CString("")
}

func printNewInfo(info keys.Info) {
	keyname := info.GetName()
	keytype := info.GetType()
	keypub := info.GetPubKey()
	keyaddr := info.GetAddress()
	keypath, _ := info.GetPath()

	fmt.Printf("* %s (%s) - addr: %v pub: %v, path: %v",
		keyname, keytype, keyaddr, keypub, keypath)
}

func main() {}

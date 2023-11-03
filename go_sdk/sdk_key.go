package client

import "C"

import (
	"github.com/gnolang/gno/tm2/pkg/commands"
)

func GenerateNormal() *C.char {
	cfg := &generateCfg{
		customEntropy: false,
	}

	err := execGenerate(cfg, []string{}, commands.NewTestIO())

	return C.CString(err)
}

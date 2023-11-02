package main

import "C"

import (
	"context"
	"fmt"
	"os"
	"strings"

	"github.com/gnolang/gno/tm2/pkg/commands"
	"github.com/gnolang/gno/tm2/pkg/crypto/keys/client"
)

//export CallSDK
func CallSDK(cargs *C.char) {
	args := strings.Split(C.GoString(cargs), " ") // Split the C string into a Go string slice

	fmt.Println("Args: ", args) // Print the arguments

	cmd := client.NewRootCmd(commands.NewDefaultIO())
	if err := cmd.ParseAndRun(context.Background(), args); err != nil {
		_, _ = fmt.Fprintf(os.Stderr, "%+v\n", err)
		os.Exit(1)
	}
}

func main() {}

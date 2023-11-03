#include <stdio.h>
#include <stdlib.h> 
#include <string.h>
#include "keygen_sdk.h"

char* GenerateMnemonicCode(){
    int result;
    char* mnemonic = Generate(0,"",&result);
    if (result == 0){
        printf("Mnemonic: %s\n", mnemonic);
        return mnemonic;
    } else {
        printf("Err: %s\n", mnemonic);
        return "";
    }
}

void AddAccountSDK(char* KEYNAME, char* MnemonicCode){
    int result;
    char* err = AddAccount(KEYNAME, MnemonicCode, &result);
    if (result == 0){
        printf("Account Added\n");
    } else {
        printf("Err: %s\n", err);
    }
}

char* ListAccount(){
    // char *args = "list";
    // return CallSDK(args);
}

int main() {
    printf("Using keygen lib from C:\n");
    char* code = GenerateMnemonicCode();
    AddAccountSDK("testKey", code);
    ListAccount();
    return 0;
}


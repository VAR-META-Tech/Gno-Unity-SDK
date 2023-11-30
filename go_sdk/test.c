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
    char* err = AddAccount(KEYNAME, MnemonicCode, 0, &result);
    if (result == 0){
        printf("Account Added\n");
    } else {
        printf("Err: %s\n", err);
    }
}

char* ListAccountSDK(){
    int result;
    char* error = ListKeys(&result);
    if (result == 0){
        printf("List Accounts: %s\n", error);
        return error;
    } else {
        printf("Err: %s\n", error);
        return "";
    }
}

char* QueryAccountBalance(){
    int result;
    char* error = queryHandler("test3.gno.land:36657","auth/accounts/g1xluae6ppak2dx69j3ypsne4qydkrf303khaz9p",   &result);
    if (result == 0){
        printf("Balance Accounts: %s\n", error);
        return error;
    } else {
        printf("Err: %s\n", error);
        return "";
    }
}

int main() {
    printf("Using keygen lib from C:\n");
    char* code = GenerateMnemonicCode();
    AddAccountSDK("testKey", code);
    ListAccountSDK();
    QueryAccountBalance();
    return 0;
}


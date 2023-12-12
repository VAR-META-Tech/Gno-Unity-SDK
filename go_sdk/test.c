#include <stdio.h>
#include <stdlib.h> 
#include <string.h>
#include "keygen_sdk.h"

// char* GenerateMnemonicCode(){
//     int result;
//     char* mnemonic = Generate(0,"",&result);
//     if (result == 0){
//         printf("Mnemonic: %s\n", mnemonic);
//         return mnemonic;
//     } else {
//         printf("Err: %s\n", mnemonic);
//         return "";
//     }
// }

// void AddAccountSDK(char* KEYNAME, char* MnemonicCode){
//     int result;
//     char* err = AddAccount(KEYNAME, MnemonicCode, 0, &result);
//     if (result == 0){
//         printf("Account Added\n");
//     } else {
//         printf("Err: %s\n", err);
//     }
// }

// char* ListAccountSDK(){
//     int result;
//     char* error = ListKeys(&result);
//     if (result == 0){
//         printf("List Accounts: %s\n", error);
//         return error;
//     } else {
//         printf("Err: %s\n", error);
//         return "";
//     }
// }

// char* QueryAccountBalance(){
//     int result;
//     char* error = queryHandler("test3.gno.land:36657","auth/accounts/g1xluae6ppak2dx69j3ypsne4qydkrf303khaz9p",   &result);
//     if (result == 0){
//         printf("Balance Accounts: %s\n", error);
//         return error;
//     } else {
//         printf("Err: %s\n", error);
//         return "";
//     }
// }

enum {
    Success = 0,
    Fail = 1
};

int main() {
    printf("Using keygen lib from C:\n");
    // char* code = GenerateMnemonicCode();
    // AddAccountSDK("testKey", code);
    // ListAccountSDK();
    // QueryAccountBalance();
    int ret = SetRemote("192.168.1.1");
    if (ret == Success){
        printf("Success\n");
    } else {
        printf("Fail\n");
    }
    printf("Remote is %s \n",GetRemote());

    printf("GenerateRecoveryPhrase is %s \n",GenerateRecoveryPhrase());

    int len;

    KeyInfo **keyInfos = ListKeyInfo(&len);

    if (keyInfos != NULL) {
        // Use the keyInfos array
        for (int i = 0; i < len; ++i) {
            KeyInfo *keyInfo = keyInfos[i];
            // Do something with keyInfo, e.g., print it
            printf("Key Name: %s\n", keyInfo->Name);
            // Remember that you will need to free keyInfo->Name
            // and any other allocated fields within keyInfo if necessary
        }
    }
    return 0;
}


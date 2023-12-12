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
    Success = 1,
    Fail = 0
};

int main() {
    printf("Using keygen lib from C:\n");
    // char* code = GenerateMnemonicCode();
    // AddAccountSDK("testKey", code);
    // ListAccountSDK();
    // QueryAccountBalance();
    int ret = SetRemote("testnet.gno.berty.io:26657");
    if (ret == Success){
        printf("Set Remote Success\n");
    } else {
        printf("Set Remote Fail\n");
    }
    printf("Remote is %s \n",GetRemote());

    SetChainID("dev");

    char* chainID = GetChainID();

    printf("chainID %d\n",chainID);

    if (!HasKeyByName("test2")){

        char* mnemo = GenerateRecoveryPhrase();

        printf("GenerateRecoveryPhrase is %s \n",mnemo);

        CreateAccount("test2", "duty theme supreme path potato end net jump casino bunker material sense target patient junk series cover tumble material foster quantum juice celery race", "", "", 0, 0);
    }

    int len;

    KeyInfo **keyInfos = ListKeyInfo(&len);

    if (keyInfos != NULL) {
        // Use the keyInfos array
        for (int i = 0; i < len; ++i) {
            KeyInfo *keyInfo = keyInfos[i];
            // Do something with keyInfo, e.g., print it
            printf("Key Name: %s\n", keyInfo->Name);
            printf("Key Type: %d\n", keyInfo->Type);
            printf("Key Address: %s\n", keyInfo->Address);
            printf("Key PubKey: %s\n", keyInfo->PubKey);
        }
    }

    UserAccount* user = SelectAccount("test2");
    printf("User name: %s\n", user->Info->Name);
    printf("User pass: %s\n", user->Password);

    BaseAccount* acc = QueryAccount(user->Info->Address);
    if (acc){
        printf("User coins length %d\n",acc->Coins->Length);
    }
    return 0;
}


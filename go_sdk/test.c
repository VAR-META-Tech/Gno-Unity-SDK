#include <stdio.h>
#include "keygen_sdk.h"

int main() {
    printf("Using keygen lib from C:\n");
   
    char *args = "generate";
    CallSDK(args);
}
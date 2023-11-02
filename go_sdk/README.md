# Gno-Unity-SDK
Make native sdk for each platform

#Windows
go build -o keygen_sdk.dll -buildmode=c-shared

#For Test
gcc -o test test.c ./helloworld.dll
./test.exe
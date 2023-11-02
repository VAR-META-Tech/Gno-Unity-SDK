# Gno-Unity-SDK
Make native sdk for each platform

#Windows
go build -o keygen_sdk.dll -buildmode=c-shared .\keygen_sdk.go

#For Test
gcc -o test test.c ./keygen_sdk.dll
./test.exe
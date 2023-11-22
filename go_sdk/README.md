# Gno-Unity-SDK
Make native sdk for each platform

#Windows
go build -o keygen_sdk.dll -buildmode=c-shared ./keygen_sdk.go
#Mac Os
go build -o keygen_sdk.so  -buildmode=c-shared ./keygen_sdk.go

#Build For Test 
#Windows
gcc -o test test.c ./keygen_sdk.dll
#Mac Os
gcc -o test test.c ./keygen_sdk.so  

#Run Test
./test.exe
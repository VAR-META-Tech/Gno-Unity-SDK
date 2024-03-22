# Gno-Unity-SDK
Make native sdk for each platform

#Windows
go build -o gno_native_sdk.dll -buildmode=c-shared ./api.go
#Mac Os
go build -o gno_native_sdk.so  -buildmode=c-shared ./api.go

#Build For Test 
#Windows
gcc -o test test.c ./gno_native_sdk.dll
#Mac Os
gcc -o test test.c ./gno_native_sdk.so  

#Run Test
./test.exe
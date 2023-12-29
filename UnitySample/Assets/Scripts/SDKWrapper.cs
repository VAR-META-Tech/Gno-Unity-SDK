using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class SDKWrapper 
{
    const string DllPath = "/Users/baoquoc/VarMeta/Projects/AR_VR/Gno-Unity-SDK/go_sdk/gno_native_sdk.so";

    [DllImport(DllPath)]
    public static extern int SetRemote(string remote);

    [DllImport(DllPath)]
    public static extern string GetRemote();

    [DllImport(DllPath)]
    public static extern int SetChainID(string chainID);

    [DllImport(DllPath)]
    public static extern string GetChainID();

    [DllImport(DllPath)]
    public static extern int HasKeyByName(string name);

    [DllImport(DllPath)]
    public static extern string GenerateRecoveryPhrase();

    [DllImport(DllPath)]
    public static extern IntPtr ListKeyInfo(out int length);

    [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr QueryAccount(byte[] address);

    [DllImport(DllPath)]
    public static extern IntPtr CreateAccount(
        string nameOrBech32,
        string mnemonic,
        string bip39Passwd,
        string password,
        int account,
        int index
    );

    [DllImport(DllPath)]
    public static extern IntPtr SelectAccount(string nameOrBech32);

    [DllImport(DllPath)]
    public static extern String AddressToBech32(byte[] address);

    [DllImport(DllPath)]
    public static extern IntPtr GetKeyInfoByAddress(byte[] address);

    [DllImport(DllPath, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr Call(
            string packagePath,
            string fnc,
            string[] args,
            string gasFee,
            ref ulong gasWanted,
            string send,
            string memo,
            out int retLen
    );
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class SDKWrapper 
{
    const string DllPath = "/Users/anhnph/Downloads/Work/Products/Gno-Unity-SDK/go_sdk/keygen_sdk.so";

    [DllImport(DllPath)]
    public static extern IntPtr Generate(byte customEntropy, string inputEntropy, out int result);

    [DllImport(DllPath)]
    public static extern IntPtr AddAccount(string cname, string cmnemonic, int index, out int result);

    [DllImport(DllPath)]
    public static extern IntPtr ListKeys(out int result);
}

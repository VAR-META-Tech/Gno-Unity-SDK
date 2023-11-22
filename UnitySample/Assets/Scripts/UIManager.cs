using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UIManager;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public TMP_InputField _mnemonicInput;


    [SerializeField]
    public TMP_InputField _nameInput;

    [SerializeField]
    public TMP_Dropdown _listKeyDropdown;

    public class KeyInfo
    {
        public int KeyNumber { get; set; }
        public string KeyName { get; set; }
        public string KeyType { get; set; }
        public string Address { get; set; }
        public string PublicKey { get; set; }
        public string Path { get; set; }
    }

    private string mnemonicCode;
    private List<KeyInfo> keyInfos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onGenerateButtonClicked()
    {
        byte customEntropy = 0;
        string inputEntropy = "";
        int result;

        IntPtr ptr = SDKWrapper.Generate(customEntropy, inputEntropy, out result);
        string generateResult = Marshal.PtrToStringAnsi(ptr);

        Debug.Log($"Generate result: {generateResult}");
        Debug.Log($"Int result: {result}");
        _mnemonicInput.text = generateResult;
        if (generateResult.Length > 0)
        {
            mnemonicCode = generateResult;
        }
    }

    public void onAddAccountButtonClicked()
    {
        listKeys();
        if (mnemonicCode.Length > 0)
        {
            string cname = "testKey";

            if (_nameInput.text.Length > 0)
            {
                cname = _nameInput.text;
            }

            int result;
            int index = 0;
            if (keyInfos.Count > 0)
            {
                index = keyInfos.Count;
            }

            IntPtr ptr = SDKWrapper.AddAccount(cname, mnemonicCode, index, out result);
            string addAccountResult = Marshal.PtrToStringAnsi(ptr);

            Debug.Log($"AddAccount result: {addAccountResult}");
            Debug.Log($"Int result: {result}");

            listKeys();
        } else
        {
            Debug.Log($"Missing mnemonic");
        }
    }

    public List<KeyInfo> ParseKeyInfo(string keyInfoString)
    {
        var regex = new Regex(@"(\d+)\.\s([^ ]+)\s\(([^)]+)\)\s-\saddr:\s([^ ]+)\spub:\s([^,]+),\spath:\s([^1-9]+)");
        var matches = regex.Matches(keyInfoString);

        keyInfos = new List<KeyInfo>();

        foreach (Match match in matches)
        {
            keyInfos.Add(new KeyInfo
            {
                KeyNumber = int.Parse(match.Groups[1].Value),
                KeyName = match.Groups[2].Value,
                KeyType = match.Groups[3].Value,
                Address = match.Groups[4].Value,
                PublicKey = match.Groups[5].Value,
                Path = match.Groups[6].Value
            });
        }

        return keyInfos;
    }

    public void listKeys()
    {
        int result;
        IntPtr ptr = SDKWrapper.ListKeys(out result);
        string listKeysResult = Marshal.PtrToStringAnsi(ptr);

        List<KeyInfo> keyInfos = ParseKeyInfo(listKeysResult);

        foreach (var keyInfo in keyInfos)
        {
            Debug.Log($"Key number: {keyInfo.KeyNumber}");
            Debug.Log($"Key name: {keyInfo.KeyName}");
            Debug.Log($"Key type: {keyInfo.KeyType}");
            Debug.Log($"Address: {keyInfo.Address}");
            Debug.Log($"Public key: {keyInfo.PublicKey}");
            Debug.Log($"Path: {keyInfo.Path}");
        }

        List<string> dropdownOptions = new List<string>();
        foreach (var keyInfo in keyInfos)
        {
            dropdownOptions.Add(keyInfo.KeyName + " (" + keyInfo.Address + ")");
        }

        _listKeyDropdown.ClearOptions();
        _listKeyDropdown.AddOptions(dropdownOptions);
    }
}

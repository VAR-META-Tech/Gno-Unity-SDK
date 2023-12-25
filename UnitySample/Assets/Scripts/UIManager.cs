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

    public static class Constants
    {
        public const int PUBKEY_SIZE = 58;
        public const int ADDRESS_SIZE = 20;
    }

    [SerializeField] private PanelTab accountTab;
    [SerializeField] private PanelTab sendTransactionTab;
    [SerializeField] private PanelTab mintNFTTab;
    [SerializeField] private PanelTab nftLoaderTab;
    [SerializeField] private PanelTab addAccountTab;
    [Space]
    [Header("Infos")]
    [SerializeField] private TMP_Dropdown walletListDropDown;
    [SerializeField] private TMP_Dropdown networkDropDown;

    [Header("Add Account")]
    [SerializeField] private TMP_InputField createdMnemonicInputField;
    [SerializeField] private TMP_InputField importMnemonicInputField;

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyInfo
    {
        public uint Type;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.PUBKEY_SIZE)]
        public byte[] PubKey;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.ADDRESS_SIZE)]
        public byte[] Address;
    }


    public static string ByteArrayToHexString(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
    public class ResponseInfo
    {
        public Status status;
        public string message;
        public enum Status
        {
            Success,
            Pending,
            NotFound,
            Failed,
            Warning
        }
    }
    private string mnemonicCode = "source bonus chronic canvas draft south burst lottery vacant surface solve popular case indicate oppose farm nothing bullet exhibit title speed wink action roast";
    // Start is called before the first frame update
    void Start()
    {
        InitStatusCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitStatusCheck()
    {
        if (PlayerPrefs.GetString("Gno.land") != string.Empty)
        {
            ToggleEmptyState(false);
        }
        else
        {
            ToggleEmptyState(true);
        }
    }

    public void ToggleEmptyState(bool _empty)
    {
        accountTab.DeActive(_empty);
        sendTransactionTab.DeActive(_empty);
        mintNFTTab.DeActive(_empty);
        nftLoaderTab.DeActive(_empty);    
    }
    public void onAddAccountButtonClicked()
    {
        // Set the remote endpoint for blockchain interactions.
            int ret = SDKWrapper.SetRemote("testnet.gno.berty.io:26657");
            Debug.Log($"Remote is {SDKWrapper.GetRemote()}");

            // Set the chain ID for subsequent operations.
            SDKWrapper.SetChainID("dev");
            Debug.Log($"chainID {SDKWrapper.GetChainID()}");

            // Refresh the list of keys/accounts.

            // Check if mnemonic code is present and has length greater than 0.
            if (!string.IsNullOrEmpty(mnemonicCode))
            {
                // Default account name.
                string cname = "testKey";

                // If a name has been entered, use that instead.
                if (!string.IsNullOrEmpty(_nameInput.text))
                {
                    cname = _nameInput.text;
                }

                // Determine the index for the new account.
                int index = 0;

                // Create another account with the given name and mnemonic.
               IntPtr keyInfoPtr = SDKWrapper.CreateAccount(cname, mnemonicCode, "", "", index, index);
                   if (keyInfoPtr != IntPtr.Zero)
                   {
                        KeyInfo keyInfo = Marshal.PtrToStructure<KeyInfo>(keyInfoPtr);
                        string addressString = ByteArrayToHexString(keyInfo.Address);
                        Debug.Log($"addressString {addressString}");
                        walletListDropDown.ClearOptions();
                        walletListDropDown.value = 0;
                        createdMnemonicInputField.text = "Go.land";

                        List<string> options = new List<string>();
                        options.Add(addressString);
                        walletListDropDown.AddOptions(options);
                        ToggleEmptyState(false);
            }

            // Refresh the key list to show the new account.
        }
            else
            {
                Debug.LogError("Missing mnemonic code.");
            }
    }

    
}

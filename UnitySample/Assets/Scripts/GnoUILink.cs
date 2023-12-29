using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Gno.Unity.Sample.UI
{
    public class GnoUILink : MonoBehaviour
    {
        static public GnoUILink Instance { get; set; }

        [HideInInspector]
        public string mnemonicsKey = "MnemonicsKey";
        [HideInInspector]
        public string privateKey = "PrivateKey";
        [HideInInspector]
        public string currentAddressIndexKey = "CurrentAddressIndexKey";

        //[SerializeField] private int accountNumLimit = 10;
        public List<string> addressList;

        public event Action<float> onGetBalance;
        public static class Constants
        {
            public const int PUBKEY_SIZE = 58;
            public const int ADDRESS_SIZE = 20;
        }
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
        [StructLayout(LayoutKind.Sequential)]
        public struct BaseAccount
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.ADDRESS_SIZE)]
            public byte[] Address;

            public IntPtr Coins; // Pointer to Coins, assuming it's a class or another struct

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.PUBKEY_SIZE)]
            public byte[] PubKey;

            public ulong AccountNumber;
            public ulong Sequence;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct UserAccount
        {
            public IntPtr Info; // Pointer to KeyInfo
            public IntPtr Password; // Pointer to a null-terminated string
        }
        public static string ByteArrayToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
        //private Wallet wallet;
        //private string faucetEndpoint = "https://faucet.devnet.aptoslabs.com";

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void InitWalletFromCache()
        {
            GetWalletAddressAsync();
            //LoadCurrentWalletBalance();
        }
        public bool CreateNewWallet()
        {

            string mnemo = SDKWrapper.GenerateRecoveryPhrase();
            PlayerPrefs.SetString(mnemonicsKey, mnemo.ToString());
            PlayerPrefs.SetInt(currentAddressIndexKey, 0);

            GetWalletAddressAsync();
            //LoadCurrentWalletBalance();

            if (mnemo.ToString() != string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool RestoreWallet(string _mnemo)
        {
            try
            {
                PlayerPrefs.SetString(mnemonicsKey, _mnemo.ToString());
                PlayerPrefs.SetInt(currentAddressIndexKey, 0);

                GetWalletAddressAsync();
                //LoadCurrentWalletBalance();
                if(addressList.Count == 0)
                {
                    return false;
                }
                return true;
            }
            catch
            {

            }

            return false;
        }

        public List<string> GetWalletAddressAsync()
        {
            addressList = new List<string>();
            string mnemonics = PlayerPrefs.GetString(mnemonicsKey);

            // Check if mnemonic code is present and has length greater than 0.
            if (!string.IsNullOrEmpty(mnemonics))
            {
                // Default account name.
                string cname = "testKey";
                // Determine the index for the new account.
                int index = 0;

                // Create another account with the given name and mnemonic.
                IntPtr keyInfoPtr = SDKWrapper.CreateAccount(cname, mnemonics, "", "", index, index);
                if (keyInfoPtr != IntPtr.Zero)
                {
                    KeyInfo keyInfo = Marshal.PtrToStructure<KeyInfo>(keyInfoPtr);
                    IntPtr keyInfoTest = SDKWrapper.GetKeyInfoByAddress(keyInfo.Address);
                    IntPtr accountTest = SDKWrapper.QueryAccount(keyInfo.Address);
                    //IntPtr accountTest = await QueryAccountAsync(keyInfo.Address);

                    string addressString = ByteArrayToHexString(keyInfo.Address);
                    addressList.Add(addressString);
                }

                // Refresh the key list to show the new account.
            }
            else
            {
                Debug.LogError("Missing mnemonic code.");
            }

            return addressList;
        }
        public static async Task<IntPtr> QueryAccountAsync(byte[] address)
        {
            return await Task.Run(() => {
                return SDKWrapper.QueryAccount(address);
            });
        }

        public float GnoTokenToFloat(float _token)
        {
            return _token / 100000000f;
        }

        public long GnoFloatToToken(float _amount)
        {
            return Convert.ToInt64(_amount * 100000000);
        }
    }
}
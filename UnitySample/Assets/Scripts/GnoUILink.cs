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
        public struct Coin
        {
            public string Denom;
            public ulong Amount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Coins
        {
            public IntPtr Array; // Pointer to the first Coin element
            public ulong Length; // Number of elements in the Coins array
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

        public Coin[] GetCoinsArray(IntPtr coinsPtr, ulong length)
        {
            if (coinsPtr == IntPtr.Zero)
            {
                return new Coin[0];
            }

            Coin[] coinsArray = new Coin[length];
            int coinSize = Marshal.SizeOf(typeof(Coin));

            for (ulong i = 0; i < length; i++)
            {
                IntPtr currentCoinPtr = new IntPtr(coinsPtr.ToInt64() + (long)(i * (ulong)coinSize));
                coinsArray[i] = Marshal.PtrToStructure<Coin>(currentCoinPtr);
            }

            return coinsArray;
        }

        public bool CreateNewWallet(string name)
        {

            //string mnemo = SDKWrapper.GenerateRecoveryPhrase();
            string mnemo = "source bonus chronic canvas draft south burst lottery vacant surface solve popular case indicate oppose farm nothing bullet exhibit title speed wink action roast";

            // Check if mnemonic code is present and has length greater than 0.
            if (!string.IsNullOrEmpty(mnemo))
            {
                // Determine the index for the new account.
                int index = 0;

                // Create another account with the given name and mnemonic.
                IntPtr keyInfoPtr = SDKWrapper.CreateAccount(name, mnemo, "", "", index, index);
                if (keyInfoPtr != IntPtr.Zero)
                {
                    KeyInfo keyInfo = Marshal.PtrToStructure<KeyInfo>(keyInfoPtr);
                    IntPtr userAccountPrt = SDKWrapper.SelectAccount(name);
                    //IntPtr keyInfoTest = SDKWrapper.GetKeyInfoByAddress(keyInfo.Address);
                    IntPtr accountTest = SDKWrapper.QueryAccount(keyInfo.Address);
                    if (accountTest != IntPtr.Zero)
                    {
                        BaseAccount accountBase = Marshal.PtrToStructure<BaseAccount>(accountTest);
                        // Marshal the Coins structure
                        Coins coinsStruct = Marshal.PtrToStructure<Coins>(accountBase.Coins);
                        Coin[] coins = GetCoinsArray(coinsStruct.Array, coinsStruct.Length);
                        foreach (var coin in coins)
                        {
                            if (coin.Denom == "ugnot")
                            {
                                onGetBalance(coin.Amount);
                            } 
                        }
                    }

                    string addressString = ByteArrayToHexString(keyInfo.Address);
                    addressList.Add(name);
                    return true;
                }

                // Refresh the key list to show the new account.
            }
            else
            {
                Debug.LogError("Missing mnemonic code.");
            }

            return false;
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

           

            return addressList;
        }
        public static async Task<IntPtr> QueryAccountAsync(byte[] address)
        {
            return await Task.Run(() => {
                return SDKWrapper.QueryAccount(address);
            });
        }
    }
}
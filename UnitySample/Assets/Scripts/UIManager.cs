using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Gno.Unity.Sample.UI;
using TMPro;
using UnityEngine;

namespace Aptos.Unity.Sample.UI
{
    public class UIManager : MonoBehaviour
    {
        static public UIManager Instance { get; set; }

        [SerializeField]
        public TMP_InputField _mnemonicInput;


        [SerializeField]
        public TMP_InputField _nameInput;

        [SerializeField]
        public TMP_Dropdown _listKeyDropdown;

        

        [SerializeField] private PanelTab accountTab;
        [SerializeField] private PanelTab sendTransactionTab;
        [SerializeField] private PanelTab mintNFTTab;
        [SerializeField] private PanelTab addAccountTab;
        [Space]
        [Header("Infos")]
        [SerializeField] private TMP_Dropdown walletListDropDown;
        [SerializeField] private TMP_Dropdown networkDropDown;

        [Header("Add Account")]
        [SerializeField] private TMP_InputField createdMnemonicInputField;
        [SerializeField] private TMP_InputField importMnemonicInputField;

        


        
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
        // Start is called before the first frame update

        private void Awake()
        {
            Instance = this;
        }
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
            if (PlayerPrefs.GetString(GnoUILink.Instance.mnemonicsKey) != string.Empty)
            {
                ToggleEmptyState(false);
            }
            else
            {
                ToggleEmptyState(true);
            }

        }
        public void AddWalletAddressListUI(List<string> _addressList)
        {
            walletListDropDown.ClearOptions();
            walletListDropDown.value = 0;

            List<string> addressList = new List<string>();
            foreach (string _s in _addressList)
            {
                //addressList.Add(ShortenString(_s, 4));
                addressList.Add(_s);
            }

            walletListDropDown.AddOptions(addressList);

            //senderAddress.text = AptosUILink.Instance.GetCurrentWalletAddress();
        }
        public void ToggleEmptyState(bool _empty)
        {
            accountTab.DeActive(_empty);
            sendTransactionTab.DeActive(_empty);
            mintNFTTab.DeActive(_empty);
        }
        public void onAddAccountButtonClicked()
        {
            if (GnoUILink.Instance.CreateNewWallet())
            {
                createdMnemonicInputField.text = PlayerPrefs.GetString(GnoUILink.Instance.mnemonicsKey);
                ToggleEmptyState(false);
            }
            else
            {
                ToggleEmptyState(true);
            }
            AddWalletAddressListUI(GnoUILink.Instance.addressList);
        }


    }
}
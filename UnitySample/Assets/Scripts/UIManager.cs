using System;
using System.Collections.Generic;
using Gno.Unity.Sample.UI;
using TMPro;
using UnityEngine;
using Gno.Unity.Rest;
using Gno.Unity.Rest.Model;

namespace Gno.Unity.Sample.UI
{
    public class UIManager : MonoBehaviour
    {
        static public UIManager Instance { get; set; }

        [Header("General")]
        public List<PanelTab> panelTabs;
        [Space]
        [SerializeField] private TMP_Text mainPanelTitle;
        [SerializeField] private GameObject notificationPrefab;
        [Space]
        [SerializeField] private PanelTab accountTab;
        [SerializeField] private PanelTab sendTransactionTab;
        [SerializeField] private PanelTab mintNFTTab;
        [SerializeField] private PanelTab addAccountTab;
        [Space]
        [Header("Infos")]
        [SerializeField] private TMP_Dropdown walletListDropDown;
        [SerializeField] private TMP_Dropdown networkDropDown;
        [SerializeField] private TMP_Text balanceText;

        [Header("Add Account")]
        [SerializeField] private TMP_InputField createdMnemonicInputField;
        [SerializeField] private TMP_InputField importMnemonicInputField;

        [Header("Notification")]
        [SerializeField] private Transform notificationPanel;

        // Start is called before the first frame update

        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            InitStatusCheck();
            GnoUILink.Instance.onGetBalance += UpdateBalance;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void InitStatusCheck()
        {
            if (PlayerPrefs.GetString(GnoUILink.Instance.mnemonicsKey) != string.Empty)
            {
                GnoUILink.Instance.InitWalletFromCache();
                AddWalletAddressListUI(GnoUILink.Instance.addressList);
                ToggleEmptyState(false);
                ToggleNotification(ResponseInfo.Status.Success, "Successfully Import the Wallet");
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
            if (_empty)
            {
                walletListDropDown.ClearOptions();
                List<string> options = new List<string>();
                options.Add("Please Create Wallet First");
                walletListDropDown.AddOptions(options);
                balanceText.text = "n/a APT";
                createdMnemonicInputField.text = String.Empty;
                importMnemonicInputField.text = String.Empty;

                OpenTabPanel(addAccountTab);
            }
        }
        public void OpenTabPanel(PanelTab _panelTab)
        {
            foreach (PanelTab _childPanelTab in panelTabs)
            {
                if (_childPanelTab.panelGroup == _panelTab.panelGroup)
                {
                    _childPanelTab.UnSelected();
                }
            }

            _panelTab.Selected();

            if (_panelTab.panelGroup == PanelGroup.mainPanel)
            {
                mainPanelTitle.text = _panelTab.tabName;
            }
        }
        public void ToggleNotification(ResponseInfo.Status status, string _message)
        {
            NotificationPanel np = Instantiate(notificationPrefab, notificationPanel).GetComponent<NotificationPanel>();
            np.Toggle(status, _message);
        }
        public void onAddAccountButtonClicked()
        {
            if (GnoUILink.Instance.CreateNewWallet())
            {
                createdMnemonicInputField.text = PlayerPrefs.GetString(GnoUILink.Instance.mnemonicsKey);
                ToggleEmptyState(false);
                ToggleNotification(ResponseInfo.Status.Success, "Successfully Create the Wallet");
            }
            else
            {
                ToggleEmptyState(true);
                ToggleNotification(ResponseInfo.Status.Failed, "Fail to Create the Wallet");
            }
            AddWalletAddressListUI(GnoUILink.Instance.addressList);
        }
        public void OnImportWalletClicked(TMP_InputField _input)
        {
            if (GnoUILink.Instance.RestoreWallet(_input.text))
            {
                AddWalletAddressListUI(GnoUILink.Instance.addressList);
                ToggleEmptyState(false);
                ToggleNotification(ResponseInfo.Status.Success, "Successfully Import the Wallet");
            }
            else
            {
                ToggleEmptyState(true);
                ToggleNotification(ResponseInfo.Status.Failed, "Fail to Import the Wallet");
            }
        }
        void UpdateBalance(float _amount)
        {
            balanceText.text = GnoUILink.Instance.GnoTokenToFloat(_amount).ToString("0.0000") + " APT";
        }
        public void Logout()
        {
            PlayerPrefs.DeleteKey(GnoUILink.Instance.mnemonicsKey);

            ToggleEmptyState(true);
        }
    }
}
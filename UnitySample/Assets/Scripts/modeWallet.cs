using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class modeWallet : MonoBehaviour
{
    public GameObject createWallet;
    public GameObject importWallet;
    public TextMeshProUGUI txtTitleWallet;
    public GameObject dropDownName;


    public void ModeWallet( )
    {
        int valueOption = dropDownName.GetComponent<TMP_Dropdown>().value;
        if(valueOption == 0)
        {
            txtTitleWallet.text = "Import Wallet";
            createWallet.SetActive(false);
            importWallet.SetActive(true);
        }
        if (valueOption == 1)
        {
            txtTitleWallet.text = "Create Wallet";
            createWallet.SetActive(true);
            importWallet.SetActive(false);
        }
    }
}

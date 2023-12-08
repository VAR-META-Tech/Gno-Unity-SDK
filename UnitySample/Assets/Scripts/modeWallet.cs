using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class modeWallet : MonoBehaviour
{
    public TextMeshProUGUI txtButtonWallet;
    public TextMeshProUGUI txtTitleWallet;
    public GameObject dropDownName;

    public void ModeWallet( )
    {
        int valueOption = dropDownName.GetComponent<TMP_Dropdown>().value;
        if(valueOption == 0)
        {
            txtButtonWallet.text = "Import Wallet";
            txtTitleWallet.text = "Import Wallet";
        }
        if (valueOption == 1)
        {
            txtButtonWallet.text = "Create Wallet";
            txtTitleWallet.text = "Create Wallet";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Gno.Unity.Sample.UI
{
    public class PanelTab : MonoBehaviour
    {
        public List<PanelTab> panelTabs;

        [SerializeField] private TMP_Text mainPanelTitle;

        public PanelGroup panelGroup;
        public string tabName;
        public GameObject targetPanel;
        public GameObject lockIcon;

        public bool isSelected;

        public Color selectedColor;
        public Color unselectedColor;

        public GameObject textTab;
        public GameObject iconTab;

        private Button m_button;

        private void Awake()
        {
            m_button = this.GetComponent<Button>();
            m_button.onClick.AddListener(delegate { OpenTabPanel(this); });

            //selectedColor = m_button.colors.selectedColor;
            //unselectedColor = m_button.colors.normalColor;

            //unselectedTextColor = contentText.color;
        }

        private void Start()
        {
            if (isSelected)
            {
                OpenTabPanel(this);
            }
        }

        public void DeActive(bool _lock)
        {
            if (this.gameObject.activeSelf)
            {
                lockIcon.SetActive(_lock);
                m_button.interactable = !_lock;
            }
        }

        public void Selected()
        {
            isSelected = true;

            targetPanel.SetActive(true);

            //ColorBlock _colorBlock = m_button.colors;
            //_colorBlock.normalColor = selectedColor;
            //m_button.colors = _colorBlock;
            iconTab.GetComponent<Image>().color = selectedColor;
            textTab.GetComponent<TextMeshProUGUI>().color = selectedColor;
            //contentText.color = selectedTextColor;
        }

        public void UnSelected()
        {
            if (this.gameObject.activeSelf)
            {
                isSelected = false;

                targetPanel.SetActive(false);

                //ColorBlock _colorBlock = m_button.colors;
                //_colorBlock.normalColor = unselectedColor;
                //m_button.colors = _colorBlock;
                iconTab.GetComponent<Image>().color = unselectedColor;
                textTab.GetComponent<TextMeshProUGUI>().color = unselectedColor;

                //contentText.color = unselectedTextColor;
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
    }

    public enum PanelGroup
    {
        mainPanel,
        addAccount,
        mintNFT,
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuButton : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject[] buttons;
    public GameObject activePanel;
    public bool first;

    public Transform furnitureProductPanelTr;


    private void Start()
    {
        if (first)
        {
            OnClickButton();
        }
    }

    public void OnClickButton()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
            buttons[i].GetComponent<RectTransform>().SetAsFirstSibling();
        }

        activePanel.gameObject.SetActive(true);
        gameObject.GetComponent<RectTransform>().SetAsLastSibling();
    }
}


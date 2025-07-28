using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenBar : MonoBehaviour
{

    public string key; //[가구종류]_[등급]
    public Image wineImage;
    public Image wineProgressBar;

    private void Awake()
    {
        key = gameObject.name;

        string[] strs = key.Split('_');

        wineImage = GetComponent<Image>();
        wineProgressBar = transform.Find("WineProgressBar").GetComponent<Image>();
        wineImage.sprite = Resources.Load<Sprite>($"Images/Kitchen/{strs[1]}");
        wineProgressBar.sprite = wineImage.sprite;
    }

}



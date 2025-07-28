using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCanvas : MonoBehaviour //말풍선 안에 주문 이미지
{

    public Image menuImage;

    public void SetOrderMenu(MenuData data)
    {
        Debug.Log("전달된 data: " + data.key + ", 이미지: " + data.menuOrderImage);

        gameObject.SetActive(true);
        menuImage.sprite = data.menuOrderImage;
    }

}

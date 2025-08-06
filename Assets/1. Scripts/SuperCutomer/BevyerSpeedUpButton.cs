using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//옵저버 패턴

public class BevyerSpeedUpButton : MonoBehaviour
{
    public Image image;

    private void Start()
    {
        image = GetComponentInChildren<Image>();
        image.enabled = false;
        //image.enabled = true;
        StartCoroutine(CoCheckButton());
    }

    IEnumerator CoCheckButton()
    {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (true)
        {
            CheckButton();
            if (image.enabled)
                break;

            yield return wait;

        }

    }

    private void CheckButton() // 상점에서 가구를 구매했을때만 호출
    {
        UserKitchen userKitchen  = User.Instance.GetUserKitchen("KitchenBar0_0");
        if (userKitchen != null && userKitchen.purchased)
        {
            image.enabled = true;
        }
    }


}

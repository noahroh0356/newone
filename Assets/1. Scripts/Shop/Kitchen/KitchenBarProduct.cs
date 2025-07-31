using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class KitchenBarProduct : MonoBehaviour
{

    public string key;
    KitchenData kitchenData;
    public TMP_Text priceText;

    public void Start()
    {

        Transform buttonTr = transform.Find("**Button");


        KitchenManager mgr = FindObjectOfType<KitchenManager>();
        kitchenData = mgr.GetKitchenData(key);

        if (!string.IsNullOrEmpty(kitchenData.nextProductKey))
        {
            if (kitchenData.price == 0)
            {
                // 시스템 언어 확인
                if (Application.systemLanguage == SystemLanguage.Korean)
                    priceText.text = "무료";
                else
                    priceText.text = "Free";
            }
            else
            {
                priceText.text = kitchenData.price.ToString();
            }

            buttonTr.gameObject.SetActive(true);
        }
        else
        {
            buttonTr.gameObject.SetActive(false);
        }
        //if (!string.IsNullOrEmpty(kitchenData.nextProductKey))
        //{
        //    priceText.text = kitchenData.price.ToString();
        //    buttonTr.gameObject.SetActive(true);

        //}

        //else
        //{
        //    buttonTr.gameObject.SetActive(false);
        //}


        Button button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnClickedOpenKitchen);
    }

    public void OnClickedOpenKitchen()
    {
        Debug.Log("kitchenbaproduct OnClickedOpenKitchen" + key);
        KitchenCanvas.Instance.Open(key);

    }

    //키값에 해당하는 주방 가구를 구매 시도하는 함수
    public void OnClickPurchase()
    {
        Debug.Log(key + "구매시도");
        if (User.Instance.userData.coin < kitchenData.price)
        {
            ToastCanvas.Instance.ShowToast("needarcon");
            return;
        }

        else
        {
            User.Instance.AddKitchenFurniture(key);
            KitchenManager.Instance.UpdateKitchen();

            MainQuestManager.Instance.DoQuest(MainQuestType.PurchaseKitchen);
            GetComponentInParent<KitchenBarPlaceProduct>().UpdateKitchenBarPlace(); 
            User.Instance.userData.coin -= kitchenData.price;
            //User.Instance.UpdateCoinText();


        }

    }


}


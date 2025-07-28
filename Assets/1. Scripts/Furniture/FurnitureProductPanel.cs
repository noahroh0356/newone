using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FurnitureProductPanel : MonoBehaviour
{

    public string key;
    public TextMeshProUGUI PorUtext;
    public TextMeshProUGUI Pricetext;
    public Image tableImage;

    FurnitureData furnitureData;
    

    public void Start()
    {
        Transform buttonTr = transform.Find("**Button");

        FurnitureManager mgr = FindObjectOfType<FurnitureManager>();
        furnitureData = mgr.GetFurnitureData(key);

        if (!string.IsNullOrEmpty(furnitureData.nextfurniturekey))
        {
            Pricetext.text = furnitureData.price.ToString();
            buttonTr.gameObject.SetActive(true);
        }

        else
        {
            buttonTr.gameObject.SetActive(false);
        }

        //버튼 컴포넌트를 동적으로(런타임 실행 중) 추가하기
        Button button = gameObject.AddComponent< Button > ();
        button.onClick.AddListener(OnClickedOpenFurniture);
    }

    public void OnClickedOpenFurniture()
    {
        Debug.Log("furnitureProductPanel OnClickedOpenFurniture" + key);
        FurnitureCanvas.Instance.Open(key);

    }


    public void OnClickPurchased()
    {

        Debug.Log(key + "구매시도");
        if (User.Instance.userData.coin < furnitureData.price)
        {
            ToastCanvas.Instance.ShowToast("needarcon");

            return;
        }

        else
        {
            User.Instance.AddFurniture(key);
            FurnitureManager.Instance.PurchaseFurniture(key);


            MainQuestManager.Instance.DoQuest(MainQuestType.PurchaseFurniture);
            Debug.Log(key + "구매완료");

            GetComponentInParent<TablePlaceProducts>().UpdateTablePlace();
            User.Instance.userData.coin -= furnitureData.price;
            //User.Instance.UpdateCoinText();
            FurnitureManager.Instance.UpdateFurniture();
            //PorUtext.text = "업그레이드";

        }
    }
}
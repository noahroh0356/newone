using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FoxProductPanel : MonoBehaviour
{
    public string key;
    public TextMeshProUGUI PorUtext;
    public TextMeshProUGUI Pricetext;
    public Image foxImage;
    FoxData foxData;


    public void Start()
    {

   

        FoxManager mgr = FindObjectOfType<FoxManager>();
        foxData = mgr.GetFoxData(key);

        Pricetext.text = foxData.price.ToString();

        Button button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnClickedOpenFox);
    }

    public void OnClickedOpenFox()
    {
        Debug.Log("OnClickedOpenFox" + key);
        WorkerCanvas.Instance.Open(key);

    }

    public void OnClickPurchased()
    {

        Debug.Log(key + "구매시도");
        if (User.Instance.userData.coin < foxData.price)
        {
            ToastCanvas.Instance.ShowToast("needarcon");
            return;
        }

        else
        {
            User.Instance.AddFox(key);
            FoxManager.Instance.AddFox(key);

            MainQuestManager.Instance.DoQuest(MainQuestType.PurchaseFox);

            User.Instance.userData.coin -= foxData.price;
            Transform buttonTr = transform.Find("**Button");
            buttonTr.gameObject.SetActive(false);

            //User.Instance.UpdateCoinText();
            //FoxManager.Instance.UpdateFox();
            //PorUtext.text = "업그레이드";

        }
    }
}

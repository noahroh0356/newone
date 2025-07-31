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

        if (foxData.price == 0)
        {
            if (Application.systemLanguage == SystemLanguage.Korean)
                Pricetext.text = "무료";
            else
                Pricetext.text = "Free";
        }
        else
        {
            Pricetext.text = foxData.price.ToString();
        }

            Transform buttonTr = transform.Find("**Button"); // 실제 버튼 이름 넣어야 함
    UserFox userFox = User.Instance.GetUserFox(key);

        if (userFox != null && userFox.purchased)
        {
            buttonTr.gameObject.SetActive(false);

        }

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
            PorUtext.text = "보유 중";
        }
    }
}

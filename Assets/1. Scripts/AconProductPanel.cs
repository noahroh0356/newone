using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class AconProductPanel : MonoBehaviour
{
    public string inAppId;
    public int aconReward;
    public TMP_Text priceText;

    private void Start()
    {
        priceText.text = InAppMgr.Instance.GetProductPrice(inAppId);
        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(OnClickedPurchase);

    }

    public void OnClickedPurchase()
    {
        Debug.Log("도토리 구매");
        InAppMgr.Instance.Purchase(inAppId, Purchased);
    }


    //구매가 성공 또는 실패 했을때 호출됨 = 인앱매니저에서purchasecallback(결제 성공 or 실패)이 호출하는 상황에서 호출
    public void Purchased(bool success)
    {
        if (success)
        {
            User.Instance.AddCoin(aconReward);
        }

    }
}

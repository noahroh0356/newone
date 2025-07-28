using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GatchaCoinProductPanel : MonoBehaviour
{
    public string inAppId;
    public int gatchaCoinReward;
    public TMP_Text priceText;

    private void Start()
    {
        priceText.text = InAppMgr.Instance.GetProductPrice(inAppId);
        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(OnClickedPurchase);
    }

    public void OnClickedPurchase()
    {
        InAppMgr.Instance.Purchase(inAppId, Purchased);
    }


    //구매가 성공 또는 실패 했을때 호출됨 = 인앱매니저에서purchasecallback(결제 성공 or 실패)이 호출하는 상황에서 호출
    public void Purchased(bool success)
    {
        if (success)
        {
            User.Instance.AddGatchaCoin(gatchaCoinReward);
        }

    }

}

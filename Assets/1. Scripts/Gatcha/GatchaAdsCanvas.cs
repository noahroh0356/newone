using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GatchaAdsCanvas 활성화 되는 타이밍 조정
// GatchaManager.Instance.StartGatcha(); 호출 전에 가챠 캔버스 게임 활성화 시키기 

public class GatchaAdsCanvas : MonoBehaviour
{


    public void OnClickedAds()
    {
        AdsMgr.Instance.ShowAd(AdUnitType.RV, result =>
    {
            if (result)
            {
            GatchaManager.Instance.StartGacha();
            gameObject.SetActive(false);
        }
    });

    }


    public void OnClickedClose()
    {
        gameObject.SetActive(false);
    }

}

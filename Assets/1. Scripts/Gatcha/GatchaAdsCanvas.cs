using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GatchaAdsCanvas 활성화 되는 타이밍 조정
// GatchaManager.Instance.StartGatcha(); 호출 전에 가챠 캔버스 게임 활성화 시키기 

public class GatchaAdsCanvas : MonoBehaviour
{


    public void Init()
    {
        gameObject.SetActive(true);
        StartCoroutine(AutoCloseAfterDelay(5f));

    }

    IEnumerator AutoCloseAfterDelay(float seconds)
    {
        Debug.Log("코루틴은 실행");

        yield return new WaitForSeconds(seconds);

        if (gameObject.activeSelf) // 혹시 이미 닫혔는지 체크
        {
            Debug.Log("자동 5초 후 닫기 실행");
            CloseAll();
        }
    }

    public void OnClickedAds()
    {

        AdsMgr.Instance.ShowAd(AdUnitType.RV, result =>
        {
            if (result)
            {
                gameObject.SetActive(false);
                GatchaManager.Instance.RetryGatchaFromAd();
            }
            else
            {
                // ✅ 광고 강제 종료 시에도 가챠 종료
                gameObject.SetActive(false);
                GatchaManager.Instance.gatchaHead.EndGatcha();
            }
        });
    }
    //public void OnClickedAds()
    //{
    //    AdsMgr.Instance.ShowAd(AdUnitType.RV, result =>
    //{
    //        if (result)
    //        {
    //        GatchaManager.Instance.StartGacha();
    //        gameObject.SetActive(false);
    //    }
    //});

    //}
    public  void CloseAll()
    {
        gameObject.SetActive(false);

        if (GatchaManager.Instance != null && GatchaManager.Instance.gachaCanvas != null)
        {
            GatchaManager.Instance.gachaCanvas.SetActive(false);
        }

        GatchaManager.Instance.gatchaHead.EndGatcha();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GatchaAdsCanvas 활성화 되는 타이밍 조정
// GatchaManager.Instance.StartGatcha(); 호출 전에 가챠 캔버스 게임 활성화 시키기 

public class GatchaAdsCanvas : MonoBehaviour
{


    public void Awake()
    {
        Debug.Log("awake 호출됨");  // ✅ 이 로그가 콘솔에 나오는지 확인!
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
            OnClickedClose();
        }
    }


    public void OnClickedAds()
    {
        GatchaManager.Instance.gatchaHead.ResetGatcha(); // 기존 가챠볼 제거
        AdsMgr.Instance.ShowAd(AdUnitType.RV, result =>
    {
        if (result)
        {
            GatchaManager.Instance.gatchaHead.hasWatchedAd = true; // ✅ 광고 시청 플래그 설정

            GatchaManager.Instance.StartGacha();
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            GatchaManager.Instance.gatchaHead.EndGatcha(); // 실패 처리
        }
    });


    }

    public void OnClickedClose()
    {
        gameObject.SetActive(false);
        GatchaManager.Instance.gatchaHead.EndGatcha();

    }

}

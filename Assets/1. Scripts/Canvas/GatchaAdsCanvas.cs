using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//GatchaAdsCanvas 활성화 되는 타이밍 조정
// GatchaManager.Instance.StartGatcha(); 호출 전에 가챠 캔버스 게임 활성화 시키기 

public class GatchaAdsCanvas : MonoBehaviour
{

    public TMP_Text countdownText; // 👈 인스펙터에서 할당
    private bool isWatchingAd = false;

    public void Open()
    {
        GatchaManager.Instance.gatchaHead.hasWatchedAd = true; // ✅ 광고 시청 플래그 설정
        gameObject.SetActive(true);
        isWatchingAd = false;
        StartCoroutine(AutoCloseAfterDelay(5f));

    }

    IEnumerator AutoCloseAfterDelay(float seconds)
    {
        //yield return new WaitForSeconds(seconds);
        float remaining = seconds;
        while (remaining > 0)
        {
            if (!isWatchingAd && countdownText != null)
            {
                countdownText.text = $"{Mathf.CeilToInt(remaining)}초 뒤 닫힙니다";
            }

            yield return new WaitForSeconds(1f); // 0.5초 단위로 줄어들게
            remaining -= 1f;
        }

        if (!isWatchingAd && gameObject.activeSelf)
        {
            OnClickedClose();
        }
    }


    public void OnClickedAds()
    {

        isWatchingAd = true;

        GatchaManager.Instance.gatchaHead.ResetGatcha(); // 기존 가챠볼 제거
        AdsMgr.Instance.ShowAd(AdUnitType.RV, result =>
    {
        isWatchingAd = false;

        if (result)
        {
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

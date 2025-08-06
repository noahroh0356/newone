using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TMP_Text를 사용하므로 추가


public class KitchenSpeedUpAdsCanvas : MonoBehaviour
{
    public TMP_Text processText;

    SpeedUpData speedUpData;

    public Bevyer bevyer; // 🔥 Bevyer 오브젝트 직접 Drag & Drop


    public void SetSpeedUpData(SpeedUpData data)
    {
        speedUpData = data;
    }

    public void OnClickedOpen()
    {
        speedUpData = KitchenManager.Instance.GetSpeedUpData();

        if (speedUpData == null)
        {
            //Debug.LogWarning("SpeedUpData가 세팅되지 않았습니다.");
            return;
        }

        processText.text = $"오늘 {speedUpData.max}회 중 {speedUpData.leftover}회 남음";
        gameObject.SetActive(true);
    }
    public void OnClickedAds()
    {
        AdsMgr.Instance.ShowAd(AdUnitType.RV, result =>
        {
            if (result)
            {
                KitchenManager.Instance.TryStartKitchenBuffWithAd();
                gameObject.SetActive(false);
                if (bevyer != null)
                {
                    bevyer.gameObject.SetActive(true); // 비활성화된 상태면 활성화
                    bevyer.Enter();
                }
                else
                {
                    //Debug.LogError("Bevyer가 인스펙터에 연결되지 않았습니다.");
                }
            }

            else
            {
                ToastCanvas.Instance.ShowToast("하루 최대 5번만 이용할 수 있어요!");
                gameObject.SetActive(false);
            }
        });

    }

    IEnumerator WaitAndEnter()
    {
        yield return null; // 한 프레임 대기

        if (Bevyer.Instance != null)
        {
            Bevyer.Instance.Enter();
        }
        else
        {
            //Debug.LogError("Bevyer.Instance가 여전히 null입니다.");
        }
    }



    public void OnClickedClose()
    {
        gameObject.SetActive(false);
    }
}

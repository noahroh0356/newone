using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TipBoxCanvas : MonoBehaviour
{
    //현재 팁 정보 업그레이드 기능

    public Image thumImage;
    public TMP_Text nameText;
    public TMP_Text upgradeText;
    public TMP_Text capacityText;
    public TMP_Text amountText;

    public TMP_Text descriptionText;

    public GameObject upgradeGameobject;

    TipBoxData tipBoxData;

    public void Open(TipBoxData tipBox)
    {


        tipBoxData = tipBox;
        nameText.text = tipBoxData.name;
        descriptionText.text = tipBoxData.description;
        // 다음 팁박스 데이터 가져오기
        TipBoxData nextTipBoxData = null;
        if (!string.IsNullOrEmpty(tipBoxData.nextKey))
        {
            nextTipBoxData = GetComponentInParent<TipBoxManager>().GetTipBoxData(tipBoxData.nextKey);
        }

        // upgradeText에 다음 팁박스의 price를 표시
        if (nextTipBoxData != null)
        {
            upgradeText.text = "업그레이드 비용: " + nextTipBoxData.price.ToString();
            upgradeGameobject.SetActive(true); // 다음 단계가 있으면 업그레이드 버튼 활성화
        }
        else
        {
            upgradeText.text = "최종 단계"; // 다음 단계가 없으면 최종 단계로 표시
            upgradeGameobject.SetActive(false); // 다음 단계가 없으면 업그레이드 버튼 비활성화
        }
        capacityText.text = "팁박스 한도: " + tipBoxData.capacity.ToString();
        Debug.Log("썸네일 스프라이트: " + (tipBoxData.thum == null ? "null" : tipBoxData.thum.name));
        thumImage.sprite = tipBoxData.thum;
        thumImage.SetNativeSize();
        UpdateCanvas();


    }


    public void OnClickedUpgrade()
    {
     TipBoxData nextTipBoxData = GetComponentInParent<TipBoxManager>().GetTipBoxData(tipBoxData.nextKey);

        if (User.Instance.userData.coin < nextTipBoxData.price)
        {
            ToastCanvas.Instance.ShowToast("needarcon");
            return;
        }

        else
        {
            User.Instance.UpgradeTipBox(tipBoxData.key, tipBoxData.nextKey);
            MainQuestManager.Instance.DoQuest(MainQuestType.UpgradeTipBox);

            TipBoxManager.Instance.UpdateTipBoxVisual(nextTipBoxData.key);
            User.Instance.AddCoin(-tipBoxData.price);

            Open(nextTipBoxData);

        }

    }

    public void UpdateCanvas()
    {
        float aconAmount = GetComponentInParent<TipBoxManager>().aconAmount;
        amountText.text = "모인 도토리: " + (aconAmount >= 1f ? Mathf.FloorToInt(aconAmount).ToString() : "0");

        if (!string.IsNullOrEmpty(tipBoxData.nextKey))
        {
            upgradeGameobject.SetActive(true);
        }
        else
        {
            upgradeGameobject.SetActive(false);
        }
    }


    //현재까지 충전된 도토리를 얻는 코드
    public void OnClickedReceive()
    {
        int reward = Mathf.FloorToInt(TipBoxManager.Instance.aconAmount);
        User.Instance.AddCoin(reward);
        TipBoxManager.Instance.ClearTipBox();
        amountText.text = reward >= 1 ? reward.ToString() : "0";
        UpdateCanvas();
    }

    public void OnClickedAd()
    {
        //광고보고 두배로 리워드 받는 기능
        AdsMgr.Instance.ShowAd(AdUnitType.RV, AdResult);                                             
    }
    //광고에 대한 결과
    void AdResult(bool success)
    {
        if (success)
        {
            int reward = (int)TipBoxManager.Instance.aconAmount * 2;
            User.Instance.AddCoin(reward);
            TipBoxManager.Instance.ClearTipBox();
            UpdateCanvas();

        }
    }

}

//nextTipBoxData.price; 보유한 코인 보다 적으면 업그레이드 되도록 하는데에 활용 
//tipBoxData.nextKey
//
//User.Instance.AddFurniture()
//구매 이후 화면 갱신




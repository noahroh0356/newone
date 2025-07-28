using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Helper : MonoBehaviour
{
    public List<RewardData> rewardDatas = new List<RewardData>();

    public void CheckShowUp()
    {
        //마지막 접속 시, 현재 시간
        //마지막 접속 시간은 라스트 타임 변수에 담기
        string lasTimeStr = PlayerPrefs.GetString("HelperLastTime", null);

        if (!string.IsNullOrEmpty(lasTimeStr))
        {
            DateTime lastTime = DateTime.Parse(lasTimeStr);

            if (DateTime.Now.Date != lastTime.Date && DateTime.Now.Hour >= 9)
            {
                //랜덤 선물, 랜덤 대사
            } 

        }
        else
        {
            //첫플레이
        }
        
    }

    void ShowUp()
    {
        Debug.Log("헬퍼다람쥐 등장");
        //**랜덤 하게 가져오는 기능 추가 
        if (rewardDatas[0].key == "coin")
        {
            User.Instance.AddCoin(rewardDatas[0].count);
        }
    }

}

[System.Serializable]
public class RewardData
{
    public string key;
    public int count;
}
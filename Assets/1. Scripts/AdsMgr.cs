using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdsMgr : MonoBehaviour
{

    public static AdsMgr Instance;
    private void Awake()
    {
        Instance = this;
    }

    public AdMob adMob;

    private void Start()
    {
        adMob.Init();
    }

    public void ShowAd(AdUnitType adUnitType, Action<bool> callback) //action<bool> callback에는 팁박스 캔버스 온클릭애드 애드리절트가 담겨있음
    {
        adMob.ShowAd(adUnitType, callback);
    }
}

public enum AdUnitType
{
    RV, //리워드
    IS, //전면
    BN //배너 
}
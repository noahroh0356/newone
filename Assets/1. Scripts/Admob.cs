//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GoogleMobileAds.Api;
//using System;

//public class AdMob : MonoBehaviour
//{
//    //public void Init()
//    //{
//    //    MobileAds.RaiseAdEventsOnUnityMainThread = true;
//    //    MobileAds.Initialize((InitializationStatus initStatus) => {
//    //        Debug.Log("AdMob 초기화 완료");

//    //        LoadAd(AdUnitType.IS);
//    //        LoadAd(AdUnitType.RV);


//    //    });
//    //}

//    // These ad units are configured to always serve test ads.
//#if UNITY_IOS
//  private string ISUnitId = "[]"; //전면 광고 iOS id
//#elif UNITY_ANDROID
//  private string ISUnitId = ""; //전면 광고 Android id
//#else
//    private string ISUnitId = "ca-app-pub-3940256099942544/3419835294"; //테스트용 광고 ID
//#endif

//#if UNITY_IOS
//  private string RVUnitId = "ca-app-pub-2619060223794940/7440637554"; //리워드 광고 테스트 id **진짜 ca-app-pub-2619060223794940/7440637554
//#elif UNITY_ANDROID
//    private string RVUnitId = "ca-app-pub-3940256099942544/5224354917"; //리워드 광고 테스트 id
//#else
//    private string RVUnitId = "ca-app-pub-3940256099942544/1712485313";
//#endif


//    //private InterstitialAd interstitialAd;
//    //private RewardedAd rewardedAd;


//    //광고 로드
//    public void LoadAd(AdUnitType adUnitType)
//    {
//        switch (adUnitType)
//        {
//            case AdUnitType.IS:

//                // 기존 로드된 전면 광고 제거하기
//                if (interstitialAd != null)
//                {
//                    interstitialAd.Destroy();
//                    interstitialAd = null;
//                }

//                Debug.Log("Loading the interstitial ad.");

//                // create our request used to load the ad.
//                var adRequest = new AdRequest();



//                //// send the request to load the ad.
//                InterstitialAd.Load(ISUnitId, adRequest,
//                    (InterstitialAd ad, LoadAdError error) =>
//                    {
//                        if (error != null || ad == null)
//                        {
//                            Debug.LogError("전면 광고 로드 실패 : " + error);
//                            return;
//                        }

//                        Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());

//                        interstitialAd = ad;
//                        RegisterISEventHandlers(interstitialAd);
//                    });
//                break;

                

//            case AdUnitType.RV:
//                // Clean up the old ad before loading a new one.
//                if (rewardedAd != null)
//                {
//                    rewardedAd.Destroy();
//                    rewardedAd = null;
//                }

//                // create our request used to load the ad.
//                adRequest = new AdRequest();

//                // send the request to load the ad.
//                RewardedAd.Load(RVUnitId, adRequest,
//                    (RewardedAd ad, LoadAdError error) =>
//                    {
//                        // if error is not null, the load request failed.
//                        if (error != null || ad == null)
//                        {
//                            Debug.LogError("리워드 광고 로드 실패 : " + error);
//                            return;
//                        }

//                        Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
//                        rewardedAd = ad;
//                        RegisterRVEventHandlers(rewardedAd);
//                    });
//                break;
//        }

//    }

//    private void RegisterRVEventHandlers(RewardedAd ad)
//    {
//        // 전면 광고가 시작 시
//        ad.OnAdFullScreenContentOpened += () =>
//        {
//            Debug.Log("리워드 광고 시작");
//        };
//        // 전면 광고가 닫혔을 때
//        ad.OnAdFullScreenContentClosed += () =>
//        {
//            Debug.Log("리워드 광고가 닫힘");
//            endCallback?.Invoke(true);
//            endCallback = null;
//            LoadAd(AdUnitType.RV);
//        };
//        // 전면 광고 노출 실패 시
//        ad.OnAdFullScreenContentFailed += (AdError error) =>
//        {
//            Debug.LogError(" 전면 광고 노출 실패 " + "with error : " + error);
//        };
//    }

//    Action<bool> endCallback;
//    //광고 송출
//    public void ShowAd(AdUnitType adUnitType, Action<bool> callback)
//    {
//        endCallback = callback;
//        bool showedAd = false;
//        switch (adUnitType)
//        {
//            case AdUnitType.IS:
//                if (interstitialAd != null && interstitialAd.CanShowAd())
//                {
//                    showedAd = true;
//                    interstitialAd.Show();
//                }
//                break;

//            case AdUnitType.RV:
//                if (rewardedAd != null && rewardedAd.CanShowAd())
//                {
//                    showedAd = true;
//                    rewardedAd.Show((Reward reward) =>  //Reward 객체는 리워드 광고 단위 설정할 때 설정했던 정보가 들어옵니다. 
//                    {
//                        endCallback?.Invoke(true); //★★리워드 제공해야함★★
//                        endCallback = null;
//                    });
//                }
//                break;
//        }

//        if (!showedAd)
//        {
//            endCallback?.Invoke(false);
//            endCallback = null;
//            LoadAd(adUnitType);
//        }
//    }

//    //전면 광고 이벤트 핸들러 등록
//    private void RegisterISEventHandlers(InterstitialAd ad)
//    {
//        // 전면 광고가 시작 시
//        ad.OnAdFullScreenContentOpened += () =>
//        {
//            Debug.Log("전면 광고 시작");
//        };
//        // 전면 광고가 닫혔을 때
//        ad.OnAdFullScreenContentClosed += () =>
//        {
//            Debug.Log("전면 광고가 닫힘");
//            endCallback?.Invoke(true);
//            endCallback = null;
//            LoadAd(AdUnitType.IS);
//        };
//        // 전면 광고 노출 실패 시
//        ad.OnAdFullScreenContentFailed += (AdError error) =>
//        {
//            Debug.LogError(" 전면 광고 노출 실패 " + "with error : " + error);
//        };
//    }
//}
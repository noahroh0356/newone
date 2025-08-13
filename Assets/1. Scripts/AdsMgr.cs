using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsMgr : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsMgr Instance;

    [Header("Unity Ads IDs")]
    [SerializeField] string _androidGameId = "5921988";
    [SerializeField] string _iosGameId = "5921989";
    [SerializeField] string _androidRVId = "Rewarded_Android";
    [SerializeField] string _iosRVId = "Rewarded_iOS";
    [SerializeField] string _androidISId = "Interstitial_Android";
    [SerializeField] string _iosISId = "Interstitial_iOS";
    [SerializeField] string _androidBNId = "Banner_Android";
    [SerializeField] string _iosBNId = "Banner_iOS";

    private string _gameId;
    private string _rvAdId;
    private string _isAdId;
    private string _bnAdId;

    private bool _initialized;

    // 로드/요청 상태 플래그
    private bool _rvLoaded, _isLoaded;
    private bool _wantShowRV, _wantShowIS;

    private Action<bool> _rewardCallback;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeAds();
        LoadBannerAd(); // 배너는 초기화 직후 자동 로드/표시 시도
    }

    public void InitializeAds()
    {
#if UNITY_IOS
        _gameId = _iosGameId;
        _rvAdId = _iosRVId;
        _isAdId = _iosISId;
        _bnAdId = _iosBNId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
        _rvAdId = _androidRVId;
        _isAdId = _androidISId;
        _bnAdId = _androidBNId;
#elif UNITY_EDITOR
        // 에디터에서는 Android 설정을 사용
        _gameId = _androidGameId;
        _rvAdId = _androidRVId;
        _isAdId = _androidISId;
        _bnAdId = _androidBNId;
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, true, this); // true = Test Mode
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        _initialized = true;

        // 초기 로드만 하고, Show는 사용자 요청 시에만 실행
        LoadRewardedAd();
        LoadInterstitialAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }

    // --- Public API ---
    public void ShowAd(AdUnitType adUnitType, Action<bool> callback)
    {
        if (!_initialized)
        {
            Debug.LogWarning("Unity Ads not initialized yet.");
#if UNITY_EDITOR
            // 에디터 디버그 편의: 초기화 전에는 보상 시뮬레이션
            if (adUnitType == AdUnitType.RV) callback?.Invoke(true);
#endif
            return;
        }

        switch (adUnitType)
        {
            case AdUnitType.RV:
                _rewardCallback = callback;
                _wantShowRV = true;
                if (_rvLoaded)
                {
                    Advertisement.Show(_rvAdId, this);
                }
                else
                {
                    Advertisement.Load(_rvAdId, this);
                }
                break;

            case AdUnitType.IS:
                _wantShowIS = true;
                if (_isLoaded)
                {
                    Advertisement.Show(_isAdId, this);
                }
                else
                {
                    Advertisement.Load(_isAdId, this);
                }
                break;

            case AdUnitType.BN:
                // 배너는 LoadBannerAd에서 자동 Show
                break;
        }
    }

    // --- Load helpers ---
    private void LoadRewardedAd()
    {
        _rvLoaded = false;
        Advertisement.Load(_rvAdId, this);
    }

    private void LoadInterstitialAd()
    {
        _isLoaded = false;
        Advertisement.Load(_isAdId, this);
    }

    private void LoadBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(_bnAdId, new BannerLoadOptions
        {
            loadCallback = () =>
            {
                Debug.Log("Banner ad loaded.");
                Advertisement.Banner.Show(_bnAdId);
            },
            errorCallback = (message) =>
            {
                Debug.Log($"Banner Load Error: {message}");
            }
        });
    }

    // --- IUnityAdsLoadListener ---
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log($"Ad Loaded: {adUnitId}");

        if (adUnitId.Equals(_rvAdId))
        {
            _rvLoaded = true;
            if (_wantShowRV)
            {
                Advertisement.Show(_rvAdId, this);
            }
        }
        else if (adUnitId.Equals(_isAdId))
        {
            _isLoaded = true;
            if (_wantShowIS)
            {
                Advertisement.Show(_isAdId, this);
            }
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error} - {message}");
#if UNITY_EDITOR
        // 에디터 디버그 편의: 로드 실패 시 리워드 보상 시뮬레이션
        if (adUnitId.Equals(_rvAdId) && _wantShowRV)
        {
            _wantShowRV = false;
            _rewardCallback?.Invoke(true);
        }
#endif
    }

    // --- IUnityAdsShowListener ---
    public void OnUnityAdsShowStart(string adUnitId)
    {
        if (adUnitId.Equals(_rvAdId))
        {
            _rvLoaded = false;
            _wantShowRV = false;
        }
        else if (adUnitId.Equals(_isAdId))
        {
            _isLoaded = false;
            _wantShowIS = false;
        }
    }

    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_rvAdId))
        {
            bool rewarded = (showCompletionState == UnityAdsShowCompletionState.COMPLETED);
            _rewardCallback?.Invoke(rewarded);
            LoadRewardedAd(); // 다음 광고 미리 로드
        }
        else if (adUnitId.Equals(_isAdId))
        {
            LoadInterstitialAd();
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error} - {message}");
        if (adUnitId.Equals(_rvAdId))
        {
            _rewardCallback?.Invoke(false);
            LoadRewardedAd();
        }
        else if (adUnitId.Equals(_isAdId))
        {
            LoadInterstitialAd();
        }
    }
}

public enum AdUnitType
{
    RV, // 리워드
    IS, // 전면
    BN  // 배너
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//public class AdsMgr : MonoBehaviour
//{

//    public static AdsMgr Instance;
//    private void Awake()
//    {
//        Instance = this;
//    }

//    //public AdMob adMob;

//    private void Start()
//    {
//        //adMob.Init();
//    }

//    public void ShowAd(AdUnitType adUnitType, Action<bool> callback) //action<bool> callback에는 팁박스 캔버스 온클릭애드 애드리절트가 담겨있음
//    {
//        //adMob.ShowAd(adUnitType, callback);
//    }
//}

//public enum AdUnitType
//{
//    RV, //리워드
//    IS, //전면
//    BN //배너 
//}
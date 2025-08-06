using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Boomlagoon.JSON;

public class KitchenManager : MonoBehaviour
{
    public static KitchenManager Instance;
    public KitchenBarPlace[] kitchenBarPlaces;
    public KitchenDetail[] kitchenDetail;

    public Button kitchenButton;
    public Transform center;


    public KitchenData[] kitchenData;


    public float speedUp = 1;


    bool hasPurchasedKitchenBar = false; // 구매한 키친바가 있는지 여부

    // 하루에 다섯 번만
    // 하루가 지났는지, 몇번 봤는지
    public void StartKichenBuff()
    {
        speedUp = 3;
        StartCoroutine(KitchenBuffTimer());
    }

    private IEnumerator KitchenBuffTimer()
    {
        yield return new WaitForSeconds(100f);
        EndKichenBuff();
    }
    public void EndKichenBuff()
    {
        speedUp = 1;
    }
    private void Awake()
    {
        Instance = this;

        TextAsset textAsset = Resources.Load<TextAsset>("Json/KitchenDetail");
        JSONObject jsonObj = JSONObject.Parse(textAsset.text);
        JSONArray jArr = jsonObj.GetArray("JSON");
        kitchenDetail = new KitchenDetail[jArr.Length];

        for (int i = 0; i < jArr.Length; i++)
        {
            kitchenDetail[i] = new KitchenDetail();
            kitchenDetail[i].kitchenBarKey = jArr[i].Obj.GetString("kitchenBarKey");
            kitchenDetail[i].name = jArr[i].Obj.GetString("name");
            kitchenDetail[i].description = jArr[i].Obj.GetString("description");
            float.TryParse(jArr[i].Obj.GetString("reduceMakingTime"), out kitchenDetail[i].reduceMakingTime);
            //kitchenDetail[i].autoSpawnAcon = int.Parse(jArr[i].Obj.GetString("autoSpawnAcon"));
            //kitchenDetail[i].autoSpawnSec = float.Parse(jArr[i].Obj.GetString("autoSpawnSec"));

            string thumbnailFileName = jArr[i].Obj.GetString("kitchenBarKey"); // tableKey에서 파일 이름을 가져옵니다.

            kitchenDetail[i].thum = Resources.Load<Sprite>("Thumbnails/" + thumbnailFileName);


        }
    }

    public void Start()
    {
        string lasTimeStr = PlayerPrefs.GetString("KitchenBuffLastTime", null);
        int watchCount = PlayerPrefs.GetInt("KitchenBuffCount", 0);

        //lasTimeStr = null일때
        // if (DateTime.Now.Date != lastTime.Date) ㅏ참일때 기회가 5!
        // n번 시청한 횟수가 저정되어 있어야됨 PlayerPrefs.SetString 이 함수로 처리 해야됨
        if (string.IsNullOrEmpty(lasTimeStr) == false)
        {
          DateTime lastTime  =  DateTime.Parse(lasTimeStr);
            if (DateTime.Now.Date != lastTime.Date)
            {
                watchCount = 0;
                PlayerPrefs.SetInt("KitchenBuffCount", 0);
            }
        }
        PlayerPrefs.SetString("KitchenBuffLastTime", DateTime.Now.ToString());
        UpdateKitchen();
    }

    public void TryStartKitchenBuffWithAd()
    {
        if (IsAvailableToWatchAd())
        {
            StartKichenBuff();
            IncrementKitchenBuffCount();
        }
        else
        {
            ToastCanvas.Instance.ShowToast("오늘은 더 이상 버프를 받을 수 없어요!");
        }
    }

    // 오늘 광고 시청 가능 여부 확인
    public bool IsAvailableToWatchAd()
    {
        int watchCount = PlayerPrefs.GetInt("KitchenBuffCount", 0);
        return watchCount < 5;
    }

    // 시청 횟수 1 증가
    public void IncrementKitchenBuffCount()
    {
        int watchCount = PlayerPrefs.GetInt("KitchenBuffCount", 0);
        watchCount++;
        PlayerPrefs.SetInt("KitchenBuffCount", watchCount);
    }



    public void StartArea()
    {
        kitchenButton.gameObject.SetActive(false);
    }


    public void EndArea()
    {
        kitchenButton.gameObject.SetActive(true);
    }

    public void Order(MenuData orderMenu, Customer customer)
    {
        waitingOrderMenus.Add(orderMenu);
        waitingCustomers.Add(customer);

        MatchOrder();
    }


    public KitchenBarPlace GetAvailableKitchenBarPlace()

    {
        KitchenBarPlace kitchenBarPlace = null;

        for (int i = 0; i < kitchenBarPlaces.Length; i++)
        {
            if (kitchenBarPlaces[i].curKitchenBar != null && kitchenBarPlaces[i].making == false)
            {
                kitchenBarPlace = kitchenBarPlaces[i];
                break;
            }
        }

        return kitchenBarPlace;
    }

    List<MenuData> waitingOrderMenus = new List<MenuData>();
    List<Customer> waitingCustomers = new List<Customer>();
    public List<UserKitchen> userKitchenList = new List<UserKitchen>();


    public void MatchOrder()
    {
        if (waitingOrderMenus.Count <= 0)
            return;

        bool hasAnyKitchenBar = User.Instance.userData.userKitchenList.Count > 0; // ✅ User의 키친바 리스트 사용

        foreach (var userKitchen in User.Instance.userData.userKitchenList)
        {
            if (userKitchen.purchased)
            {
                hasPurchasedKitchenBar = true;
                break; // 하나라도 구매했으면 true로 바꾸고 루프 종료
            }
        }

        if (!hasPurchasedKitchenBar) // 구매한 키친바가 없을 경우
        {
            if (waitingCustomers.Count > 0)
            {
                ToastCanvas.Instance.ShowToast("needdecenter");

                //ToastCanvas.Instance.ShowToast("[상점]에서 디캔더를 사야해요!");
                StartCoroutine(waitingCustomers[0].StartExitMove(2));
                waitingCustomers.RemoveAt(0);

                if (waitingOrderMenus.Count > 0)
                {
                    waitingOrderMenus.RemoveAt(0);
                }
            }
            //if (!hasAnyKitchenBar) // ❌ 유저가 키친바가 없을 경우
            //{
            //    if (waitingCustomers.Count > 0)
            //    {
            //        Debug.Log("키친바 없음 -> 손님 퇴장");
            //        StartCoroutine(waitingCustomers[0].StartExitMove(2));
            //        waitingCustomers.RemoveAt(0);

            //        if (waitingOrderMenus.Count > 0)
            //        {
            //            waitingOrderMenus.RemoveAt(0);
            //        }
            //    }
            return;
        }

        KitchenBarPlace barPlace = GetAvailableKitchenBarPlace();
        if (barPlace == null)
        {
            Debug.Log("키친바가 있지만 모두 사용 중 -> 대기 유지");
            return;
        }
        MainQuestManager.Instance.DoQuest(MainQuestType.TakeOrder);//** 퀘스트
        barPlace.StartMake(waitingOrderMenus[0], waitingCustomers[0]);
        waitingOrderMenus.RemoveAt(0);
        waitingCustomers.RemoveAt(0);

        MatchOrder();
    }


    //주방에 대한 시설 업데이트 필요시 호출 
    public void UpdateKitchen()
    {
        //모든 가구를 업데이트
        for (int i = 0; i < kitchenBarPlaces.Length; i++)
        {
            kitchenBarPlaces[i].UpdateKitchenBarPlace();
            //Debug.Log("키친바업데이트");
                }
        // 테이블이 추가될때 settarget
    }



    public KitchenData GetKitchenData(string key)
    {
        for (int i = 0; i < kitchenData.Length; i++)

        {
            if (kitchenData[i].key == key)
            {
                return kitchenData[i];
            }
        }
        return null;

    }

    public void PurchaseKitchenBar(string key)
    {
        KitchenData kitchenData = GetKitchenData(key);
        if (kitchenData != null)
        {
            kitchenData.purchased = true;
        }
    }

    public void CancelOrder(Customer customer)
    {
        int index = waitingCustomers.IndexOf(customer);

        if (index >= 0)
        {
            Debug.Log($"[주문취소] {customer.name}의 주문이 취소되었습니다.");

            waitingCustomers.RemoveAt(index);
            if (index < waitingOrderMenus.Count)
            {
                waitingOrderMenus.RemoveAt(index);
            }
        }
    }

    public KitchenDetail GetKitchenDetail(string kitchenBarKey)
    {

        for (int i = 0; i < kitchenDetail.Length; i++)

        {
            if (kitchenDetail[i].kitchenBarKey == kitchenBarKey)
            {

                return kitchenDetail[i];
            }
        }
        return null;

    }

    public SpeedUpData GetSpeedUpData()
    {
        SpeedUpData data = new SpeedUpData();
        data.max = 5; // 하루 최대 5회

        int watchCount = PlayerPrefs.GetInt("KitchenBuffCount", 0);
        data.leftover = Mathf.Clamp(data.max - watchCount, 0, data.max);

        return data;
    }
}



[System.Serializable]
public class KitchenData
{
    public string key;
    public string kitchenBarKey;
    public string nextProductKey;
    public Sprite thum;
    public string name;
    public int abilityLv;

    public KitchenPlaceType kitchenPlaceType;
    public int price;
    public bool purchased; // 초기값 false

}

[System.Serializable]
public class KitchenDetail
{

    public string kitchenBarKey;
    public string name;
    public string description;
    public float reduceMakingTime; // 디캔더 제작 속도 증가율 % 단위로 적용

    public Sprite thum;

}

public enum KitchenPlaceType
{
    KitchenBar_0,
    KitchenBar_1,
    KitchenBar_2,
    KitchenBar_3,
    KitchenBar_4,
    KitchenBar_5,
}

[System.Serializable]
public class SpeedUpData
{
    public int max;
    public int leftover;

}

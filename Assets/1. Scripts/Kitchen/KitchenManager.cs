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

    bool hasPurchasedKitchenBar = false; // 구매한 키친바가 있는지 여부

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
        UpdateKitchen();
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
            Debug.Log("키친바업데이트");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class User : MonoBehaviour
{
    public static User Instance;
    public UserData userData;

    public UserFurniture GetSetUpFurniture(FurniturePlace furniturePlace)
    {

        for (int i = 0; i < userData.userFurnitureList.Count; i++)
        {
            if (userData.userFurnitureList[i].setup == false)
                continue;

            FurnitureData furnitureData = FurnitureManager.Instance.GetFurnitureData(userData.userFurnitureList[i].furniturekey);
            if (furnitureData == null) // ⭐ 이 부분이 핵심적인 안전 장치
            {
                Debug.LogError($"[NullRef] FurnitureData not found for key: {userData.userFurnitureList[i].furniturekey}. Please check your FurnitureManager data or saved UserData for invalid keys.");
                continue; // 해당 가구 키가 유효하지 않으면 다음 항목으로 넘어갑니다.
            }

            if (furnitureData.furniturePlace == furniturePlace)
            {
                return userData.userFurnitureList[i];
            }
        }
        return null;
        //for (int i = 0; i < userData.userFurnitureList.Count; i++)
        //{
        //    if (userData.userFurnitureList[i].setup == false)
        //        continue;

        //    FurnitureData furnitureData = FurnitureManager.Instance.GetFurnitureData(userData.userFurnitureList[i].furniturekey);

        //    if (furnitureData.furniturePlace == furniturePlace)
        //    {
        //        return userData.userFurnitureList[i];

        //    }
        //}
        //return null;

    }

    public UserKitchen GetSetUpKitchen(KitchenPlaceType kitchenPlaceType)
    {

        for (int i = 0; i < userData.userKitchenList.Count; i++)
        {
            if (userData.userKitchenList[i].setup == false)
                continue;
            KitchenData kitchenData = KitchenManager.Instance.GetKitchenData(userData.userKitchenList[i].kitchenkey);

            if (kitchenData.kitchenPlaceType == kitchenPlaceType)
            {
                return userData.userKitchenList[i];
            }
        }
        return null;

    }

    public List<UserFox> GetSetUpFox()
    {
        return userData.userFoxes; // 모든 UserFox 리스트를 반환하거나, 필요한 조건에 따라 필터링하여 반환
    }

    public List<UserWine> GetSetUpWine()
    {
        return userData.userWines; // 모든 UserFox 리스트를 반환하거나, 필요한 조건에 따라 필터링하여 반환
    }


    public List<UserCustomer> GetSetUpCustomer()
    {
        return userData.userCustomers; // 모든 Usercustomer 리스트를 반환하거나, 필요한 조건에 따라 필터링하여 반환
    }

    public void AddExp(int e)
    {
        userData.exp += e;
        if (userData.exp >= MaxExp(userData.level))
        {
            LevelUp();
        }

    }

    int MaxExp(int lv)
    {
        return 15 + lv * 3;
    }

    void LevelUp()
    {
        userData.level++;
        userData.exp = 0;
        SaveMgr.SaveData("UserData", userData);
    }


    private void Awake()
    {
        Instance = this;
        userData = SaveMgr.LoadData<UserData>("UserData");

        if (userData == null) // 신규유저
        {
            PlayerPrefs.DeleteAll();
            userData = new UserData();
            userData.coin = 0;
            userData.gatchaCoin = 0;

            userData.userCustomers.Clear();
            UserCustomer defaultCustomer = new UserCustomer();
            defaultCustomer.key = "Racoon1"; // 기본 고객의 키 설정
            defaultCustomer.open = true; // 기본 고객은 항상 열려있음
            userData.userCustomers.Add(defaultCustomer);
            Debug.Log("디폴트손님추가");

            userData.userWines.Clear();
            UserWine defaultWine = new UserWine();
            defaultWine.key = "wine_0";
            defaultWine.open = true;
            userData.userWines.Add(defaultWine);

            Debug.Log("디폴트와인추가");


            userData.userFurnitureList.Clear();
            UserFurniture defaultFurniture = new UserFurniture();
            defaultFurniture.furniturekey = "tipbox_0";
            defaultFurniture.purchased = true;
            userData.userFurnitureList.Add(defaultFurniture);
            defaultFurniture.setup = true;

            Debug.Log("디폴트팁박추가");


            SaveMgr.SaveData<UserData>("UserData", userData);
        }


    }



    //void Start()
    //{
    //    //userData = new UserData();

    //    //userData.coin = PlayerPrefs.GetInt("Coin", 0);        
    //    //UpdateCoinText();
    //    //FindObjectOfType<RestaurantManager>();

    //}

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    AddCoin(10);
        //}
    }

    public void UpgradeTipBox(string oldKey, string newKey)
    {

        UserFurniture currentTipBox = null;

        // 1. 세팅된 기존 팁박스를 찾음
        for (int i = 0; i < userData.userFurnitureList.Count; i++)
        {
            UserFurniture furniture = userData.userFurnitureList[i];
            if (furniture.furniturekey == oldKey && furniture.setup == true)
            {
                currentTipBox = furniture;
                currentTipBox.setup = false;
                break;
            }
        }

            UserFurniture newFurniture = GetUserFurniture(newKey);
    newFurniture.purchased = true;
    newFurniture.setup = true;

        SaveMgr.SaveData("UserData", userData);

    }


    public void AddFurniture(string key)
    {
        FurnitureData data = FurnitureManager.Instance.GetFurnitureData(key);
        UserFurniture setupedFurniture = GetSetUpFurniture(data.furniturePlace);
        if (setupedFurniture != null)
        {
            setupedFurniture.setup = false;
        }


        UserFurniture userFurniture = GetUserFurniture(key);
        userFurniture.purchased = true;
        userFurniture.setup = true;

        SaveMgr.SaveData("UserData", userData);
    }

    public void AddKitchenFurniture(string key)
    {
        KitchenData data = KitchenManager.Instance.GetKitchenData(key);
        UserKitchen setupedKitchen = GetSetUpKitchen(data.kitchenPlaceType);
        if (setupedKitchen != null)
        {
            setupedKitchen.setup = false;
        }

        UserKitchen userKitchen = GetUserKitchen(key);
        userKitchen.purchased = true;
        userKitchen.setup = true;

        SaveMgr.SaveData("UserData", userData);

    }

    public void AddCustomer(string key)
    {
        UserCustomer userCustomer = GetUserCustomer(key);
        if (userCustomer == null) // 이미 등록된 손님인지 확인
        {
            userCustomer = new UserCustomer();
            userCustomer.key = key;
            userCustomer.open = true; // 또는 획득 여부를 나타내는 다른 상태로 설정
            userData.userCustomers.Add(userCustomer);
        }

        SaveMgr.SaveData<UserData>("UserData", userData); // 데이터 저장

    }


    public void AddFox(string key)
    {
        UserFox userFox = GetUserFox(key);
        if (userFox == null) // 이미 등록된 손님인지 확인
        {
            userFox = new UserFox();
            userFox.key = key;
            userFox.purchased = true; // 또는 획득 여부를 나타내는 다른 상태로 설정
            userData.userFoxes.Add(userFox);
        }
        SaveMgr.SaveData<UserData>("UserData", userData); // 데이터 저장

    }

    public void AddWine(string key)
    {
        UserWine userWine = GetUserWine(key);
        if (userWine == null)
        {
            userWine = new UserWine();
            userWine.key = key;
            userWine.open = true; // 또는 획득 여부를 나타내는 다른 상태로 설정
            userData.userWines.Add(userWine);
        }
        SaveMgr.SaveData<UserData>("UserData", userData); // 데이터 저장

    }

    public UserFurniture GetUserFurniture(string key)
    {
        for (int i = 0; i < userData.userFurnitureList.Count; i++)
        {
            if (userData.userFurnitureList[i].furniturekey == key)
            {
                return userData.userFurnitureList[i];
            }
        }
        UserFurniture userFurniture = new UserFurniture();
        userFurniture.furniturekey = key;
        userFurniture.purchased = false;
        userData.userFurnitureList.Add(userFurniture);
        return userFurniture;
    }

    public UserKitchen GetUserKitchen(string key)
    {
        for (int i = 0; i < userData.userKitchenList.Count; i++)
        {
            if (userData.userKitchenList[i].kitchenkey == key)
            {
                return userData.userKitchenList[i];
            }
        }
        UserKitchen userKitchen = new UserKitchen();
        userKitchen.kitchenkey = key;
        userKitchen.purchased = false;
        userData.userKitchenList.Add(userKitchen);
        return userKitchen;
    }

    public UserCustomer GetUserCustomer(string key)
    {
        for (int i = 0; i < userData.userCustomers.Count; i++)
        {
            if (userData.userCustomers[i].key == key)
            {
                return userData.userCustomers[i];
            }
        }
        return null;
    }


    public UserFox GetUserFox(string key)
    {
        for (int i = 0; i < userData.userFoxes.Count; i++)
        {
            if (userData.userFoxes[i].key == key)
            {
                return userData.userFoxes[i];
            }
        }
        return null;
    }

    public UserWine GetUserWine(string key)
    {
        for (int i = 0; i < userData.userWines.Count; i++)
        {
            if (userData.userWines[i].key == key)
            {
                return userData.userWines[i];
            }
        }
        return null;
    }


    public void AddCoin(int c)
    {
        userData.coin += c;
        //UpdateCoinText();
        //Debug.Log("coinText.text 값: " + coinText.text);
        SaveMgr.SaveData<UserData>("UserData", userData);

        CoinBoard.Instance.AddedCoin(+c); // 싱글톤 접근

    }


    public void AddRuby(int r)
    {
        userData.ruby += r;
        SaveMgr.SaveData<UserData>("UserData", userData);

    }

    public void AddGatchaCoin(int c)
    {
        userData.gatchaCoin += c;
        SaveMgr.SaveData<UserData>("UserData", userData);

    }


    public bool useGatchaCoin()
    {
        if (userData.gatchaCoin > 0)
        {
            userData.gatchaCoin--;
            return true;
        }
        else
        {
            return false;
        }
    }
}


    [System.Serializable]
    public class UserData
    {

        public int level; // 레벨 
        public int exp; //경험치 - 메인 퀘스트를 통해서 획득
        public int coin; //acon으로 변경?
        public int ruby;
        public int gatchaCoin;
        public List<UserFurniture> userFurnitureList = new List<UserFurniture>();
        public List<UserKitchen> userKitchenList = new List<UserKitchen>();
        public List<UserCustomer> userCustomers = new List<UserCustomer>();
        public List<UserFox> userFoxes = new List<UserFox>();
        public List<UserWine> userWines = new List<UserWine>();

    }

    [System.Serializable]
    public class UserFurniture
    {
        public string furniturekey;
        public bool purchased; // 초기값 false
        public bool setup;// 초기값 false
    }

    [System.Serializable]

    public class UserKitchen
    {
        public string kitchenkey;
        public bool purchased; // 초기값 false
        public bool setup;// 초기값 false


    }

    [System.Serializable]

    public class UserCustomer
    {
        public string key;
        public bool open;

    }

    [System.Serializable]

    public class UserFox
    {
        public string key;
        public bool purchased;

    }


    [System.Serializable]

    public class UserWine
    {
        public string key;
        public bool open;

}


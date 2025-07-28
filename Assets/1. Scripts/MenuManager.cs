using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{

    public MenuData[] menuDatas;
    public static MenuManager Instance;
    public UserData userData;

    void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        userData = User.Instance.userData;


        menuDatas = new MenuData[12];

        menuDatas[0] = new MenuData();
        menuDatas[0].key = "wine_0";
        menuDatas[0].price = 10;
        menuDatas[0].makingTime = 10;
        menuDatas[0].menuOrderImage = Resources.Load<Sprite>("Images/wine_0_1");
        menuDatas[0].menuImage = Resources.Load<Sprite>("Images/wine_0");

        menuDatas[1] = new MenuData();
        menuDatas[1].key = "wine_1";
        menuDatas[1].price = 12;
        menuDatas[1].makingTime = 12;
        menuDatas[1].menuOrderImage = Resources.Load<Sprite>("Images/wine_1_1");
        menuDatas[1].menuImage = Resources.Load<Sprite>("Images/wine_1");

        menuDatas[2] = new MenuData();
        menuDatas[2].key = "wine_2";
        menuDatas[2].price = 14;
        menuDatas[2].makingTime = 13;
        menuDatas[2].menuOrderImage = Resources.Load<Sprite>("Images/wine_2_1");
        menuDatas[2].menuImage = Resources.Load<Sprite>("Images/wine_2");


        menuDatas[3] = new MenuData();
        menuDatas[3].key = "wine_3";
        menuDatas[3].price = 20;
        menuDatas[3].makingTime = 13;
        menuDatas[3].menuOrderImage = Resources.Load<Sprite>("Images/wine_3_1");
        menuDatas[3].menuImage = Resources.Load<Sprite>("Images/wine_3");

        menuDatas[4] = new MenuData();
        menuDatas[4].key = "wine_4";
        menuDatas[4].price = 22;
        menuDatas[4].makingTime = 8;
        menuDatas[4].menuOrderImage = Resources.Load<Sprite>("Images/wine_4_1");
        menuDatas[4].menuImage = Resources.Load<Sprite>("Images/wine_4");

        menuDatas[5] = new MenuData();
        menuDatas[5].key = "wine_5";
        menuDatas[5].price = 25;
        menuDatas[5].makingTime = 12;
        menuDatas[5].menuOrderImage = Resources.Load<Sprite>("Images/wine_5_1");
        menuDatas[5].menuImage = Resources.Load<Sprite>("Images/wine_5");

        menuDatas[6] = new MenuData();
        menuDatas[6].key = "wine_6";
        menuDatas[6].price = 30;
        menuDatas[6].makingTime = 10;
        menuDatas[6].menuOrderImage = Resources.Load<Sprite>("Images/wine_6_1");
        menuDatas[6].menuImage = Resources.Load<Sprite>("Images/wine_6");

        menuDatas[7] = new MenuData();
        menuDatas[7].key = "wine_7";
        menuDatas[7].price = 32;
        menuDatas[7].makingTime = 12;
        menuDatas[7].menuOrderImage = Resources.Load<Sprite>("Images/wine_7_1");
        menuDatas[7].menuImage = Resources.Load<Sprite>("Images/wine_7");

        menuDatas[8] = new MenuData();
        menuDatas[8].key = "wine_8";
        menuDatas[8].price = 35;
        menuDatas[8].makingTime = 13;
        menuDatas[8].menuOrderImage = Resources.Load<Sprite>("Images/wine_8_1");
        menuDatas[8].menuImage = Resources.Load<Sprite>("Images/wine_8");

        menuDatas[9] = new MenuData();
        menuDatas[9].key = "wine_9";
        menuDatas[9].price = 38;
        menuDatas[9].makingTime = 10;
        menuDatas[9].menuOrderImage = Resources.Load<Sprite>("Images/wine_9_1");
        menuDatas[9].menuImage = Resources.Load<Sprite>("Images/wine_9");

        menuDatas[10] = new MenuData();
        menuDatas[10].key = "wine_10";
        menuDatas[10].price = 40;
        menuDatas[10].makingTime = 15;
        menuDatas[10].menuOrderImage = Resources.Load<Sprite>("Images/wine_10_1");
        menuDatas[10].menuImage = Resources.Load<Sprite>("Images/wine_10");

        menuDatas[11] = new MenuData();
        menuDatas[11].key = "wine_11";
        menuDatas[11].price = 45;
        menuDatas[11].makingTime = 12;
        menuDatas[11].menuOrderImage = Resources.Load<Sprite>("Images/wine_11_1");
        menuDatas[11].menuImage = Resources.Load<Sprite>("Images/wine_11");
    }

    public MenuData GetRandomMenuData()
    {
        if (userData.userWines == null || userData.userWines.Count == 0)
        {
            Debug.LogError("사용 가능한 와인 데이터가 없습니다.");
            return null; // 또는 다른 기본 메뉴 데이터 반환
        }

        // 활성화된(open이 true인) 와인만 담을 리스트 생성
        List<UserWine> openWines = new List<UserWine>();
        for (int i = 0; i < userData.userWines.Count; i++)
        {
            if (userData.userWines[i].open)
            {
                openWines.Add(userData.userWines[i]);
            }
        }

        if (openWines.Count == 0)
        {
            Debug.LogError("활성화된 와인 데이터가 없습니다.");
            return null;
        }

        int randomIdx = Random.Range(0, openWines.Count);
        string randomWineKey = openWines[randomIdx].key; // UserWine 객체의 key 값 접근

        // for 문을 사용하여 menuDatas 배열 순회
        for (int i = 0; i < menuDatas.Length; i++)
        {
            if (menuDatas[i].key == randomWineKey)
            {
                return menuDatas[i];
            }
        }

        Debug.LogError($"키 '{randomWineKey}'에 해당하는 메뉴 데이터를 찾을 수 없습니다.");
        return null; // 찾지 못했을 경우
    }
}

[System.Serializable]
public class MenuData
{
    public string key;
    public int price;
    public float makingTime;
    public Sprite menuOrderImage;
    public Sprite menuImage;
}

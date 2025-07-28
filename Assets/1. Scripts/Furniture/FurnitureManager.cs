using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Boomlagoon.JSON;

public class FurnitureManager : MonoBehaviour
{

    //public List<FurnitureData> furnitures = new List<FurnitureData>();

    public FurnitureData[] furnitureDatas;
    public FurnitureDetail[] furnitureDetails;


    public TablePlace[] tablePlaces; // 테이블 위치
    public Customer customer;



    public static FurnitureManager Instance;


    void Awake()
    {
        Instance = this;

        TextAsset textAsset = Resources.Load<TextAsset>("Json/FurnitureData");
        JSONObject jsonObj = JSONObject.Parse(textAsset.text);
        JSONArray jArr = jsonObj.GetArray("JSON");
        furnitureDatas = new FurnitureData[jArr.Length];

        for (int i = 0; i < jArr.Length; i++)
        {
            furnitureDatas[i] = new FurnitureData();
            furnitureDatas[i].key = jArr[i].Obj.GetString("key");
            furnitureDatas[i].nextfurniturekey = jArr[i].Obj.GetString("nextfurniturekey");
            furnitureDatas[i].price = int.Parse(jArr[i].Obj.GetString("price"));
            furnitureDatas[i].tableKey = jArr[i].Obj.GetString("tableKey");
            furnitureDatas[i].furniturePlace = System.Enum.Parse<FurniturePlace>(jArr[i].Obj.GetString("furniturePlace"));
        }

        textAsset = Resources.Load<TextAsset>("Json/FurnitureDetail");
        jsonObj = JSONObject.Parse(textAsset.text); 
        jArr = jsonObj.GetArray("JSON");
        furnitureDetails = new FurnitureDetail[jArr.Length];

        for (int i = 0; i < jArr.Length; i++)
        {
            furnitureDetails[i] = new FurnitureDetail();
            furnitureDetails[i].key = jArr[i].Obj.GetString("key");
            furnitureDetails[i].name = jArr[i].Obj.GetString("name");
            furnitureDetails[i].description = jArr[i].Obj.GetString("description");
            furnitureDetails[i].autoSpawnAcon = int.Parse(jArr[i].Obj.GetString("autoSpawnAcon"));
            furnitureDetails[i].autoSpawnSec = float.Parse(jArr[i].Obj.GetString("autoSpawnSec"));

            string thumbnailFileName = jArr[i].Obj.GetString("key"); // tableKey에서 파일 이름을 가져옵니다.

            furnitureDetails[i].thum = Resources.Load<Sprite>("Thumbnails/" + thumbnailFileName);


        }



    }

void Start()
    {
        tablePlaces = FindObjectsOfType<TablePlace>();
        
    }

    void Update()
    {

    }

    public void UpdateFurniture()
    {
        //모든 가구를 업데이트
        for (int i = 0; i < tablePlaces.Length; i++)
        {
            tablePlaces[i].UpdateTablePlace();
        }
        // 테이블이 추가될때 settarget
    }

    public FurnitureData GetFurnitureData(string key)
    {

        for (int i = 0; i < furnitureDatas.Length; i++)

        {
            if (furnitureDatas[i].key == key)
            {

                return furnitureDatas[i];
            }
        }
        return null;

    }

    public void PurchaseFurniture(string key)
    {
        FurnitureData furniture = GetFurnitureData(key);
        if (furniture != null)
        {
            furniture.purchased = true;
        }
    }

    public FurnitureDetail GetFurnitureDetail(string key)
    {

        for (int i = 0; i < furnitureDetails.Length; i++)

        {
            if (furnitureDetails[i].key == key)
            {

                return furnitureDetails[i];
            }
        }
        return null;

    }

}



[System.Serializable]
public class FurnitureData
{
    public string key;
    public string nextfurniturekey;
    public int price;
    public FurniturePlace furniturePlace;
    public bool purchased;
    public Sprite thum;
    public string name;
    //public int abilityLv;
    public string description;
    public int autoSpawnAcon; // 도토리 자동 생성 개수
    public float autoSpawnSec; // 도토리 자동 생성 시간
    public string tableKey; // 현재 테이블의 종류
}

public enum FurniturePlace
{
    table0,
    table1,
    table2,
    table3,
    table4,
    table5,
    Tipbox
}

[System.Serializable]
public class FurnitureDetail
{

    public string key;
    public string name;
    public string description;
    public int autoSpawnAcon;
    public float autoSpawnSec;
    public Sprite thum;

}
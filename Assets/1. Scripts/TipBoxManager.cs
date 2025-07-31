using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipBoxManager : MonoBehaviour
{

    //신규 유저라면 tipbox 기본 지급
    //현재 보유한 팁박스 중 가장 좋은 팁박스에 해당하는 게임 오브젝트를 활성화 처리하기

    [SerializeField] TipBox[] tipBoxes;
    [SerializeField] TipBoxData[] tipBoxDatas;

    //int capacity = 3000;
    float aconPerSec = 0.01f;
    public float aconAmount = 0; // 충전된 양


    public TipBoxCanvas tipboxCanvas;

    public TMP_Text aconText;

    public static TipBoxManager Instance;



    public void Awake()
    {

        Instance = this;

    }

    public void Start()
    {
        tipBoxes = GetComponentsInChildren<TipBox>(true);

        //Debug.Log("TipBox 개수: " + tipBoxes.Length);

        foreach (var tipBox in tipBoxes)
        {
            //Debug.Log($"탐지된 TipBox: {tipBox.name}, key: {tipBox.key}");
        }

        UserFurniture userFurniture = User.Instance.GetSetUpFurniture(FurniturePlace.Tipbox);

        if (userFurniture != null)
        {
            for (int i = 0; i < tipBoxes.Length; i++)
            {
                if (userFurniture.furniturekey == tipBoxes[i].key)
                {
                    tipBoxes[i].gameObject.SetActive(true);
                }
                else
                {
                    tipBoxes[i].gameObject.SetActive(false);
                }

            }

        }
        else
        {
            Debug.LogWarning("설정된 팁박스가 없습니다. 기본 팁박스를 세팅합니다.");

            // 기본값 세팅 로직
            tipBoxes[0].gameObject.SetActive(true); // 예시: 0번 팁박스를 기본으로
        }

        TipBoxData tipBoxData = GetTipBoxData(userFurniture.furniturekey);

        aconAmount = PlayerPrefs.GetFloat("TipBoxAcon", 0);
        string lasTimeStr = PlayerPrefs.GetString("TipBoxLastTime", null);

        Debug.Log("tipboxmanager start()" + lasTimeStr);


        if (string.IsNullOrEmpty(lasTimeStr) == false)
        {
            DateTime lastTime = DateTime.Parse(lasTimeStr);
            int pastSec = (int)(DateTime.Now - lastTime).TotalSeconds;
            aconAmount += pastSec / 600;


            if (aconAmount >= tipBoxData.capacity)
            {
                aconAmount = tipBoxData.capacity;
            }

            //User.Instance.AddCoin(pastSec);
            // 0.5초에 1씩 확
            // 로그 찍어서 문자열로 확인해보기 
        }

        StartCoroutine(CoFillTipBox());
        InvokeRepeating("UpdateText", 1f, 1f);
    }

    public void UpdateText()
    {
        if (aconAmount < 1f)
            aconText.text = "0";
        else
            aconText.text = Mathf.FloorToInt(aconAmount).ToString();
    }


    public void OnClickButton()
    {
        //tipBoxes[0].key에 해당하는 tipboxdatas에 담긴 객체를 오픈 함수의 인자로 전달하
        UserFurniture userFurniture = User.Instance.GetSetUpFurniture(FurniturePlace.Tipbox);

        tipboxCanvas.Open(GetTipBoxData(userFurniture.furniturekey));
        tipboxCanvas.gameObject.SetActive(true);

    }

    public void UpdateTipBoxVisual(string currentKey)
    {
        foreach (TipBox box in tipBoxes)
        {
            box.gameObject.SetActive(box.key == currentKey);
        }
    }

    public TipBoxData GetTipBoxData(string key)
    {
        //Debug.Log("GetTipBoxData 호출됨. 찾으려는 key: " + key);


        for (int i = 0; i < tipBoxDatas.Length; i++)
        {
            //Debug.Log($"비교중: {tipBoxDatas[i].key}");

            if (tipBoxDatas[i].key == key)
            {
                //Debug.Log("일치하는 TipBoxData 찾음");

                return tipBoxDatas[i];
            }
            //Debug.LogError("TipBoxData 못 찾음: " + key);

        }
        return null;
    }

    IEnumerator CoFillTipBox()
    {
        while (true)
        {
            //aconText.text = aconAmount.ToString();
            yield return new WaitForSeconds(1);
            aconAmount += aconPerSec;
            PlayerPrefs.SetFloat("TipBoxAcon", aconAmount);
            PlayerPrefs.SetString("TipBoxLastTime", DateTime.Now.ToString());
            UserFurniture userFurniture = User.Instance.GetSetUpFurniture(FurniturePlace.Tipbox);

            TipBoxData tipBoxData = GetTipBoxData(userFurniture.furniturekey);

            if (aconAmount >= tipBoxData.capacity)
            {
                aconAmount = tipBoxData.capacity;
            }
        }

    }


    public void ClearTipBox()
    {
        aconAmount = 0;
    }

}

    [System.Serializable]
    public class TipBoxData
    {
        public string key;
        public string nextKey; // 다음 팁 박스 구분자 키값
        public int price; //현재 가격 설명란에 다음 업그레이드 가격추 가 ?
       public Sprite thum;
        public int capacity;
        public string name;
        public string description;


}
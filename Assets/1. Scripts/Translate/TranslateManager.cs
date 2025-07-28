using System.Collections.Generic;
using UnityEngine;
using Boomlagoon.JSON;

public class TranslateManager : MonoBehaviour
{
    public static TranslateManager Instance;
    public Language curLanguage; //현재 내 게임에서 보여줄 언어!
    public Dictionary<string, TranslateData> translateDic = new Dictionary<string, TranslateData>();


    private void Awake()
    {
        Instance = this;

        //Application.systemLanguage 현재 운영체제에 설정된 언어 가져오기 
        SystemLanguage systemLanguage = Application.systemLanguage;
        if (systemLanguage == SystemLanguage.Korean)
        {
            curLanguage = Language.Kr;
        }
        else
        {
            curLanguage = Language.En;
        }

        TextAsset textAsset = Resources.Load<TextAsset>("Translate");
        JSONObject jObj = JSONObject.Parse(textAsset.text);
        JSONArray jArr = jObj.GetArray("Translate");

        for (int i = 0; i < jArr.Length; i++)
        {
            TranslateData translateData = new TranslateData();
            translateData.key = jArr[i].Obj.GetString("key");
            translateData.en = jArr[i].Obj.GetString("en");
            translateData.kr = jArr[i].Obj.GetString("kr");

                translateDic.Add(translateData.key, translateData);
        }
    }

    public string GetText(string key)
    {
        if (curLanguage == Language.Kr) //현재 설정된 언어가 한글인 경우
        {
            return translateDic[key].kr;
        }

        return translateDic[key].en;

    }

}


//게임에서 사용될 수 있는 타겟할 언어
public enum Language
{
    Kr,
    En
}

[System.Serializable]
public class TranslateData
{
    public string key;
    public string en;
    public string kr;
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TranslateManager : MonoBehaviour
//{
//    public static TranslateManager Instance;
//    public Language curLanguage; // 현재 내 게임에서 보여줄 언
//    public Dictionary<string, TranslateData> translateDic = new Dictionary<string, TranslateData>();

//private void Awake()
//    {
//        Instance = this;

//        //시트의 데이터를 가져와서 translatedic에 넣어야 함
//        TranslateData tD1 = new TranslateData();
//        tD1.key = "1";
//        tD1.en = "one";
//        tD1.kr = "일";
//        translateDic.Add("1", tD1);

//        TranslateData tD2 = new TranslateData();
//        tD2.key = "2";
//        tD2.en = "two";
//        tD2.kr = "이";
//        translateDic.Add("2", tD2);

//        SystemLanguage systemLanguage = Application.systemLanguage;

//        if (systemLanguage == SystemLanguage.Korean)
//        {
//            curLanguage = Language.Kr;
//        }
//        else
//        {
//            curLanguage = Language.En;
//        }

//    }


//    //key에 해당하는 글씨를 전달해
//    public string GetText(string key)
//    {

//        if (curLanguage == Language.Kr)
//        {
//            return translateDic[key].kr;
//        }
//        return translateDic[key].en;
//        }
//}

//[System.Serializable]
//public class TranslateData
//{
//    public string key;
//    public string en;
//    public string kr;

//}


//public enum Language
//{
//    Kr,
//    En
//}
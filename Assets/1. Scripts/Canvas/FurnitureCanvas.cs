using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FurnitureCanvas : MonoBehaviour
{
    public Image thumImage;
    public TMP_Text nameText;
    public TMP_Text description;
    public TMP_Text abilityInfoText;

    //public TMP_Text abilityText;

    private static FurnitureCanvas instance; // 정적 변수
    public static FurnitureCanvas Instance // 정적 속성
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<FurnitureCanvas>(FindObjectsInactive.Include);

            return instance;
        }
    }



    public void Open(string key)
    {
        gameObject.SetActive(true);

        FurnitureData furnitureData = FurnitureManager.Instance.GetFurnitureData(key);

        string detailKeyToFind = furnitureData.tableKey;

        FurnitureDetail furnitureDetail = FurnitureManager.Instance.GetFurnitureDetail(detailKeyToFind);


        thumImage.sprite = furnitureDetail.thum;

        nameText.text = GetTranslatedOrRaw(furnitureDetail.name);
        description.text = GetTranslatedOrRaw(furnitureDetail.description);
        //autoSpawnAcon.text = furnitureDetail.autoSpawnAcon.ToString();
        //autoSpawnSec.text = furnitureDetail.autoSpawnSec.ToString();

       string format = GetTranslatedOrRaw("auto_spawn_template"); // 예: "도토리를 {0}분마다 {1}개씩 얻음"
        abilityInfoText.text = string.Format(format, furnitureDetail.autoSpawnSec / 60f, furnitureDetail.autoSpawnAcon);

    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
    }
    private string GetTranslatedOrRaw(string raw)
    {
        string translated = TranslateManager.Instance.GetText(raw);
        return string.IsNullOrEmpty(translated) ? raw : translated;
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;


//public class FurnitureCanvas : MonoBehaviour
//{
//    public Image thumImage;
//    public TMP_Text nameText;
//    public TMP_Text description;
//    public TMP_Text abilityInfoText;

//    //public TMP_Text abilityText;

//    private static FurnitureCanvas instance; // 정적 변수
//    public static FurnitureCanvas Instance // 정적 속성
//    {
//        get
//        {
//            if (instance == null)
//                instance = FindFirstObjectByType<FurnitureCanvas>(FindObjectsInactive.Include);

//            return instance;
//        }
//    }



//    public void Open(string key)
//    {
//        gameObject.SetActive(true);

//        FurnitureData furnitureData = FurnitureManager.Instance.GetFurnitureData(key);

//        string detailKeyToFind = furnitureData.tableKey;

//        FurnitureDetail furnitureDetail = FurnitureManager.Instance.GetFurnitureDetail(detailKeyToFind);


//        thumImage.sprite = furnitureDetail.thum;

//        nameText.text = furnitureDetail.name;
//        description.text = furnitureDetail.description;
//        //autoSpawnAcon.text = furnitureDetail.autoSpawnAcon.ToString();
//        //autoSpawnSec.text = furnitureDetail.autoSpawnSec.ToString();

//        abilityInfoText.text = string.Format("도토리를 {0}분마다 {1}개씩 얻음", furnitureDetail.autoSpawnSec / 60f, furnitureDetail.autoSpawnAcon);

//    }

//    public void CloseButton()
//    {
//        gameObject.SetActive(false);
//    }

//}

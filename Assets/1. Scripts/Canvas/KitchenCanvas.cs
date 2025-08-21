using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class KitchenCanvas : MonoBehaviour
{
    public Image thumImage;
    public TMP_Text nameText;
    public TMP_Text description;
    public TMP_Text abilityInfoText;


    private static KitchenCanvas instance; // 정적 변수
    public static KitchenCanvas Instance // 정적 속성
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<KitchenCanvas>(FindObjectsInactive.Include);

            return instance;
        }
    }



    public void Open(string key)
    {
        Debug.Log("KitchenCanvas Open" + key);
        gameObject.SetActive(true);

        KitchenData kitchenData = KitchenManager.Instance.GetKitchenData(key);
        string detailKeyToFind = kitchenData.kitchenBarKey;

        KitchenDetail kitchenDetail = KitchenManager.Instance.GetKitchenDetail(detailKeyToFind);

        thumImage.sprite = kitchenDetail.thum;
        thumImage.SetNativeSize();

        nameText.text = GetTranslatedOrRaw(kitchenDetail.name);
        description.text = GetTranslatedOrRaw(kitchenDetail.description);

        string format = GetTranslatedOrRaw("decanting_speed_template");
        int percent = Mathf.RoundToInt(kitchenDetail.reduceMakingTime * 100f);
        abilityInfoText.text = string.Format(format, percent);
        //abilityInfoText.text = string.Format("디캔딩 속도가 {0}% 빨라짐", kitchenDetail.reduceMakingTime * 100);

    }

    private string GetTranslatedOrRaw(string raw)
    {
        string translated = TranslateManager.Instance.GetText(raw);
        return string.IsNullOrEmpty(translated) ? raw : translated;
    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
    }

}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;


//public class KitchenCanvas : MonoBehaviour
//{
//    public Image thumImage;
//    public TMP_Text nameText;
//    public TMP_Text description;
//    public TMP_Text abilityInfoText;


//    private static KitchenCanvas instance; // 정적 변수
//    public static KitchenCanvas Instance // 정적 속성
//    {
//        get
//        {
//            if (instance == null)
//                instance = FindFirstObjectByType<KitchenCanvas>(FindObjectsInactive.Include);

//            return instance;
//        }
//    }



//    public void Open(string key)
//    {
//        Debug.Log("KitchenCanvas Open" + key);
//        gameObject.SetActive(true);

//        KitchenData kitchenData = KitchenManager.Instance.GetKitchenData(key);
//        string detailKeyToFind = kitchenData.kitchenBarKey;

//        KitchenDetail kitchenDetail = KitchenManager.Instance.GetKitchenDetail(detailKeyToFind);

//        thumImage.sprite = kitchenDetail.thum;
//        thumImage.SetNativeSize();

//        nameText.text = kitchenDetail.name;
//        description.text = kitchenDetail.description;
//        int percent = Mathf.RoundToInt(kitchenDetail.reduceMakingTime * 100f);
//        abilityInfoText.text = $"디캔딩 속도가 {percent}% 빨라짐";
//        //abilityInfoText.text = string.Format("디캔딩 속도가 {0}% 빨라짐", kitchenDetail.reduceMakingTime * 100);

//    }

//    public void CloseButton()
//    {
//        gameObject.SetActive(false);
//    }

//}
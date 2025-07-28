using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkerCanvas : MonoBehaviour
{
    public Image thumImage;
    public TMP_Text nameText;
    public TMP_Text description;
    public TMP_Text abilityText;

    public static WorkerCanvas instance;
    public static WorkerCanvas Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<WorkerCanvas>(FindObjectsInactive.Include);
            return instance;
        }
    }

    public void Open(string key)
    {
        Debug.Log("WorkerCanvas Open: " + key);
        gameObject.SetActive(true);

        FoxData foxData = FoxManager.Instance.GetFoxData(key);
        thumImage.sprite = foxData.thum;
        thumImage.SetNativeSize();

        FoxDetail foxDetail = FoxManager.Instance.GetFoxDetail(foxData.key);
        nameText.text = GetTranslatedOrRaw(foxDetail.name);
        description.text = GetTranslatedOrRaw(foxDetail.description);
        abilityText.text = GetTranslatedOrRaw(foxDetail.effect);

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

//public class WorkerCanvas : MonoBehaviour
//{
//    public Image thumImage;
//    public TMP_Text nameText;
//    public TMP_Text description;
//    public TMP_Text abilityText;

//    public static WorkerCanvas instance; // 정적 변수
//    public static WorkerCanvas Instance // 정적 속성
//    {
//        get
//        {
//            if (instance == null)
//                instance = FindFirstObjectByType<WorkerCanvas>(FindObjectsInactive.Include);

//            return instance;
//        }
//    }



//    public void Open(string key)
//    {
//        Debug.Log("WorkerCanvas Open" + key);
//        gameObject.SetActive(true);

//        FoxData foxData = FoxManager.Instance.GetFoxData(key);
//        //string detailKeyToFind = foxData.key;


//        thumImage.sprite = foxData.thum;
//        thumImage.SetNativeSize();

//        string detailKeyToFind = foxData.key;


//        FoxDetail foxDetail = FoxManager.Instance.GetFoxDetail(detailKeyToFind);
//        nameText.text = foxDetail.name;
//        description.text = foxDetail.description;
//        abilityText.text = foxDetail.effect;

//        //abilityText.text = "도토리 획득량 +" + foxData.abilityLv;

//    }

//    public void CloseButton()
//    {
//        gameObject.SetActive(false);
//    }

//}

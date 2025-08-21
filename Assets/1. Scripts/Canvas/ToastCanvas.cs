using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToastCanvas : MonoBehaviour
{

    public static ToastCanvas Instance;

    public TMP_Text toastText;
    public GameObject toast;

    public void Awake()
    {
        Instance = this;
    }

    public void ShowToast(string key) // 변수명을 'key'로 사용
    {
        string translatedText = TranslateManager.Instance.GetText(key); // 전달받은 'key' 사용

        if (string.IsNullOrEmpty(translatedText))
        {
            //Debug.LogWarning($"[ToastCanvas] No translation found for key: '{key}'. Displaying raw key as toast message.");
            translatedText = key;
        }

        toast.gameObject.SetActive(true);
        toastText.text = translatedText;

        CancelInvoke("OffToast");
        Invoke("OffToast", 2);
    }
    //public void ShowToast(string text)
    //{
    //    toast.gameObject.SetActive(true);
    //    toastText.text = text;
    //    CancelInvoke();
    //    Invoke("OffToast", 2);

    //}

    public void OffToast()
    {
        toast.SetActive(false);
    }

}

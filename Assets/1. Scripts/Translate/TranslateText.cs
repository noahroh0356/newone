using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslateText : MonoBehaviour
{
    public string key; // 번역을 구분하는 구분자
    public TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        //key에 해당하는 글씨를 가져와라 
        text.text = TranslateManager.Instance.GetText(key);
    }
}

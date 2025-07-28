#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using TMPro;

public class AssignKeyFromCurrentText : MonoBehaviour
{
    [MenuItem("Tools/Localization/Set Key from Current Text")]
    static void SetKeyFromText()
    {
        TranslateText[] allTranslateTexts = FindObjectsOfType<TranslateText>(true);
        int count = 0;

        foreach (var tt in allTranslateTexts)
        {
            if (tt.text == null)
                tt.text = tt.GetComponent<TMP_Text>();

            if (tt == null || tt.text == null)
                continue;

            // 현재 텍스트 내용을 key로 설정 (공백 제거)
            string currentText = tt.text.text.Trim();

            // 이미 설정된 경우는 스킵
            if (!string.IsNullOrEmpty(tt.key) && tt.key != "0")
                continue;

            tt.key = currentText;
            EditorUtility.SetDirty(tt); // 저장 표시
            count++;
        }

        Debug.Log($"✅ {count}개 TranslateText 오브젝트에 key 자동 세팅 완료!");
    }
}
#endif

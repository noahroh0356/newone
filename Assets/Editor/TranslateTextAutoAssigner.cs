#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using TMPro;

public class TranslateTextAutoAssigner : MonoBehaviour
{
    [MenuItem("Tools/Localization/Auto Assign TranslateText")]
    static void AssignTranslateTextToAll()
    {
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true); // 비활성 포함
        int count = 0;

        foreach (var text in allTexts)
        {
            // 이미 TranslateText가 붙어있으면 패스
            if (text.GetComponent<TranslateText>() != null)
                continue;

            // 빈 텍스트나 특수 문자만 있는 경우 제외
            string clean = text.text.Trim();
            if (string.IsNullOrEmpty(clean) || clean.StartsWith("["))
                continue;

            // 새로운 컴포넌트 추가
            TranslateText tt = text.gameObject.AddComponent<TranslateText>();
            tt.key = clean; // 초기값은 텍스트 자체를 key로 설정
            tt.text = text;

            count++;
        }

        Debug.Log($"✅ TranslateText 자동 적용 완료: {count}개 텍스트에 추가됨");
    }
}
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TMP_Text를 사용하므로 추가

public class MainQuestPanel : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text processText;
    public TMP_Text reward1Text;
    public TMP_Text reward2Text;
    MainQuestData mainQuestdata; // 현재 패널이 표시하는 퀘스트 데이터
    public Button questButton; // 유니티 에디터에서 직접 할당 권장
    public GameObject alert; // '퀘스트 완료!' 시 표시될 느낌표/표시
    public AudioSource clickSound;

    private bool isQuestCompleted = false; // 현재 퀘스트가 '완료' 상태이고 클릭을 기다리는지
    private bool isListenerAdded = false; // 버튼 리스너가 추가되었는지 여부

    // 이 스크립트가 붙은 GameObject가 활성화될 때마다 호출됩니다.
    private void OnEnable()
    {
        Debug.Log("MainQuestPanel OnEnable() called!");

        // 버튼 리스너를 한 번만 추가하도록 처리 (OnEnable은 여러 번 호출될 수 있음)
        if (!isListenerAdded)
        {
            if (questButton == null)
            {
                // Inspector에서 할당되지 않았다면 자식에서 찾아서 할당 (Fallback)
                questButton = GetComponentInChildren<Button>();
            }

            if (questButton != null)
            {
                questButton.onClick.RemoveAllListeners(); // 혹시 모를 기존 리스너 모두 제거
                questButton.onClick.AddListener(OnPanelClicked);
                isListenerAdded = true; // 리스너가 추가되었음을 표시
                Debug.Log("Button listener added in OnEnable!");
            }
            else
            {
                Debug.LogError("Quest Button is null in MainQuestPanel (OnEnable)! Please assign it in the Inspector or ensure it's a child.");
            }
        }

        // 패널이 활성화될 때마다 현재 퀘스트 정보를 MainQuestManager로부터 받아와 업데이트합니다.
        // 이는 게임 로드 시 또는 다른 UI에서 이 패널을 켤 때 최신 상태를 반영하기 위함입니다.
        if (MainQuestManager.Instance != null && MainQuestManager.Instance.userMainQuest != null)
        {
            // 현재 퀘스트 데이터를 가져옵니다.
            int currentQuestIndex = MainQuestManager.Instance.userMainQuest.curQuestIndex;
            if (currentQuestIndex < MainQuestManager.Instance.mainQuestDatas.Length)
            {
                MainQuestData currentQuestData = MainQuestManager.Instance.mainQuestDatas[currentQuestIndex];
                StartQuest(currentQuestData); // 현재 퀘스트 정보로 패널 UI 업데이트

                // 현재 퀘스트가 이미 완료 상태이고, 클릭을 기다리는 상태라면 "완료" UI로 변경
                if (!MainQuestManager.Instance.userMainQuest.processing) // processing이 false면 완료 상태 (클릭 대기)
                {
                    SetCompletedState();
                }
            }
            else // 모든 퀘스트 완료 상태일 경우
            {
                SetCompletedState(); // 모든 퀘스트 완료 메시지 표시 (선택적)
                titleText.text = GetTranslatedOrRaw("QuestCompleteMessage"); // 예: "모든 퀘스트 완료!"
                processText.gameObject.SetActive(false); // 보상, 진행도 숨기기
                //reward1Text.gameObject.SetActive(false);
                //reward2Text.gameObject.SetActive(false);
            }
        }
    }

    // 이 스크립트가 붙은 GameObject가 비활성화될 때마다 호출됩니다.
    private void OnDisable()
    {
        Debug.Log("MainQuestPanel OnDisable() called!");
        if (questButton != null && isListenerAdded)
        {
            questButton.onClick.RemoveListener(OnPanelClicked);
            isListenerAdded = false; // 리스너가 제거되었음을 표시
            Debug.Log("Button listener removed in OnDisable!");
        }
    }

    // 퀘스트 목표 달성 시 MainQuestManager에서 호출하여 패널을 '완료' 상태로 바꿉니다.
    public void SetCompletedState()
    {
        //Debug.Log("MainQuestPanel: SetCompletedState 호출됨!");
        titleText.text = GetTranslatedOrRaw("QuestCompleteMessage"); // "QuestCompleteMessage" 키에 맞는 번역 사용 (번역 파일에 정의 필요)
        alert.SetActive(true); // 완료 알림 활성화
        isQuestCompleted = true; // 패널 상태를 '완료됨'으로 설정

        // 보상, 진행도 텍스트를 숨겨서 '완료' 상태임을 더 명확히 보여줍니다.
        processText.gameObject.SetActive(false);
        //reward1Text.gameObject.SetActive(false);
        //reward2Text.gameObject.SetActive(false);
    }

    // 패널의 버튼 클릭 시 호출됩니다.
    public void OnPanelClicked()
    {
        // Debug.Log($"MainQuestPanel: 클릭 감지! isQuestCompleted: {isQuestCompleted}"); // 디버그 로그 추가

        if (MainQuestManager.Instance == null)
        {
            Debug.LogError("MainQuestManager.Instance is null! Cannot process quest completion.");
            return;
        }

        // 퀘스트가 '완료' 상태일 때만 실제 퀘스트 완료 처리 진행
        if (isQuestCompleted)
        {
            isQuestCompleted = false; // 중복 클릭 방지를 위해 상태 변경 (클릭 후 바로 false로)
            if (clickSound != null) clickSound.Play(); // 사운드 재생

            Debug.Log("퀘스트 패널 클릭됨 - 보상 지급 및 다음 퀘스트 시작 MainQuestManager에 요청");
            // MainQuestManager에게 현재 퀘스트 완료 처리를 요청합니다.
            MainQuestManager.Instance.CompleteAndAdvanceToNextQuest();
        }
        else // 퀘스트가 아직 완료되지 않았다면 (진행 중이라면) 클릭 무시
        {
            Debug.Log("퀘스트가 아직 완료되지 않아 패널 클릭 무시됨.");
            // 클릭 사운드 등 다른 피드백 추가 가능
        }
    }

    // 새로운 퀘스트 정보로 패널 UI를 초기화합니다.
    public void StartQuest(MainQuestData data)
    {
        mainQuestdata = data;

        // 새 퀘스트 시작 시, 패널의 상태를 초기화
        titleText.text = GetTranslatedOrRaw(data.No); // data.No를 번역 키로 사용
        alert.SetActive(false); // 완료 알림 비활성화
        isQuestCompleted = false; // 완료 상태 해제

        // 숨겼던 텍스트 컴포넌트들을 다시 활성화합니다.
        processText.gameObject.SetActive(true);
        reward1Text.gameObject.SetActive(true);
        reward2Text.gameObject.SetActive(true);

        // 퀘스트 진행도 및 보상 텍스트 업데이트
        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + data.goal;
        reward1Text.text = "+" + data.aconReward1.ToString();
        reward2Text.text = "+" + data.gatchaCoinReward2.ToString();
    }

    // 번역 키를 사용하여 텍스트를 가져오거나, 번역이 없으면 원본 키 또는 data.Title을 반환합니다.
    public string GetTranslatedOrRaw(string key)
    {
        if (TranslateManager.Instance == null)
        {
            Debug.LogError("TranslateManager.Instance is null! Cannot get translation.");
            return key; // TranslateManager 없으면 그냥 키 반환
        }

        // 1. 전달받은 키(data.No 또는 "QuestCompleteMessage")가 null이거나 빈 문자열이면 즉시 폴백 처리
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning($"[TranslateManager] Received a null or empty key for translation. Falling back to default quest title if available.");
            // 이 경우 mainQuestdata가 유효하다면 그 Title을 사용 (StartQuest 호출 전일수 있으므로 mainQuestdata null 체크)
            return mainQuestdata != null ? mainQuestdata.title : "Invalid Quest Title";
        }

        // 2. TranslateManager에서 번역 시도
        string translated = TranslateManager.Instance.GetText(key);

        // 3. 번역 결과가 null이거나 빈 문자열인 경우
        if (string.IsNullOrEmpty(translated))
        {
            Debug.LogWarning($"[TranslateManager] No translation found for key: '{key}'. Falling back to MainQuestData.Title (if available).");

            // ⭐ 폴백 로직: 번역 키에 해당하는 번역이 없을 때 MainQuestData.title을 사용
            // 이 로직은 mainQuestdata가 null이 아닐 때만 유효합니다.
            if (mainQuestdata != null)
            {
                return mainQuestdata.title;
            }
            else
            {
                // mainQuestdata도 null이면, 그냥 받은 key 자체를 반환 (최후의 수단)
                return key;
            }
        }

        // 4. 번역된 텍스트가 있으면 반환
        return translated;
    }

    // 패널의 진행도 등을 업데이트할 때 호출됩니다. (퀘스트 완료 상태가 아닐 때만 업데이트)
    public void UpdatePanel()
    {
        if (mainQuestdata == null)
        {
            Debug.LogError("UpdatePanel called but mainQuestdata is null! Cannot update panel.");
            return;
        }

        // 퀘스트가 완료 상태가 아닐 때만 진행도를 업데이트합니다.
        // 완료 상태일 때는 '퀘스트 완료' 텍스트와 숨겨진 진행도가 유지되어야 합니다.
        if (!isQuestCompleted)
        {
            titleText.text = GetTranslatedOrRaw(mainQuestdata.No); // 진행도 업데이트 시 제목도 다시 가져옴
            processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + mainQuestdata.goal;
        }
    }
}// 퀘스트가 아직 완료
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class MainQuestPanel : MonoBehaviour
//{
//    public TMP_Text titleText;
//    public TMP_Text processText;
//    public TMP_Text reward1Text;
//    public TMP_Text reward2Text;
//    MainQuestData mainQuestdata;
//    public Button questButton;
//    public GameObject alert;
//    public AudioSource clickSound;

//    private bool isQuestCompleted = false; // 현재 패널이 '퀘스트 완료' 상태인지
//    private bool isInitialized = false;

//    private void Start()
//    {
//        Debug.Log("MainQuestPanel Start() called!");
//        if (questButton == null)
//        {
//            questButton = GetComponentInChildren<Button>();
//        }
//        if (questButton != null)
//        {
//            questButton.onClick.AddListener(OnPanelClicked);
//        }

//        isInitialized = true;
//    }

//    // 퀘스트 목표 달성 시 호출되어 패널을 '완료' 상태로 바꿉니다.
//    public void SetCompletedState()
//    {
//        titleText.text = GetTranslatedOrRaw("QuestCompleteMessage");
//        alert.SetActive(true);
//        isQuestCompleted = true; // 패널이 완료 상태임을 표시

//        // 보상, 진행도 텍스트는 숨깁니다.
//        processText.gameObject.SetActive(false);
//        reward1Text.gameObject.SetActive(false);
//        reward2Text.gameObject.SetActive(false);
//    }

//    public void OnPanelClicked()
//    {
//        if (!isInitialized) return;

//        // ⭐ 변경점: isQuestCompleted 상태일 때만 다음 퀘스트로 진행하는 함수 호출
//        if (isQuestCompleted)
//        {
//            isQuestCompleted = false; // 중복 클릭 방지를 위해 상태 변경
//            clickSound.Play();

//            Debug.Log("퀘스트 패널 클릭됨 - 보상 지급 및 다음 퀘스트 시작");
//            // MainQuestManager의 새로운 함수를 호출하여 보상을 받고 다음 퀘스트로 넘어갑니다.
//            MainQuestManager.Instance.CompleteAndAdvanceToNextQuest();
//        }
//        else // 퀘스트가 아직 완료되지 않았다면 (진행 중이라면) 클릭 무시
//        {
//            Debug.Log("퀘스트가 아직 완료되지 않아 패널 클릭 무시됨.");
//        }
//    }

//    // 새로운 퀘스트가 시작될 때 호출되어 패널을 초기화합니다.
//    public void StartQuest(MainQuestData data)
//    {
//        mainQuestdata = data;

//        // 새 퀘스트 시작 시, 패널의 상태를 초기화
//        titleText.text = GetTranslatedOrRaw(data.No);
//        alert.SetActive(false);
//        isQuestCompleted = false; // 완료 상태 해제

//        // 숨겼던 텍스트 컴포넌트들을 다시 활성화합니다.
//        processText.gameObject.SetActive(true);
//        reward1Text.gameObject.SetActive(true);
//        reward2Text.gameObject.SetActive(true);

//        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + data.goal;
//        reward1Text.text = "+" + data.aconReward1.ToString();
//        reward2Text.text = "+" + data.gatchaCoinReward2.ToString();
//    }

//    // GetTranslatedOrRaw 함수는 이전과 동일하게 유지됩니다.
//    private string GetTranslatedOrRaw(string key)
//    {
//        // 1. 전달받은 키(data.No)가 null이거나 빈 문자열이면 즉시 폴백 처리
//        if (string.IsNullOrEmpty(key))
//        {
//            Debug.LogWarning($"[TranslateManager] Received a null or empty key for translation. Falling back to default quest title if available.");
//            // 이 경우 mainQuestdata가 유효하다면 그 Title을 사용
//            return mainQuestdata != null ? mainQuestdata.title : "Invalid Quest Title";
//        }

//        // 2. TranslateManager에서 번역 시도
//        string translated = TranslateManager.Instance.GetText(key);

//        // 3. 번역 결과가 null이거나 빈 문자열인 경우
//        if (string.IsNullOrEmpty(translated))
//        {
//            // 경고 로그 출력
//            Debug.LogWarning($"[TranslateManager] No translation found for key: '{key}'. Falling back to MainQuestData.Title.");

//            // ⭐ 폴백 로직: 번역 키에 해당하는 번역이 없을 때 MainQuestData.title을 사용
//            // 이 로직은 mainQuestdata가 null이 아닐 때만 유효합니다.
//            if (mainQuestdata != null)
//            {
//                return mainQuestdata.title;
//            }
//            else
//            {
//                // mainQuestdata도 null이면, 그냥 받은 key 자체를 반환 (최후의 수단)
//                return key;
//            }
//        }

//        // 4. 번역된 텍스트가 있으면 반환
//        return translated;
//    }

//    public void UpdatePanel()
//    {
//        if (mainQuestdata == null)
//        {
//            Debug.LogError("UpdatePanel called but mainQuestdata is null! Cannot update panel.");
//            return;
//        }

//        // 퀘스트가 완료 상태가 아닐 때만 진행도를 업데이트합니다.
//        if (!isQuestCompleted)
//        {
//            titleText.text = GetTranslatedOrRaw(mainQuestdata.No);
//            processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + mainQuestdata.goal;
//        }
//        // 퀘스트가 완료 상태일 때는 '퀘스트 완료' 텍스트를 유지하고, processText는 숨겨집니다.
//    }
//}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class MainQuestPanel : MonoBehaviour
//{
//    public TMP_Text titleText;
//    public TMP_Text processText;
//    public TMP_Text reward1Text;
//    public TMP_Text reward2Text;
//    MainQuestData mainQuestdata; // 현재 패널이 표시하는 퀘스트 데이터
//    public Button questButton;
//    public GameObject alert;
//    public AudioSource clickSound;

//    private bool isQuestCompleted = false;
//    private bool isInitialized = false;

//    private void Start()
//    {
//        // 버튼 리스너는 한 번만 추가
//        if (questButton == null) // Inspector에서 할당하지 않았다면 찾아서 할당 (권장: Inspector에서 직접 할당)
//        {
//            questButton = GetComponentInChildren<Button>();
//        }
//        if (questButton != null)
//        {
//            questButton.onClick.AddListener(OnPanelClicked);
//        }

//        isInitialized = true;
//    }

//    // OnEnable을 사용하여 패널이 활성화될 때마다 초기화 또는 업데이트 로직을 수행할 수 있습니다.
//    // 하지만 MainQuestManager에서 SafeCallToPanel을 통해 StartQuest를 명시적으로 호출한다면 필요 없을 수 있습니다.
//    // private void OnEnable() 
//    // {
//    //     // 패널이 켜질 때마다 UI를 업데이트해야 한다면 여기에 StartQuest를 호출하는 로직이 필요
//    //     // 예: if (MainQuestManager.Instance != null && MainQuestManager.Instance.userMainQuest != null)
//    //     //     StartQuest(MainQuestManager.Instance.mainQuestDatas[MainQuestManager.Instance.userMainQuest.curQuestIndex]);
//    // }

//    public void CompleteQuest()
//    {
//        titleText.text = GetTranslatedOrRaw("QuestCompleteMessage");
//        alert.SetActive(true);
//        isQuestCompleted = true;
//    }

//    public void OnPanelClicked()
//    {
//        if (!isInitialized || !isQuestCompleted)
//            return;

//        isQuestCompleted = false;
//        clickSound.Play();

//        Debug.Log("퀘스트 완료");
//        MainQuestManager.Instance.CompleteCurrentQuest();
//    }

//    public void StartQuest(MainQuestData data)
//    {
//        // 새로 할당된 퀘스트 데이터를 저장
//        mainQuestdata = data;

//        // data.No를 번역 키로 사용합니다.
//        // GetTranslatedOrRaw는 이제 번역 실패 시 data.Title을 폴백으로 사용할 수 있게 수정됩니다.
//        titleText.text = GetTranslatedOrRaw(data.No);

//        isQuestCompleted = false;
//        alert.SetActive(false);

//        // processText와 rewardText는 번역이 필요 없는 값이므로 그대로 사용
//        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + data.goal;
//        reward1Text.text = "+" + data.aconReward1.ToString();
//        reward2Text.text = "+" + data.gatchaCoinReward2.ToString();
//    }



//    public void UpdatePanel()
//    {
//        // mainQuestdata가 유효한지 확인 (만약을 대비하여)
//        if (mainQuestdata == null)
//        {
//            Debug.LogError("UpdatePanel called but mainQuestdata is null! Cannot update panel.");
//            return;
//        }

//        // 퀘스트 진행도가 업데이트될 때도 mainQuestdata.No를 사용하여 제목을 다시 가져옵니다.
//        // GetTranslatedOrRaw 함수 내에서 폴백 로직이 처리됩니다.
//        titleText.text = GetTranslatedOrRaw(mainQuestdata.No);

//        // 진행도 텍스트는 그대로 유지
//        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + mainQuestdata.goal;
//    }
//}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class MainQuestPanel : MonoBehaviour
//{
//    public TMP_Text titleText;
//    public TMP_Text processText;
//    public TMP_Text reward1Text;
//    public TMP_Text reward2Text;
//    MainQuestData mainQuestdata;
//    public Button questButton;
//    public GameObject alert;
//    public AudioSource clickSound;

//    private bool isQuestCompleted = false;
//    private bool isInitialized = false;

//    private void Start()
//    {
//        GetComponentInChildren<Button>().onClick.AddListener(OnPanelClicked);
//        isInitialized = true;
//    }

//    public void CompleteQuest()
//    {
//        // 퀘스트 완료 메시지는 고정된 키로 번역
//        titleText.text = GetTranslatedOrRaw("QuestCompleteMessage");
//        alert.SetActive(true);
//        isQuestCompleted = true;
//    }

//    public void OnPanelClicked()
//    {
//        if (!isInitialized || !isQuestCompleted)
//            return;

//        isQuestCompleted = false;
//        clickSound.Play();

//        Debug.Log("퀘스트 완료");
//        MainQuestManager.Instance.CompleteCurrentQuest();
//    }

//    public void StartQuest(MainQuestData data)
//    {
//        mainQuestdata = data;

//        // data.No (예: "1", "168")를 번역 키로 사용합니다.
//        titleText.text = GetTranslatedOrRaw(data.No); // <-- 이 부분이 변경됩니다.

//        isQuestCompleted = false;
//        alert.SetActive(false);

//        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + data.goal;
//        reward1Text.text = "+" + data.aconReward1.ToString();
//        reward2Text.text = "+" + data.gatchaCoinReward2.ToString();
//    }

//    private string GetTranslatedOrRaw(string key)
//    {
//        string translated = TranslateManager.Instance.GetText(key);
//        if (string.IsNullOrEmpty(translated) && !string.IsNullOrEmpty(key))
//        {
//            Debug.LogWarning($"[TranslateManager] No translation found for key: '{key}'. Using raw key.");
//            // 폴백 (fallback) 로직: 번역 키가 없으면 그냥 키값 자체를 표시
//            // 이 경우 rawKey가 "1", "168" 처럼 숫자이므로, UI에 숫자가 그대로 표시될 수 있습니다.
//            // 이보다는 Translation.json에 모든 No에 대한 번역이 정의되는 것이 가장 좋습니다.
//        }
//        return string.IsNullOrEmpty(translated) ? key : translated;
//    }

//    public void UpdatePanel()
//    {
//        // 퀘스트 진행도가 업데이트될 때도 mainQuestdata.No를 사용하여 제목을 다시 가져옵니다.
//        titleText.text = GetTranslatedOrRaw(mainQuestdata.No); // <-- 이 부분이 변경됩니다.

//        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + mainQuestdata.goal;
//    }
//}
//// ✅ 핵심 수정 내용:
//// 번역은 UI 출력에만 사용하고, 내부 로직은 string 키값(Title 등)을 기반으로만 동작하도록 유지.
//// titleText에만 번역 적용, 나머지는 key(raw string)를 그대로 사용하여 비교/판단

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class MainQuestPanel : MonoBehaviour
//{
//    public TMP_Text titleText;
//    public TMP_Text processText;
//    public TMP_Text reward1Text;
//    public TMP_Text reward2Text;
//    MainQuestData mainQuestdata;
//    public Button questButton;
//    public GameObject alert;
//    public AudioSource clickSound;

//    private bool isQuestCompleted = false;
//    private bool isInitialized = false;

//    private void Start()
//    {
//        GetComponentInChildren<Button>().onClick.AddListener(OnPanelClicked);
//        isInitialized = true;
//    }

//    public void CompleteQuest()
//    {
//        // 번역된 텍스트는 UI에만 사용
//        titleText.text = GetTranslatedOrRaw("QuestCompleteMessage");
//        alert.SetActive(true);
//        isQuestCompleted = true;
//    }

//    public void OnPanelClicked()
//    {
//        if (!isInitialized || !isQuestCompleted)
//            return;

//        isQuestCompleted = false;
//        clickSound.Play();

//        Debug.Log("퀘스트 완료");
//        MainQuestManager.Instance.CompleteCurrentQuest();
//    }

//    public void StartQuest(MainQuestData data)
//    {
//        mainQuestdata = data;

//        // 번역은 오직 UI 출력용으로만 사용
//        titleText.text = GetTranslatedOrRaw(data.title);

//        isQuestCompleted = false;
//        alert.SetActive(false);

//        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + data.goal;
//        reward1Text.text = "+" + data.aconReward1.ToString();
//        reward2Text.text = "+" + data.gatchaCoinReward2.ToString();
//    }

//    private string GetTranslatedOrRaw(string rawKey)
//    {
//        string translated = TranslateManager.Instance.GetText(rawKey);
//        return string.IsNullOrEmpty(translated) ? rawKey : translated;
//    }

//    public void UpdatePanel()
//    {
//        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + mainQuestdata.goal;
//    }
//}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//using TMPro;

//public class MainQuestPanel : MonoBehaviour
//{
//    public TMP_Text titleText;
//    public TMP_Text processText;
//    public TMP_Text reward1Text;
//    public TMP_Text reward2Text;
//    MainQuestData mainQuestdata;
//    public Button questButton; // 버튼을 수동으로 할당할 변수
//    public GameObject alert;

//    public AudioSource clickSound;


//    private bool isQuestCompleted = false;
//    private bool isInitialized = false; // 초기화 여부 플래그


//    private void Start()
//    {
//        GetComponentInChildren<Button>().onClick.AddListener(OnPanelClicked);
//        isInitialized = true; // 초기화 완료

//    }

//    //public void CompleteQuest()
//    //{
//    //    if (isQuestCompleted) return;

//    //    titleText.text = GetTranslatedOrRaw("퀘스트 완료! (터치하세요)");
//    //    alert.SetActive(true);
//    //    isQuestCompleted = true;
//    //}
//    public void CompleteQuest()
//    {
//        titleText.text = "퀘스트 완료! (터치하세요)";
//        alert.SetActive(true);
//        isQuestCompleted = true;
//    }

//    public void OnPanelClicked()
//    {
//        if (!isInitialized || !isQuestCompleted)
//            return;

//        // 중복 클릭 방지
//        isQuestCompleted = false;

//        clickSound.Play();

//        Debug.Log("퀘스트 완료");
//        MainQuestManager.Instance.CompleteCurrentQuest();
//    }

//    //public void OnPanelClicked()
//    //{
//    //    if (isInitialized && isQuestCompleted)
//    //    {
//    //        clickSound.Play();

//    //        Debug.Log("가챠코인 지급!");
//    //        MainQuestManager.Instance.CompleteCurrentQuest();
//    //        //User.Instance.AddGatchaCoin(1); // 가챠코인 지급
//    //        //MainQuestManager.Instance.StartQuest(); // 다음 퀘스트 시작
//    //    }
//    //}



//    //public void StartQuest(MainQuestData data)
//    //{
//    //    mainQuestdata = data;

//    //    titleText.text = GetTranslatedOrRaw(data.title);

//    //    // 퀘스트가 이미 완료 상태가 아닌 경우에만 초기화
//    //    if (!isQuestCompleted)
//    //    {
//    //        alert.SetActive(false);
//    //        isQuestCompleted = false;
//    //    }

//    //    processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + data.goal;
//    //    reward1Text.text = "+" + data.aconReward1.ToString();
//    //    reward2Text.text = "+" + data.gatchaCoinReward2.ToString();
//    //}
//    public void StartQuest(MainQuestData data)
//    {

//        mainQuestdata = data;

//        titleText.text = GetTranslatedOrRaw(data.title);
//        isQuestCompleted = false;
//        alert.SetActive(false);
//        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + data.goal;
//        reward1Text.text = "+" + data.aconReward1.ToString();
//        reward2Text.text = "+" + data.gatchaCoinReward2.ToString();
//    }

//    private string GetTranslatedOrRaw(string raw)
//    {
//        string translated = TranslateManager.Instance.GetText(raw);
//        return string.IsNullOrEmpty(translated) ? raw : translated;
//    }


//    public void UpdatePanel()
//    {
//        processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + mainQuestdata.goal;
//    }


//}


////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;
////using UnityEngine.UI;

////using TMPro;

////public class MainQuestPanel : MonoBehaviour
////{
////    public TMP_Text titleText;
////    public TMP_Text processText;
////    public TMP_Text reward1Text;
////    public TMP_Text reward2Text;
////    MainQuestData mainQuestdata;
////    public Button questButton; // 버튼을 수동으로 할당할 변수
////    public GameObject alert;

////    public AudioSource clickSound;


////    private bool isQuestCompleted = false;
////    private bool isInitialized = false; // 초기화 여부 플래그


////    private void Start()
////    {
////        GetComponentInChildren<Button>().onClick.AddListener(OnPanelClicked);
////        isInitialized = true; // 초기화 완료

////    }


////    public void CompleteQuest()
////    {
////        titleText.text = "퀘스트 완료! (터치하세요)";
////        alert.SetActive(true);
////        isQuestCompleted = true;
////    }

////    public void OnPanelClicked()
////    {
////        if (isInitialized && isQuestCompleted)
////        {
////            clickSound.Play();

////            Debug.Log("가챠코인 지급!");
////            MainQuestManager.Instance.CompleteCurrentQuest();
////            //User.Instance.AddGatchaCoin(1); // 가챠코인 지급
////            //MainQuestManager.Instance.StartQuest(); // 다음 퀘스트 시작
////        }
////    }



////public void StartQuest(MainQuestData data)
////{

////    mainQuestdata = data;
////    titleText.text = data.title.ToString();
////    isQuestCompleted = false;
////    alert.SetActive(false);
////    processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + data.goal;
////    reward1Text.text = "+" + data.aconReward1.ToString();
////    reward2Text.text = "+" + data.gatchaCoinReward2.ToString();
////}

////public void UpdatePanel()
////{
////    processText.text = MainQuestManager.Instance.userMainQuest.process + "/" + mainQuestdata.goal;
////}


////}

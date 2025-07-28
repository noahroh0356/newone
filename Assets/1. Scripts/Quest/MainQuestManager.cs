using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON; // JSON 파싱 라이브러리 (기존 사용 가정)
using UnityEngine;

public class MainQuestManager : MonoBehaviour
{
    public static MainQuestManager Instance;
    public MainQuestData[] mainQuestDatas; // 모든 메인 퀘스트 데이터
    public UserMainQuest userMainQuest; // 현재 유저의 메인 퀘스트 진행 상황

    // 특정 퀘스트 타입에 필요한 키 배열 (기존 사용 가정)
    public string[] purchaseFurnitureQuestKeys;
    public string[] purchaseKitchenQuestKeys;

    public MainQuestPanel mainQuestPanel; // 유니티 에디터에서 MainQuestPanel 할당 필수!


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지 (필요에 따라)
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadQuestDataFromJson(); // JSON에서 퀘스트 데이터 로드

        // 유저의 메인 퀘스트 진행 상황 로드
        userMainQuest = SaveMgr.LoadData<UserMainQuest>("UserMainQuest");

        if (userMainQuest == null)
        {
            userMainQuest = new UserMainQuest
            {
                curQuestIndex = 0,
                process = 0,
                processing = true // 첫 퀘스트는 바로 진행 중 상태
            };
            Debug.Log("[새로운 UserMainQuest 생성 및 초기화]");
            SaveMgr.SaveData("UserMainQuest", userMainQuest); // 초기 상태 저장
            StartQuest(); // 첫 퀘스트 시작
        }
        else
        {
            Debug.Log($"[UserMainQuest 로드] 인덱스: {userMainQuest.curQuestIndex}, 타입: {userMainQuest.mainQuestType}, 진행도: {userMainQuest.process}, 진행 중: {userMainQuest.processing}");

            // 로드 후 현재 퀘스트가 유효하다면 패널에 표시
            if (userMainQuest.curQuestIndex < mainQuestDatas.Length)
            {
                MainQuestData loadedQuestData = mainQuestDatas[userMainQuest.curQuestIndex];
                SafeCallToPanel(panel => panel.StartQuest(loadedQuestData)); // 일단 현재 퀘스트를 표시

                // 로드된 퀘스트가 이미 완료된 상태(processing이 false)라면 UI도 완료 상태로 표시
                if (!userMainQuest.processing)
                {
                    SafeCallToPanel(panel => panel.SetCompletedState());
                }
            }
            else // 모든 퀘스트를 이미 완료한 상태일 경우
            {
                Debug.Log("모든 메인 퀘스트가 이미 완료된 상태로 로드되었습니다.");
                SafeCallToPanel(panel => panel.SetCompletedState()); // 패널에 완료 메시지 표시
                SafeCallToPanel(panel => panel.titleText.text = panel.GetTranslatedOrRaw("AllQuestCompleted")); // 예: "모든 퀘스트 완료!"
            }
        }
    }

    // JSON 파일에서 퀘스트 데이터를 로드하는 함수
    private void LoadQuestDataFromJson()
    {
        TextAsset tAsset = Resources.Load<TextAsset>("Json/MainQuest");
        if (tAsset == null)
        {
            Debug.LogError("MainQuest JSON file not found in Resources/Json/MainQuest. Please check the path and file name.");
            return;
        }

        JSONObject jObj = JSONObject.Parse(tAsset.text);
        if (jObj == null)
        {
            Debug.LogError("Failed to parse MainQuest JSON file. Check JSON format.");
            return;
        }

        JSONArray arr = jObj.GetArray("MyQuest");
        if (arr == null)
        {
            Debug.LogError("JSON array 'MyQuest' not found. Check JSON structure.");
            return;
        }

        mainQuestDatas = new MainQuestData[arr.Length];

        for (int i = 0; i < arr.Length; i++)
        {
            mainQuestDatas[i] = new MainQuestData();
            JSONObject questObj = arr[i].Obj;

            mainQuestDatas[i].mainQuestType = System.Enum.Parse<MainQuestType>(questObj.GetString("MainQuestType"));
            mainQuestDatas[i].No = questObj.GetString("No");
            mainQuestDatas[i].goal = int.Parse(questObj.GetString("Goal"));
            mainQuestDatas[i].goalString = questObj.GetString("GoalString");
            mainQuestDatas[i].title = questObj.GetString("Title"); // 폴백으로 사용될 원본 제목
            mainQuestDatas[i].aconReward1 = int.Parse(questObj.GetString("AconReward1"));
            mainQuestDatas[i].gatchaCoinReward2 = int.Parse(questObj.GetString("GatchaCoinReward2"));
        }
        Debug.Log($"Loaded {mainQuestDatas.Length} main quests from JSON.");
    }


    // 현재 퀘스트 데이터를 패널에 표시하고 초기화하는 함수 (새 퀘스트 시작 시 호출)
    public void StartQuest()
    {
        // 모든 퀘스트를 완료했는지 확인
        if (userMainQuest.curQuestIndex >= mainQuestDatas.Length)
        {
            Debug.Log("모든 메인 퀘스트 완료!");
            userMainQuest.processing = false; // 더 이상 진행할 퀘스트 없음
            SaveMgr.SaveData("UserMainQuest", userMainQuest);
            SafeCallToPanel(panel => panel.SetCompletedState()); // 패널에 완료 메시지 표시
            SafeCallToPanel(panel => panel.titleText.text = panel.GetTranslatedOrRaw("QuestCompleteMessage")); // "모든 퀘스트 완료!"와 같은 메시지
            return;
        }

        MainQuestData data = mainQuestDatas[userMainQuest.curQuestIndex];
        userMainQuest.mainQuestType = data.mainQuestType;
        userMainQuest.process = 0; // 새 퀘스트 시작 시 진행도 초기화
        userMainQuest.processing = true; // 새 퀘스트는 진행 중 상태
        SaveMgr.SaveData("UserMainQuest", userMainQuest);

        SafeCallToPanel(panel => panel.StartQuest(data)); // MainQuestPanel에 새 퀘스트 데이터 전달
        // StartQuest에서는 CheckClear를 바로 호출하지 않습니다.
        // 진행도 업데이트 시 DoQuest -> CheckClear가 호출될 것입니다.
    }

    // 퀘스트 진행도를 업데이트하는 함수 (외부에서 호출)
    public void DoQuest(MainQuestType type)
    {
        // Debug.Log("DoQuest 호출, type: " + type + ", userMainQuest.mainQuestType: " + userMainQuest.mainQuestType); // 디버그 로그 추가

        if (!userMainQuest.processing || userMainQuest.curQuestIndex >= mainQuestDatas.Length)
        {
            // Debug.Log("DoQuest: 퀘스트가 진행 중이 아니거나 모든 퀘스트가 완료됨. 무시.");
            return; // 퀘스트가 진행 중이 아니거나 이미 모든 퀘스트 완료 시 무시
        }

        MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

        if (userMainQuest.mainQuestType == type)
        {
            if (type == MainQuestType.PurchaseKitchen ||
                type == MainQuestType.PurchaseFurniture ||
                type == MainQuestType.PurchaseFox)
            {
                // 구매 퀘스트는 바로 1로 설정하고 클리어 체크
                userMainQuest.process = 1;
                Debug.Log($"DoQuest: Purchase quest process set to {userMainQuest.process}. Checking clear...");
            }
            else if (userMainQuest.process < curQuestData.goal)
            {
                userMainQuest.process++;
                Debug.Log($"DoQuest: Normal quest process incremented to {userMainQuest.process}. Checking clear...");
            }
            SaveMgr.SaveData("UserMainQuest", userMainQuest); // 진행도 저장

            SafeCallToPanel(panel => panel.UpdatePanel()); // 진행도 UI 업데이트

            // 진행도 업데이트 후 퀘스트 완료 여부 확인
            CheckClear();
        }
    }

    // 퀘스트 완료 조건을 확인하는 함수
    public bool CheckClear()
    {
        // 퀘스트가 이미 완료 대기 상태이거나, 모든 퀘스트를 완료했다면 추가 체크 불필요
        if (!userMainQuest.processing || userMainQuest.curQuestIndex >= mainQuestDatas.Length)
        {
            // Debug.Log("CheckClear: Not processing or all quests cleared. Returning false.");
            return false;
        }

        MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];
        bool isCleared = false;

        // --- 퀘스트 타입별 클리어 조건 확인 ---
        if (userMainQuest.mainQuestType == MainQuestType.PurchaseFurniture)
        {
            string key = curQuestData.goalString;
            UserFurniture data = User.Instance.GetUserFurniture(key);
            if (data != null && data.purchased)
            {
                isCleared = true;
            }
        }
        else if (userMainQuest.mainQuestType == MainQuestType.PurchaseFox)
        {
            string key = curQuestData.goalString;
            UserFox data = User.Instance.GetUserFox(key);
            if (data != null && data.purchased)
            {
                isCleared = true;
            }
        }
        else if (userMainQuest.mainQuestType == MainQuestType.PurchaseKitchen)
        {
            string key = curQuestData.goalString;
            UserKitchen data = User.Instance.GetUserKitchen(key);
            if (data != null && data.purchased)
            {
                isCleared = true;
            }
        }
        else // 일반적인 목표 달성 퀘스트 (예: Visit, UseCoin 등)
        {
            if (userMainQuest.process >= curQuestData.goal)
            {
                isCleared = true;
            }
        }

        if (isCleared)
        {
            Debug.Log($"CheckClear: Quest '{curQuestData.No}' cleared condition met! Setting panel to complete state.");
            SafeCallToPanel(panel => panel.SetCompletedState()); // 퀘스트 완료 시 패널 UI만 업데이트
            userMainQuest.processing = false; // 퀘스트가 완료되었음을 표시 (클릭 대기 상태)
            SaveMgr.SaveData("UserMainQuest", userMainQuest); // 완료 상태 저장
            return true;
        }

        return false;
    }

    // ⭐ 새로운 함수: MainQuestPanel 클릭 시 호출되어 실제 보상 지급 및 다음 퀘스트 시작
    public void CompleteAndAdvanceToNextQuest()
    {
        // 모든 퀘스트를 이미 완료했는지 다시 확인
        if (userMainQuest.curQuestIndex >= mainQuestDatas.Length)
        {
            Debug.Log("CompleteAndAdvanceToNextQuest: 모든 퀘스트가 이미 완료되었습니다. 더 이상 진행 불가.");
            return;
        }

        MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

        Debug.Log($"CompleteAndAdvanceToNextQuest: 퀘스트 '{curQuestData.No}' 완료 보상 지급 및 다음 퀘스트 진행!");
        // 보상 지급
        User.Instance.AddGatchaCoin(curQuestData.gatchaCoinReward2);
        User.Instance.AddCoin(curQuestData.aconReward1);

        userMainQuest.curQuestIndex++; // 다음 퀘스트로 인덱스 증가

        // 인덱스 변경 후 상태 저장 및 다음 퀘스트 시작
        SaveMgr.SaveData("UserMainQuest", userMainQuest);
        StartQuest(); // 다음 퀘스트를 시작하고 패널을 업데이트합니다.
    }

    // MainQuestPanel에 안전하게 접근하기 위한 헬퍼 함수
    private void SafeCallToPanel(System.Action<MainQuestPanel> callback)
    {
        if (mainQuestPanel != null)
        {
            callback(mainQuestPanel);
        }
        else
        {
            Debug.LogError("MainQuestPanel 참조가 MainQuestManager의 Inspector에 할당되지 않았습니다! UI 업데이트 불가.");
        }
    }

    // 특정 타입의 퀘스트 데이터를 가져오는 유틸리티 함수 (기존 사용 가정)
    public MainQuestData GetMainQuestData(MainQuestType type)
    {
        foreach (var data in mainQuestDatas)
        {
            if (data.mainQuestType == type)
                return data;
        }
        return null;
    }
}

// MainQuestType (Enum 정의, 기존 사용 가정)
public enum MainQuestType
{
    CallCustomer,
    TakeOrder,
    PickUpAcon,
    PurchaseFurniture,
    PurchaseKitchen,
    PlayGatcha,
    PurchaseFox,
    UpgradeTipBox
}

// MainQuestData (클래스 정의, 기존 사용 가정)
[System.Serializable]
public class MainQuestData
{
    public MainQuestType mainQuestType;
    public string No; // 퀘스트 고유 번호 또는 번역 키 (예: "1", "168")
    public int goal;
    public string goalString; // 구매 퀘스트 등에 사용될 목표 문자열 (예: 가구 ID)
    public string title; // 번역 없을 시 폴백으로 사용될 퀘스트 제목 (원본)
    public int aconReward1;
    public int gatchaCoinReward2;
}

// UserMainQuest (클래스 정의, 기존 사용 가정)
[System.Serializable]
public class UserMainQuest
{
    public int curQuestIndex; // 현재 진행 중인 퀘스트의 인덱스
    public int process; // 현재 퀘스트의 진행도
    public MainQuestType mainQuestType; // 현재 진행 중인 퀘스트의 타입 (저장용)
    public bool processing; // 현재 퀘스트가 진행 중인지 (false면 완료 대기 상태)
}
//using System.Collections;
//using System.Collections.Generic;
//using Boomlagoon.JSON;
//using UnityEngine;

//public class MainQuestManager : MonoBehaviour
//{
//    public static MainQuestManager Instance;
//    public MainQuestData[] mainQuestDatas;
//    public UserMainQuest userMainQuest;

//    public string[] purchaseFurnitureQuestKeys;
//    public string[] purchaseKitchenQuestKeys;

//    public MainQuestPanel mainQuestPanel;


//    private void Awake()
//    {
//        Instance = this;
//    }


//    private void Start()
//    {
//        TextAsset tAsset = Resources.Load<TextAsset>("Json/MainQuest");
//        JSONObject jObj = JSONObject.Parse(tAsset.text);

//        JSONArray arr = jObj.GetArray("MyQuest");
//        mainQuestDatas = new MainQuestData[arr.Length];

//        for (int i = 0; i < arr.Length; i++)
//        {
//            mainQuestDatas[i] = new MainQuestData();

//            mainQuestDatas[i].mainQuestType = System.Enum.Parse<MainQuestType>(arr[i].Obj.GetString("MainQuestType"));
//            mainQuestDatas[i].No = arr[i].Obj.GetString("No");
//            mainQuestDatas[i].goal = int.Parse(arr[i].Obj.GetString("Goal"));
//            mainQuestDatas[i].goalString = arr[i].Obj.GetString("GoalString");
//            mainQuestDatas[i].title = arr[i].Obj.GetString("Title");
//            mainQuestDatas[i].aconReward1 = int.Parse(arr[i].Obj.GetString("AconReward1"));
//            mainQuestDatas[i].gatchaCoinReward2 = int.Parse(arr[i].Obj.GetString("GatchaCoinReward2"));
//        }

//        userMainQuest = SaveMgr.LoadData<UserMainQuest>("UserMainQuest");

//        if (userMainQuest == null)
//        {
//            userMainQuest = new UserMainQuest
//            {
//                curQuestIndex = 0,
//                process = 0,
//                processing = true
//            };
//            Debug.Log("[새로운 UserMainQuest 생성]");
//            StartQuest();
//        }
//        else
//        {
//            Debug.Log($"[UserMainQuest 로드] 인덱스: {userMainQuest.curQuestIndex}, 타입: {userMainQuest.mainQuestType}, 진행도: {userMainQuest.process}, 진행 중: {userMainQuest.processing}");
//            SafeCallToPanel(panel => panel.StartQuest(mainQuestDatas[userMainQuest.curQuestIndex]));
//            CheckClear();
//        }

//        SaveMgr.SaveData("UserMainQuest", userMainQuest);
//    }

//    public void StartQuest()
//    {
//        if (mainQuestDatas.Length <= userMainQuest.curQuestIndex)
//            return;

//        MainQuestData data = mainQuestDatas[userMainQuest.curQuestIndex];
//        userMainQuest.mainQuestType = data.mainQuestType;


//        userMainQuest.process = 0;
//        userMainQuest.processing = true;
//        SaveMgr.SaveData("UserMainQuest", userMainQuest);

//        SafeCallToPanel(panel => panel.StartQuest(data));
//        CheckClear();
//    }

//    //public void DoQuest(MainQuestType type)
//    //{
//    //    // ... (생략)
//    //    if (userMainQuest.mainQuestType == type)
//    //    {
//    //        MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

//    //        if (type == MainQuestType.PurchaseKitchen || type == MainQuestType.PurchaseFurniture || type == MainQuestType.PurchaseFox)
//    //        {
//    //            userMainQuest.process = 1; // 구매 퀘스트는 process를 1로 설정
//    //            CheckClear(); // 이 안에서 CompleteQuest()를 호출할 수 있음
//    //                          // 여기에 SafeCallToPanel(panel => panel.UpdatePanel()); 이 있어야 합니다.
//    //                          // 현재 코드는 Purchase 타입에서 UpdatePanel()을 호출하고 있습니다.
//    //                          // 따라서, 이 부분이 예상대로 작동하는지 Debug.Log로 확인이 필요합니다.
//    //            SafeCallToPanel(panel => panel.UpdatePanel());
//    //        }
//    //        else if (userMainQuest.process < curQuestData.goal)
//    //        {
//    //            userMainQuest.process++;
//    //            SaveMgr.SaveData("UserMainQuest", userMainQuest);
//    //            CheckClear();
//    //            // 일반 퀘스트도 process가 변경되었으므로 UpdatePanel() 호출이 필요합니다.
//    //            // 현재 코드에는 이 부분이 잘 구현되어 있습니다.
//    //            SafeCallToPanel(panel => panel.UpdatePanel());
//    //        }
//    //    }
//    //}
//    public void DoQuest(MainQuestType type)
//    {
//        Debug.Log("DoQuest 호출, type: " + type + ", userMainQuest.mainQuestType: " + userMainQuest.mainQuestType);

//        if (userMainQuest.mainQuestType == type)
//        {
//            MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

//            if (type == MainQuestType.PurchaseKitchen ||
//                type == MainQuestType.PurchaseFurniture ||
//                type == MainQuestType.PurchaseFox)
//            {
//                userMainQuest.process = 1;
//                CheckClear();
//                SafeCallToPanel(panel => panel.UpdatePanel());
//            }
//            else if (userMainQuest.process < curQuestData.goal)
//            {
//                userMainQuest.process++;
//                SaveMgr.SaveData("UserMainQuest", userMainQuest);
//                CheckClear();
//                SafeCallToPanel(panel => panel.UpdatePanel());
//            }
//        }
//    }


//public bool CheckClear()
//    {
//        if (!userMainQuest.processing || userMainQuest.curQuestIndex >= mainQuestDatas.Length)
//            return false;

//        MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

//        bool isCleared = false;

//        // ... (기존 퀘스트 타입별 클리어 조건 확인 로직은 그대로) ...

//        if (isCleared)
//        {
//            // 퀘스트가 클리어되면 MainQuestPanel에게 UI만 '완료' 상태로 바꾸라고 지시합니다.
//            SafeCallToPanel(panel => panel.SetCompletedState());

//            // ⭐ 중요: 여기서 userMainQuest.processing을 false로 설정하여
//            // 퀘스트가 '완료되었지만 아직 보상은 받지 않은, 클릭 대기' 상태임을 표시합니다.
//            userMainQuest.processing = false;
//            SaveMgr.SaveData("UserMainQuest", userMainQuest); // 이 상태를 저장합니다.
//            return true;
//        }

//        return false;
//    }

//    // ⭐ 새로 추가하거나 수정된 함수 (MainQuestPanel에서 클릭했을 때 호출될 함수)
//    public void CompleteAndAdvanceToNextQuest() // 함수명을 더 명확하게 변경했습니다.
//    {
//        if (userMainQuest.curQuestIndex >= mainQuestDatas.Length)
//        {
//            Debug.Log("모든 퀘스트가 이미 완료되었습니다.");
//            return;
//        }

//        MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

//        Debug.Log("퀘스트 완료 보상 지급 및 다음 퀘스트 진행!");
//        User.Instance.AddGatchaCoin(curQuestData.gatchaCoinReward2);
//        User.Instance.AddCoin(curQuestData.aconReward1);

//        userMainQuest.curQuestIndex++; // 다음 퀘스트로 인덱스 증가

//        // 다음 퀘스트를 시작합니다.
//        // StartQuest 내부에서 userMainQuest.processing = true; 가 다시 설정될 것입니다.
//        SaveMgr.SaveData("UserMainQuest", userMainQuest); // 인덱스 변경 후 저장
//        StartQuest();
//    }
//    //public bool CheckClear()
//    //{
//    //    if (!userMainQuest.processing)
//    //        return false;

//    //    MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

//    //    if (userMainQuest.mainQuestType == MainQuestType.PurchaseFurniture)
//    //    {
//    //        string key = curQuestData.goalString;
//    //        UserFurniture data = User.Instance.GetUserFurniture(key);
//    //        if (data != null && data.purchased)
//    //        {
//    //            SafeCallToPanel(panel => panel.CompleteQuest());
//    //            return true;
//    //        }
//    //    }
//    //    else if (userMainQuest.mainQuestType == MainQuestType.PurchaseFox)
//    //    {
//    //        string key = curQuestData.goalString;
//    //        UserFox data = User.Instance.GetUserFox(key);
//    //        if (data != null && data.purchased)
//    //        {
//    //            SafeCallToPanel(panel => panel.CompleteQuest());
//    //            return true;
//    //        }
//    //    }
//    //    else if (userMainQuest.mainQuestType == MainQuestType.PurchaseKitchen)
//    //    {
//    //        string key = curQuestData.goalString;
//    //        UserKitchen data = User.Instance.GetUserKitchen(key);
//    //        if (data != null && data.purchased)
//    //        {
//    //            SafeCallToPanel(panel => panel.CompleteQuest());
//    //            return true;
//    //        }
//    //    }
//    //    else
//    //    {
//    //        if (userMainQuest.process >= curQuestData.goal)
//    //        {
//    //            SafeCallToPanel(panel => panel.CompleteQuest());
//    //            return true;
//    //        }
//    //    }

//    //    return false;
//    //}

//    public void CompleteCurrentQuest()
//    {
//        MainQuestData curQuestData = mainQuestDatas[userMainQuest.curQuestIndex];

//        Debug.Log("퀘스트 완료!");
//        userMainQuest.curQuestIndex++;
//        userMainQuest.processing = false;
//        User.Instance.AddGatchaCoin(curQuestData.gatchaCoinReward2);
//        User.Instance.AddCoin(curQuestData.aconReward1);

//        SaveMgr.SaveData("UserMainQuest", userMainQuest);
//        StartQuest();
//    }

//    private void SafeCallToPanel(System.Action<MainQuestPanel> callback)
//    {
//        callback(mainQuestPanel);
//    }

//    public MainQuestData GetMainQuestData(MainQuestType type)
//    {
//        foreach (var data in mainQuestDatas)
//        {
//            if (data.mainQuestType == type)
//                return data;
//        }
//        return null;
//    }
//}

//[System.Serializable]
//public class UserMainQuest
//{
//    public MainQuestType mainQuestType;
//    public int curQuestIndex;
//    public int process;
//    public bool processing;
//    public int clearPurchaseFurnitureCount;
//    public int clearPurchaseKitchenCount;
//}

//[System.Serializable]
//public class MainQuestData
//{
//    public MainQuestType mainQuestType;
//    public string No;
//    public int goal;
//    public string goalString;
//    public string title;
//    public int aconReward1;
//    public int gatchaCoinReward2;


//    public string GetGoal()
//    {
//        if (mainQuestType == MainQuestType.PurchaseFurniture)
//        {
//            int idx = MainQuestManager.Instance.userMainQuest.clearPurchaseFurnitureCount;
//            if (idx >= MainQuestManager.Instance.purchaseFurnitureQuestKeys.Length)
//                return null;
//            return MainQuestManager.Instance.purchaseFurnitureQuestKeys[idx];
//        }
//        else if (mainQuestType == MainQuestType.PurchaseKitchen)
//        {
//            int idx = MainQuestManager.Instance.userMainQuest.clearPurchaseKitchenCount;
//            if (idx >= MainQuestManager.Instance.purchaseKitchenQuestKeys.Length)
//                return null;
//            return MainQuestManager.Instance.purchaseKitchenQuestKeys[idx];
//        }
//        return null;
//    }
//}

//public enum MainQuestType
//{
//    CallCustomer,
//    TakeOrder,
//    PickUpAcon,
//    PurchaseFurniture,
//    PurchaseKitchen,
//    PlayGatcha,
//    PurchaseFox,
//    UpgradeTipBox
//}

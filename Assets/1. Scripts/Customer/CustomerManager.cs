using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    public Transform enterance; // 입구 위치
    public CustomerData[] customerDatas;
    public List<UserCustomer> userCustomers = new List<UserCustomer>(); // 손님의 종류 리스트
    public List<Customer> customerPrefabs = new List<Customer>(); // 손님의 종류 리스트
    public List<Customer> waitingCustomers = new List<Customer>(); //대기 손님

    public RestaurantManager restaurantManager;
    public static CustomerManager Instance;

    public FatJinsangCustomer fatThiefPrefab;
    public JinsangCustomer thiefPrefab;
    public KickOutCustomer kickoutPrefab;

    public Jinsang[] jinsangPrefabs;

    public PinkPanda pinkPandaPrefab; // PinkPanda 프리팹 연결 (씬에서 FindObjectOfType으로 찾을 예정)
    public redFox redFoxPrefab; // RedFox 유닛 오브젝트 연결 (씬에서 Find로 찾을 예정)

    float minsec = 5f;
    float maxsec = 25f;

    // 각 유닛별 고유 속도를 위한 변수 추가
    [Header("Pink Panda Speeds")]
    public float pinkPandaMinSec = 2f; // 핑크판다 있을 때 최소 시간
    public float pinkPandaMaxSec = 6f;   // 핑크판다 있을 때 최대 시간

    [Header("Red Fox Speeds")]
    public float redFoxMinSec = 4f;   // 레드폭스 있을 때 최소 시간
    public float redFoxMaxSec = 20f;     // 레드폭스 있을 때 최대 시간

    public bool isFastEnterActive = false; // 패스트 엔터 활성화 여부 (현재 어떤 강화가 적용되었는지 상태)


    public Image inviteGageBar;

    private void Awake()
    {
        Instance = this;

        InvokeRepeating("EnterJinsang", 60f, Random.Range(90f, 150f));
        InvokeRepeating("CallEnterCustomer", 0f, 1f);
    }

    private void CallEnterCustomer()
    {
        if (waitingCustomers.Count <= 5)
        {
            float chance = Random.Range(0f, 1f);
            float spawnProbability = 1f / Random.Range(minsec, maxsec);

            if (chance <= spawnProbability)
            {
                EnterCustomer();
            }
        }
    }

    // 이 FastEnter 함수는 이제 직접 호출하지 않고, Update에서 속도 값을 설정하는 용도로 사용됩니다.
    public void SetCustomerSpawnSpeed(float newMinSec, float newMaxSec, bool enableFastEnter)
    {
        minsec = newMinSec;
        maxsec = newMaxSec;
        isFastEnterActive = enableFastEnter; // 어떤 강화가 활성화되었는지 상태 업데이트

        //if (enableFastEnter)
        //{
        //    Debug.Log($"손님 등장 속도 활성화! 간격: {minsec}초 ~ {maxsec}초");
        //}
        //else
        //{
        //    Debug.Log($"손님 등장 속도 비활성화 (기본)! 간격: {minsec}초 ~ {maxsec}초");
        //}
    }

    public Jinsang jinsang;

    public void EnterJinsang()
    {
        int randomIdx = Random.Range(0, jinsangPrefabs.Length);
        if (jinsang == null)
        {
            jinsang = Instantiate(jinsangPrefabs[randomIdx]);
            jinsang.Enter();
        }
    }


    public void EnterCustomer()
    {
        if (waitingCustomers.Count > 5)
        {
            Invoke("EnterCustomer", Random.Range(minsec, maxsec));
            return;
        }
        MainQuestManager.Instance.DoQuest(MainQuestType.CallCustomer);

        List<UserCustomer> activeUserCustomers = User.Instance.userData.userCustomers.FindAll(customer => customer.open);
        if (activeUserCustomers.Count > 0)
        {
            int randomIndex = Random.Range(0, activeUserCustomers.Count);
            string customerKey = activeUserCustomers[randomIndex].key;

            Customer customerPrefab = FindCustomerPrefabByKey(customerKey);

            if (customerPrefab != null)
            {
                Customer newCustomerObject = Instantiate(customerPrefab, enterance.position, Quaternion.identity);
                Customer newCustomer = newCustomerObject.GetComponent<Customer>();
                newCustomer.Enter();
                waitingCustomers.Add(newCustomer);
                SortCustomer();
            }
            else
            {
                Debug.LogError("Customer prefab not found for key: " + customerKey);
            }
        }
        else
        {
            Debug.LogWarning("No active customers available.");
        }
    }

    public Customer FindCustomerPrefabByKey(string key)
    {
        CustomerData foundCustomerData = null;
        for (int i = 0; i < customerDatas.Length; i++)
        {
            if (customerDatas[i].key == key)
            {
                foundCustomerData = customerDatas[i];
                break;
            }
        }

        if (foundCustomerData != null)
        {
            for (int i = 0; i < customerPrefabs.Count; i++)
            {
                if (customerPrefabs[i].name.Contains(foundCustomerData.key))
                {
                    return customerPrefabs[i];
                }
            }
        }
        return null;
    }

    private void Update()
    {
        if (waitingCustomers.Count > 0)
        {
            AssignCustomerToTable();
        }

        PinkPanda existingPinkPanda = FindObjectOfType<PinkPanda>();
        GameObject existingRedFox = GameObject.Find("RedFox");

        // 어떤 유닛이 있는지에 따라 속도를 설정
        if (existingPinkPanda != null)
        {
            // 핑크판다가 있을 때의 속도 적용
            SetCustomerSpawnSpeed(pinkPandaMinSec, pinkPandaMaxSec, true);
        }
        else if (existingRedFox != null)
        {
            // 레드폭스가 있을 때의 속도 적용 (핑크판다가 없을 때만 적용)
            SetCustomerSpawnSpeed(redFoxMinSec, redFoxMaxSec, true);
        }
        else
        {
            // 둘 다 없을 때 기본 속도 적용
            SetCustomerSpawnSpeed(3f, 15f, false);
        }
    }

    private Table GetRandomAvailableTable()
    {
        return restaurantManager.GetRandomAvailableTable();
    }

    public void AssignCustomerToTable()
    {
        if (waitingCustomers.Count > 0)
        {
            Table table = GetRandomAvailableTable();
            if (table != null)
            {
                Customer customer = waitingCustomers[0];
                customer.SetTarget(table);
                waitingCustomers.RemoveAt(0);
            }
        }
    }

    void SortCustomer()
    {
        for (int i = 0; i < waitingCustomers.Count; i++)
        {
            float randomX = Random.Range(-0.3f, 0.3f);
            float plusY = i * 0.6f;
            waitingCustomers[i].transform.position = (Vector2)enterance.position + new Vector2(randomX, plusY);
        }
    }
}

[System.Serializable]
public class CustomerData
{
    public string key;
    public int order;
    public Sprite thum;
}
//using UnityEngine;
//using System.Collections.Generic;
//using UnityEngine.UI;

//public class CustomerManager : MonoBehaviour
//{
//    public Transform enterance; // 입구 위치
//    public CustomerData[] customerDatas;
//    public List<UserCustomer> userCustomers = new List<UserCustomer>(); // 손님의 종 리스트
//    public List<Customer> customerPrefabs = new List<Customer>(); // 손님의 종 리스트
//    public List<Customer> waitingCustomers = new List<Customer>(); //대기 손님

//    public RestaurantManager restaurantManager;
//    public static CustomerManager Instance;

//    public FatJinsangCustomer fatThiefPrefab;
//    public JinsangCustomer thiefPrefab;
//    public KickOutCustomer kickoutPrefab;

//    public Jinsang[] jinsangPrefabs;

//    public PinkPanda pinkPandaPrefab; // PinkPanda 프리팹 연결

//    float minsec =5f;
//    float maxsec=25f;

//    public bool isFastEnterActive = false; // 패스트 엔터 활성화 여부


//    public Image inviteGageBar;

//    //public Customer normalCustomerPrefab;
//    //public Customer obCustomerPrefab;


//    private void Awake()
//    {
//        Instance = this; //this 자신의 객체 = User 스크립

//        InvokeRepeating("EnterJinsang", 60f, Random.Range(90f, 150f));
//        //InvokeRepeating("EnterFatJinsang", Random.Range(40f, 90f), Random.Range(120f, 180f));
//        //InvokeRepeating("EnterKickoutJinsang", Random.Range(70f, 120f), Random.Range(150f, 210f));

//        InvokeRepeating("CallEnterCustomer", 0f, 1f);
//    }
//    private void CallEnterCustomer()
//    {
//        // 현재 대기 중인 손님 수가 최대치를 넘지 않았는지 확인
//        if (waitingCustomers.Count <= 5)
//        {
//            // 확률적으로 손님 생성 (호출 빈도를 조절하여 간접적으로 생성 확률 조절)
//            float chance = Random.Range(0f, 1f);
//            float spawnProbability = 1f / Random.Range(minsec, maxsec); // 현재 간격에 따른 확률 조정

//            if (chance <= spawnProbability)
//            {
//                EnterCustomer();
//            }
//        }
//    }

//    public void FastEnter(bool enable)
//    {
//        isFastEnterActive = enable;
//        if (isFastEnterActive)
//        {
//            minsec = 1f;
//            maxsec = 3f;
//            Debug.Log("패스트 엔터 활성화!");
//        }
//        else
//        {
//            minsec = 3f;
//            maxsec = 15f;
//            Debug.Log("패스트 엔터 비활성화!");
//        }

//        // 현재 예약된 EnterCustomer 함수를 취소하고 새로운 간격으로 다시 예약
//        //CancelInvoke("EnterCustomer");
//        //Invoke("EnterCustomer", Random.Range(minsec, maxsec));
//    }


//    public Jinsang jinsang;


//    public void EnterJinsang()
//    {

//        int randomIdx = Random.Range(0, jinsangPrefabs.Length);
//        if (jinsang == null)
//        {
//            jinsang = Instantiate(jinsangPrefabs[randomIdx]);
//            jinsang.Enter();
//        }

//    }






//    public void EnterCustomer()
//    {
//        if (waitingCustomers.Count > 5)
//        {
//            Invoke("EnterCustomer", Random.Range(minsec, maxsec));

//            return;
//        }
//        MainQuestManager.Instance.DoQuest(MainQuestType.CallCustomer);
//        //int randomIndex = Random.Range(0, userCustomers.Count);
//        //Customer newCustomerObject = Instantiate(userCustomers[randomIndex], enterance.position, Quaternion.identity);

//        List<UserCustomer> activeUserCustomers = User.Instance.userData.userCustomers.FindAll(customer => customer.open);
//        if (activeUserCustomers.Count > 0)
//        {
//            int randomIndex = Random.Range(0, activeUserCustomers.Count);
//            string customerKey = activeUserCustomers[randomIndex].key;

//            // customerKey에 해당하는 Customer 프리팹을 찾습니다.
//            Customer customerPrefab = FindCustomerPrefabByKey(customerKey);

//            if (customerPrefab != null)
//            {
//                Customer newCustomerObject = Instantiate(customerPrefab, enterance.position, Quaternion.identity);
//                Customer newCustomer = newCustomerObject.GetComponent<Customer>();
//                newCustomer.Enter();
//                waitingCustomers.Add(newCustomer);
//                SortCustomer();
//                //AssignCustomerToTable();
//            }
//            else
//            {
//                Debug.LogError("Customer prefab not found for key: " + customerKey);
//            }
//        }
//        else
//        {
//            Debug.LogWarning("No active customers available.");
//        }
//        //int randomIndex = Random.Range(0, customerPrefabs.Count);
//        //Customer newCustomerObject = Instantiate(customerPrefabs[randomIndex], enterance.position, Quaternion.identity);

//        //Customer newCustomer = newCustomerObject.GetComponent<Customer>();
//        //newCustomer.Enter();
//        //waitingCustomers.Add(newCustomer);
//        //SortCustomer();
//        //AssignCustomerToTable();

//    }
//    public Customer FindCustomerPrefabByKey(string key)
//        {
//        // key에 해당하는 CustomerData를 찾습니다.
//        CustomerData foundCustomerData = null;
//        for (int i = 0; i < customerDatas.Length; i++)
//        {
//            if (customerDatas[i].key == key)
//            {
//                foundCustomerData = customerDatas[i];
//                break;
//            }
//        }

//        if (foundCustomerData != null)
//        {
//            // foundCustomerData를 기반으로 Customer 프리팹을 찾습니다.
//            for (int i = 0; i < customerPrefabs.Count; i++)
//            {
//                // Customer 프리팹에 CustomerData가 없으므로, 다른 방법으로 비교해야 합니다.
//                // 예를 들어, Customer 프리팹의 이름이나 태그를 사용하여 비교할 수 있습니다.
//                // 여기서는 Customer 프리팹의 이름에 CustomerData의 key가 포함되어 있다고 가정합니다.
//                if (customerPrefabs[i].name.Contains(foundCustomerData.key))
//                {
//                    return customerPrefabs[i];
//                }
//            }
//        }

//        return null;
//    }

//    private void Update()
//    {
//        if (waitingCustomers.Count > 0)
//        {
//            AssignCustomerToTable(); // 매 프레임마다 빈 테이블 확인 및 배정
//        }
//        PinkPanda existingPinkPanda = FindObjectOfType<PinkPanda>();

//        if (existingPinkPanda != null && !isFastEnterActive)
//        {
//            FastEnter(true);
//        }
//        else if (existingPinkPanda == null && isFastEnterActive)
//        {
//            FastEnter(false);
//        }


//    }

//    // 빈 테이블 찾기 (RestaurantManager와 연동)
//    private Table GetRandomAvailableTable()
//    {
//        return restaurantManager.GetRandomAvailableTable();
//    }

//    public void AssignCustomerToTable() 
//    {
//        if (waitingCustomers.Count > 0)
//        {
//            Table table = GetRandomAvailableTable();
//            if (table != null)
//            {
//                Customer customer = waitingCustomers[0];
//                customer.SetTarget(table);
//                waitingCustomers.RemoveAt(0);
//            }
//        }
//    }

//    void SortCustomer()
//    {
//        for (int i = 0; i < waitingCustomers.Count; i++)
//        {
//            float randomX = Random.Range(-0.3f, 0.3f);
//            float plusY = i*0.6f;
//            waitingCustomers[i].transform.position = (Vector2)enterance.position + new Vector2(randomX, plusY);
//            // **기존 애들 포지션도 바뀌는데 왜 그?
//        }

//    }
//}

//[System.Serializable]
//public class CustomerData
//{
//    public string key;
//    public int order;
//    public Sprite thum;


//}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RestaurantManager : MonoBehaviour
{
    public Table[] tables;
    public CustomerManager customerManager; // CustomerManager 스크립트 참조
    private List<Table> availableTables = new List<Table>();
    //Table[] foundTables = FindObjectsOfType<Table>();
    public static RestaurantManager Instance;
    public TutorialManager tutorialManager;

    public GameObject[] MoveAroundPoints;

    public Transform center;

    public Button restaurantButton;


    private void Awake()
    {
        Instance = this; //this 자신의 객체 = User 스크립
    }

    private void Start()
    {

        int firstOpen = PlayerPrefs.GetInt("FirstOpen");
        if (firstOpen == 0)
        {
            // 튜토리얼 컷툰 시작
            if (tutorialManager != null) // tutorialManager가 할당되었는지 확인
            {
                Debug.Log("튜토리얼오");
                tutorialManager.StartTutorial();
            }
            PlayerPrefs.SetInt("FirstOpen", 1);
        }

        tables = FindObjectsOfType<Table>();
        StartArea();

        //**진상 코루StartCoroutine(CoAddJinsang());
        //Invoke("A", 10);

    }

    public void StartArea()
    {
        restaurantButton.gameObject.SetActive(false);
    }


    public void EndArea()
    {
        restaurantButton.gameObject.SetActive(true);
    }

    //public void A()
    //{
    //    CustomerManager.Instance.JinsangCustomer();
    //    Invoke("A", 10);
    //}

    //코루틴 함수 선언 방법

    //IEnumerator CoAddJinsang() //첫번째 반환형은 IEnumerator
    //{
    //    yield return new WaitForSeconds(2); //두번째 함수 내에 yield return 키워드 반드시 필요
    //    Debug.Log("진상손님소환하기");
    //    CustomerManager.Instance.JinsangCustomer();
    //}

    public void UpdateFurniture()
    {

    }

    //float timer = 0;

    private void Update()
    {



        if (Input.GetMouseButtonDown(0))
        {
            Vector2 screenPoint = Input.mousePosition;
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

            //Debug.Log("screenpoint" + screenPoint);
            //Debug.Log("worldPoint" + worldPoint);
            Collider2D[] cols = Physics2D.OverlapPointAll(worldPoint, LayerMask.GetMask("Pickable")); //컴포넌트 콜리더가 배열로 담


            if (cols.Length <= 0)
            {
                return;
            }
            for (int i = 0; i < cols.Length; i++)
            {
                Coin coin = cols[i].GetComponent<Coin>();
                coin.Picked();
            }

        }

    }


    public Table GetRandomAvailableTable()
    {

        availableTables.Clear(); // 매 호출마다 빈 테이블 리스트 초기화
        for (int i =0; i< FurnitureManager.Instance.tablePlaces.Length;  i++)
        {
            Table table = FurnitureManager.Instance.tablePlaces[i].GetTable();
            if (table != null && table.isOccupied == false) 
            {
                availableTables.Add(table);
            }
            //씬상에 존재하고 있는 테이블
        }

        // 모든 테이블을 순회하며 빈 테이블만 availableTables에 추가

        // 빈 테이블이 하나도 없으면 null 반환
        if (availableTables.Count == 0)
        {
            return null;
        }

        // 빈 테이블 리스트에서 랜덤으로 하나 선택
        int randomIndex = Random.Range(0, availableTables.Count);
        return availableTables[randomIndex];

    }

    
}

//public Table FindRandomAvailableTable()
//{

//    foreach (Table table in tables)
//    {
//        if (!table.isOccupied)
//        {
//            return table;
//        }
//    }
//    return null; // 빈 테이블이 없으면 null 반환
//}
//    void Start()
//{
//    tables = FindObjectsOfType<Table>();
//}

//public Table FindAvailableTable()
//{
//    foreach (Table table in tables)
//    {
//        if (!table.IsOccupied())
//        {
//            return table; // 빈 테이블 반환
//        }
//    }
//    return null; // 빈 테이블이 없으면 null 반환
//}

//void Update()
//{



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class RestaurantManager : MonoBehaviour
//{

//    public Table[] tables;


//    void Start()
//    {
//        tables = FindObjectsOfType<Table>();
//    }

//    public void CheckOrAddCustomer()
//    {

//    }

//    void Update()
//    {

//    }
//}

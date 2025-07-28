using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Customer : MonoBehaviour
{

    Vector2 enterancePosition = new Vector2(4f, 5f);
    public Table targetTable; // 목표 테이블
    public float moveSpeed = 1f; // 이동 속도
    public bool moving;
    public Coin coinPrefab;
    public FurnitureManager fm;
    public RestaurantManager rm;
    public TablePlace tablePlace;



    public OrderCanvas orderCanvas;

    //public Image orderMenuImage;
    //public GameObject bubbleCanvas;


    public void Start()
    {
        orderCanvas = GetComponentInChildren<OrderCanvas>(true);
        FurnitureManager.Instance.UpdateFurniture();
    }

    public void ReceiveMenu()
    {
        if (targetTable == null)
        {
            return;
        }

        if (targetTable != null)
        {
            Debug.Log("ReceiveMenu");
            targetTable.SetTableMenuImage(orderMenu); // 수정된 함수 호출
        }

        StartCoroutine(Pay());
    }

    public void SetTarget(Table table)
    {
        if (table.isOccupied) // 이미 점유된 테이블인지 확인
        {
            Debug.LogWarning($"테이블 {table.name}은 이미 점유 중입니다.");
            return;
        }
        table.isOccupied = true;

        targetTable = table;
        moving = true;
        //table.OccupyTable(); // 테이블 점유
        rm = RestaurantManager.Instance;
        StartCoroutine(CoProcessTable(table));
    }




    IEnumerator CoProcessTable(Table table)
    {
        while(true)
        { 
            Vector2 targetPosition = targetTable.sitPointTr.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetTable.sitPointTr.position) < 0.5f)
            {
                transform.position = targetTable.sitPointTr.position;
                SitTable();
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(2);
        //랜덤 이동 처리
    }

    public void Enter()
    {
        transform.position = CustomerManager.Instance.enterance.position;
    }

    public void Exit()
    { }



    public IEnumerator StartRandomMove()
    {
        yield return new WaitForSeconds(2f); // 2초 대기
        Debug.Log("랜덤 이동 시작");
        if (targetTable != null)
        {
            targetTable.VacateTable();
        }

        targetTable = null;
        if (rm.MoveAroundPoints != null && rm.MoveAroundPoints.Length > 0) // 배열이 비어 있지 않은 경우
        {
            int randomPositionPick = Random.Range(0, rm.MoveAroundPoints.Length);
            Vector2 randomPosition = rm.MoveAroundPoints[randomPositionPick].transform.position;

            // 이동 로직 실행
            while (Vector2.Distance(transform.position, randomPosition) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, randomPosition, moveSpeed * Time.deltaTime);
                yield return null; // 다음 프레임까지 대기
            }

            Debug.Log("랜덤 이동 완료");

            StartCoroutine(StartExitMove(2));
        }
    }

    public IEnumerator StartExitMove(float wait)
    {
        yield return new WaitForSeconds(wait); // 2초 대기

        if (targetTable != null)
        {
            targetTable.customer = null; // 👈 이 줄도 꼭 추가!
            targetTable.VacateTable();
        }


        Debug.Log("출구 이동 시작");
        while (Vector2.Distance(transform.position, enterancePosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, enterancePosition, moveSpeed * Time.deltaTime);
            yield return null;
            // 목적지에 도착하면 오브젝트 삭제
            if (Vector2.Distance(transform.position, enterancePosition) < 0.1f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public  void Update()
    {

        if (targetTable == null)
            return;

        if (moving)
        {

        }

            }

    MenuData orderMenu;
    Coroutine waitAcceptCoroutine;

    public void SitTable()
    {
        moving = false;
        if (targetTable != null)
        {
            targetTable.Taken(this); // 👈 여기!
        }

        OrderMenu();
        waitAcceptCoroutine = StartCoroutine(CoWaitAccept());
    }

    public void OrderMenu()
    {
        orderMenu = MenuManager.Instance.GetRandomMenuData();
        orderCanvas.SetOrderMenu(orderMenu);

    }

    public void AcceptOrder()
    {
        orderCanvas.gameObject.SetActive(false);
        KitchenManager.Instance.Order(orderMenu, this);
        StopCoroutine(waitAcceptCoroutine);
    }

    IEnumerator CoWaitAccept() 
    {
        yield return new WaitForSeconds(5);
        orderCanvas.gameObject.SetActive(false);
        StartCoroutine(StartExitMove(2));
    }

    IEnumerator Pay()
    {
        yield return new WaitForSeconds(5f);
        Coin coin = Instantiate(coinPrefab);
        coin.SetPrice(orderMenu.price);
        coin.transform.position = (Vector2)transform.position + Vector2.up * 0.5f;
        Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();

        if (rb != null)

        {
            rb.AddForce(Vector2.up * 200f);
            rb.AddForce(Vector2.right * 60f);
        }

        StartCoroutine(StartRandomMove());

    }

    public void KickOut()
    {
        orderCanvas.gameObject.SetActive(false);

        // 주문이 들어간 상태라면 주방에서 취소
        KitchenManager.Instance.CancelOrder(this);

        // 대기 중인 주문 코루틴도 정지
        if (waitAcceptCoroutine != null)
        {
            StopCoroutine(waitAcceptCoroutine);
            waitAcceptCoroutine = null;
        }

        StartCoroutine(StartExitMove(0));
    }



}


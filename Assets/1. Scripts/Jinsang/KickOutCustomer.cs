using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickOutCustomer : Jinsang
{

    public float moveSpeed = 2f; // 악당 이동 속도
    private Customer target; // 쫓아낼 목표 손님
    Vector2 enterPosition = new Vector2(4f, 5f);
    public bool isExiting; // 퇴장 중인지 여부를 나타내는 변수
    public int touchCount;
    public AudioSource clickSound;

    public override void Enter()
    {
        touchCount = 0;
        isExiting = false; // 퇴장 상태 초기화
        transform.position = CustomerManager.Instance.enterance.position;

        StartCoroutine(CoJinsang());
    }


    IEnumerator CoJinsang()
    {
        Debug.Log("[진상] 타겟 손님을 지속적으로 감시합니다.");

        while (!isExiting)
        {
            List<Customer> curSitDownCustomers = new List<Customer>();

            for (int i = 0; i < RestaurantManager.Instance.tables.Length; i++)
            {
                Table table = RestaurantManager.Instance.tables[i];
                if (table == null || table.customer == null) continue;

                curSitDownCustomers.Add(table.customer);
            }

            if (curSitDownCustomers.Count > 0)
            {
                target = curSitDownCustomers[Random.Range(0, curSitDownCustomers.Count)];
                Debug.Log($"[진상] 타겟 설정: {target.name}");
                yield return StartCoroutine(MoveToCustomer());
            }

            yield return new WaitForSeconds(0.5f); // 대기하면서 다음 타겟 대기
        }
    }
    //IEnumerator CoJinsang()
    //{
    //    List<Customer> curSitDownCustomers = new List<Customer>();
    //    for (int i = 0; i < RestaurantManager.Instance.tables.Length; i++)
    //    {
    //        if (RestaurantManager.Instance.tables[i] == null)
    //        {
    //            Debug.Log($"[{i}] 테이블이 null");
    //            continue;
    //        }
    //        if (RestaurantManager.Instance.tables[i].customer == null)
    //        {
    //            Debug.Log($"[{i}] 테이블의 customer가 null");
    //            continue;
    //        }

    //        curSitDownCustomers.Add(RestaurantManager.Instance.tables[i].customer);
    //        Debug.Log($"[{i}] 손님 추가됨: {RestaurantManager.Instance.tables[i].customer.name}");

    //    }

    //    if (curSitDownCustomers.Count <= 0)
    //    {
    //        yield break;
    //    }


    //    target = curSitDownCustomers[Random.Range(0, curSitDownCustomers.Count)];
    //    Debug.Log("타겟설정");
    //    StartCoroutine(MoveToCustomer());
    //    //target.KickOut();

    //}

    IEnumerator MoveToCustomer()
    {
        if (target == null)
        {
            Debug.Log("[진상] 타겟이 null입니다. 이동 취소.");
            yield break;
        }

        while (target != null)
        {
            Vector2 targetPosition = target.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.6f)
            {
                target.KickOut();
                break; // 이동 완료되었으니 빠져나감
            }

            yield return null;
        }

        target = null; // 타겟 초기화
        yield return new WaitForSeconds(0.5f); // 약간 쉬고 다음 루프로
    }
    //IEnumerator MoveToCustomer()
    //{
    //    while (target != null)
    //    {
    //        Vector2 targetPosition = target.transform.position;
    //        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

    //        if (Vector2.Distance(transform.position, targetPosition) < 0.6f) // 가까워지면 쫓아냄 (임계 거리 조절 가능)
    //        {
    //            target.KickOut();
    //            //StartCoroutine(StartExitMove());
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //    // 목표 손님이 사라진 경우 이동 중단
    //}

    public void Update()
    {

        if (isExiting)
        {
            return;
        }


        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] cols = Physics2D.OverlapPointAll(pos);
            Debug.Log($"JinsangCustomer Update() cols.Length{cols.Length}");

            for (int i = 0; i < cols.Length; i++)

            {
                KickOutCustomer jCus = cols[i].GetComponent<KickOutCustomer>();
                if (jCus == null)
                    continue;
                if (jCus == this)
                {
                    clickSound.Play();

                    if (tibetFox.IsHired)
                    {
                        StartCoroutine(StartExitMove()); // 터치 한 번으로 나감
                    }

                    else
                    {
                        touchCount++;
                        if (touchCount >= 10) // 10번 터치했다면
                        {
                            StartCoroutine(StartExitMove()); // StartExitMove 함수를 호출합니다.
                            touchCount = 0; // 터치 횟수를 초기화합니다.
                        }
                    }
                }
            }
        }


    }


    public IEnumerator StartExitMove()
    {
        isExiting = true;

        yield return new WaitForSeconds(2f); // 2초 대기


        Debug.Log("출구 이동 시작");
        while (Vector2.Distance(transform.position, enterPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, enterPosition, moveSpeed * Time.deltaTime);
            yield return null;
            // 목적지에 도착하면 오브젝트 삭제
            if (Vector2.Distance(transform.position, enterPosition) < 0.1f)
            {
                Destroy(this.gameObject);
            }
        }
    }

}

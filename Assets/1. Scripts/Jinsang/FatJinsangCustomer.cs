
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatJinsangCustomer : Jinsang
{
    // 레스토랑에 들어옴 ,

    public float moveSpeed = 1f;

    //public float enterTime = 0f; // 경과 시간을 저장할 변수
    public bool isExiting; // 퇴장 중인지 여부를 나타내는 변수

    //public Transform spawn;
    public static FatJinsangCustomer Instance;
    public int touchCount;
    public AudioSource clickSound;



    public Transform enterancePosition;
    Vector2 enterPosition = new Vector2(4f, 5f);


    public void Awake()
    {
        Instance = this;
    }

    //public void Spawn()

    //{
    //    Vector3 spawnPosition = spawn.position;
    //    transform.position = spawnPosition;

    //    //StartCoroutine(MoveToEnterance());

    //    //어디선가 생성될텐데 거기서부터 입구까지 들어오게 처
    //}


    public override void Enter()
    {
        touchCount = 0;
        //enterTime = 0f; // 등장 시 enterTime 초기화
        isExiting = false; // 퇴장 상태 초기화
        transform.position = CustomerManager.Instance.enterance.position;
        StartCoroutine(CoStealAllCoins());
        Debug.Log("진상짓시작");
    }


    IEnumerator CoStealAllCoins()
    {

        while (true)
        {
            Coin[] coins = FindObjectsOfType<Coin>();
            if (coins.Length <= 0)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
            //가장 가까운 코인 찾기
            Coin closestCoin = coins[0];

            while (true)
            {
                if (closestCoin == null || !closestCoin.gameObject.activeSelf || closestCoin.isPicked)
                {
                    closestCoin = null;
                    break;
                }

                if (Vector2.Distance(transform.position, closestCoin.transform.position) < 0.3f)
                {
                    break;
                }
                transform.position =
                    Vector2.MoveTowards(transform.position, closestCoin.transform.position, Time.deltaTime * moveSpeed);

                yield return null;
            }

            closestCoin?.DestroyAcon();
            closestCoin = null;
            yield return null;
        }


    }

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
                FatJinsangCustomer jCus = cols[i].GetComponent<FatJinsangCustomer>();
                if (jCus == null)
                    continue;
                if (jCus == this)
                {
                    clickSound.Play();
                    touchCount++;
                    if (tibetFox.IsHired)
                    {
                        StartCoroutine(StartExitMove()); // 터치 한 번으로 나감
                    }

                    else
                    {
                        if (touchCount >= 30) // 10번 터치했다면
                        {
                            StartCoroutine(StartExitMove()); // StartExitMove 함수를 호출합니다.
                            touchCount = 0; // 터치 횟수를 초기화합니다.
                        }
                    }
                }
            }
        }

        //if (!isExiting) // 퇴장 중이 아닐 때만 시간 측정
        //{
        //enterTime += Time.deltaTime; // 경과 시간 증가

        //    if (enterTime >= 30f) // 30초 경과 시
        //    {
        //        isExiting = true; // 퇴장 상태로 변경
        //        StartCoroutine(StartExitMove()); // 퇴장 코루틴 시작
        //    }
        //}



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

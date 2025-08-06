using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bevyer : MonoBehaviour
{
    public static Bevyer Instance;
    
    public List<Vector2> wineSpots; // 6개의 포인트를 인스펙터에서 세팅
    public float moveSpeed = 5f;

    private bool isExiting = false;
    //private Coroutine moveRoutine;

    private void Awake()
    {
        Instance = this;

    }

    public void Enter()
    {
        //주방에 활성화된 디캔더가 뭔지 확인
        //
        wineSpots.Clear();


        for (int i = 0; i < KitchenManager.Instance.kitchenBarPlaces.Length; i++)
        {
            if (KitchenManager.Instance.kitchenBarPlaces[i].curKitchenBar != null)
            {
                wineSpots.Add(KitchenManager.Instance.kitchenBarPlaces[i].curKitchenBar.transform.position + Vector3.up);
            }
        }

        transform.position = CustomerManager.Instance.enterance.position;
        gameObject.SetActive(true);

        StartCoroutine(MakeWine());
        Invoke("Exit", 100f);

    }

    IEnumerator MakeWine()
    {
        while (!isExiting)
        {
            foreach (var spot in wineSpots)
            {
                yield return StartCoroutine(MoveToSpot(spot));

                if (isExiting)
                    break;

                yield return new WaitForSeconds(1f); // 각 장소에서 1초 대기
            }
        }

        yield return StartCoroutine(ExitRoutine());
    }

    IEnumerator MoveToSpot(Vector2 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void Exit()
    {
        isExiting = true;
    }

    IEnumerator ExitRoutine()
    {
        Transform entrance = CustomerManager.Instance.enterance;
        yield return StartCoroutine(MoveToSpot(entrance.position));
        Debug.Log("퇴장 완료");
        // 여기서 비활성화하거나 Destroy(gameObject) 할 수 있음
        gameObject.SetActive(false);
    }


}

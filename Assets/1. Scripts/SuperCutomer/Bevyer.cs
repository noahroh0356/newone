using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bevyer : MonoBehaviour
{
    public static Bevyer Instance;

    public List<Transform> wineSpots; // 6개의 포인트를 인스펙터에서 세팅
    public float moveSpeed = 5f;

    private bool isExiting = false;
    private Coroutine moveRoutine;

    private void Awake()
    {
        Instance = this;
    }

    public void Enter()
    {

        transform.position = CustomerManager.Instance.enterance.position;
        gameObject.SetActive(true);

        StartCoroutine(MakeWine());
        Invoke("Exit", 60f);

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

    IEnumerator MoveToSpot(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
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
        yield return StartCoroutine(MoveToSpot(entrance));
        Debug.Log("퇴장 완료");
        // 여기서 비활성화하거나 Destroy(gameObject) 할 수 있음
        gameObject.SetActive(false);
    }


}

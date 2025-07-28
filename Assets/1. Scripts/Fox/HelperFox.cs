using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFox : Fox
{
    //줍기 + More Tip
    public float moveSpeed = 0.5f;
    //Enter()에 레스토랑에 등장 + 동전을 줍기 시작
    public void Start()
    {
        StartCoroutine(CoPickUpCoin());
    }


    IEnumerator CoPickUpCoin()
    {
        while (true)
        {
            Coin[] coins = FindObjectsOfType<Coin>();
            if (coins.Length <= 0)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            // 가장 가까운 코인 찾기 (for문 사용)
            Coin closestCoin = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < coins.Length; i++)
            {
                if (coins[i] == null || !coins[i].gameObject.activeSelf || coins[i].isPicked)
                    continue;

                float distance = Vector2.Distance(transform.position, coins[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCoin = coins[i];
                }
            }

            // 줍을 코인이 없으면 다시 대기
            if (closestCoin == null)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            // 가장 가까운 코인으로 이동
            while (true)
            {
                if (closestCoin == null || !closestCoin.gameObject.activeSelf || closestCoin.isPicked)
                    break;

                if (Vector2.Distance(transform.position, closestCoin.transform.position) < 0.3f)
                {
                    closestCoin.Picked();
                    break;
                }

                transform.position = Vector2.MoveTowards(
                    transform.position,
                    closestCoin.transform.position,
                    Time.deltaTime * moveSpeed
                );

                yield return null;
            }

            yield return null;
        }
    }
    //IEnumerator CoPickUpCoin()
    //{

    //    while (true)
    //    {
    //        Coin[] coins = FindObjectsOfType<Coin>();
    //        if (coins.Length <= 0)
    //        {
    //            yield return new WaitForSeconds(1f);
    //            continue;
    //        }
    //        //가장 가까운 코인 찾기
    //        Coin closestCoin = coins[0];

    //        while (true)
    //        {
    //            if (closestCoin == null || !closestCoin.gameObject.activeSelf || closestCoin.isPicked)
    //            {
    //                closestCoin = null;
    //                break;
    //            }

    //            if (Vector2.Distance(transform.position, closestCoin.transform.position) < 0.3f)
    //            {
    //                break;
    //            }
    //            transform.position =
    //                Vector2.MoveTowards(transform.position, closestCoin.transform.position, Time.deltaTime * moveSpeed);

    //            yield return null;
    //        }
    //        closestCoin.Picked();
    //    }


    //}

}

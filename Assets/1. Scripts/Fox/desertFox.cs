using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class desertFox : MonoBehaviour
{

    public Coin coinPrefab;
    public int minPrice = 5;
    public int maxPrice = 30;

    public float minDelay = 5f;
    public float maxDelay = 60f;

    private void Start()
    {
        StartCoroutine(CoinRainLoop());
    }

    IEnumerator CoinRainLoop()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            StartCoroutine(Pay());
        }
    }

    IEnumerator Pay()
    {
        // 랜덤 가격 설정
        int randomPrice = Random.Range(minPrice, maxPrice + 1);

        // 코인 생성
        Coin coin = Instantiate(coinPrefab);
        coin.SetPrice(randomPrice);
        coin.transform.position = (Vector2)transform.position + Vector2.up * 0.5f;

        // 물리 효과 적용
        Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(Vector2.up * 200f); // 위로 튀는 힘
            rb.AddForce(Vector2.left * Random.Range(50f, 100f)); // 왼쪽 방향으로 랜덤하게
        }

        yield return null;
    }
}

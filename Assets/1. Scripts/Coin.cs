using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 100f;
    public bool isPicked = false;
    public AudioSource clickSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Invoke("StopCoin", 1.1f);
    }

    private void Update()
    {
        if (isPicked)
        {
            GetCoin();
        }
    }


    void StopCoin()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // 물리 시뮬레이션에서 제외
    }

    //    private void OnTriggerEnter2D(Collider2D collision)
    //    {
    //rb.velocity = Vector2.zero;
    //rb.angularVelocity = 0f;
    //rb.gravityScale = 0f; // 중력 비활성화
    // rb.isKinematic = true; // 물리 시뮬레이션에서 제외
    //    }

    int price;

    //**메뉴 데이터에서 가져온 메뉴의 가격, 커스터머>페이 함수 안에서 오더메뉴(메뉴데이터)를 불러왔기 때문에 그대로 받 
    public void SetPrice(int price)
    {
        this.price = price;
    }

    public void Picked()
    {
        MainQuestManager.Instance.DoQuest(MainQuestType.PickUpAcon); // ***퀘스트
        clickSound.Play();
        User.Instance.AddCoin(price);
        isPicked = true;      
    }

    public void DestroyAcon()
    {
        Destroy(gameObject);
    }

    //코인에 대한 가격 설정

    public void GetCoin()
    {
        CoinBoard coinBoard = FindObjectOfType<CoinBoard>();
        Vector2 coinboardPosition = Camera.main.ScreenToWorldPoint(coinBoard.transform.position);  

        //Vector2 coinboardPosition = new Vector2 (-9f, 18.5f);    
        transform.position = Vector2.MoveTowards(transform.position, coinboardPosition
            , moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, coinboardPosition) < 0.1f)
        {
            Destroy(this);
        }
    }
    


}

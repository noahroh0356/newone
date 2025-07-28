using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatchaBall : MonoBehaviour
{
    public Rigidbody2D rgdy;
    public GatchaType gatchaType;
    public GatchaCanvas gatchaCanvas;
    public GatchaManager gatchaManager;

    //public float shakeAmount = 0.0000001f; // 흔들리는 폭 (가챠 헤드보다 작게)
    //public float shakeSpeed = 0.000001f; // 흔들리는 속도
    //private float shakeTimer = 0f;

    //private bool isShaking = false;
    //private Vector3 originalPosition;

    public bool isSuccess = false;

    public string key;

    protected virtual void Awake()
    {
        SetupPhysics();
    }

    protected void SetupPhysics()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.angularDrag = 0.7f;
            rb.gravityScale = 1.5f;
        }

        // Collider가 Polygon이면 버그 줄이기 위해 AutoTiling 해제 등 (선택)
        Collider2D col = GetComponent<Collider2D>();
        if (col is PolygonCollider2D poly)
        {
            poly.useDelaunayMesh = true; // 유니티 2022 이상에서 지원
        }
    }

    public virtual void SetGatchaBall(string key)
    {
        this.key = key;
    }
    
    //    public void StartShaking()
    //{
    //    isShaking = true;
    //    originalPosition = transform.localPosition;
    //    shakeTimer = 0f; // 흔들림 시작 시 타이머 초기화

    //}

    //public void StopShaking()
    //{
    //    isShaking = false;
    //    transform.localPosition = originalPosition;
    //}

    //private void Update()
    //{
    //    // 기존 Update() 내용

    //    if (isShaking)
    //    {
    //        shakeTimer += Time.deltaTime * shakeSpeed; // Time.deltaTime을 더해 시간 흐름 생성
    //        float offsetX = Mathf.Sin(shakeTimer) * shakeAmount;
    //        transform.localPosition = originalPosition + new Vector3(offsetX, 0, 0);

    //    }
    //}
    public void ReceiveReward()
    {
        if (gatchaType == GatchaType.Customer)
        {
            //key에 해당하는 손님 획득
            User.Instance.AddCustomer(key); // **

        }

        else if (gatchaType == GatchaType.Acon)
        {
            int coinReward = 5 + User.Instance.userData.level * 2;
            User.Instance.AddCoin(coinReward);
            //도토리 숫자 정의
            //MainQuestManager.Instance.curQuestIndex
            //*손님인형과 도토리가 인형뽑기 안에 쌓이게 하기 숫자대, 위치 지정
        }


        else if (gatchaType == GatchaType.Wine)
        {
            User.Instance.AddWine(key);

            //gatchaManager.ResetGatcha();
        }



    }

}

public enum GatchaType
{
Wine,
Customer,
Acon
}
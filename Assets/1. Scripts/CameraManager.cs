using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{

    Transform cameraTr;
    public Transform rigthEnd;

    public Vector2 touchStart;

    public Vector2 touchEnd;

    public string currentArea = "Restaurant";


    public static CameraManager Instance;

    private float targetSize;

    private bool isSwiping = false; // 추가됨
    private bool isMoving = false;  // 카메라 이동 중 상태


    public void Awake()
    {
        cameraTr = Camera.main.transform;
        Instance = this;
    }


    private void Start()
    {
        SetCameraSizeByAspect();
        Camera.main.orthographicSize = targetSize;
        //float sideDistance = rigthEnd.position.x;
        //Camera.main.orthographicSize = sideDistance / Camera.main.aspect;

        //float aspect = (float)Screen.width / (float)Screen.height;

        //// 가로/세로 비율로 iPad 구분 (예: 4:3 또는 비슷한 비율)
        //if (aspect < 0.8f) // 4:3 비율 이하 → iPad 계열일 확률 높음
        //{
        //    Camera.main.orthographicSize = 9.5f;
        //}
        //else
        //{
        //    Camera.main.orthographicSize = 10f; // 기본값
        //}

    }

    void SetCameraSizeByAspect()
    {
        float aspect = (float)Screen.width / (float)Screen.height;

        if (aspect < 0.8f)
        {
            targetSize = 9.7f; // iPad 같은 4:3 비율
        }
        else
        {
            targetSize = 10f; // 일반 스마트폰
        }
    }

    private void LateUpdate()
    {
        if (Mathf.Abs(Camera.main.orthographicSize - targetSize) > 0.01f)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, Time.deltaTime * 5f);
        }
    }
    //private void LateUpdate()
    //{
    //    if (!isSwiping && !isMoving && Mathf.Abs(Camera.main.orthographicSize - targetSize) > 0.01f)
    //    {
    //        Camera.main.orthographicSize = targetSize;
    //    }
    //}


    public void MoveTo(string areaName)
    {
        if (areaName == "Restaurant")
        {
            KitchenManager.Instance.EndArea();
            StartCoroutine(CoMoveTo(RestaurantManager.Instance.center.position, areaName));
        }

        else if (areaName == "Kitchen")
        {
            RestaurantManager.Instance.EndArea();
            StartCoroutine(CoMoveTo(KitchenManager.Instance.center.position, areaName));

        }
        currentArea = areaName;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
            isSwiping = true; // 스와이프 시작

        }
        else if (Input.GetMouseButtonUp(0))
        {
            touchEnd = Input.mousePosition;
            isSwiping = false; // 스와이프 끝


            float swipeDistanceX = touchStart.x - touchEnd.x;

            if (Mathf.Abs(swipeDistanceX) > 70f)
            {
                if (swipeDistanceX > 0)
                {
                    if (currentArea == "Restaurant")
                    {
                        MoveTo("Kitchen");
                    }
                }
                else
                {
                    if (currentArea == "Kitchen")
                    {
                        MoveTo("Restaurant");
                    }
                }
            }
        }
    }

    IEnumerator CoMoveTo(Vector2 targetPoint, string areaName)
    {
        isMoving = true;
        float currentZ = cameraTr.position.z;


        while (true)
        {
            if (Vector2.Distance(cameraTr.position, targetPoint) < 0.01f)
            {
                break;
            }

            cameraTr.position = Vector2.MoveTowards(cameraTr.position, targetPoint, Time.deltaTime * 30);
            yield return null;
        }

        // 최종 위치 & 카메라 사이즈 강제 세팅
        cameraTr.position = targetPoint;
        Camera.main.orthographicSize = targetSize; // ✅ 사이즈 고정

        isMoving = false;

        // 영역 진입 처리
        if (areaName == "Restaurant")
        {
            RestaurantManager.Instance.StartArea();
        }
        else if (areaName == "Kitchen")
        {
            KitchenManager.Instance.StartArea();
        }
    }
    //IEnumerator CoMoveTo(Vector2 targetPoint,string areaName)
    //{
    //    isMoving = true;

    //    while (true)
    //    {

    //        if (Vector2.Distance(cameraTr.position, targetPoint) < 0.1f)
    //        {
    //            break;
    //        }
    //        cameraTr.position = Vector2.MoveTowards(cameraTr.position, targetPoint, Time.deltaTime * 30);

    //        yield return null;
    //    }
    //    cameraTr.position = targetPoint;
    //    isMoving = false;


    //    if (areaName == "Restaurant")
    //    {
    //        RestaurantManager.Instance.StartArea();//각 함수에 버튼 온오프 넣기

    //    }
    //    else if (areaName == "Kitchen")
    //    {
    //        KitchenManager.Instance.StartArea();//각 함수에 버튼 온오프 넣기

    //    }

}


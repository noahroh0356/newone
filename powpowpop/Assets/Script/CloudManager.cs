using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public Sprite cloud;

    public GameObject cloudPrefeb;

    // 계단 랜덤하게 만들기 클라우스 포인트 보고 확인

    public void Start()
    {

        Vector2 cloudPoint = new Vector2(0, 0);

        for (int i = 0; i<5; i++)
        {
            //xy값을 더해주는 코드
            //Cloud.width, Cloud.height를 반복해서 더해주기
            MakeCloud(cloudPoint); // 5번 생
            //cloudPoint = cloudPoint + new Vector2(Cloud.width, Cloud.height);

            cloudPoint += new Vector2(Cloud.width, Cloud.height);
        }

    }


    public void MakeCloud(Vector2 point)
    {
        //clouObj 신상에 복제된 프리
        GameObject cloudObj = Instantiate(cloudPrefeb);

        //1번
        //cloudObj.GetComponent<Transform>().position = new Vector3(0, 0, 0);

        //2번 
        //Transform cloudTr = cloudObj.GetComponent<Transform>();
        //cloudTr.position = new Vector3(0, 0, 0);

        cloudObj.transform.position = point; // 1,2와 같은 코드


    }




}

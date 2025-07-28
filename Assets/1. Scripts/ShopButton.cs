using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public GameObject ShopCanvas;

    void Start()
    {
        ShopCanvas.SetActive(false); //게임 오브젝트 활성화
    }

    public void OnClickedButton() //직접해보기 - 상점 버튼을 클릭 시 호출시켜주세요.

    {

            ShopCanvas.SetActive(true); //게임 오브젝트 활성화
        
    }




    void Update()
    {

    }
}
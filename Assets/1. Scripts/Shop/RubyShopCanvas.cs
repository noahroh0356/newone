using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyShopCanvas : MonoBehaviour
{

    private static RubyShopCanvas instance; // 정적 변수
    public static RubyShopCanvas Instance // 정적 속성
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<RubyShopCanvas>(FindObjectsInactive.Include);

            return instance;
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }


}

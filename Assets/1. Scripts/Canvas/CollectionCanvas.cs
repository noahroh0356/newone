using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCanvas : MonoBehaviour
{

    private static CollectionCanvas instance; // 정적 변수
    public static CollectionCanvas Instance // 정적 속성
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<CollectionCanvas>(FindObjectsInactive.Include);

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


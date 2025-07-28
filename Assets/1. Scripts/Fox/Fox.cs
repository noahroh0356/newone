using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    public string key;
    //public GameObject[] foxObjects; // 활성화/비활성화할 Fox 객체 목록 (Inspector에서 할당)



    public virtual void Enter()
    {
        gameObject.SetActive(true);

    }


}


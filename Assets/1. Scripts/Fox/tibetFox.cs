using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tibetFox : MonoBehaviour
{
    public static tibetFox Instance { get; private set; }

    public static bool IsHired => Instance != null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("티벳여우 등장!");
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }

}

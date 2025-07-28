using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToButton : MonoBehaviour
{
    public string areaName;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedMoveTo); // 인스펙터에 넣지 않아도 여기서 호
    }

    public void OnClickedMoveTo()
    {
        CameraManager.Instance.MoveTo(areaName);
    }

}

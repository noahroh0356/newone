using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{

    public GameObject shopCanvas;

    public GameObject bgmObject; // "BGM" 오브젝트
    public GameObject gatchaBgmObject; // "gatchabgm" 오브젝트
    public GatchaHead gatchaHead;


    public void OnClickButton()
    {
        shopCanvas.SetActive(false);

        if (bgmObject != null)
        {
            bgmObject.SetActive(true);
        }

        if (gatchaBgmObject != null)
        {
            gatchaHead.ResetGatcha();
            gatchaBgmObject.SetActive(false);
        }
    }

}

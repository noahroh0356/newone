using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaButton : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject activePanel;

    public GatchaManager gatchaManager;

    public GameObject bgmObject; // "BGM" 오브젝트
    public GameObject gatchaBgmObject; // "gatchabgm" 오브젝트

    public GatchaHead gatchaHead;

    public void OnClickButton()
    {


        if (User.Instance.userData.gatchaCoin < 1)
        {
            activePanel.gameObject.SetActive(false);
            ToastCanvas.Instance.ShowToast("needgatchacoin");
            return;
        }
        else
        {
            GatchaManager.Instance.gatchaHead.hasWatchedAd = false;
            User.Instance.userData.gatchaCoin -= 1;
            activePanel.gameObject.SetActive(true);
            //gatchaHead.ResetGatcha();
            gatchaManager.StartGacha();

        }


        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }



        if (bgmObject != null)
        {
            bgmObject.SetActive(false);
        }

        if (gatchaBgmObject != null)
        {
            gatchaBgmObject.SetActive(true);
        }

    }

}


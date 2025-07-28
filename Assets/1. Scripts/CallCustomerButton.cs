using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallCustomerButton : MonoBehaviour
{
    public int touch;
    public Image inviteGageBar;

    public AudioSource clickSound;

    public int inviteMaxcount = 10;

    //public CustomerManager customerManager;
    private void Start()
    {
        inviteGageBar.fillAmount = 0;

    }

    public void OnclickedButton()
    {
        touch ++;
        clickSound.Play();
        //clickSound.Stop();
        //clickSound.Pause();
        //if (clickSound.isPlaying == true)
        //{ }
        //clickSound.volume = 0;
        //AudioListener.volume =0; // 0.5 등으로 설정 가능 

        if (touch >= inviteMaxcount)
        {
            touch = 0;
            //CustomerManager customerMgr = FindObjectOfType<CustomerManager>();
            //customerMgr.EnterCustomer();

            CustomerManager.Instance.EnterCustomer();
            //파인드 오브젝트는 업데이트에서는 못씀
            //EnterCustomer();
            }

        inviteGageBar.fillAmount = (float)touch / (float)inviteMaxcount;

    }

    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        
    }
}

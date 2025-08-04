using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatchaManager : MonoBehaviour
{
    public GatchaBall cusGatchaPrefab;
    public GatchaBall aconGatchaPrefab;
    public GatchaBall wineGatchaPrefab;
    public GatchaHead gatchaHead;

    public GameObject gachaCanvas; // 가챠 캔버스
    public Transform gachaBallParent; //  가챠볼이 생성될 부모 오브젝트



    public static GatchaManager Instance;

    public void Awake()
    {
        Instance = this;
    }



    public void StartGacha()
    {
        MainQuestManager.Instance.DoQuest(MainQuestType.PlayGatcha);

        MakeCustomerBall();
        MakeWineBall();
        MakeAcon();
        //User.Instance.Lv; 포도알을 모으면 소믈리에 뱃지 레벨

    }


    void MakeAcon()
    {

        for (int i = 0; i < 30; i++)
        {
            GatchaBall ball = Instantiate(aconGatchaPrefab, gachaBallParent); 
            ball.gatchaType = GatchaType.Acon;
            float randomX = Random.Range(-2f, 2f);
            ball.transform.localPosition = new Vector2(randomX, 0);

        }
    }

    void MakeCustomerBall()
    {
        List<string> ownedCustomerKeys = new List<string>();

        for (int i = 0; i < User.Instance.userData.userCustomers.Count; i++)
        {
            if (User.Instance.userData.userCustomers[i].open)
            {
                ownedCustomerKeys.Add(User.Instance.userData.userCustomers[i].key);
            }
        }
        //  보유한 손님

        List<CustomerData> canGatchaCustomerDatas = new List<CustomerData>();

        for (int i = 0; i < CustomerManager.Instance.customerDatas.Length; i++)
        {
            CustomerData customerData = CustomerManager.Instance.customerDatas[i];
            bool isOwned = false;
            for (int j = 0; j < ownedCustomerKeys.Count; j++)
            {
                if (customerData.key == ownedCustomerKeys[j])
                {
                    isOwned = true;
                    break;
                }

            }
            if (!isOwned)
            {
                canGatchaCustomerDatas.Add(customerData);
            }

            //뽑을 수 있는 손님 키
            if (canGatchaCustomerDatas.Count >= 2)
            {
                break;
            }


        }

        //가챠 볼을 생성시키canGatchCustomerDatas에 따라서 생성시키

        for (int i = 0; i < canGatchaCustomerDatas.Count; i++)
        {
            int count = 3 - i;

            for (int j = 0; j < count; j++)
            {
                GatchaBall ball = Instantiate(cusGatchaPrefab, gachaBallParent); //생성된 프리팹 오브젝트의 컴포넌트
                ball.gatchaType = GatchaType.Customer;
                float randomX = Random.Range(-4f, 2f);
                ball.transform.position = gachaBallParent.position + new Vector3(randomX, 0);

                //생성되어야할 손님의 키 값
                ball.SetGatchaBall(canGatchaCustomerDatas[i].key);
            }

        }
        // 가챠로 뽑을 수 있는 손님 리스트 중 첫번째 손님(0번)이면 3마리, 두번째 손님(1번)이면 2마리
        // 얘는 커스터머프리팹 리스트랑 다르게 프리팹의 종류가 다르지가 않기 때문에 가챠볼안에서 코딩으로 구분할 수 있도록 해줘야함 

    }



    void MakeWineBall()
    {
        List<string> ownedWineKeys = new List<string>();

        for (int i = 0; i < User.Instance.userData.userWines.Count; i++)
        {
            if (User.Instance.userData.userWines[i].open)
            {
                ownedWineKeys.Add(User.Instance.userData.userWines[i].key);
            }
        }

        List<MenuData> canGatchaWineDatas = new List<MenuData>();

        for (int i = 0; i < MenuManager.Instance.menuDatas.Length; i++)
        {
            MenuData menuData = MenuManager.Instance.menuDatas[i];
            bool isOwned = false;
            for (int j = 0; j < ownedWineKeys.Count; j++)
            {
                if (menuData.key == ownedWineKeys[j])
                {
                    isOwned = true;
                    break;
                }

            }
            if (!isOwned)
            {
                canGatchaWineDatas.Add(menuData);
            }

            if (canGatchaWineDatas.Count >= 2)
            {
                break;
            }


        }

        for (int i = 0; i < canGatchaWineDatas.Count; i++)
        {
            int count = 3 - i;

            for (int j = 0; j < count; j++)
            {
                GatchaBall ball = Instantiate(wineGatchaPrefab, gachaBallParent); //생성된 프리팹 오브젝트의 컴포넌트
                ball.gatchaType = GatchaType.Wine;
                float randomX = Random.Range(-4f, 2f);
                ball.transform.position = gachaBallParent.position + new Vector3(randomX, 0);

                //생성되어야할 손님의 키 값
                ball.SetGatchaBall(canGatchaWineDatas[i].key);
            }

        }
        // 가챠로 뽑을 수 있는 손님 리스트 중 첫번째 손님(0번)이면 3마리, 두번째 손님(1번)이면 2마리
        // 얘는 커스터머프리팹 리스트랑 다르게 프리팹의 종류가 다르지가 않기 때문에 가챠볼안에서 코딩으로 구분할 수 있도록 해줘야함 

    }

}



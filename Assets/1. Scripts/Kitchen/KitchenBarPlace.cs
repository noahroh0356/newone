using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenBarPlace : MonoBehaviour
{
    public KitchenBar[] kitchenBars;
    public KitchenBar curKitchenBar; // 설치된 키친바
    public KitchenPlaceType kichenPlaceType;

    public bool making = false;

    // Firefox가 존재할 때 적용할 속도 가속 %
    // 0.3f는 30% 가속을 의미합니다. (1 + 0.3 = 1.3배 빨라짐)
    [SerializeField] private float firefoxSpeedBoostPercentage = 0.3f;

    public void StartMake(MenuData menuData, Customer customer)
    {
        curKitchenBar.wineProgressBar.fillAmount = 1;
        making = true;
        Debug.Log("StartMake");
        StartCoroutine(Cocomplete(menuData, customer));
    }

    IEnumerator Cocomplete(MenuData menuData, Customer customer)
    {
        float foxAbilityValue = FoxManager.Instance.GetFoxAbility(FoxAbilityType.KitchenSpeedUp) + 1;
        KitchenData data = KitchenManager.Instance.GetKitchenData(curKitchenBar.key);
        KitchenDetail detail = KitchenManager.Instance.GetKitchenDetail(data.kitchenBarKey);

        float time = menuData.makingTime;
        float timer = time;
        Debug.Log("StartMake1");

        // --- Firefox 존재 여부에 따른 속도 보너스 계산 ---
        float firefoxBonus = 0f;    
        // 씬에서 "Firefox"라는 이름을 가진 GameObject를 찾습니다.
        // 만약 Firefox가 특정 컴포넌트를 가지고 있다면 FindObjectOfType<Firefox>()를 사용할 수도 있습니다.
        if (GameObject.Find("fireFox") != null)
        {
            firefoxBonus = firefoxSpeedBoostPercentage;
            Debug.Log("Firefox 존재! 제작 속도 " + (firefoxSpeedBoostPercentage * 100) + "% 가속 적용.");
        }
        // --- End Firefox 보너스 ---


        //timer에 담긴 시간만큼 반복되는 코드
        while (true)
        {
            Debug.Log("StartMake2");
            if (timer <= 0)
                break;
            yield return null; // 한 프레임 시간 만큼 대기

            // 제작 속도 계산에 Firefox 보너스 추가
            // (1 + detail.reduceMakingTime) * foxAbilityValue는 기본 속도 증가 요인
            // 여기에 (1 + firefoxBonus)를 곱하여 추가 가속을 적용합니다.
            timer -= Time.deltaTime * (1 + detail.reduceMakingTime) * foxAbilityValue * (1 + firefoxBonus) * KitchenManager.Instance.speedUp;
            curKitchenBar.wineProgressBar.fillAmount = timer / time;
        }

        curKitchenBar.wineProgressBar.fillAmount = 0;
        customer.ReceiveMenu();

        making = false;

        KitchenManager.Instance.MatchOrder();
    }


    private void Awake()
    {
        kitchenBars = GetComponentsInChildren<KitchenBar>();
        curKitchenBar = kitchenBars[0];
    }

    public void UpdateKitchenBarPlace()
    {
        UserKitchen userKitchen = User.Instance.GetSetUpKitchen(kichenPlaceType);
        // 장소에 맞는 키친바를 활성화

        for (int i = 0; i < kitchenBars.Length; i++)
        {
            if (userKitchen != null && kitchenBars[i].key == userKitchen.kitchenkey)
            {
                curKitchenBar = kitchenBars[i];
                kitchenBars[i].gameObject.SetActive(true);
            }
            else
            {
                kitchenBars[i].gameObject.SetActive(false);
            }
        }
    }
}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class KitchenBarPlace : MonoBehaviour
//{
//    public KitchenBar[] kitchenBars;
//    public KitchenBar curKitchenBar; // 설치된 키친바
//    public KitchenPlaceType kichenPlaceType;

//    public bool making = false;
//    public void StartMake(MenuData menuData, Customer customer)
//    {
//        curKitchenBar.wineProgressBar.fillAmount = 1;        
//        making = true;
//        Debug.Log("StartMake");
//        StartCoroutine(Cocomplete(menuData, customer));
//    }

//    IEnumerator Cocomplete(MenuData menuData, Customer customer)
//    {
//        float foxAbilityValue = FoxManager.Instance.GetFoxAbility(FoxAbilityType.KitchenSpeedUp)+1; // 빠르게 해줘야 해서 1더함 아니면 0.n이라 오히려 감소하게 
//        KitchenData data = KitchenManager.Instance.GetKitchenData(curKitchenBar.key);
//        KitchenDetail detail = KitchenManager.Instance.GetKitchenDetail(data.kitchenBarKey);


//        float time = menuData.makingTime; //* (1 - data.reduceMakingTime);
//        float timer = time;
//        Debug.Log("StartMake1");

//        //timer에 담긴 시간만큼 반복되는 코드
//        while (true)
//        {
//            Debug.Log("StartMake2");
//            if (timer <= 0)
//                break;
//            yield return null; // 한 프레임 시간 만큼 대기
//            timer -= Time.deltaTime * (1 + detail.reduceMakingTime) * foxAbilityValue; ; //한 프레임 간 시간 간격, 제작 속도 조
//            curKitchenBar.wineProgressBar.fillAmount = timer/time;
//        }

//        //yield return new WaitForSeconds(timer);


//        curKitchenBar.wineProgressBar.fillAmount = 0;
//        customer.ReceiveMenu();

//        making = false;

//        KitchenManager.Instance.MatchOrder();
//    }


//    private void Awake()
//    {
//        kitchenBars = GetComponentsInChildren<KitchenBar>();
//        curKitchenBar = kitchenBars[0];
//    }

//    public void UpdateKitchenBarPlace()
//    {
//        UserKitchen userKitchen = User.Instance.GetSetUpKitchen(kichenPlaceType);
//        // 장소에 맞는 키친바를 활성화

//        for (int i = 0; i < kitchenBars.Length; i++)
//        {
//            if (userKitchen != null && kitchenBars[i].key == userKitchen.kitchenkey)
//            {
//                curKitchenBar = kitchenBars[i];
//                kitchenBars[i].gameObject.SetActive(true);
//            }
//            else
//            {
//                kitchenBars[i].gameObject.SetActive(false);
//            }
//        }

//    }

//}

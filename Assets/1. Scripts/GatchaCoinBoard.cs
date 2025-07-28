using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GatchaCoinBoard : MonoBehaviour
{
    public TMP_Text gatchaCoinText;
    public TMP_Text coinEffectText;
    public RectTransform startPoint;
    //public UserData userData;

    public static GatchaCoinBoard Instance;


    void Awake()
    {
        Instance = this;
    }

    public void AddedCoin(int gatchaCoin)
    {

        coinEffectText.transform.DOKill(); // 이전 트윈 제거
        coinEffectText.gameObject.SetActive(false); // 강제로 비활성화 (혹시 남아있는 상태 방지)

        coinEffectText.transform.position = startPoint.position;
        coinEffectText.text = $"+{gatchaCoin}"; // ✅ + 붙이기

        coinEffectText.gameObject.SetActive(true); // 다시 활성화

        coinEffectText.transform.DOMove(coinEffectText.transform.position + new Vector3(0, 50, 0), 0.4f)
            .OnKill(() => {
                coinEffectText.gameObject.SetActive(false); // ✅ 강제 비활성화 보장
            })
            .OnComplete(() => {
                coinEffectText.gameObject.SetActive(false); // ✅ 이중 방지
            });
        //coinEffectText.transform.position = startPoint.position;
        //coinEffectText.gameObject.SetActive(true);
        //coinEffectText.text = $"+{gatchaCoin}";
        ////coinEffectText.text = coin.ToString(); // 얻은 코인 값 설정
        //coinEffectText.transform.DOKill(); // 이전에 두트윈 기능이 동작하고 있다면 꺼라
        //coinEffectText.transform.DOMove(coinEffectText.transform.position + new Vector3(0, 50, 0), 0.4f)
        //            .OnComplete(() => {
        //                coinEffectText.gameObject.SetActive(false);
        //            });

    }


    private void Update()
    {
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        if (gatchaCoinText != null)
        {
            gatchaCoinText.text = User.Instance.userData.gatchaCoin.ToString();
        }
        else
        {
            Debug.LogError("gatchaCoin가 null입니다.");
        }
    }

}

using UnityEngine;
using UnityEngine.UI; // UI 요소를 사용하기 위해 필요
using System.Collections;
using System.Collections.Generic; // List를 사용하기 위해 필요

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel; // 유니티 인스펙터에서 TutorialPanel 게임 오브젝트를 할당
    public List<Sprite> tutorialImages; // 튜토리얼 이미지 스프라이트들을 순서대로 할당
    public Image displayImage; // 현재 튜토리얼 이미지를 표시할 Image 컴포넌트를 할당
    public Button nextButton; // 다음/건너뛰기 버튼을 할당

    private int currentTutorialIndex = 0; // 현재 표시 중인 튜토리얼 이미지의 인덱스

    // 싱글톤 패턴 (옵션이지만 매니저 스크립트에 유용)
    public static TutorialManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // 이 스크립트의 인스턴스를 저장
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 중복 방지
        }
    }

    public void StartTutorial()
    {
        tutorialPanel.SetActive(true); // 튜토리얼 패널 활성화
        currentTutorialIndex = 0; // 첫 번째 이미지부터 시작
        DisplayCurrentTutorialImage(); // 현재 이미지 표시
        nextButton.onClick.RemoveAllListeners(); // 이전에 추가된 리스너 제거
        nextButton.onClick.AddListener(OnNextButtonClicked); // 버튼 클릭 시 호출될 함수 등록
    }

    void DisplayCurrentTutorialImage()
    {
        if (currentTutorialIndex < tutorialImages.Count)
        {
            // 튜토리얼 이미지가 남아있다면 해당 이미지를 표시
            displayImage.sprite = tutorialImages[currentTutorialIndex];
        }
        else
        {
            // 모든 이미지를 다 보여줬다면 튜토리얼 종료
            EndTutorial();
        }
    }

    void OnNextButtonClicked()
    {
        currentTutorialIndex++; // 다음 이미지로 이동
        if (currentTutorialIndex < tutorialImages.Count)
        {
            DisplayCurrentTutorialImage(); // 다음 이미지 표시
        }
        else
        {
            EndTutorial(); // 튜토리얼 종료
        }
    }

    void EndTutorial()
    {
        tutorialPanel.SetActive(false); // 튜토리얼 패널 비활성화
        // 튜토리얼 종료 후 게임 플레이 요소 활성화 등 추가 동작을 여기에 넣을 수 있습니다.
    }
}
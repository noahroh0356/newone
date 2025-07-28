using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideButton : MonoBehaviour
{
    public TutorialManager tutorialManager;

    public void OnClickedButton()
    {
        if (tutorialManager != null)
        {
            tutorialManager.StartTutorial(); // 반드시 StartTutorial 호출
        }
    }



}

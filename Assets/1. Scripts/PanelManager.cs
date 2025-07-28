using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    public GameObject questCanvas; // 실제 내용 패널
    public GameObject questIcon; // 실제 내용 패널
    public GameObject backgroundCloseArea; // 투명 버튼

    void Awake()
    {
        Instance = this;
    }

    public void OpenPanel()
    {
        questCanvas.SetActive(true);
        backgroundCloseArea.SetActive(true);
        questCanvas.transform.SetAsLastSibling();

        backgroundCloseArea.transform.SetSiblingIndex(questCanvas.transform.GetSiblingIndex() - 1); // questCanvas 바로 아래에 배치

    }

    public void ClosePanel()
    {
        questIcon.SetActive(true);
        questCanvas.SetActive(false);
        backgroundCloseArea.SetActive(false);
    }




}

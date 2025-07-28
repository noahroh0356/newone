using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablePlaceProducts : MonoBehaviour
{
    public FurnitureProductPanel[] furnitureProductPanels;
    public FurniturePlace furniturePlace;

    private void Awake()
    {
        furnitureProductPanels = GetComponentsInChildren<FurnitureProductPanel>();
    }

    private void Start()
    {
        UpdateTablePlace();
    }

    public void UpdateTablePlace()
    {
        UserFurniture userFurniture = User.Instance.GetSetUpFurniture(furniturePlace);

        // 모든 패널 비활성화 (기존 코드 유지)
        foreach (var panel in furnitureProductPanels)
        {
            panel.gameObject.SetActive(false);
        }

        if (userFurniture == null)
        {
            // 초기 상태: 첫 번째 패널만 활성화 (기존 코드 유지)

            if (furnitureProductPanels.Length > 0)
            {
                furnitureProductPanels[0].gameObject.SetActive(true);
            }
            return;
        }

        FurnitureData furnitureData = FurnitureManager.Instance.GetFurnitureData(userFurniture.furniturekey);
        if (furnitureData == null)
        {
            Debug.LogError("Furniture data not found for key: " + userFurniture.furniturekey);
            return;
        }

        // 핵심 변경: nextfurniturekey에 해당하는 패널만 활성화
        if (!string.IsNullOrEmpty(furnitureData.nextfurniturekey)) // nextfurniturekey가 비어있지 않은 경우에만 검색
        {

            foreach (var panel in furnitureProductPanels)
            {
                if (panel.key == furnitureData.nextfurniturekey)
                {
                    //Debug.Log(furnitureData.nextfurniturekey + "키값맞는패널활성");
                    panel.gameObject.SetActive(true);
                    return; // 찾았으면 루프 종료
                }
            }
        }
        else // nextfurniturekey가 비어있는 경우(마지막 단계), 마지막 패널 활성화
        {
            if (furnitureProductPanels.Length > 0)
            {
                furnitureProductPanels[furnitureProductPanels.Length - 1].gameObject.SetActive(true);
            }
        }
    }
}
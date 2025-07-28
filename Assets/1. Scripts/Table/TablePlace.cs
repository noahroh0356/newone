using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TablePlace : MonoBehaviour
{
    public Table[] table;
    public FurniturePlace furniturePlace;
    public Customer customer;
    public Table tables;
    //public Image tableMenuImage; 


    private void Awake()
    {
        table = GetComponentsInChildren<Table>(true);
    }

    private void Start()
    {
        UpdateTablePlace();
    }



    public void UpdateTablePlace()
    {
        UserFurniture userFurniture = User.Instance.GetSetUpFurniture(furniturePlace);
        // 장소에 맞는 테이블을 활성화


        for (int i = 0; i < table.Length; i++)
        {
            if (userFurniture != null && table[i].key == userFurniture.furniturekey)
            {
                table[i].gameObject.SetActive(true);
            }
            else
            {
                table[i].gameObject.SetActive(false);
            }
        }

    }

    public Table GetTable()
    {
        UserFurniture userFurniture = User.Instance.GetSetUpFurniture(furniturePlace);
        
        // 장소에 맞는 테이블을 활성화
        for (int i = 0; i < table.Length; i++)
        {
            if (userFurniture != null && table[i].key == userFurniture.furniturekey)
            {
                return table[i];
            }
            
        }
        return null;
        //설치된 테이블을 리
    }

}

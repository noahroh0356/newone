using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenBarPlaceProduct : MonoBehaviour
{
    public KitchenBarProduct[] products;
    public KitchenPlaceType placeType;
    public KitchenData kitchenData;


    void Start()
    {
        products = GetComponentsInChildren<KitchenBarProduct>(true);
        UpdateKitchenBarPlace();
    }

    public void UpdateKitchenBarPlace()
    {

        for(int i =0; i<products.Length; i++)
        {
            products[i].gameObject.SetActive(false);

        }

        UserKitchen userKitchen = User.Instance.GetSetUpKitchen(placeType);

        if (userKitchen != null)
        {           
            kitchenData = KitchenManager.Instance.GetKitchenData(userKitchen.kitchenkey);
            foreach (var panel in products)
            {
                if (panel.key == kitchenData.nextProductKey)
                {
                    Debug.Log(kitchenData.nextProductKey + "키값맞는패널활성");
                    panel.gameObject.SetActive(true);
                    return;
                }
            }
        }

        else
        {
            products[0].gameObject.SetActive(true);
        }

    }


    void Update()
    {
        
    }
}

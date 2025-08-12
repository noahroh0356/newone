using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerCollectionPanel : MonoBehaviour
{

    public CustomerCollectionData collectionData;

    public string key;

    public Image customerImage1;
    public Image customerImage2;
    public Image customerImage3;


    private void Awake()
    {
        collectionData = Resources.Load<CustomerCollectionData>($"CustomerColletion/{key}");

    }

    private void Start()
    {
        CustomerData customerData1 = CustomerManager.Instance.GetCustomerData(collectionData.customer1);
        customerImage1.sprite = customerData1.thum;
        UserCustomer userCustomer1 =  User.Instance.GetUserCustomer(collectionData.customer1);

        if (userCustomer1.open)
        {
            customerImage1.color = Color.white;
        }
        else
        customerImage1.color = Color.grey;

        CustomerData customerData2 = CustomerManager.Instance.GetCustomerData(collectionData.customer2);
        customerImage2.sprite = customerData2.thum;
        UserCustomer userCustomer2 = User.Instance.GetUserCustomer(collectionData.customer2);

        if (userCustomer2.open)
        {
            customerImage2.color = Color.white;
        }
        else
            customerImage2.color = Color.grey;


        CustomerData customerData3 = CustomerManager.Instance.GetCustomerData(collectionData.customer3);
        customerImage3.sprite = customerData3.thum;
        UserCustomer userCustomer3 = User.Instance.GetUserCustomer(collectionData.customer3);

        if (userCustomer3.open)
        {
            customerImage3.color = Color.white;
        }
        else
            customerImage3.color = Color.grey;
    }


}

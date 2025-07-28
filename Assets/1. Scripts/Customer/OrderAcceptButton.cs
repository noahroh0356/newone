using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderAcceptButton : MonoBehaviour
{

    public void OnClickOrderAcceptButton()
    {
        GetComponentInParent<Customer>().AcceptOrder();

    }
}

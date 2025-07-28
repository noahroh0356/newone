using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatchaCanvas : MonoBehaviour
{
    public bool pushDownLeft;
    public bool pushDownRight;
    public bool pushDown;
    public GatchaHead gatchaHead;
    public void OnClickedDown()
    {
        gatchaHead.StartMoveDown();
        Debug.Log("down");

    }

    public void OnClickedLeftDown()
    {
        pushDownLeft = true;
    }
    public void OnClickedLeftUp()
    {
        pushDownLeft = false;
    }

    public void OnClickedRightDown()
    {
        pushDownRight = true;
    }
    public void OnClickedRightUp()
    {
        pushDownRight = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hello_I_am_a_sign : MonoBehaviour
{
    public string message;

    public void InteractWithMe()
    {
        GameManager.EXPLORE.OpenSign();
        GameManager.EXPLORE.current_Sign_panel.GetComponentInChildren<Text>().text = message;
    }
    public void CloseMe()
    {
        GameManager.EXPLORE.ClearAllScreens();
    }
}

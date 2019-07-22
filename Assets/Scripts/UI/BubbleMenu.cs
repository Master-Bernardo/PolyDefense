using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BubbleMenu : MonoBehaviour
{
    // is responsible for interacting between player and other things
    //do we need this?
    public GameObject menu;
    public GameObject[] buttons;

    public void Show()
    {
        menu.GetComponent<Canvas>().enabled = true;
    }

    public void Hide()
    {
        menu.GetComponent<Canvas>().enabled = false;
    }
}

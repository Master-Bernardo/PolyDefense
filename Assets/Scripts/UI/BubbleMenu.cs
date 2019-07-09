using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BubbleMenu : MonoBehaviour
{
    // is responsible for interacting between player and other things
    //do we need this?
    public GameObject menu;
    public GameObject[] buttons;

    public void Show()
    {
        menu.SetActive(true);
    }

    public void Hide()
    {
        menu.SetActive(false);
    }
}

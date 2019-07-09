using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMenuButton : MonoBehaviour
{
    enum BubbleMenuType
    {
        Toogleable, //can be turned on/off
        Stackable, //like units rectuitment, has a text element which shows how many times it was selected
        Default //can be clicked and activates event
    }

    [SerializeField]
    BubbleMenuType type;

    
}

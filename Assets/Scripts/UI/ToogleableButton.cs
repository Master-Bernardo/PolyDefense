using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class ToogleableButton : MonoBehaviour, IPointerClickHandler
{
    bool active;

    public Color activeColor;
    public Color inactiveColor;
    public Image background;

    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;



    public void OnPointerClick(PointerEventData eventData)
    {
        active = !active;

        if (active)
        {
            OnActivate.Invoke();
            background.color = activeColor;
        }
        else
        {
            OnDeactivate.Invoke();
            background.color = inactiveColor;
        }
    }

    public void Deactivate()
    {
        background.color = inactiveColor;
        active = false;
    }

    public void Activate()
    {
        background.color = activeColor;
        active = true;
    }
}

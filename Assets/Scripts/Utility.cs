using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public static class Utility
{
    public static bool HitsUI(Vector3 mousePosition)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, results);

        if (results.Count > 0) return true;
        else return false;
    }
}

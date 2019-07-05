using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementTrigger : MonoBehaviour
{
    public int collidersInsideTrigger = 0;

    private void OnTriggerEnter(Collider other)
    {
        collidersInsideTrigger++;
        Debug.Log(other + "enters");
    }

    private void OnTriggerExit(Collider other)
    {
        collidersInsideTrigger--;
        Debug.Log(other + "exits");
    }

    public bool Collides()
    {
        if (collidersInsideTrigger > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

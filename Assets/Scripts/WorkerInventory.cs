using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class WorkerInventory 
{
    /*
     * which ressources does this worker carry and how many? 
     * the worker saves this value in this inventory - also add a visual representation
     */

    public Transform ressourceHolder; //this Go holds and scales the ressources
    public GameObject ferLook;
    public GameObject merLook; //theese both get switched to show what the workers are carrying depending on the ressource

    public RessourceType currentType; //we can only carry one type of ressource at once
    public int  currentRessourceTransportLoad;
    public int  ressourceTransportLoad;

    public void AddRessource(RessourceType type, int value)
    {
        if(type != currentType)
        {
            currentType = type;
            currentRessourceTransportLoad = value;
        }
        else
        {
            currentRessourceTransportLoad += value;
        }

        if(currentType == RessourceType.fer)
        {
            ferLook.SetActive(true);
            merLook.SetActive(false);

        }
        else if(currentType == RessourceType.mer)
        {
            merLook.SetActive(true);
            ferLook.SetActive(false);
        }

        UpdateVisuals();
    }

    public void Clear()
    {
        currentRessourceTransportLoad = 0;
        UpdateVisuals();

    }

    void UpdateVisuals()
    {
        ressourceHolder.localScale = new Vector3(1f, 1f*currentRessourceTransportLoad/ ressourceTransportLoad, 1f);
        if(currentRessourceTransportLoad == 0)
        {
            ressourceHolder.gameObject.SetActive(false);
        }
        else
        {
            ressourceHolder.gameObject.SetActive(true);
        }
    }
}

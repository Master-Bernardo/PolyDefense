using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_HarvestingBuilding : Ability
{
    public RessourceType type;

    public Ab_Worker[] assignedWorkers;

    public int workerCapacity;

    public float width; //TODO change this into some kind of interface

    public void DepotRessource(int amount)
    {
        PlayerManager.Instance.AddRessource(type, amount);
    }

    public void AssignWorker()
    {

    }
}

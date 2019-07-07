using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ressource : GameEntity
{
    public int maxRessourceAmount;
    public bool infinite;
    int currentRessourceAmount;
    public RessourceType type;

    private void Awake()
    {
        currentRessourceAmount = maxRessourceAmount;
    }

    //returns the requestet amount or fewer if fewer is left
    public int TakeRessource(int amount)
    {
        if (amount > currentRessourceAmount)
        {
            currentRessourceAmount = 0;
            OnDie();
            return currentRessourceAmount;
        }
        else
        {
            currentRessourceAmount -= amount;
            if (currentRessourceAmount == 0) OnDie();
            return amount;
        }
    }

}

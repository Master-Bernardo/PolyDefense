using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ressource : GameEntity
{
    public int maxRessourceAmount;
    public bool infinite;
    int currentRessourceAmount;
    public RessourceType type;
    public float width;

    private void Start()
    {
        currentRessourceAmount = maxRessourceAmount;
        RessourcesManager.Instance.AddRessource(this);

    }

    //returns the requestet amount or fewer if fewer is left
    public int TakeRessource(int amount)
    {
        if (amount > currentRessourceAmount)
        {
            currentRessourceAmount = 0;
            Die();
            return currentRessourceAmount;
        }
        else
        {
            currentRessourceAmount -= amount;
            if (currentRessourceAmount == 0) Die();
            return amount;
        }
    }

    public override void Die()
    {
        RessourcesManager.Instance.RemoveRessource(this);
        base.Die();

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_Populator : EntityComponent
{
    public int populationValue;

    public override void SetUpComponent(GameEntity entity)
    {
        PlayerManager.Instance.RaisePopulation(populationValue);
    }

    public override void OnDie()
    {
        Debug.Log("on dieee");
        PlayerManager.Instance.LowerPopulation(populationValue);
    }
}

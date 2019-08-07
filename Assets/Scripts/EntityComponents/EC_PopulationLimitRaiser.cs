using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_PopulationLimitRaiser : EntityComponent
{
    public int populationLimit; //this building raises the populationLimit by this amount

    public override void SetUpComponent(GameEntity entity)
    {
        PlayerManager.Instance.RaisePopulationLimit(populationLimit);
    }
    public override void OnDie()
    {
        PlayerManager.Instance.LowerPopulationLimit(populationLimit);
    }
}

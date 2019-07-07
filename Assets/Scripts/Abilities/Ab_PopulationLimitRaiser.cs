using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_PopulationLimitRaiser : PassiveAbility
{
    public int populationLimit; //this building raises the populationLimit by this amount

    public override void SetUpAbility(GameEntity entity)
    {
        PlayerManager.Instance.RaisePopulationLimit(populationLimit);
        Debug.Log("setting uppe");
    }
    public override void OnDie()
    {
        PlayerManager.Instance.LowerPopulationLimit(populationLimit);
    }
}

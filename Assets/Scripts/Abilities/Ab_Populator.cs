using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Populator : Ability
{
    public int populationValue;

    public override void SetUpAbility(GameEntity entity)
    {
        PlayerManager.Instance.RaisePopulation(populationValue);
    }
}

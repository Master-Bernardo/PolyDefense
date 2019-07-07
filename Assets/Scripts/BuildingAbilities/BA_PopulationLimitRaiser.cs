using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_PopulationLimitRaiser : PassiveBuildingAbility
{
    public int populationLimit; //this building raises the populationLimit by this amount

    public override void SetUpAbility()
    {
        PlayerManager.Instance.populationLimit += populationLimit;
    }
    public override void OnDie()
    {
        PlayerManager.Instance.populationLimit -= populationLimit;
    }
}

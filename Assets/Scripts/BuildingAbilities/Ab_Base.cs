using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the base ability
public class Ab_Base : Ability
{
    [Tooltip("at which distancde to the base can buildings be constructed?")]
    public float buildingDistance;

    public override void SetUpAbility(GameEntity entity)
    {
        BuildingSystem.Instance.AddBaseBuilding(this);
    }


}

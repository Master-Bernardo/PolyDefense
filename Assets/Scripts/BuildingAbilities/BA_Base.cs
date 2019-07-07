using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the base ability
public class BA_Base : BuildingAbility
{
    [Tooltip("at which distancde to the base can buildings be constructed?")]
    public float buildingDistance;

    public override void SetUpAbility()
    {
        BuildingSystem.Instance.AddBaseBuilding(this);
    }


}

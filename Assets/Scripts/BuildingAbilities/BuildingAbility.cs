using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAbility : MonoBehaviour
{
    public virtual void SetUpAbility()
    {

    }

    public virtual void UpdateAbility()
    {
    }

    public virtual void OnDie()
    {

    }
}

//can be toogled on of via UI
public class PassiveToogleableBuildingAbility : BuildingAbility
{
    public bool active;

    public void ToogleActive()
    {
        active = !active;
    }
}

//can be clicked to activate via UI
public class ActiveBuildingAbility : BuildingAbility
{
    public virtual void ActivateAbility()
    {

    }
}

public class PassiveBuildingAbility : BuildingAbility
{

}

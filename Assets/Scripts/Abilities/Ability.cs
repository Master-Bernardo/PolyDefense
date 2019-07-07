using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{

    protected GameEntity myEntity;

    public virtual void SetUpAbility(GameEntity entity)
    {
        myEntity = entity;
    }

    public virtual void UpdateAbility()
    {

    }

    public virtual void OnDie()
    {

    }
}

//can be toogled on of via UI
public class PassiveToogleableAbility : Ability
{
    public bool active;

    public void ToogleActive()
    {
        active = !active;
    }
}

//can be clicked to activate via UI
public class ActiveAbility : Ability
{
    public virtual void ActivateAbility()
    {

    }
}

public class PassiveAbility : Ability
{

}

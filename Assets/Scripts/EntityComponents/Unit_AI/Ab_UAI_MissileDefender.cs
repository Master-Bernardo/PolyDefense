using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_UAI_MissileDefender : Ab_UnitAI
{
    public B_MissileFighter missileBehaviour;
    public B_WanderAroundPosition wanderBehaviour;

    public EC_Movement movement;
    public EC_ScanForEnemyUnits sensing;
    public EC_MissileWeapon weapon;

    // Start is called before the first frame update
    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        currentBehaviour = null;
        missileBehaviour.SetUpBehaviour(entity, movement, sensing, weapon);
        wanderBehaviour.SetUpBehaviour(entity, movement);
    }

    public void SetPositionToWanderAround(Transform position)
    {
        Debug.Log("Sette 2");
        Debug.Log("pos: " + position);
        wanderBehaviour.SetPositionToWanderAround(position);
       
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            SetCurrentBehaviour(missileBehaviour);
        }
        else
        {
            SetCurrentBehaviour(wanderBehaviour);
        }
    }
}


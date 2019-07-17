using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_UAI_MissileDefender : Ab_UnitAI
{
    public B_MissileFighter missileBehaviour;
    public B_WanderAroundPosition wanderBehaviour;

    public Ab_Movement movement;
    public Ab_ScanForEnemyUnits sensing;
    public Ab_MissileWeapon weapon;

    // Start is called before the first frame update
    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        currentBehaviour = null;
        missileBehaviour.SetUpBehaviour(this, movement, sensing, weapon);
        wanderBehaviour.SetUpBehaviour(this, movement);
        wanderBehaviour.SetPositionToWanderAround(transform);
    }

    public void SetPositionToWanderAround(Transform position)
    {
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


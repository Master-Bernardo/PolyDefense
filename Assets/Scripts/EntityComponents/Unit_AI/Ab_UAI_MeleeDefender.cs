using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_UAI_MeleeDefender: Ab_UnitAI
{
    public B_MeleeFighter meleeBehaviour;
    public B_WanderAroundPosition wanderBehaviour;

    public EC_Movement movement;
    public EC_ScanForEnemyUnits sensing;
    public EC_MeleeWeapon weapon;

    // Start is called before the first frame update
    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        currentBehaviour = null;
        meleeBehaviour.SetUpBehaviour(entity, movement, sensing, weapon);
        wanderBehaviour.SetUpBehaviour(entity, movement);
    }

    public void SetPositionToWanderAround(Transform position)
    {
        wanderBehaviour.SetPositionToWanderAround(position);
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            SetCurrentBehaviour(meleeBehaviour);
        }
        else
        {
            SetCurrentBehaviour(wanderBehaviour);
        }
    }
}

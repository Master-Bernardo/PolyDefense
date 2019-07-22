using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_UAI_MissileAttacker : Ab_UnitAI
{
    public B_MissileFighter missileBehaviour;
    public B_MissileAttackBuilding attackBaseBehaviour;

    public EC_Movement movement;
    public EC_ScanForEnemyUnits sensing;
    public EC_MissileWeapon weapon;

    // Start is called before the first frame update
    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        currentBehaviour = null;
        missileBehaviour.SetUpBehaviour(entity, movement, sensing, weapon);
        attackBaseBehaviour.SetUpBehaviour(entity, movement, weapon);

    }


    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            SetCurrentBehaviour(missileBehaviour);
        }
        else
        {
            attackBaseBehaviour.SetTargetBuilding(BuildingSystem.Instance.playersBaseLocation.GetComponent<Building>());

            SetCurrentBehaviour(attackBaseBehaviour);

        }

        //Debug.Log("currentBehaviour: + " + currentBehaviour);
    }
}

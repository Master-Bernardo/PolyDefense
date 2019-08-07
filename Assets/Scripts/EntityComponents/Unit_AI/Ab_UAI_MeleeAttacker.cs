using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_UAI_MeleeAttacker : Ab_UnitAI
{
    public B_MeleeFighter meleeBehaviour;
    public B_MeleeAttackBuilding attackBaseBehaviour;

    public EC_Movement movement;
    public EC_ScanForEnemyUnits sensing;
    public EC_MeleeWeapon weapon;

    // Start is called before the first frame update
    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        currentBehaviour = null;
        meleeBehaviour.SetUpBehaviour(entity, movement, sensing, weapon);
        attackBaseBehaviour.SetUpBehaviour(entity, movement, weapon);
        //Debug.Log("setted this building: " + BuildingSystem.Instance.playersBaseLocation);
        //Debug.Log("setted this building2: " + BuildingSystem.Instance.playersBaseLocation.GetComponent<Building>());

       
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            SetCurrentBehaviour(meleeBehaviour);
        }
        else
        {
            attackBaseBehaviour.SetTargetBuilding(BuildingSystem.Instance.playersBaseLocation.GetComponent<Building>());

            SetCurrentBehaviour(attackBaseBehaviour);
        }
    }
}

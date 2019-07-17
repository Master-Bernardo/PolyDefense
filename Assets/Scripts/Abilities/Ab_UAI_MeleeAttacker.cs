using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_UAI_MeleeAttacker : Ab_UnitAI
{
    public B_MeleeFighter meleeBehaviour;
    public B_MeleeAttackBuilding attackBaseBehaviour;

    public Ab_Movement movement;
    public Ab_ScanForEnemyUnits sensing;
    public Ab_MeleeWeapon weapon;

    // Start is called before the first frame update
    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        currentBehaviour = null;
        meleeBehaviour.SetUpBehaviour(this, movement, sensing, weapon);
        attackBaseBehaviour.SetUpBehaviour(this, movement, weapon);
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

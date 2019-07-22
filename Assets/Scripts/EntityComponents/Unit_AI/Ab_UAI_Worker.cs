using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_UAI_Worker : Ab_UnitAI
{
    public B_Worker workerBehaviour;
    public B_Flee fleeBehaviour;

    public EC_Movement movement;
    public EC_ScanForEnemyUnits sensing;

    // Start is called before the first frame update
    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        currentBehaviour = null;
        fleeBehaviour.SetUpBehaviour(entity, movement, sensing);
        workerBehaviour.SetUpBehaviour(entity, movement);
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            Debug.Log("flee");
            SetCurrentBehaviour(fleeBehaviour);
        }
        else
        {
            SetCurrentBehaviour(workerBehaviour);
            Debug.Log("no flee");

        }
    }


    public override void OnDie()
    {
        workerBehaviour.OnDie();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_UAI_Worker : Ab_UnitAI
{
    public B_Worker workerBehaviour;
    public B_Flee fleeBehaviour;

    public EC_Movement movement;
    public EC_ScanForEnemyUnits sensing;

    // Start is called before the first frame update
    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);      
        fleeBehaviour.SetUpBehaviour(entity, movement, sensing);
        workerBehaviour.SetUpBehaviour(entity, movement);
        SetCurrentBehaviour(workerBehaviour);
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            SetCurrentBehaviour(fleeBehaviour);
        }
        else
        {
            SetCurrentBehaviour(workerBehaviour);

        }
    }


    public override void OnDie()
    {
        workerBehaviour.OnDie();
    }
}

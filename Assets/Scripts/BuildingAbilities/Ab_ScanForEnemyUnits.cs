using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//maybe change this to sensing with scanning obptions or let all the scanning options derive from snesing later
public class Ab_ScanForEnemyUnits : Ability
{
    public int enemyUnitsLayer;

    public GameEntity[] enemiesInRange;
    public GameEntity nearestEnemy;
    

    public float scanInterval;
    public float scanRadius;
    float nextScanTime;

    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        nextScanTime = Time.time + Random.Range(0, scanInterval);
    }

    public override void UpdateAbility()
    {
        if (Time.time > nextScanTime)
        {
            nextScanTime = Time.time + scanInterval;
            Scan();
        }
    }

    void Scan()
    {

    }
}

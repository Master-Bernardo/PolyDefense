using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_ScanForEnemyBuildings : Ability
{
    public int buildingsLayer;
    public BuildingType buidingToScanFor;

    public HashSet<GameEntity> buildingsInRange = new HashSet<GameEntity>();
    public GameEntity nearestBuilding;

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
        int layerMask = 1 << buildingsLayer;

        Collider[] visibleColliders = Physics.OverlapSphere(transform.position, scanRadius, layerMask);
        buildingsInRange.Clear();

        for (int i = 0; i < visibleColliders.Length; i++)
        {
            Building currentEntity = visibleColliders[i].GetComponent<Building>();
            if (currentEntity.teamID != myEntity.teamID && currentEntity.buildingType == BuildingType.Defense)
            {
                buildingsInRange.Add(currentEntity);
            }
        }

        //get the nearest
        float nearestDistance = Mathf.Infinity;
        nearestBuilding = null;

        foreach (GameEntity enemy in buildingsInRange)
        {
            float currentDistance = (transform.position - enemy.transform.position).sqrMagnitude;
            if (currentDistance < nearestDistance)
            {
                nearestDistance = currentDistance;
                nearestBuilding = enemy;
            }
        }


    }

}

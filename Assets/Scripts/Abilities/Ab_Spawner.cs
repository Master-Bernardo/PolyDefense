using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Spawner : PassiveToogleableAbility
{
    public float populationSpawnInterval;
    [SerializeField]
    float nextPopulationSpawnTime;

    public Transform spawnPoint;
    public GameObject workerPrefab;

    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        nextPopulationSpawnTime = Time.time + populationSpawnInterval;
    }

    public override void UpdateAbility()
    {
        if (Time.time > nextPopulationSpawnTime)
        {
            if (PlayerManager.Instance.SpawnPossible(1))
            {
                //this should happen if the unit spawns
                nextPopulationSpawnTime = Time.time + populationSpawnInterval;
                GameObject instantiatedWorker = Instantiate(workerPrefab);
                instantiatedWorker.transform.position = spawnPoint.position;
            }
           
        }
    }
}


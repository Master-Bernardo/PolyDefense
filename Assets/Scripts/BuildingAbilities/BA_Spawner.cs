using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_Spawner : PassiveToogleableBuildingAbility
{
    public float populationSpawnInterval;
    float nextPopulationSpawnTime;

    public Transform spawnPoint;
    public GameObject workerPrefab;

    public override void SetUpAbility()
    {
        nextPopulationSpawnTime = Time.time + populationSpawnInterval;
    }

    public override void UpdateAbility()
    {
        if (Time.time > nextPopulationSpawnTime)
        {
            nextPopulationSpawnTime += populationSpawnInterval;
            GameObject instantiatedWorker = Instantiate(workerPrefab);
            instantiatedWorker.transform.position = spawnPoint.position;
        }
    }
}


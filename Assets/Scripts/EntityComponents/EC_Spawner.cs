using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_Spawner : EntityComponent
{
    public float populationSpawnInterval;
    [SerializeField]
    float nextPopulationSpawnTime;

    public Transform spawnPoint;
    public GameObject workerPrefab;

    bool reachedLimit;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        nextPopulationSpawnTime = Time.time + populationSpawnInterval;
        reachedLimit = false;
    }

    public override void UpdateComponent()
    {
        if (reachedLimit)
        {
            if (PlayerManager.Instance.SpawnPossible(1))
            {
                nextPopulationSpawnTime = Time.time + populationSpawnInterval;
                reachedLimit = false;
            }
        }
        else
        {
            if (Time.time > nextPopulationSpawnTime)
            {
                if (PlayerManager.Instance.SpawnPossible(1))
                {
                    //this should happen if the unit spawns
                    nextPopulationSpawnTime = Time.time + populationSpawnInterval;
                    GameObject instantiatedWorker = Instantiate(workerPrefab, spawnPoint.position, spawnPoint.rotation);
                }
                else
                {
                    reachedLimit = true;
                }

            }
        }
        

        
    }
}


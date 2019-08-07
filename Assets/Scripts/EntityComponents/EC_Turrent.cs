using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EC_Turrent : EntityComponent
{

    public float shootingInterval;
    float nextShootingTime;
    public float damage;

    public float turningSpeed;
    public Transform turrentTower; //rotates to the sides
    public Transform turrentBarrel; //rotates up and down
    /*public float maxXAngle;
    public float minXAngle;
    public float maxYAngle;
    public float minYAngle;
    public float maxZAngle;
    public float minZAngle;*/

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public EC_ScanForEnemyUnits scanForEnemies;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        nextShootingTime = Time.time + Random.Range(0, shootingInterval);
    }

    public override void UpdateComponent()
    {
        if (scanForEnemies.nearestEnemy != null)
        {
            NavMeshAgent agent = scanForEnemies.nearestEnemy.GetComponent<NavMeshAgent>();
            GameEntity entity = scanForEnemies.nearestEnemy.GetComponent<GameEntity>();
            //TODO calculate exact position by measuring distance of barrel travel time aginst projectile speed etc - add aimingSPeed random rotation before shooting
            RotateTowards(agent.transform.position + scanForEnemies.nearestEnemy.aimingCorrector + agent.velocity*0);
            if (Time.time > nextShootingTime)
            {
                nextShootingTime = Time.time + shootingInterval;
                Projectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation).GetComponent<Projectile>();
                projectile.projectileTeamID = myEntity.teamID;
            }
        }
    }

    void RotateTowards(Vector3 targetPosition)
    {
        //1.rotate tower to the sides
        Vector3 desiredTowerDirection = (targetPosition - turrentTower.position);
        desiredTowerDirection.y = 0;
        Quaternion desiredTowerRotation = Quaternion.LookRotation(desiredTowerDirection);
        turrentTower.rotation = Quaternion.RotateTowards(turrentTower.transform.rotation, desiredTowerRotation, turningSpeed);

        //2. rotate the barrel up and down 
        Vector3 desiredBarrelDirection = (targetPosition - turrentBarrel.position);
        desiredTowerDirection.y = desiredBarrelDirection.y;
        Quaternion desiredBarrelRotation = Quaternion.LookRotation(desiredTowerDirection);
        turrentBarrel.rotation = Quaternion.RotateTowards(turrentBarrel.transform.rotation, desiredBarrelRotation, turningSpeed);
        
        /*
        Vector3 desiredBarrelDirection = turrentBarrel.InverseTransformDirection((targetPosition - turrentBarrel.position));
        desiredBarrelDirection.x = 0;
        desiredBarrelDirection.z = 0;
        Quaternion desiredBarrelRotation = Quaternion.LookRotation(turrentBarrel.TransformDirection(desiredBarrelDirection));
        turrentBarrel.rotation = Quaternion.RotateTowards(turrentBarrel.transform.rotation, desiredBarrelRotation, turningSpeed);
        */
        //now cap the rotation;
        /*Vector3 cappedAngles;

        Vector3 currentAngles = transform.rotation.eulerAngles;
        if (currentAngles.x > maxXAngle) cappedAngles.x = maxXAngle;
        else if(currentXAngles)*/
    }
}

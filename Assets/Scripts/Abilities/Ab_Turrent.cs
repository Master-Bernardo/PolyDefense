using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Turrent : Ability
{

    public float shootingInterval;
    float nextShootingTime;
    public float damage;

    public float turningSpeed;
    public Transform turrenRotatingBarrel;
    /*public float maxXAngle;
    public float minXAngle;
    public float maxYAngle;
    public float minYAngle;
    public float maxZAngle;
    public float minZAngle;*/

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public Ab_ScanForEnemyUnits scanForEnemies;

    public override void SetUpAbility(GameEntity entity)
    {
        nextShootingTime = Time.time + Random.Range(0, shootingInterval);
    }

    public override void UpdateAbility()
    {
        if (scanForEnemies.nearestEnemy != null)
        {
            RotateTowards(scanForEnemies.nearestEnemy.transform.position);
            if (Time.time > nextShootingTime)
            {
                nextShootingTime = Time.time + shootingInterval;
                Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            }
        }
    }

    void RotateTowards(Vector3 position)
    {
        Quaternion desiredLookRotation = Quaternion.LookRotation(position - turrenRotatingBarrel.transform.position);
        turrenRotatingBarrel.transform.rotation = Quaternion.RotateTowards(turrenRotatingBarrel.transform.rotation, desiredLookRotation, turningSpeed);

        //now cap the rotation;
        /*Vector3 cappedAngles;

        Vector3 currentAngles = transform.rotation.eulerAngles;
        if (currentAngles.x > maxXAngle) cappedAngles.x = maxXAngle;
        else if(currentXAngles)*/
    }
}

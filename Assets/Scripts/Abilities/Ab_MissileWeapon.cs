using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_MissileWeapon : Ability
{
    public float shootingInterval;
    float nextShootingTime;
    public float damage;

    public float turningSpeed;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    Vector3 initialLookVector;
    bool aiming = false;
    Transform currentTarget;

    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        nextShootingTime = Time.time + Random.Range(0, shootingInterval);
        initialLookVector = transform.forward;
    }

    public override void UpdateAbility()
    {
        if (aiming)
        {
            RotateTowards(currentTarget.position - transform.position);
        }
        else
        {
            RotateTowards(initialLookVector);
        }
    }

    public void Shoot()
    {
        Projectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation).GetComponent<Projectile>();
        projectile.projectileTeamID = myEntity.teamID;
    }

    public bool CanShoot()
    {
        if (Time.time > nextShootingTime)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void AimAt(Transform target)
    {
        aiming = true;
        currentTarget = target;

    }

    public void AimAt(Vector3 position)
    {
        aiming = true;
    }

    public void StopAiming()
    {
        aiming = false;
    }


    void RotateTowards(Vector3 desiredLookVector)
    {
        //Quaternion desiredLookRotation = Quaternion.LookRotation(position - turrenRotatingBarrel.transform.position);
        Quaternion desiredLookRotation = Quaternion.LookRotation(desiredLookVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredLookRotation, turningSpeed);

    }
}

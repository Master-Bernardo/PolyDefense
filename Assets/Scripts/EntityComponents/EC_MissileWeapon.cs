using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_MissileWeapon : Ability
{
    public float shootingInterval;
    float nextShootingTime;
    public float damage;

    public float turningSpeed;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    public Transform parent;
    bool aiming = false;
    GameEntity currentTarget;

    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        nextShootingTime = Time.time + shootingInterval;
    }

    public override void UpdateAbility()
    {
        if (aiming)
        {
          if(currentTarget!=null)  RotateTowards(currentTarget.GetPositionForAiming() - transform.position);
        }
        else
        {
            RotateTowards(parent.forward);
        }
    }

    public void Shoot()
    {
        Projectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation).GetComponent<Projectile>();
        projectile.projectileTeamID = myEntity.teamID;
        nextShootingTime = Time.time + shootingInterval;
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

    public void AimAt(GameEntity target)
    {
        aiming = true;
        currentTarget = target;

    }

    /*public void AimAt(Vector3 position)
    {
        aiming = true;
    }*/

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

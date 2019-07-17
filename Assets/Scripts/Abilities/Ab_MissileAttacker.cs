using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_MissileAttacker : Ability
{
    public Ab_Movement movement;
    public Ab_ScanForEnemyUnits sensing;
    public Ab_MissileWeapon weapon;
    //goes to nearest enemy, shoots  and looks at them when in range, tries not to get too close

    public float maximalShootingDistance;
    public float minimalShootingDistance;

    public float checkingInterval;
    float nextCheckTime;

    enum ShootingState
    {
        Idle,
        TooFarAway,
        TooNear,
        InShootingDistance,
    }

    ShootingState shootingState;

    enum MissileFighterState
    {
        Idle,
        AttackingEnemy,
        AttackingBase
    }

    MissileFighterState state;

    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);

        maximalShootingDistance *= maximalShootingDistance;
        minimalShootingDistance *= minimalShootingDistance;
    }



    public override void UpdateAbility()
    {
        switch (state)
        {
            case MissileFighterState.Idle:

                if (sensing.nearestEnemy != null)
                {
                    state = MissileFighterState.AttackingEnemy;
                }

                break;

            case MissileFighterState.AttackingEnemy:

                break;

            case MissileFighterState.AttackingBase:

                break;
        }

        switch (shootingState)
        {
            case ShootingState.TooNear:

                if (Time.time > nextCheckTime)
                {
                    nextCheckTime = Time.time + checkingInterval;

                    float distanceToEnemy = (sensing.nearestEnemy.transform.position - transform.position).sqrMagnitude;
                    if (distanceToEnemy > minimalShootingDistance)
                    {
                        shootingState = ShootingState.InShootingDistance;
                        movement.Stop();
                    }
                    else
                    {
                        //move to a position perfectly in the shooting range
                        movement.MoveTo((transform.position - sensing.nearestEnemy.transform.position).normalized * (minimalShootingDistance + (maximalShootingDistance - minimalShootingDistance) / 2));
                        weapon.AimAt(sensing.nearestEnemy);
                        if (weapon.CanShoot()) weapon.Shoot();
                    }
                }

                break;

            case ShootingState.TooFarAway:

                if (Time.time > nextCheckTime)
                {
                    nextCheckTime = Time.time + checkingInterval;

                    float distanceToEnemy = (sensing.nearestEnemy.transform.position - transform.position).sqrMagnitude;
                    if (distanceToEnemy < maximalShootingDistance)
                    {
                        shootingState = ShootingState.InShootingDistance;
                        movement.LookAt(sensing.nearestEnemy.transform);
                        weapon.AimAt(sensing.nearestEnemy);
                        movement.Stop();
                    }
                    else
                    {
                        movement.MoveTo((transform.position - sensing.nearestEnemy.transform.position).normalized * (minimalShootingDistance + (maximalShootingDistance - minimalShootingDistance) / 2));
                    }
                }
                break;

            case ShootingState.InShootingDistance:

                //1. check if we need to change state
                if (Time.time > nextCheckTime)
                {
                    nextCheckTime = Time.time + checkingInterval;
                    float distanceToEnemy = (sensing.nearestEnemy.transform.position - transform.position).sqrMagnitude;

                    if (distanceToEnemy < minimalShootingDistance)
                    {
                        shootingState = ShootingState.TooNear;
                        movement.LookAt(sensing.nearestEnemy.transform);
                        movement.MoveTo((transform.position - sensing.nearestEnemy.transform.position).normalized * (minimalShootingDistance + (maximalShootingDistance - minimalShootingDistance) / 2));
                    }
                    else if (distanceToEnemy > maximalShootingDistance)
                    {
                        shootingState = ShootingState.TooFarAway;
                        movement.StopLookAt();
                        weapon.StopAiming();
                        movement.MoveTo((transform.position - sensing.nearestEnemy.transform.position).normalized * (minimalShootingDistance + (maximalShootingDistance - minimalShootingDistance) / 2));
                    }
                    else
                    {
                        if (weapon.CanShoot()) weapon.Shoot();
                    }

                }

                break;
        }
    }
}

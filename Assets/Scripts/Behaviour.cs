using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Behaviour 
{
    //every behaviour needs some components that the unitAI needs to have saved
    protected Ab_UnitAI unitAI;

    /*public Behaviour(Ab_UnitAI unitAI)
    {
        this.unitAI = unitAI;
    }*/

    public virtual void SetUpBehaviour(Ab_UnitAI unitAI)
    {
        this.unitAI = unitAI;
    }

    public virtual void UpdateBehaviour()
    {

    }

    public virtual void OnBehaviourEnter()
    {

    }

    public virtual void OnBehaviourExit()
    {

    }
}

[System.Serializable]
public class B_WanderAroundPosition: Behaviour
{
    public float idleMovementInterval;
    float nextIdleMovementTime;
    public float maxDistanceToPositionWhileWandering;
    Ab_Movement movement;
    Transform positionToWanderAround;

    public void SetUpBehaviour(Ab_UnitAI unitAI, Ab_Movement movement)
    {
        this.unitAI = unitAI;
        this.movement = movement;

        nextIdleMovementTime = UnityEngine.Random.Range(0, idleMovementInterval);
    }

    public void SetPositionToWanderAround(Transform positionToWanderAround)
    {
        this.positionToWanderAround = positionToWanderAround;
    }

    public override void UpdateBehaviour()
    {
        nextIdleMovementTime = Time.time + idleMovementInterval;
        Vector3 wanderPoint = UnityEngine.Random.insideUnitSphere * 4;
        wanderPoint.y = unitAI.transform.position.y;

        wanderPoint += unitAI.transform.forward * 4 + unitAI.transform.position;

        Vector3 basePosition = positionToWanderAround.position;
        //if he would stray off to far, bring him back to base
        if (Vector3.Distance(basePosition, wanderPoint) > maxDistanceToPositionWhileWandering)
        {
            wanderPoint += (basePosition - wanderPoint) / 4;
        }

        movement.MoveTo(wanderPoint);
    }
}

[System.Serializable]
public class B_MeleeFighter : Behaviour
{
    Ab_ScanForEnemyUnits enemySensing;
    Ab_Movement movement;

    public float checkingInterval;
    float nextCheckTime;

    //meleefighting
    Ab_MeleeWeapon weapon;

    public void SetUpBehaviour(Ab_UnitAI unitAI, Ab_Movement movement, Ab_ScanForEnemyUnits enemySensing, Ab_MeleeWeapon weapon)
    {
        this.unitAI = unitAI;
        this.movement = movement;
        this.enemySensing = enemySensing;
        this.weapon = weapon;

        nextCheckTime = UnityEngine.Random.Range(0, checkingInterval);
    }

    enum MeleeFighterState
    {
        MovingToEnemy,
        InMeleeDistance
    }

    MeleeFighterState state;

    public override void UpdateBehaviour()
    {
        switch (state)
        {
            case MeleeFighterState.MovingToEnemy:

                if (Time.time > nextCheckTime)
                {
                    nextCheckTime = Time.time + checkingInterval;

                    if ((enemySensing.nearestEnemy.transform.position - unitAI.transform.position).sqrMagnitude < weapon.meleeRange)
                    {
                        state = MeleeFighterState.InMeleeDistance;
                        movement.Stop();
                    }
                    else
                    {
                        movement.MoveTo(enemySensing.nearestEnemy.transform.position);
                    }
                }

                break;

            case MeleeFighterState.InMeleeDistance:

                if ((enemySensing.nearestEnemy.transform.position - unitAI.transform.position).sqrMagnitude > weapon.meleeRange)
                {
                    state = MeleeFighterState.MovingToEnemy;
                }
                else
                {
                    if (weapon.CanAttack())
                    {
                        weapon.Attack(enemySensing.nearestEnemy.GetComponent<IDamageable<float>>());
                    }                   
                }

                break;
        }

       
    }
}


[System.Serializable]
public class B_MissileFighter : Behaviour
{
    Ab_Movement movement;
    Ab_ScanForEnemyUnits sensing;
    Ab_MissileWeapon weapon;
    //goes to nearest enemy, shoots  and looks at them when in range, tries not to get too close

    public float maximalShootingDistance;
    float maximalShootingDistanceSquared;
    public float minimalShootingDistance;
    float minimalShootingDistanceSquared;


    public float checkingInterval;
    float nextCheckTime;

    enum MissileFighterState
    {
        TooFarAway,
        TooNear,
        InShootingDistance,
    }

    MissileFighterState state;

    public void SetUpBehaviour(Ab_UnitAI unitAI, Ab_Movement movement, Ab_ScanForEnemyUnits sensing, Ab_MissileWeapon weapon)
    {
        this.sensing = sensing;
        this.unitAI = unitAI;
        this.movement = movement;
        this.weapon = weapon;

        nextCheckTime = UnityEngine.Random.Range(0, checkingInterval);

        maximalShootingDistanceSquared =  maximalShootingDistance * maximalShootingDistance;
        minimalShootingDistanceSquared = minimalShootingDistance  * minimalShootingDistance;
    }



    public override void UpdateBehaviour()
    {
        switch (state)
        {
            case MissileFighterState.TooNear:

                if (Time.time > nextCheckTime)
                {
                    nextCheckTime = Time.time + checkingInterval;

                    float distanceToEnemy = (sensing.nearestEnemy.transform.position - unitAI.transform.position).sqrMagnitude;
                    if (distanceToEnemy > minimalShootingDistanceSquared) 
                    {
                        state = MissileFighterState.InShootingDistance;
                        movement.Stop();
                    }
                    else
                    {
                        //move to a position perfectly in the shooting range
                        movement.MoveTo(unitAI.transform.position + (unitAI.transform.position - sensing.nearestEnemy.transform.position).normalized * (maximalShootingDistance/2));
                        Debug.Log("move");
                        Debug.Log("myPos: + " + unitAI.transform.position);
                        Debug.Log("divide: + " + (maximalShootingDistance/2));
                        Debug.Log("pos to move to: " + unitAI.transform.position + (unitAI.transform.position - sensing.nearestEnemy.transform.position).normalized * (maximalShootingDistance / 2));
                    }
                }

                if (weapon.CanShoot()) weapon.Shoot();


                break;

            case MissileFighterState.TooFarAway:

                if (Time.time > nextCheckTime)
                {
                    nextCheckTime = Time.time + checkingInterval;

                    float distanceToEnemy = (sensing.nearestEnemy.transform.position - unitAI.transform.position).sqrMagnitude;
                    if (distanceToEnemy < maximalShootingDistanceSquared)
                    {
                        state = MissileFighterState.InShootingDistance;
                        movement.LookAt(sensing.nearestEnemy.transform);
                        weapon.AimAt(sensing.nearestEnemy);
                        movement.Stop();
                    }
                    else
                    {
                        movement.MoveTo(sensing.nearestEnemy.transform.position);
                    }
                }
                    break;

            case MissileFighterState.InShootingDistance:

                //1. check if we need to change state
                if (Time.time > nextCheckTime)
                {
                    nextCheckTime = Time.time + checkingInterval;
                    float distanceToEnemy = (sensing.nearestEnemy.transform.position - unitAI.transform.position).sqrMagnitude;

                    if (distanceToEnemy < minimalShootingDistanceSquared)
                    {
                        state = MissileFighterState.TooNear;
                        movement.LookAt(sensing.nearestEnemy.transform);
                        movement.MoveTo(unitAI.transform.position + (unitAI.transform.position - sensing.nearestEnemy.transform.position).normalized * (maximalShootingDistance / 2));
                    }
                    else if (distanceToEnemy > maximalShootingDistanceSquared)
                    {
                        state = MissileFighterState.TooFarAway;
                        movement.StopLookAt();
                        weapon.StopAiming();
                        movement.MoveTo(sensing.nearestEnemy.transform.position);
                    }

                }

                if (weapon.CanShoot()) weapon.Shoot();


                break;
        }
    }

    public override void OnBehaviourEnter()
    {
        float distanceToEnemy = (sensing.nearestEnemy.transform.position - unitAI.transform.position).sqrMagnitude;
        if (distanceToEnemy < minimalShootingDistanceSquared)
        {
            state = MissileFighterState.TooNear;
            movement.LookAt(sensing.nearestEnemy.transform);
            weapon.AimAt(sensing.nearestEnemy);
            movement.MoveTo(unitAI.transform.position + (unitAI.transform.position - sensing.nearestEnemy.transform.position).normalized * (maximalShootingDistance / 2));
        }
        else if (distanceToEnemy > maximalShootingDistanceSquared)
        {
            state = MissileFighterState.TooFarAway;
            movement.StopLookAt();
            movement.MoveTo(sensing.nearestEnemy.transform.position);
            weapon.StopAiming();
        }
        else
        {
            movement.LookAt(sensing.nearestEnemy.transform);
            weapon.AimAt(sensing.nearestEnemy);
            movement.Stop();
            state = MissileFighterState.InShootingDistance;
        }
    }

    public override void OnBehaviourExit()
    {
        movement.StopLookAt();
    }
}

[System.Serializable]
public class B_MeleeAttackBuilding : Behaviour
{
    Building targetBuilding;
    Ab_Movement movement;

    Ab_MeleeWeapon weapon;

    public float distanceCheckingInterval;
    float nextDistanceCheckTime;

    enum AttackBuildingState
    {
        BehaviourStart,
        MovingToBuilding,
        InMeleeDistance
    }

    AttackBuildingState state;

    public void SetUpBehaviour(Ab_UnitAI unitAI, Ab_Movement movement, Ab_MeleeWeapon weapon)
    {
        this.unitAI = unitAI;
        this.movement = movement;
        this.weapon = weapon;

        nextDistanceCheckTime = UnityEngine.Random.Range(0, distanceCheckingInterval);

    }

    public void SetTargetBuilding(Building targetBuilding)
    {
        this.targetBuilding = targetBuilding;
    }

    public override void UpdateBehaviour()
    {
        switch (state)
        {
            case AttackBuildingState.BehaviourStart:

                movement.MoveTo(targetBuilding.transform.position);
                state = AttackBuildingState.MovingToBuilding;

                break;

            case AttackBuildingState.MovingToBuilding:

                if (Time.time > nextDistanceCheckTime)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;

                    if (weapon.IsInMeleeRange(targetBuilding.transform.position))
                    {
                        state = AttackBuildingState.InMeleeDistance;
                        movement.Stop();
                    }
                }

                break;

            case AttackBuildingState.InMeleeDistance:

                if (!weapon.IsInMeleeRange(targetBuilding.transform.position))
                {
                    state = AttackBuildingState.BehaviourStart;
                }
                else
                {
                    if (weapon.CanAttack())
                    {
                        weapon.Attack(targetBuilding.GetComponent<IDamageable<float>>());
                    }
                }



                    break;
        }
    }
}


[System.Serializable]
public class B_MissileAttackBuilding : Behaviour
{
    Building targetBuilding;
    Ab_Movement movement;

    Ab_MissileWeapon weapon;

    public float distanceCheckingInterval;
    float nextDistanceCheckTime;

    public float maximalShootingDistance;
    float maximalShootingDistanceSquared;
    public float minimalShootingDistance;
    float minimalShootingDistanceSquared;

    enum AttackBuildingState
    {
        TooFarAway,
        TooNear,
        InShootingDistance,
    }

    AttackBuildingState state;

    public void SetUpBehaviour(Ab_UnitAI unitAI, Ab_Movement movement, Ab_MissileWeapon weapon)
    {
        this.unitAI = unitAI;
        this.movement = movement;
        this.weapon = weapon;

        maximalShootingDistanceSquared = maximalShootingDistance * maximalShootingDistance;
        minimalShootingDistanceSquared = minimalShootingDistance * minimalShootingDistance;

        nextDistanceCheckTime = UnityEngine.Random.Range(0, distanceCheckingInterval);

    }

    public void SetTargetBuilding(Building targetBuilding)
    {
        this.targetBuilding = targetBuilding;
    }

    public override void OnBehaviourEnter()
    {
        Debug.Log("behaviour enters");
        float distanceToEnemy = (targetBuilding.transform.position - unitAI.transform.position).sqrMagnitude;
        if (distanceToEnemy < minimalShootingDistanceSquared)
        {
            Debug.Log("1");
            state = AttackBuildingState.TooNear;
            movement.LookAt(targetBuilding.transform);
            weapon.AimAt(targetBuilding);
            movement.MoveTo(unitAI.transform.position + (unitAI.transform.position- targetBuilding.transform.position).normalized * (maximalShootingDistance / 2));
        }
        else if (distanceToEnemy > maximalShootingDistanceSquared)
        {
            Debug.Log("2");

            state = AttackBuildingState.TooFarAway;
            movement.StopLookAt();
            movement.MoveTo(targetBuilding.transform.position);
            weapon.StopAiming();
        }
        else
        {
            Debug.Log("3");
            movement.LookAt(targetBuilding.transform);
            movement.Stop();
            state = AttackBuildingState.InShootingDistance;
            weapon.AimAt(targetBuilding);
        }
    }

    public override void UpdateBehaviour()
    {
        switch (state)
        {
            case AttackBuildingState.TooNear:

                if (nextDistanceCheckTime > Time.time)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;
                    if((targetBuilding.transform.position - unitAI.transform.position).sqrMagnitude > minimalShootingDistanceSquared)
                    {
                        movement.Stop();
                        state = AttackBuildingState.InShootingDistance;
                    }
                }

                if (weapon.CanShoot()) weapon.Shoot();




                break;

            case AttackBuildingState.InShootingDistance:

                if (nextDistanceCheckTime > Time.time)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;
                    float distanceToEnemy = (targetBuilding.transform.position - unitAI.transform.position).sqrMagnitude;

                    if (distanceToEnemy < minimalShootingDistanceSquared)
                    {
                        state = AttackBuildingState.TooNear;
                        movement.LookAt(targetBuilding.transform);
                        movement.MoveTo(unitAI.transform.position + (unitAI.transform.position - targetBuilding.transform.position).normalized * (maximalShootingDistance / 2));
                    }
                    else if (distanceToEnemy > maximalShootingDistanceSquared)
                    {
                        state = AttackBuildingState.TooFarAway;
                        movement.StopLookAt();
                        movement.MoveTo(targetBuilding.transform.position);
                        weapon.StopAiming();
                    }
                }

                if (weapon.CanShoot()) weapon.Shoot();



                break;

            case AttackBuildingState.TooFarAway:

                if (nextDistanceCheckTime > Time.time)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;

                    if ((targetBuilding.transform.position - unitAI.transform.position).sqrMagnitude < maximalShootingDistanceSquared)
                    {
                        movement.LookAt(targetBuilding.transform);
                        movement.Stop();
                        state = AttackBuildingState.InShootingDistance;
                        weapon.AimAt(targetBuilding);
                    }

                }
                    break;

        }
    }
}


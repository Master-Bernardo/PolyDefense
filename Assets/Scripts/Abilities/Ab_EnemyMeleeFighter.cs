using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_EnemyMeleeFighter : Ability
{
    public Ab_Movement movement;
    public Ab_ScanForEnemyUnits sensing;
    public Ab_ScanForEnemyBuildings buildingsSensing;


    Transform baseBuilding;
    //some idle movement

    public float meleeDistance;
    public float meleeAttackInterval;
    float nextMeleeAttackTime;
    public float meleeDamage;

    enum EnemyState
    {
        Idle,
        MovingToEnemy,
        MovingToAttackBase,
        InMeleeDistance
    }

    EnemyState state;

    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        meleeDistance *= meleeDistance;
    }

    public override void UpdateAbility()
    {
        switch (state)
        {
            case EnemyState.Idle:

                if (sensing.nearestEnemy != null)
                {
                    state = EnemyState.MovingToEnemy;
                }
                else 
                {
                    state = EnemyState.MovingToAttackBase;
                    movement.MoveTo(BuildingSystem.Instance.playersBaseLocation.position);
                }

                break;

            case EnemyState.MovingToEnemy:

                if (sensing.nearestEnemy != null)
                {
                    if ((sensing.nearestEnemy.transform.position - transform.position).sqrMagnitude < meleeDistance)
                    {
                        state = EnemyState.InMeleeDistance;
                    }
                    else
                    {
                        movement.MoveTo(sensing.nearestEnemy.transform.position);
                    }
                }
                else
                {
                    state = EnemyState.Idle;
                }

                break;

            case EnemyState.InMeleeDistance:

                if (sensing.nearestEnemy != null)
                {
                    if ((sensing.nearestEnemy.transform.position - transform.position).sqrMagnitude > meleeDistance)
                    {
                        state = EnemyState.MovingToEnemy;
                    }
                    else
                    {
                        if (Time.time > nextMeleeAttackTime)
                        {
                            nextMeleeAttackTime = Time.time + meleeAttackInterval;
                            sensing.nearestEnemy.GetComponent<IDamageable<float>>().TakeDamage(meleeDamage);
                        }
                    }
                }
                else
                {
                    state = EnemyState.Idle;
                }

                break;

            case EnemyState.MovingToAttackBase:

                if (sensing.nearestEnemy != null)
                {
                    state = EnemyState.MovingToEnemy;
                }
                else if ((BuildingSystem.Instance.playersBaseLocation.position - transform.position).sqrMagnitude < meleeDistance)
                {
                    if (Time.time > nextMeleeAttackTime)
                    {
                        nextMeleeAttackTime = Time.time + meleeAttackInterval;
                        BuildingSystem.Instance.playersBaseLocation.gameObject.GetComponent<IDamageable<float>>().TakeDamage(meleeDamage);
                    }
                }

                break;
        }
    }
}

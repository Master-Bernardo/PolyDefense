using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_EnemyMeleeFighter : Ability
{
    public EC_Movement movement;
    public EC_ScanForEnemyUnits sensing;
    //public Ab_ScanForEnemyBuildings buildingsSensing;
    public EC_MeleeWeapon weapon;


    Transform baseBuilding;
    //some idle movement

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
                    if ((sensing.nearestEnemy.transform.position - transform.position).sqrMagnitude < weapon.meleeRange)
                    {
                        movement.Stop();
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
                    if ((sensing.nearestEnemy.transform.position - transform.position).sqrMagnitude > weapon.meleeRange)
                    {
                        state = EnemyState.MovingToEnemy;
                    }
                    else
                    {
                        if (weapon.CanAttack()) weapon.Attack(sensing.nearestEnemy.GetComponent<IDamageable<float>>());
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
                else if ((BuildingSystem.Instance.playersBaseLocation.position - transform.position).sqrMagnitude < weapon.meleeRange)
                {
                    if (weapon.CanAttack())
                    {
                        weapon.Attack(BuildingSystem.Instance.playersBaseLocation.gameObject.GetComponent<IDamageable<float>>());
                    }
                }

                break;
        }
    }
}

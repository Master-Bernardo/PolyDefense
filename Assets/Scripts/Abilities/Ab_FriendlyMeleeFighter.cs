using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_FriendlyMeleeFighter : Ability
{
    public Ab_Movement movement;
    public Ab_ScanForEnemyUnits sensing;


    public Transform buildingToGoAround;
    //some idle movement
    public float idleMovementInterval;
    float nextIdleMovementTime;
    public float maxDistanceToBaseForWander;

    public float meleeDistance;
    public float meleeAttackInterval;
    float nextMeleeAttackTime;
    public float meleeDamage;

    enum DefenderState
    {
        Idle,
        MovingToEnemy,
        InMeleeDistance
    }

    DefenderState state;

    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        nextIdleMovementTime = Random.Range(0, idleMovementInterval);
        meleeDistance *= meleeDistance;
        buildingToGoAround = transform; //workaround - to let the work if set into scene by hand
    }

    public void SetSpawnedBuilding(Transform building)
    {
        buildingToGoAround = building;
    }

    public override void UpdateAbility()
    {
        switch (state)
        {
            case DefenderState.Idle:

                if (sensing.nearestEnemy != null)
                {
                    state = DefenderState.MovingToEnemy;
                }
                else
                {
                    nextIdleMovementTime = Time.time + idleMovementInterval;
                    Vector3 wanderPoint = Random.insideUnitSphere * 4;
                    wanderPoint.y = transform.position.y;

                    wanderPoint += transform.forward * 4 + transform.position;

                    Vector3 basePosition = buildingToGoAround.position;
                    //if he would stray off to far, bring him back to base
                    if (Vector3.Distance(basePosition, wanderPoint) > maxDistanceToBaseForWander)
                    {
                        wanderPoint += (basePosition - wanderPoint) / 4;
                    }

                    movement.MoveTo(wanderPoint);
                }

                break;

            case DefenderState.MovingToEnemy:

                if (sensing.nearestEnemy != null)
                {
                    if ((sensing.nearestEnemy.transform.position - transform.position).sqrMagnitude < meleeDistance)
                    {
                        state = DefenderState.InMeleeDistance;
                    }
                    else
                    {
                        movement.MoveTo(sensing.nearestEnemy.transform.position);
                    }
                }
                else
                {
                    state = DefenderState.Idle;
                }

                break;

            case DefenderState.InMeleeDistance:

                if (sensing.nearestEnemy != null)
                {
                    if ((sensing.nearestEnemy.transform.position - transform.position).sqrMagnitude > meleeDistance)
                    {
                        state = DefenderState.MovingToEnemy;
                    }
                    else
                    {
                        if (Time.time > nextMeleeAttackTime)
                        {
                            nextMeleeAttackTime = Time.time + meleeAttackInterval;
                            sensing.nearestEnemy.GetComponent<IDamageable<float>>().TakeDamage(meleeDamage);
                            //Debug.Log("test: " + sensing.nearestEnemy.GetComponent<IDamageable<float>>());
                        }
                    }
                }
                else
                {
                    state = DefenderState.Idle;
                }


                break;
        }
        
     

     
    }
}

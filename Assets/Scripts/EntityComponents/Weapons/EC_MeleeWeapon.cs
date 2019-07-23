using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_MeleeWeapon : Ability
{
    //public float meleeRange;
    public float meleeDamage;
    public float meleeAttackInterval;
    float nextPrepareMeleeAttackTime;

    //public Transform weaponTransform;
    public Animator weaponAnimator;
  
    float nextMeleeAttackTime;
    public float attackDuration;
    bool attack;

    public bool drawDamageGizmo;
    [Tooltip("position relative to the unit")]
    public Vector3 hitPosition;
    public float hitSphereRadius;
    int weaponTeamID;

    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        //meleeRange *= meleeRange; //because we do a square magnitude check
        weaponTeamID = entity.teamID;
    }

    public void Attack()
    {
        if (Time.time > nextPrepareMeleeAttackTime)
        {
            nextPrepareMeleeAttackTime = Time.time + meleeAttackInterval;

            //target.TakeDamage(meleeDamage);
            attack = true;
            nextMeleeAttackTime = Time.time + attackDuration;
            weaponAnimator.SetTrigger("Attack");
            //currentTarget = target;
        }
    }

    public override void UpdateAbility()
    {
        if (attack)
        {
            if (Time.time > nextMeleeAttackTime)
            {
                ExecuteAttack();
                attack = false;
            }
        }
    }

    void ExecuteAttack()
    {
        // if (currentTarget != null) currentTarget.TakeDamage(meleeDamage);

        Collider[] visibleColliders = Physics.OverlapSphere(myEntity.transform.TransformPoint(hitPosition), hitSphereRadius);

        for (int i = 0; i < visibleColliders.Length; i++)
        {
            IDamageable<float> damageable = visibleColliders[i].gameObject.GetComponent<IDamageable<float>>();


            if (damageable != null)
            {
                // check who did we hit, check if he has an gameEntity
                GameEntity entity = visibleColliders[i].gameObject.GetComponent<GameEntity>();
                if (entity != null)
                {
                    if (!Settings.Instance.friendlyFire)
                    {
                        DiplomacyStatus diplomacyStatus = Settings.Instance.GetDiplomacyStatus(weaponTeamID, entity.teamID);
                        if (diplomacyStatus == DiplomacyStatus.War)
                        {
                            damageable.TakeDamage(meleeDamage);
                        }

                    }
                    else
                    {
                        damageable.TakeDamage(meleeDamage);
                    }

                }
                else
                {
                    damageable.TakeDamage(meleeDamage);
                }
                return;
            }

        }




    }

    public bool CanAttack()
    {
        if (Time.time > nextPrepareMeleeAttackTime) return true;
        else return false;
    }

    public bool IsInMeleeRange(Vector3 target)
    {//refsctor use the hit box range

        /* if ((target - myEntity.transform.position).sqrMagnitude < meleeRange)
         {
             return true;
         }
         else
         {
             return false;
         }/*/
        return false;
    }

    private void OnDrawGizmos()
    {
        if (myEntity != null)
        {
            if (drawDamageGizmo)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(myEntity.transform.TransformPoint(hitPosition), hitSphereRadius);
            }
        }   
    }
}

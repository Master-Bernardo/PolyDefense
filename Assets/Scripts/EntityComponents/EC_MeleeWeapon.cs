using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_MeleeWeapon : Ability
{
    public float meleeRange;
    public float meleeDamage;
    public float meleeAttackInterval;
    float nextMeleeAttackTime;

    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        meleeRange *= meleeRange; //because we do a square magnitude check
    }

    public void Attack(IDamageable<float> target)
    {
        if (Time.time > nextMeleeAttackTime)
        {
            nextMeleeAttackTime = Time.time + meleeAttackInterval;

            target.TakeDamage(meleeDamage);
        }
    }

    public bool CanAttack()
    {
        if (Time.time > nextMeleeAttackTime) return true;
        else return false;
    }

    public bool IsInMeleeRange(Vector3 target)
    {
        if ((target - myEntity.transform.position).sqrMagnitude < meleeRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

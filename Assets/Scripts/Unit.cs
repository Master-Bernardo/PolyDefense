using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GameEntity, IDamageable<float>
{
    public UnitData unitData;
    float currentHealth;
    public float width;

    public virtual void Awake()
    {
        currentHealth = unitData.healthPoints;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDie();
        }

    }
}

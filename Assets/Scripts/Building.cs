using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Economy,
    Defense,
    Other
}

public class Building : GameEntity, IDamageable<float>
{
    public BuildingData buildingData;
    float currentHealth;
    public float width;
    public BuildingType buildingType;   



    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDie();
        }
  
    }

    public virtual void Awake()
    {
        currentHealth = buildingData.healthPoints;
    }

    
}


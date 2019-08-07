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
    public float currentHealth;
    public BuildingType buildingType;
    public bool isBase; //refactor another time



    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(isBase) UIManager.Instance.SetBaseHP(currentHealth, buildingData.healthPoints);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
  
    }


    public virtual void Awake()
    {
        currentHealth = buildingData.healthPoints;
    }

    
}


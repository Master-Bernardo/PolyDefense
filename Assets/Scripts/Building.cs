using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : GameEntity
{
    public BuildingData buildingData;
    public BuildingAbility[] buildingAbilities;
    float currentHealth;


    public override void TakeDamage(float damage)
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

    private void Start()
    {
        foreach(BuildingAbility ability in buildingAbilities)
        {
            ability.SetUpAbility();
        }
    }

    public void Update()
    {
        foreach (BuildingAbility ability in buildingAbilities)
        {
            ability.UpdateAbility();
        }
    }

    public override void OnDie()
    {
        foreach (BuildingAbility ability in buildingAbilities)
        {
            ability.OnDie();
        }
        base.OnDie();
    }
}


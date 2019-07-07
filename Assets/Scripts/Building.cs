using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : GameEntity
{
    public BuildingData buildingData;
    public Ability[] buildingAbilities;
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
        foreach(Ability ability in buildingAbilities)
        {
            ability.SetUpAbility(this);
        }
    }

    public void Update()
    {
        foreach (Ability ability in buildingAbilities)
        {
            ability.UpdateAbility();
        }
    }

    public override void OnDie()
    {
        foreach (Ability ability in buildingAbilities)
        {
            ability.OnDie();
        }
        base.OnDie();
    }
}


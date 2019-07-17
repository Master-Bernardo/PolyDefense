using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * takes care of construction a building and activates the finished go at completion
 */
public class BuildingInConstruction : GameEntity, IDamageable<float>
{
    public BuildingData buildingData;

    public float width; //used by units who want to build it, so the know when they are near enough

    public GameObject inConstructionModel;

    public GameObject constructedBuildingPrefab;
    public BuildingConstructionModelChanger buildingConstructionModelChanger;

    //constructionprocess
    int currentConstructionPoints;

   
    void Awake()
    {
        BuildingSystem.Instance.AddBuildingWaitingForConstruction(this);
    }


    public void Construct(int addedConstructionPoints)
    {
        //Debug.Log("added " + addedConstructionPoints);
        //Debug.Log("current: " + currentConstructionPoints);

        currentConstructionPoints += addedConstructionPoints;

        if(currentConstructionPoints >= buildingData.constructionPoints)
        {
            currentConstructionPoints = buildingData.constructionPoints;
            FinishCostruction();
        }

        buildingConstructionModelChanger.ChangeModel(currentConstructionPoints*1f / buildingData.constructionPoints);

    }

    public void TakeDamage(float damage)
    {
        currentConstructionPoints -= (int)damage;
        if (currentConstructionPoints <= 0)
        {
            currentConstructionPoints = 0;
            Die();
        }
    }

    public override void Die()
    {
        base.Die();
        BuildingSystem.Instance.RemoveBuildingWaitingForConstruction(this);

    }


    public void FinishCostruction()
    {
        BuildingSystem.Instance.RemoveBuildingWaitingForConstruction(this);

        Instantiate(constructedBuildingPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Construct(10);
        }
    }
}

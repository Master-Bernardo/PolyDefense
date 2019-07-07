using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingConstructionState
{
    PlaningPossible,
    PlaningImpossible,
    InConstruction,
}
/*
 * takes care of placing the building and constructing it 
 */
public class ConstructingBuilding : GameEntity
{
    public BuildingConstructionState state;
    public BuildingData buildingData;

    public GameObject inConstructionModel;
    public GameObject planingModel;
    public MeshRenderer planingModelRenderer;
    public GameObject constructedBuilding;
    public BuildingPlacementTrigger buildingPlacementTrigger;
    public BuildingConstructionModelChanger buildingConstructionModelChanger;

    //constructionprocess
    int currentConstructionPoints;

    [Tooltip("the building gets raised by this value above ground")]
    public float heightRiser;

    public void SetPlaningPossible()
    {
        var block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", Color.green);

        planingModelRenderer.SetPropertyBlock(block);
    }

    public void SetPlaningImpossible()
    {
        var block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", Color.red);

        planingModelRenderer.SetPropertyBlock(block);
    }

    public void StartConstruction()
    {
        state = BuildingConstructionState.InConstruction;
        planingModel.SetActive(false);
        inConstructionModel.SetActive(true);
    }

    public void Construct(int addedConstructionPoints)
    {
        Debug.Log("added " + addedConstructionPoints);
        Debug.Log("current: " + currentConstructionPoints);

        currentConstructionPoints += addedConstructionPoints;

        if(currentConstructionPoints >= buildingData.constructionPoints)
        {
            currentConstructionPoints = buildingData.constructionPoints;
            FinishCostruction();
        }

        buildingConstructionModelChanger.ChangeModel(currentConstructionPoints*1f / buildingData.constructionPoints);

    }

    public override void TakeDamage(float damage)
    {
        currentConstructionPoints -= (int)damage;
        if (currentConstructionPoints <= 0)
        {
            currentConstructionPoints = 0;
            OnDie();
        }
    }

    public override void OnDie()
    {
        base.OnDie();
    }


    public void FinishCostruction()
    {
        inConstructionModel.SetActive(false);
        constructedBuilding.SetActive(true);
        constructedBuilding.transform.SetParent(null);
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

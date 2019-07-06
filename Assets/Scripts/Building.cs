using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingState
{
    PlaningPossible,
    PlaningImpossible,
    InConstruction,
    Constructed
}

public class Building : MonoBehaviour
{
    public BuildingState state;
    public BuildingData buildingData;

    public GameObject inConstructionModel;
    public GameObject planingModel;
    public MeshRenderer planingModelRenderer;
    public GameObject constructedModel;
    public BuildingPlacementTrigger buildingPlacementTrigger;
    public BuildingConstructionModelChanger buildingConstructionModelChanger;

    //constructionprocess
    public int constructionPoints; // how long is the buildingProcess?
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
        state = BuildingState.InConstruction;
        planingModel.SetActive(false);
        inConstructionModel.SetActive(true);
    }

    public void Construct(int addedConstructionPoints)
    {
        Debug.Log("added " + addedConstructionPoints);
        Debug.Log("current: " + currentConstructionPoints);

        currentConstructionPoints += addedConstructionPoints;

        if(currentConstructionPoints >= constructionPoints)
        {
            currentConstructionPoints = constructionPoints;
            FinishCostruction();
        }

        buildingConstructionModelChanger.ChangeModel(currentConstructionPoints*1f / constructionPoints);

    }

    public void FinishCostruction()
    {
        state = BuildingState.Constructed;
        inConstructionModel.SetActive(false);
        constructedModel.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Construct(10);
        }
    }
}

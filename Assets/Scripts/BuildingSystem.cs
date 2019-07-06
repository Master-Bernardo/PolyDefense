using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingSystemState
{
    Disabled,
    Planning
}

//aka buildingSystem
public class BuildingSystem : MonoBehaviour
{
    public float gridCellSize;
    //public int gridSize;

    Building currentBuilding;
    GameObject currentBuildingPrefab;

    public void StartPlaning(GameObject buildingPrefab)
    {
        currentBuilding = Instantiate(buildingPrefab).GetComponent<Building>();
        currentBuildingPrefab = buildingPrefab;
    }
    
    public void StopPlaning()
    {
        Destroy(currentBuilding.gameObject);
        currentBuilding = null;
    }
    
    //snaps the current building to the grid
    public void PlanBuilding(Vector3 clickedPoint)
    {
        SnapBuildingToGrid(clickedPoint);
        CheckIfPlaningPossible();
    }

    public bool PlaceBuilding()
    {
        if (CheckIfPlaningPossible())
        {
            PlayerManager.Instance.RemoveRessources(currentBuilding.buildingData.cost);
            currentBuilding.StartConstruction();
            currentBuilding = Instantiate(currentBuildingPrefab).GetComponent<Building>();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIfPlaningPossible()
    {
        bool isPossible = true;

        //1 first check if we have the ressources
        if (!PlayerManager.Instance.HasRessources(currentBuilding.buildingData.cost))
        {
            isPossible = false;
        }
        
        
        // 3 . check if there is room
        if ((currentBuilding.buildingPlacementTrigger.Collides()))
        {
            isPossible = false;
        }

        if (isPossible)
        {
            currentBuilding.SetPlaningPossible();
            return true;
        }
        else
        {
            currentBuilding.SetPlaningImpossible();
            return false;
        }
    }

    public void SnapBuildingToGrid(Vector3 clickedPoint)
    {
        currentBuilding.transform.position = new Vector3(Mathf.Round(clickedPoint.x / gridCellSize)*gridCellSize, clickedPoint.y + currentBuilding.heightRiser, Mathf.Round(clickedPoint.z / gridCellSize) * gridCellSize);
        //currentBuilding.transform.position = clickedPoint;
    }
}

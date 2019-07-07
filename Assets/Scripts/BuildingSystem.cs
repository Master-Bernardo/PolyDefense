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

    ConstructingBuilding currentBuilding;
    GameObject currentBuildingPrefab;

    public Transform playersBaseLocation;
    BA_Base playerBase;

    public static BuildingSystem Instance;

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance);
        }
        else
        {
            Instance = this;
        }

    }

    public void AddBaseBuilding(BA_Base playerBase)
    {
        this.playersBaseLocation = playerBase.transform;
        this.playerBase = playerBase;
    }

    public void StartPlaning(GameObject buildingPrefab)
    {
        currentBuilding = Instantiate(buildingPrefab).GetComponent<ConstructingBuilding>();
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
        Vector3 snappedPoint = SnapBuildingToGrid(clickedPoint);
        CheckIfPlaningPossible(snappedPoint);
    }

    public bool PlaceBuilding(Vector3 clickedPoint)
    {
        Vector3 snappedPoint = SnapBuildingToGrid(clickedPoint);
        if (CheckIfPlaningPossible(snappedPoint))
        {
            PlayerManager.Instance.RemoveRessources(currentBuilding.buildingData.cost);
            currentBuilding.StartConstruction();
            currentBuilding = Instantiate(currentBuildingPrefab).GetComponent<ConstructingBuilding>();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIfPlaningPossible(Vector3 snappedPosition)
    {
        bool isPossible = true;

        //1 first check if we have the ressources
        if (!PlayerManager.Instance.HasRessources(currentBuilding.buildingData.cost))
        {
            isPossible = false;
        }

        //2. check if this building is not too far
        if (Vector3.Distance(playersBaseLocation.position, snappedPosition) > playerBase.buildingDistance)
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

    public Vector3 SnapBuildingToGrid(Vector3 clickedPoint)
    {
        Vector3 newPosition = new Vector3(Mathf.Round(clickedPoint.x / gridCellSize) * gridCellSize, clickedPoint.y + currentBuilding.heightRiser, Mathf.Round(clickedPoint.z / gridCellSize) * gridCellSize);
        currentBuilding.transform.position = newPosition;
        return newPosition;
    }
}

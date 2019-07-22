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

    BuildingPlacer currentBuildingPlacer;

    public Transform playersBaseLocation;
    EC_Base playerBase;

    HashSet<BuildingInConstruction> buildingsToConstruct = new HashSet<BuildingInConstruction>();

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

    #region Management of other buildings 
    public void SetBaseBuilding(EC_Base playerBase)
    {
        this.playersBaseLocation = playerBase.transform;
        this.playerBase = playerBase;
    }

    public void AddBuildingWaitingForConstruction(BuildingInConstruction building)
    {
        buildingsToConstruct.Add(building);
    }

    public void RemoveBuildingWaitingForConstruction(BuildingInConstruction building)
    {
        buildingsToConstruct.Remove(building);
    }

    public HashSet<BuildingInConstruction> GetBuildingsWaitingForConstruction()
    {
        return buildingsToConstruct;
    }

    public bool AreThereBuildingsToConstruct()
    {
        if(buildingsToConstruct.Count>0) return true;
        else return false;
    }

    #endregion


    #region Building placement

    public void StartPlaning(GameObject currentBuildingPlacerGO)
    {
        if (currentBuildingPlacer != null) Destroy(currentBuildingPlacer.gameObject);
        currentBuildingPlacer = Instantiate(currentBuildingPlacerGO).GetComponent<BuildingPlacer>();
    }
    
    public void StopPlaning()
    {
        if(currentBuildingPlacer!=null) Destroy(currentBuildingPlacer.gameObject);
    }

    //snaps the current building to the grid
    public void PlanBuilding(Vector3 clickedPoint)
    {
        Vector3 snappedPoint = SnapBuildingToGrid(clickedPoint);
        CheckIfPlacingPossible(snappedPoint);
    }

    public bool CheckIfPlacingPossible(Vector3 snappedPosition)
    {
        bool isPossible = true;

        //1 first check if we have the ressources
        if (!PlayerManager.Instance.HasRessources(currentBuildingPlacer.buildingData.cost))
        {
            isPossible = false;
        }

        //2. check if this building is not too far
        if (Vector3.Distance(playersBaseLocation.position, snappedPosition) > playerBase.buildingDistance)
        {
            isPossible = false;
        }

        // 3 . check if there is room
        if ((currentBuildingPlacer.buildingPlacementTrigger.Collides()))
        {
            isPossible = false;
        }

        if (isPossible)
        {
            currentBuildingPlacer.SetPlaningPossible();
            return true;
        }
        else
        {
            currentBuildingPlacer.SetPlaningImpossible();
            return false;
        }
    }

    public bool PlaceBuilding(Vector3 clickedPoint)
    {
        Vector3 snappedPoint = SnapBuildingToGrid(clickedPoint);
        if (CheckIfPlacingPossible(snappedPoint))
        {
            PlayerManager.Instance.RemoveRessources(currentBuildingPlacer.buildingData.cost);
            currentBuildingPlacer.SpawnBuildingForConstruction();
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector3 SnapBuildingToGrid(Vector3 clickedPoint)
    {
        // Vector3 newPosition = new Vector3(Mathf.Round(clickedPoint.x / gridCellSize) * gridCellSize, clickedPoint.y + currentBuildingPlacer.heightRiser, Mathf.Round(clickedPoint.z / gridCellSize) * gridCellSize);
        Vector3 newPosition = new Vector3(Mathf.Round(clickedPoint.x / gridCellSize) * gridCellSize, clickedPoint.y , Mathf.Round(clickedPoint.z / gridCellSize) * gridCellSize);

        currentBuildingPlacer.transform.position = newPosition;
        return newPosition;
    }

   #endregion

}

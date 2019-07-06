using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerControllerState
{
    BuildingMode,
    Default
}


public class PlayerController : MonoBehaviour
{
    public PlayerControllerState state = PlayerControllerState.Default;
    public BuildingSystem buildingSystem;
    public CameraController cameraController;

    public GameObject testBuildingPrefab;

    public int groundLayer;


    // Update is called once per frame
    void Update()
    {
        cameraController.MoveCamera(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Mouse ScrollWheel"));

        if (Input.GetKeyDown(KeyCode.B))
        {
            if(state == PlayerControllerState.Default)
            {
                state = PlayerControllerState.BuildingMode;
                buildingSystem.StartPlaning(testBuildingPrefab);
            }
            else 
            {
                state = PlayerControllerState.Default;
                buildingSystem.StopPlaning();
            }
        }

        if(state == PlayerControllerState.BuildingMode)
        {
            int layerMask = 1 << groundLayer;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                buildingSystem.PlanBuilding(hit.point);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("click");
                buildingSystem.PlaceBuilding();

            }
        }
    }
}

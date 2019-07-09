using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerControllerState
{
    BuildingMode,
    EconomyManagement,
    Default
}


public class PlayerController : MonoBehaviour
{
    public PlayerControllerState state = PlayerControllerState.Default;
    public BuildingSystem buildingSystem;
    public CameraController cameraController;

    public GameObject testBuildingPrefab;

    public int groundLayer;
    public int buildingsLayer;
    int layerMask;
    Ray ray;
    RaycastHit hit;

    BubbleMenu currentOpenBubbleMenu;




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

        switch (state)
        {
            case PlayerControllerState.Default:

                if (Input.GetMouseButtonDown(0))
                {

                    if (!Utility.HitsUI(Input.mousePosition))
                    {
                        if (currentOpenBubbleMenu != null) currentOpenBubbleMenu.Hide();

                    }
                    layerMask = 1 << buildingsLayer;

                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                    {
                        //all buildings need to have buildingMenu attached
                        BubbleMenu bubbleMenu = hit.collider.GetComponent<BubbleMenu>();
                        if (bubbleMenu != null)
                        {
                            currentOpenBubbleMenu = bubbleMenu;
                            currentOpenBubbleMenu.Show();
                        }
                    }
                }
                break;

            case PlayerControllerState.BuildingMode:

                layerMask = 1 << groundLayer;

                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    buildingSystem.PlanBuilding(hit.point);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("click");
                    buildingSystem.PlaceBuilding(hit.point);
                }

                break;

            case PlayerControllerState.EconomyManagement:

                break;
        }
    }

    public void StartPlaningBuilding(BuildingData building)
    {
        state = PlayerControllerState.BuildingMode;
        buildingSystem.StartPlaning(building.placerPrefab);
        if (currentOpenBubbleMenu != null) currentOpenBubbleMenu.Hide();


    }

    public void StopPlaningBuilding()
    {
        state = PlayerControllerState.Default;
        buildingSystem.StopPlaning();

    }

    public void EnterEconomyMenagementMode()
    {
        state = PlayerControllerState.EconomyManagement;
        if (currentOpenBubbleMenu != null) currentOpenBubbleMenu.Hide();

    }

    public void ExitEconomyMenagementMode()
    {
        state = PlayerControllerState.Default;

    }
}

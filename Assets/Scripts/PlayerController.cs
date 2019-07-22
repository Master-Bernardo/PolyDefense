using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerControllerState
{
    BuildingMode,
    //EconomyManagement,
    Default
}


public class PlayerController : MonoBehaviour
{
    public PlayerControllerState state;
    public BuildingSystem buildingSystem;
    public CameraController cameraController;
    public UIManager uIManager;

    public GameObject testBuildingPrefab;
    public bool economyActive = false;

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


        #region input
  
        if (Input.GetKeyDown(KeyCode.B))
        {
            uIManager.ToogleBuildingPanel();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if(economyActive)
            {
                ExitEconomyMenagementMode();
            }
            else
            {
                EnterEconomyMenagementMode();
            }
        }



        #endregion

        #region Handle states in update

        switch (state)
        {
            case PlayerControllerState.Default:
                


                if (Input.GetMouseButtonDown(0))
                {
                    if (!economyActive)
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
                    if (!Utility.HitsUI(Input.mousePosition))
                    {
                        Debug.Log("click");
                        buildingSystem.PlaceBuilding(hit.point);
                    }
                       
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    state = PlayerControllerState.Default;
                    buildingSystem.StopPlaning();
                }

                break;


                #endregion
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
        uIManager.ActivateEconomyView();
        economyActive = true;
        if (currentOpenBubbleMenu != null) currentOpenBubbleMenu.Hide();
    }

    public void ExitEconomyMenagementMode()
    {
        economyActive = false;
        uIManager.DeactivateEconomyView();
    }
}

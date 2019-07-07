using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * gets attached to the mouse and snaps to posiions - places a building
 */
public class BuildingPlacer : MonoBehaviour
{
    public BuildingData buildingData;

    public GameObject planingModel;
    public MeshRenderer planingModelRenderer;
   

    public BuildingPlacementTrigger buildingPlacementTrigger;

    public GameObject buildingToSpawn;

    //[Tooltip("the building gets raised by this value above ground")]
   // public float heightRiser;



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

    public void SpawnBuildingForConstruction()
    {
        Instantiate(buildingToSpawn, transform.position, transform.rotation);
    }

}

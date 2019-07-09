using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlacerButton : MonoBehaviour
{
    public BuildingData buildingData;
    public Image image;
    

    private void Awake()
    {
        image.sprite = buildingData.menuImage;
    }
}

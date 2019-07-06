using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "New Building", menuName = "BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;

    public RessourceValuePair[] cost;

    
}

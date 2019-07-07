using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "New Building", menuName = "BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public int constructionPoints; // how long is the buildingProcess?
    public int healthPoints;

    public RessourceValuePair[] cost;



    
}

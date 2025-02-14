﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "New Building", menuName = "BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public int constructionPoints; // how long is the buildingProcess?
    public int healthPoints;
    public string description;

    public RessourceValuePair[] cost;


    [Tooltip("the image which gets shown in the UI")]
    public Sprite menuImage;
    public GameObject placerPrefab;



    
}

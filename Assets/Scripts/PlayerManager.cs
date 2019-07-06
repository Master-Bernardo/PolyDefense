using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * keeps track of player Data like ressources etc throughout the game
 */

public enum RessourceType
{
    fer, //plenty -something like iron
    mer, // not so plenty, more shiny like cristals or something
    rubith // very rare - only adventurers can find them
}

[System.Serializable]
public class RessourceValuePair
{
    public RessourceType type;
    public int value;
}

[System.Serializable]
public class ManagersRessource
{
    public RessourceType type;
    public int value;
    public int capacity;
}

public class PlayerManager : MonoBehaviour
{
    public int playerID; //if later on we would want to add another

    public ManagersRessource[] ressources;
    

    public static PlayerManager Instance;

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

        UpdateUIRessources();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddRessource(RessourceType.fer, 15);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (HasRessource(RessourceType.fer, 5)) RemoveRessource(RessourceType.fer, 5);
        }
    }

    public void AddRessource(RessourceType type, int value)
    {
        ManagersRessource ressource = GetRessource(type);
        ressource.value += value;
        if (ressource.value > ressource.capacity) ressource.value = ressource.capacity;

        UpdateUIRessources();
    }

    public void RemoveRessource(RessourceType type, int value)
    {
        GetRessource(type).value -= value;

        UpdateUIRessources();
    }

    public bool HasRessource(RessourceType type, int value)
    {
        return GetRessource(type).value >= value;
    }

    public bool HasRessources(RessourceValuePair[] cost)
    {
        bool hasRessources = true;

        foreach(RessourceValuePair ressourceNeeded in cost)
        {
            if (GetRessource(ressourceNeeded.type).value < ressourceNeeded.value) hasRessources = false;
        }

        return hasRessources;
    }

    public void RemoveRessources(RessourceValuePair[] ressourcesToRemove)
    {
        foreach (RessourceValuePair ressource in ressourcesToRemove)
        {
            GetRessource(ressource.type).value -= ressource.value;
        }

        UpdateUIRessources();
    }

    ManagersRessource GetRessource(RessourceType type)
    {
        foreach (ManagersRessource ressource in ressources)
        {
            if (ressource.type == type)
            {
                return ressource;
            }
        }
        return null;
    }

    void UpdateUIRessources()
    {
        UIManager.Instance.UpdateRessourcesUI(GetRessource(RessourceType.fer).value, GetRessource(RessourceType.mer).value, GetRessource(RessourceType.rubith).value);
    }
}

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

/*
 * keeps track of players ressources and other things - which things? 
 */
public class PlayerManager : MonoBehaviour
{
    public int playerID; //if later on we would want to add another

    public ManagersRessource[] ressources;

    public int populationLimit;
    public int currentPopulation;

    HashSet<Ab_Worker> idleWorkers = new HashSet<Ab_Worker>();
    

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

    }

    private void Start()
    {
        UpdateUIRessources();
        UpdateUIPopulation();

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

    //checks if we can spawn this unit with the specified populationValue - some bigger units consume more than 1 population
    public bool SpawnPossible(int populationValue)
    {
        if(currentPopulation + populationValue <= populationLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddIdleWorker(Ab_Worker worker)
    {
        idleWorkers.Add(worker);
    }

    public void RemoveIdleWorker(Ab_Worker worker)
    {
        idleWorkers.Remove(worker);
    }

    public int GetIdleWorkersNumber()
    {
        return idleWorkers.Count;
    }

    public Ab_Worker GetNearestIdleWorker(Vector3 position)
    {
        Ab_Worker nearestWorker = null;
        float nearestDistance = Mathf.Infinity;

        foreach(Ab_Worker worker in idleWorkers)
        {
            float currentDistance = (worker.transform.position - position).sqrMagnitude;

            if (currentDistance < nearestDistance)
            {
                nearestDistance = currentDistance;
                nearestWorker = worker;
            }
        }

        return nearestWorker;
    }

    public void RaisePopulation(int population)
    {
        currentPopulation += population;
        UpdateUIPopulation();    }

    public void LowerPopulation(int population)
    {
        currentPopulation -= population;
        UpdateUIPopulation();    }

    public void RaisePopulationLimit(int population)
    {
        populationLimit += population;
        UpdateUIPopulation();
    }

    public void LowerPopulationLimit(int population)
    {
        populationLimit -= population;
        UpdateUIPopulation();
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

    void UpdateUIPopulation()
    {
        UIManager.Instance.UpdatePopulationUI(currentPopulation, populationLimit);

    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ab_Movement))]
public class Ab_Worker : Ability
{
    enum WorkerState
    {
        Idle,
        Construction,
        Harvesting
    }

    enum ConstructionState
    {
        Idle,
        MovingToBuilding,
        Constructing
    }

    enum HarvestingState
    {
        Idle,
        MovingToRessource,
        GatheringRessource,
        TransportingRessourceToHarvester
    }

    [SerializeField]
    WorkerState state;
    [SerializeField]
    ConstructionState constructionState;
    [SerializeField]
    HarvestingState harvestingState;

    Ab_Base assignedBase;
    Ab_HarvestingBuilding assignedHarvester;
    RessourceType currentAssignedHarvesterType;
    

    BuildingInConstruction currentlyAssignedBuildingToBuild;

    Ab_Movement movement;

    //some idle movement
    public float idleMovementInterval;
    float nextIdleMovementTime;
    public float maxDistanceToBaseForWander;

    //optimising of construction and harvesterCHeck
    public float scanInterval;
    float nextScanTime;

    //solveTHis with scriptableObject later
    //for contruction
    public int constructionPoints;
    public float constructionInterval;
    float nextConstructionTime;
    bool standingBesideBaseAndWaiting = false; //if thers nothing todo thy do this

    //for ressourceGathering
    [Tooltip("how much ressources do we gather on one hit")]
    public int ressourceGatheringPower;
    public int ressourceTransportLoad;
    int currentRessourceTransportLoad = 0;
    public float ressourceGatherInterval;
    float nextRessourceGatheringTime;
    bool standingBesideHarvesterAndWaiting = false;
    Ressource currentSelectedRessource;


    public override void SetUpAbility(GameEntity entity)
    {
        base.SetUpAbility(entity);
        movement = GetComponent<Ab_Movement>();
        nextScanTime = Time.time + Random.Range(0, scanInterval);
        //AssignToIdle();
    }

    public override void UpdateAbility()
    {
        switch(state)
        {
            case WorkerState.Idle:

                if (Time.time > nextIdleMovementTime)
                {
                    nextIdleMovementTime = Time.time + idleMovementInterval;
                    Vector3 wanderPoint = Random.insideUnitSphere * 4;
                    wanderPoint.y = transform.position.y;

                    wanderPoint += transform.forward * 4 + transform.position;

                    Vector3 basePosition = BuildingSystem.Instance.playersBaseLocation.position;
                    //if he would stray off to far, bring him back to base
                    if (Vector3.Distance(basePosition, wanderPoint) > maxDistanceToBaseForWander)
                    {
                        wanderPoint += (basePosition - wanderPoint) / 4;
                    }

                    movement.MoveTo(wanderPoint);
                }

                break;

            case WorkerState.Construction:

                switch (constructionState)
                {
                    case ConstructionState.Idle:

                        //if idle /first get nearest Building  which needs construction, if there is no we just chill beside the base
                        if (Time.time > nextScanTime)
                        {
                            nextScanTime = Time.time + scanInterval;

                            if (BuildingSystem.Instance.AreThereBuildingsToConstruct())
                            {
                                standingBesideBaseAndWaiting = false;

                                BuildingInConstruction nearestBuildingToConstruct = null;
                                float nearestDistance = Mathf.Infinity;

                                foreach (BuildingInConstruction building in BuildingSystem.Instance.GetBuildingsWaitingForConstruction())
                                {
                                    float currentDistance = (building.transform.position - transform.position).sqrMagnitude;

                                    if (currentDistance < nearestDistance)
                                    {
                                        nearestDistance = currentDistance;
                                        nearestBuildingToConstruct = building;
                                    }
                                }

                                currentlyAssignedBuildingToBuild = nearestBuildingToConstruct;
                                movement.MoveTo(nearestBuildingToConstruct.transform.position);
                                constructionState = ConstructionState.MovingToBuilding;
                            }
                            else if(!standingBesideBaseAndWaiting)
                            {
                                Vector3 positonToMoveTo =  Random.insideUnitSphere*8;
                                positonToMoveTo += BuildingSystem.Instance.playersBaseLocation.position;
                                standingBesideBaseAndWaiting = true;
                                movement.MoveTo(positonToMoveTo);
                            }

                        }

                        break;

                    case ConstructionState.MovingToBuilding:

                        if (Time.time > nextScanTime)
                        {

                            nextScanTime = Time.time + scanInterval;

                            if (currentlyAssignedBuildingToBuild != null)
                            {

                                if (Vector3.Distance(transform.position, currentlyAssignedBuildingToBuild.transform.position) < currentlyAssignedBuildingToBuild.width)
                                {
                                    movement.Stop();
                                    constructionState = ConstructionState.Constructing;
                                    nextConstructionTime = Time.time + constructionInterval;
                                }
                            }
                            else
                            {
                                constructionState = ConstructionState.Idle;
                            }
                        }

                            break;

                    case ConstructionState.Constructing:

                        if(Time.time> nextConstructionTime)
                        {
                            //check if it istn completed yet
                            if (currentlyAssignedBuildingToBuild != null)
                            {
                                nextConstructionTime = Time.time + constructionInterval;
                                currentlyAssignedBuildingToBuild.Construct(constructionPoints);
                            }
                            else
                            {
                                constructionState = ConstructionState.Idle;
                            }
                           
                        }

                        break;
                }

               
               


                break;

            case WorkerState.Harvesting:

                switch (harvestingState)
                {
                    case HarvestingState.Idle:

                        //check if there are some ressources in the area - should it check with physics check or get the nearest from the ressourcesmanager?
                        if (Time.time > nextScanTime)
                        {
                            nextScanTime = Time.time + scanInterval;

                            HashSet<Ressource> ressources = RessourcesManager.Instance.GetRessources(currentAssignedHarvesterType);

                            if (ressources.Count > 0)
                            {
                                standingBesideHarvesterAndWaiting = false;


                                Ressource nearestRessource = null;
                                float nearestDistance = Mathf.Infinity;

                                foreach (Ressource ressource in ressources)
                                {
                                    float currentDistance = (ressource.transform.position - transform.position).sqrMagnitude;

                                    if (currentDistance < nearestDistance)
                                    {
                                        nearestDistance = currentDistance;
                                        nearestRessource = ressource;
                                    }
                                }

                                currentSelectedRessource = nearestRessource;
                                movement.MoveTo(currentSelectedRessource.transform.position);
                                harvestingState = HarvestingState.MovingToRessource;
                            }
                            else if(!standingBesideHarvesterAndWaiting)
                            {
                                Vector3 positonToMoveTo = Random.insideUnitSphere * 5;
                                positonToMoveTo += assignedHarvester.transform.position;
                                standingBesideHarvesterAndWaiting = true;
                                movement.MoveTo(positonToMoveTo);
                            }
                        }

                            break;

                    case HarvestingState.MovingToRessource:

                        if (Time.time > nextScanTime)
                        {
                            nextScanTime = Time.time + scanInterval;


                            if (currentSelectedRessource != null)
                            {

                                if (Vector3.Distance(transform.position, currentSelectedRessource.transform.position) < currentSelectedRessource.width)
                                {
                                    movement.Stop();
                                    harvestingState = HarvestingState.GatheringRessource;
                                    nextRessourceGatheringTime = Time.time + ressourceGatherInterval;
                                }
                            }
                            else
                            {
                                harvestingState = HarvestingState.Idle;
                            }
                        }

                        break;

                    case HarvestingState.GatheringRessource:

                        if (Time.time > nextRessourceGatheringTime)
                        {                         
                            //check if it istn completed yet
                            if (currentSelectedRessource != null)
                            {
                                nextRessourceGatheringTime = Time.time + ressourceGatherInterval;
                                //gather but check how much will fit
                                if(currentRessourceTransportLoad + ressourceGatheringPower > ressourceTransportLoad)
                                {
                                    currentRessourceTransportLoad += currentSelectedRessource.TakeRessource(ressourceTransportLoad-currentRessourceTransportLoad);
                                }
                                else
                                {
                                    currentRessourceTransportLoad += currentSelectedRessource.TakeRessource(ressourceGatheringPower);
                                }

                                //if the sack is full, go back
                                if (currentRessourceTransportLoad == ressourceTransportLoad)
                                {
                                    movement.MoveTo(assignedHarvester.transform.position);
                                    harvestingState = HarvestingState.TransportingRessourceToHarvester;
                                }
                            }
                            else
                            {
                                if (currentRessourceTransportLoad > 0)
                                {
                                    movement.MoveTo(assignedHarvester.transform.position);
                                    harvestingState = HarvestingState.TransportingRessourceToHarvester;
                                }
                                else
                                {
                                    harvestingState = HarvestingState.Idle;
                                }
                            }

                        }

                        break;

                    case HarvestingState.TransportingRessourceToHarvester:

                        if (Time.time > nextScanTime)
                        {
                            nextScanTime = Time.time + scanInterval;

                            if (Vector3.Distance(transform.position, assignedHarvester.transform.position) < assignedHarvester.width)
                            {
                                movement.Stop();
                                assignedHarvester.DepotRessource(currentRessourceTransportLoad);
                                currentRessourceTransportLoad = 0;

                                if (currentSelectedRessource != null)
                                {
                                    harvestingState = HarvestingState.MovingToRessource;
                                    movement.MoveTo(currentSelectedRessource.transform.position);
                                }
                                else
                                {
                                    harvestingState = HarvestingState.Idle;
                                }
                                

                            }
                        }
                        break;
                }

                break;
        }
            
        
    }

    public void AssignToConstruction(Ab_Base assignedBase)
    {
        if(state == WorkerState.Idle) PlayerManager.Instance.RemoveIdleWorker(this);
        this.assignedBase = assignedBase;
        state = WorkerState.Construction;
        nextScanTime = Time.time;
    }

    public void AssignToHarvesting(Ab_HarvestingBuilding harvester)
    {
        if (state == WorkerState.Idle) PlayerManager.Instance.RemoveIdleWorker(this);
        this.assignedHarvester = harvester;
        currentAssignedHarvesterType = harvester.type;
        state = WorkerState.Harvesting;
        nextScanTime = Time.time;

    }

    public void AssignToIdle()
    {
        state = WorkerState.Idle;
        PlayerManager.Instance.AddIdleWorker(this);
        nextIdleMovementTime = Random.Range(0, idleMovementInterval);
    }
}

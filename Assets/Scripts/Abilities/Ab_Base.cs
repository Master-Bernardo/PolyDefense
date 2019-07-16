using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the base ability
public class Ab_Base : Ability, IWorkerAssigneable<int>
{
    [Tooltip("at which distancde to the base can buildings be constructed?")]
    public float buildingDistance;

    List<Ab_Worker> assignedWorkers = new List<Ab_Worker>();

    int currentWorkerNumber = 0;
    public int workerCapacity;


    public override void SetUpAbility(GameEntity entity)
    {
        BuildingSystem.Instance.SetBaseBuilding(this);
    }

    //make aseperate abilty out of this worker assigners?
    public int SetWorkerNumber(int newNumber)
    {
        //add more workers
        if (newNumber > currentWorkerNumber)
        {
            //first cap the number, 
            if (PlayerManager.Instance.GetIdleWorkersNumber() < newNumber - currentWorkerNumber)
            {
                newNumber = currentWorkerNumber + PlayerManager.Instance.GetIdleWorkersNumber();
            }

            for (int i = currentWorkerNumber; i < newNumber; i++)
            {
                Ab_Worker newWorker = PlayerManager.Instance.GetNearestIdleWorker(transform.position);
                newWorker.AssignToConstruction(this);
                assignedWorkers.Add(newWorker);
            }
        }
        else
        {//release them from work
            for (int i = currentWorkerNumber - 1; i >= newNumber; i--)
            {
                assignedWorkers[i].AssignToIdle();
                assignedWorkers.RemoveAt(i);
            }

        }

        currentWorkerNumber = newNumber;

        return newNumber;

    }
}




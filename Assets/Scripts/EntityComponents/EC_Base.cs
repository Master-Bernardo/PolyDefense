using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the base ability
public class EC_Base : EntityComponent, IWorkerAssigneable<int>
{
    [Tooltip("at which distancde to the base can buildings be constructed?")]
    public float buildingDistance;

    List<B_Worker> assignedWorkers = new List<B_Worker>();

    int currentWorkerNumber = 0;
    public int workerCapacity;

    public WorkerAssignerUI assignerUI;


    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
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
                B_Worker newWorker = PlayerManager.Instance.GetNearestIdleWorker(transform.position);
                newWorker.AssignToConstruction(this);
                assignedWorkers.Add(newWorker);
            }
        }
        else
        {//release them from work
            for (int i = currentWorkerNumber - 1; i >= newNumber; i--)
            {
                if(assignedWorkers[i] != null)assignedWorkers[i].AssignToIdle();
                assignedWorkers.RemoveAt(i);
            }

        }

        currentWorkerNumber = newNumber;

        assignerUI.UpdateUI(newNumber);
        return newNumber;

    }




    public void OnWorkerDies(B_Worker worker)
    {
        Debug.Log("orker died");
        int newWorkers = currentWorkerNumber - 1;
        SetWorkerNumber(newWorkers);
        assignedWorkers.Remove(worker);
    }
}




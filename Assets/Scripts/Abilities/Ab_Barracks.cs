using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDepositioneable<T>
{
    void DepositionWorker(T worker);

    void OnWorkerGetsTaksAssigned(T worker);
}

public class RecruitmentQueueElement
{
    public UnitData unit;
    public int number;

    public RecruitmentQueueElement(UnitData unit, int number)
    {
        this.unit = unit;
        this.number = number;
    }
}

//like spawner, but it only spawns if we tell it to spawn
public class Ab_Barracks : Ability, IDepositioneable<Ab_Worker>
{
    public UnitData[] units;

    public Building building;
    public float recruitmentSpeed;
    public UnitData currentRecruitedUnit;
    public float currentUnitRecruitmentEndTime;
    public List<Ab_Worker> peopleInBarracks = new List<Ab_Worker>(); //we can only train the ones which are in the barracks
    public HashSet<Ab_Worker> peopleOnTheirWayToBarracks = new HashSet<Ab_Worker>();
    public int currentlyNeededPeople;

    Queue<RecruitmentQueueElement> unitsQueue = new Queue<RecruitmentQueueElement>();

    public void AddUnitToRecruitmentQueue(int id, int number)
    {
        UnitData unit = units[id];

        RecruitmentQueueElement elementContainingThisUnit = null;

        foreach (RecruitmentQueueElement element in unitsQueue)
        {
            if(element.unit == unit)
            {
                elementContainingThisUnit = element;
            }
        }

        if (elementContainingThisUnit != null)
        {
            elementContainingThisUnit.number += number;
        }
        else
        {
            unitsQueue.Enqueue(new RecruitmentQueueElement(unit, number));
        }

        currentlyNeededPeople += number * unit.populationValue;


        for (int i = 0; i < number * unit.populationValue; i++)
        {
            if (PlayerManager.Instance.GetIdleWorkersNumber()>0)
            {
                PlayerManager.Instance.GetNearestIdleWorker(transform.position).AssignToDeposition(building);
            }
        }
        
    }

    public override void UpdateAbility()
    {
        //how to take care of this reruitment queue

        //check if we need to get more people into the building
        if (currentlyNeededPeople > peopleInBarracks.Count + peopleOnTheirWayToBarracks.Count)
        {
            int difference = currentlyNeededPeople - (peopleInBarracks.Count + peopleOnTheirWayToBarracks.Count);
            for (int i = 0; i < difference; i++)
            {
                if (PlayerManager.Instance.GetIdleWorkersNumber() > 0)
                {
                    PlayerManager.Instance.GetNearestIdleWorker(transform.position).AssignToDeposition(building);
                }
            }
        }

        //if we currently rectuit a unit - check if the recruitment is finished
        if (currentRecruitedUnit != null)
        {
            //check if the time is over 
            if (Time.time > currentUnitRecruitmentEndTime)
            {
                FinishRecruitment();
            }
        }
        else if (unitsQueue.Count > 0)
        {
            //only start recruitment if we have enough people in barracks
            if(peopleInBarracks.Count >= unitsQueue.Peek().unit.populationValue)
            {
                StartRecruitment();
            }
        }



        if (Input.GetKeyDown(KeyCode.X))
        {
            AddUnitToRecruitmentQueue(0, 3);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AddUnitToRecruitmentQueue(1, 2);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            unitsQueue.Dequeue();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("queue peek: " + unitsQueue.Peek().unit + " " + unitsQueue.Peek().number);
        }
    }

    void StartRecruitment()
    {
        currentRecruitedUnit = unitsQueue.Peek().unit;
        Debug.Log("started recruiting: " + "name: " + currentRecruitedUnit.unitName);

        currentUnitRecruitmentEndTime = Time.time + currentRecruitedUnit.recruitingPoints * recruitmentSpeed;
    }

    void FinishRecruitment()
    {
        Debug.Log("finishedRecruitment: " + "name: " + currentRecruitedUnit.unitName);
        if (unitsQueue.Peek().number == 0) unitsQueue.Dequeue();
        currentlyNeededPeople -= currentRecruitedUnit.populationValue;
        for (int i = 0; i < currentRecruitedUnit.populationValue; i++)
        {           
            Destroy(peopleInBarracks[0]);
            peopleInBarracks.RemoveAt(0);
        }
        currentRecruitedUnit = null;

    }

    public void DepositionWorker(Ab_Worker worker)
    {
        worker.gameObject.SetActive(false);

        peopleInBarracks.Add(worker);
        peopleOnTheirWayToBarracks.Remove(worker);
    }

    public void OnWorkerGetsTaksAssigned(Ab_Worker worker)
    {
        peopleOnTheirWayToBarracks.Add(worker);
    }
}



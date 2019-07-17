using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDepositioneable<T>
{
    void DepositionWorker(T worker);

    void OnWorkerGetsTaksAssigned(T worker);
}

public interface IQueueHolder<T, F>
{
    void AddElementToQueue(T element, F number);
    void RemoveElementFromQueue(T element, F number);
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
public class Ab_Barracks : Ability, IDepositioneable<Ab_Worker>, IQueueHolder<int, int>
{
    public UnitData[] units;
    [Tooltip("needs to be the same length as units")]
    public BubbleMenuButtonStackable[] unitsUIButtons; 

    public Building building;
    public float recruitmentSpeed;
    public UnitData currentRecruitedUnit;
    public float currentUnitRecruitmentEndTime;
    public List<Ab_Worker> peopleInBarracks = new List<Ab_Worker>(); //we can only train the ones which are in the barracks
    public HashSet<Ab_Worker> peopleOnTheirWayToBarracks = new HashSet<Ab_Worker>();
    public int currentlyNeededPeople;

    public Transform spawnPoint;


    Queue<RecruitmentQueueElement> unitsQueue = new Queue<RecruitmentQueueElement>();

    //if wr want to remove, we just use negative numbas?
   public void AddElementToQueue(int unitID, int number)
    {
        UnitData unit = units[unitID];

        RecruitmentQueueElement elementContainingThisUnit = null;

        foreach (RecruitmentQueueElement element in unitsQueue)
        {
            if (element.unit == unit)
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
            if (PlayerManager.Instance.GetIdleWorkersNumber() > 0)
            {
                PlayerManager.Instance.GetNearestIdleWorker(transform.position).AssignToDeposition(building);
            }
        }
    }

    public void RemoveElementFromQueue(int unitID, int number)
    {
        UnitData unit = units[unitID];

        RecruitmentQueueElement elementContainingThisUnit = null;

        foreach (RecruitmentQueueElement element in unitsQueue)
        {
            if (element.unit == unit)
            {
                elementContainingThisUnit = element;
            }
        }
        elementContainingThisUnit.number -= number;

        if(elementContainingThisUnit.number <= 0)
        {
            if(currentRecruitedUnit == elementContainingThisUnit.unit)
            {
                StopRecruitment();
                unitsQueue.Dequeue();
            }
        }

        currentlyNeededPeople -= number;

        //we need to fire the workers - lets check if it enough to fire the ones which are currently on their way

        int workersToFireLeft = number;

        HashSet<Ab_Worker> workersToRemove = new HashSet<Ab_Worker>();

        foreach(Ab_Worker worker in peopleOnTheirWayToBarracks)
        {
            if(workersToFireLeft > 0)
            {
                worker.AssignToIdle();
                workersToFireLeft--;
                workersToRemove.Add(worker);
            }
            else
            {
                break;
            }
        }

        foreach (Ab_Worker worker in workersToRemove)
        {
            peopleOnTheirWayToBarracks.Remove(worker);
        }



        //if this is not enough, we need to release some workers which are already in the building
        //backward for for remove
        for (int i = workersToFireLeft-1; i >= 0; i--)
        {
            peopleInBarracks[i].AssignToIdle();
            peopleInBarracks[i].gameObject.SetActive(true);

            peopleInBarracks.RemoveAt(i);
        }


       
    }


    public override void SetUpAbility(GameEntity entity)
    {
        for (int i = 0; i < unitsUIButtons.Length; i++)
        {
            unitsUIButtons[i].SetUpUI(units[i]);
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
           // AddUnitToRecruitmentQueue(0, 3);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
           // AddUnitToRecruitmentQueue(1, 2);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            unitsQueue.Dequeue();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("queue peek: " + unitsQueue.Peek().unit + " " + unitsQueue.Peek().number);
        }


        //update ui buttons

        Dictionary<UnitData, int> conversionDict = new Dictionary<UnitData, int>();

        for (int i = 0; i < units.Length; i++)
        {
            conversionDict.Add(units[i], 0);
        }

        foreach(RecruitmentQueueElement element in unitsQueue)
        {
            conversionDict[element.unit] += element.number;
        }
       // Debug.Log("-----------");
        int index = 0;//workaround
        foreach(KeyValuePair<UnitData,int> pair in conversionDict)
        {
            //Debug.Log("unit: " + pair.Key.unitName + " number: " + pair.Value);
            unitsUIButtons[index].UpdateUI(pair.Value);
            index++;
        }
       // Debug.Log("-----------");

    }

    void StartRecruitment()
    {
        currentRecruitedUnit = unitsQueue.Peek().unit;
        Debug.Log("started recruiting: " + "name: " + currentRecruitedUnit.unitName);

        currentUnitRecruitmentEndTime = Time.time + currentRecruitedUnit.recruitingPoints * recruitmentSpeed;
    }

    void StopRecruitment()
    {
        currentRecruitedUnit = null;
    }

    void FinishRecruitment()
    {
        PlayerManager.Instance.RemoveRessources(currentRecruitedUnit.cost);

        Debug.Log("finishedRecruitment: " + "name: " + currentRecruitedUnit.unitName);

        unitsQueue.Peek().number--;
        
        if (unitsQueue.Peek().number == 0) unitsQueue.Dequeue();

        currentlyNeededPeople -= currentRecruitedUnit.populationValue;
        for (int i = 0; i < currentRecruitedUnit.populationValue; i++)
        {
            peopleInBarracks[0].GetComponent<GameEntity>().Die();
            peopleInBarracks.RemoveAt(0);
        }

        GameObject spawnedUnit = Instantiate(currentRecruitedUnit.prefab, spawnPoint.position, spawnPoint.rotation);
        Ab_FriendlyMeleeFighter fighter = spawnedUnit.GetComponent<Ab_FriendlyMeleeFighter>();
        if (fighter != null) fighter.SetSpawnedBuilding(transform);

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



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Behaviour 
{
    public float behaviourUpdateInterval;
    float nextBehaviourUpdateTime;
    [HideInInspector]
    //every behaviour needs some components that the unitAI needs to have saved
    public GameEntity entity;

    /*public Behaviour(Ab_UnitAI unitAI)
    {
        this.unitAI = unitAI;
    }*/

    public virtual void SetUpBehaviour(GameEntity entity)
    {
        this.entity = entity;
    }

    public virtual void UpdateBehaviour()
    {
        if(Time.time > nextBehaviourUpdateTime)
        {
            nextBehaviourUpdateTime = Time.time + behaviourUpdateInterval;
            Update();
        }
    }

    protected virtual void Update()
    {

    }

    public virtual void OnBehaviourEnter()
    {

    }

    public virtual void OnBehaviourExit()
    {

    }

    public virtual void OnDie()
    {

    }
}

[System.Serializable]
public class B_WanderAroundPosition: Behaviour
{
    public float maxDistanceToPositionWhileWandering;
    EC_Movement movement;
    public Transform positionToWanderAround;

    public void SetUpBehaviour(GameEntity entity, EC_Movement movement)
    {
        this.entity = entity;
        this.movement = movement;
        if (positionToWanderAround == null)
        {
            positionToWanderAround = entity.transform;
        }
    }

    public void SetPositionToWanderAround(Transform newPositionToWanderAround)
    {
        positionToWanderAround = newPositionToWanderAround;
        Debug.Log("Sette 3");
        Debug.Log("pos: " + newPositionToWanderAround);
    }

    protected override void Update()
    {

            Vector3 wanderPoint = UnityEngine.Random.insideUnitSphere * 4;
            wanderPoint.y = entity.transform.position.y;

            wanderPoint += entity.transform.forward * 4 + entity.transform.position;

            //if he would stray off to far, bring him back to base
            if (Vector3.Distance(positionToWanderAround.position, wanderPoint) > maxDistanceToPositionWhileWandering)
            {
                wanderPoint += (positionToWanderAround.position - wanderPoint) / 4;
            }

            movement.MoveTo(wanderPoint);
        
    }
}

[System.Serializable]
public class B_Flee: Behaviour
{
    EC_ScanForEnemyUnits sensing;
    EC_Movement movement;

    public void SetUpBehaviour(GameEntity entity, EC_Movement movement, EC_ScanForEnemyUnits sensing)
    {
        this.entity = entity;
        this.movement = movement;
        this.sensing = sensing;
    }

    protected override void Update()
    {
        if (sensing.nearestEnemy != null)
        {
            movement.MoveTo((entity.transform.position - sensing.nearestEnemy.transform.position).normalized*5);
        }
        else
        {
            movement.Stop();
        }
    }


}

[System.Serializable]
public class B_MeleeFighter : Behaviour
{
    EC_ScanForEnemyUnits enemySensing;
    EC_Movement movement;

    public float distanceCheckingInterval;
    float nextDistanceCheckTime;

    //meleefighting
    EC_MeleeWeapon weapon;

    public void SetUpBehaviour(GameEntity entity, EC_Movement movement, EC_ScanForEnemyUnits enemySensing, EC_MeleeWeapon weapon)
    {
        this.entity = entity;
        this.movement = movement;
        this.enemySensing = enemySensing;
        this.weapon = weapon;

        nextDistanceCheckTime = UnityEngine.Random.Range(0, distanceCheckingInterval);
    }

    enum MeleeFighterState
    {
        MovingToEnemy,
        InMeleeDistance
    }

    MeleeFighterState state;

    protected override void Update()
    {
        switch (state)
        {
            case MeleeFighterState.MovingToEnemy:

                if (Time.time > nextDistanceCheckTime)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;

                    if ((enemySensing.nearestEnemy.transform.position - entity.transform.position).sqrMagnitude < weapon.meleeRange)
                    {
                        state = MeleeFighterState.InMeleeDistance;
                        movement.Stop();

                        if (weapon.CanAttack())
                        {
                            weapon.Attack(enemySensing.nearestEnemy.GetComponent<IDamageable<float>>());
                        }
                    }
                    else
                    {
                        movement.MoveTo(enemySensing.nearestEnemy.transform.position);
                    }
                }

                break;

            case MeleeFighterState.InMeleeDistance:


                if (Time.time > nextDistanceCheckTime)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;
                    if ((enemySensing.nearestEnemy.transform.position - entity.transform.position).sqrMagnitude > weapon.meleeRange)
                    {
                        state = MeleeFighterState.MovingToEnemy;
                    }
                }
                
                if (weapon.CanAttack())
                {
                    weapon.Attack(enemySensing.nearestEnemy.GetComponent<IDamageable<float>>());
                }                   
              

                break;
        }

       
    }
}

[System.Serializable]
public class B_MissileFighter : Behaviour
{
    EC_Movement movement;
    EC_ScanForEnemyUnits sensing;
    EC_MissileWeapon weapon;
    //goes to nearest enemy, shoots  and looks at them when in range, tries not to get too close

    public float maximalShootingDistance;
    float maximalShootingDistanceSquared;
    public float minimalShootingDistance;
    float minimalShootingDistanceSquared;


    public float distanceCheckingInterval;
    float nextDistanceCheckTime;

    enum MissileFighterState
    {
        TooFarAway,
        TooNear,
        InShootingDistance,
    }

    MissileFighterState state;

    public void SetUpBehaviour(GameEntity entity, EC_Movement movement, EC_ScanForEnemyUnits sensing, EC_MissileWeapon weapon)
    {
        this.sensing = sensing;
        this.entity = entity;
        this.movement = movement;
        this.weapon = weapon;

        nextDistanceCheckTime = UnityEngine.Random.Range(0, distanceCheckingInterval);

        maximalShootingDistanceSquared =  maximalShootingDistance * maximalShootingDistance;
        minimalShootingDistanceSquared = minimalShootingDistance  * minimalShootingDistance;
    }

    protected override void Update()
    {
        switch (state)
        {
            case MissileFighterState.TooNear:

                if (Time.time > nextDistanceCheckTime)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;

                    float distanceToEnemy = (sensing.nearestEnemy.transform.position - entity.transform.position).sqrMagnitude;
                    if (distanceToEnemy > minimalShootingDistanceSquared) 
                    {
                        state = MissileFighterState.InShootingDistance;
                        movement.Stop();
                    }
                    else
                    {
                        //move to a position perfectly in the shooting range
                        movement.MoveTo(entity.transform.position + (entity.transform.position - sensing.nearestEnemy.transform.position).normalized * (maximalShootingDistance/2));
                    }

                    weapon.AimAt(sensing.nearestEnemy);
                    movement.LookAt(sensing.nearestEnemy.transform);

                }

                if (weapon.CanShoot()) weapon.Shoot();


                break;

            case MissileFighterState.TooFarAway:

                if (Time.time > nextDistanceCheckTime)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;



                    //also check via raycast if we see the enemy


                    float distanceToEnemy = (sensing.nearestEnemy.transform.position - entity.transform.position).sqrMagnitude;

                    if (distanceToEnemy < maximalShootingDistanceSquared)
                    {
                        state = MissileFighterState.InShootingDistance;
                        movement.Stop();
                    }
                    else
                    {
                        movement.MoveTo(sensing.nearestEnemy.transform.position);
                    }
                }
                break;

            case MissileFighterState.InShootingDistance:

                //1. check if we need to change state
                if (Time.time > nextDistanceCheckTime)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;

                    float distanceToEnemy = (sensing.nearestEnemy.transform.position - entity.transform.position).sqrMagnitude;

                    if (distanceToEnemy > maximalShootingDistanceSquared)
                    {
                        state = MissileFighterState.TooFarAway;
                        movement.StopLookAt();
                        weapon.StopAiming();
                        movement.MoveTo(sensing.nearestEnemy.transform.position);
                    }
                    else if (distanceToEnemy < minimalShootingDistanceSquared)
                    {
                        state = MissileFighterState.TooNear;
                        movement.LookAt(sensing.nearestEnemy.transform);
                        movement.MoveTo(entity.transform.position + (entity.transform.position - sensing.nearestEnemy.transform.position).normalized * (maximalShootingDistance / 2));
                    }

                    weapon.AimAt(sensing.nearestEnemy);
                    movement.LookAt(sensing.nearestEnemy.transform);


                    //add some collisionChecking
                    /*
                    RaycastHit hit;
                    if (Physics.Raycast(unitAI.transform.position, unitAI.transform.forward, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.transform != sensing.nearestEnemy.transform)
                        {
                           // movement.MoveTo.
                        }
                    }   */

                }

                if (weapon.CanShoot()) weapon.Shoot();


                break;
        }
    }

    public override void OnBehaviourEnter()
    {
        float distanceToEnemy = (sensing.nearestEnemy.transform.position - entity.transform.position).sqrMagnitude;
        if (distanceToEnemy < minimalShootingDistanceSquared)
        {
            state = MissileFighterState.TooNear;
            movement.MoveTo(entity.transform.position + (entity.transform.position - sensing.nearestEnemy.transform.position).normalized * (maximalShootingDistance / 2));
        }
        else if (distanceToEnemy > maximalShootingDistanceSquared)
        {
            state = MissileFighterState.TooFarAway;
            movement.StopLookAt();
            movement.MoveTo(sensing.nearestEnemy.transform.position);
            weapon.StopAiming();
        }
        else
        {
            movement.LookAt(sensing.nearestEnemy.transform);
            movement.Stop();
            state = MissileFighterState.InShootingDistance;
        }
    }

    public override void OnBehaviourExit()
    {
        movement.StopLookAt();
        weapon.StopAiming(); 
    }
}

[System.Serializable]
public class B_MeleeAttackBuilding : Behaviour
{
    Building targetBuilding;
    EC_Movement movement;

    EC_MeleeWeapon weapon;

    public float distanceCheckingInterval;
    float nextDistanceCheckTime;

    enum AttackBuildingState
    {
        BehaviourStart,
        MovingToBuilding,
        InMeleeDistance
    }

    AttackBuildingState state;

    public void SetUpBehaviour(GameEntity entity, EC_Movement movement, EC_MeleeWeapon weapon)
    {
        this.entity = entity;
        this.movement = movement;
        this.weapon = weapon;

        nextDistanceCheckTime = UnityEngine.Random.Range(0, distanceCheckingInterval);

    }

    public void SetTargetBuilding(Building targetBuilding)
    {
        this.targetBuilding = targetBuilding;
    }

    protected override void Update()
    {
        switch (state)
        {
            case AttackBuildingState.BehaviourStart:

                movement.MoveTo(targetBuilding.transform.position);
                state = AttackBuildingState.MovingToBuilding;

                break;

            case AttackBuildingState.MovingToBuilding:

                if (Time.time > nextDistanceCheckTime)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;

                    if (weapon.IsInMeleeRange(targetBuilding.transform.position))
                    {
                        state = AttackBuildingState.InMeleeDistance;
                        movement.Stop();
                    }
                }

                break;

            case AttackBuildingState.InMeleeDistance:

                if (!weapon.IsInMeleeRange(targetBuilding.transform.position))
                {
                    state = AttackBuildingState.BehaviourStart;
                }
                else
                {
                    if (weapon.CanAttack())
                    {
                        weapon.Attack(targetBuilding.GetComponent<IDamageable<float>>());
                    }
                }



                    break;
        }
    }
}

[System.Serializable]
public class B_MissileAttackBuilding : Behaviour
{
    Building targetBuilding;
    EC_Movement movement;

    EC_MissileWeapon weapon;

    public float distanceCheckingInterval;
    float nextDistanceCheckTime;

    public float maximalShootingDistance;
    float maximalShootingDistanceSquared;
    public float minimalShootingDistance;
    float minimalShootingDistanceSquared;

    enum AttackBuildingState
    {
        TooFarAway,
        TooNear,
        InShootingDistance,
    }

    AttackBuildingState state;

    public void SetUpBehaviour(GameEntity entity, EC_Movement movement, EC_MissileWeapon weapon)
    {
        this.entity = entity;
        this.movement = movement;
        this.weapon = weapon;

        maximalShootingDistanceSquared = maximalShootingDistance * maximalShootingDistance;
        minimalShootingDistanceSquared = minimalShootingDistance * minimalShootingDistance;

        nextDistanceCheckTime = UnityEngine.Random.Range(0, distanceCheckingInterval);

    }

    public void SetTargetBuilding(Building targetBuilding)
    {
        this.targetBuilding = targetBuilding;
    }

    public override void OnBehaviourEnter()
    {
        float distanceToEnemy = (targetBuilding.transform.position - entity.transform.position).sqrMagnitude;
        if (distanceToEnemy < minimalShootingDistanceSquared)
        {
            state = AttackBuildingState.TooNear;
            movement.LookAt(targetBuilding.transform);
            weapon.AimAt(targetBuilding);
            movement.MoveTo(entity.transform.position + (entity.transform.position- targetBuilding.transform.position).normalized * (maximalShootingDistance / 2));
        }
        else if (distanceToEnemy > maximalShootingDistanceSquared)
        {

            state = AttackBuildingState.TooFarAway;
            movement.StopLookAt();
            movement.MoveTo(targetBuilding.transform.position);
            weapon.StopAiming();
        }
        else
        {
            movement.LookAt(targetBuilding.transform);
            movement.Stop();
            state = AttackBuildingState.InShootingDistance;
            weapon.AimAt(targetBuilding);
        }
    }

    protected override void Update()
    {
        switch (state)
        {
            case AttackBuildingState.TooNear:

                if (nextDistanceCheckTime > Time.time)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;
                    if((targetBuilding.transform.position - entity.transform.position).sqrMagnitude > minimalShootingDistanceSquared)
                    {
                        movement.Stop();
                        state = AttackBuildingState.InShootingDistance;
                    }
                }

                if (weapon.CanShoot()) weapon.Shoot();




                break;

            case AttackBuildingState.InShootingDistance:

                if (nextDistanceCheckTime > Time.time)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;
                    float distanceToEnemy = (targetBuilding.transform.position - entity.transform.position).sqrMagnitude;

                    if (distanceToEnemy < minimalShootingDistanceSquared)
                    {
                        state = AttackBuildingState.TooNear;
                        movement.LookAt(targetBuilding.transform);
                        movement.MoveTo(entity.transform.position + (entity.transform.position - targetBuilding.transform.position).normalized * (maximalShootingDistance / 2));
                    }
                    else if (distanceToEnemy > maximalShootingDistanceSquared)
                    {
                        state = AttackBuildingState.TooFarAway;
                        movement.StopLookAt();
                        movement.MoveTo(targetBuilding.transform.position);
                        weapon.StopAiming();
                    }
                }

                if (weapon.CanShoot()) weapon.Shoot();



                break;

            case AttackBuildingState.TooFarAway:

                if (nextDistanceCheckTime > Time.time)
                {
                    nextDistanceCheckTime = Time.time + distanceCheckingInterval;

                    if ((targetBuilding.transform.position - entity.transform.position).sqrMagnitude < maximalShootingDistanceSquared)
                    {
                        movement.LookAt(targetBuilding.transform);
                        movement.Stop();
                        state = AttackBuildingState.InShootingDistance;
                        weapon.AimAt(targetBuilding);
                    }

                }
                    break;

        }
    }
}

[System.Serializable]
public class B_Worker: Behaviour
{
    enum WorkerState
    {
        Idle,
        Construction,
        Harvesting,
        Depositing//dont know the words but the units go to the building and dissapear into it
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

    /*enum DepositionState
    {
        MovingToBuilding
    }*/

    [SerializeField]
    WorkerState state;
    [SerializeField]
    ConstructionState constructionState;
    [SerializeField]
    HarvestingState harvestingState;

    EC_Base assignedBase;
    EC_HarvestingBuilding assignedHarvester;
    RessourceType assignedHarvesterType;


    BuildingInConstruction assignedBuildingToBuild;

    EC_Movement movement;

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
    [Header("How far does he need to stand from the buildingto be able to construct")]
    public float constructionRange;

    //for ressourceGathering
    [Tooltip("how much ressources do we gather on one hit")]
    public int ressourceGatheringPower;


    //public int ressourceTransportLoad;
    //int currentRessourceTransportLoad = 0;
    public float ressourceGatherInterval;
    [SerializeField]
    WorkerInventory ressourceInventory;
    float nextRessourceGatheringTime;
    bool standingBesideHarvesterAndWaiting = false;
    Ressource currentSelectedRessource;

    //for deposition
    Building despositionBuilding;

    [Header("for the worker look in different sections")]
    public GameObject constructionAccesories;
    public WorkerTool constructionTool;
    public GameObject ferHarvestingAccesories;
    public WorkerTool ferHarvestingTool;
    public GameObject merHarvestingAccesories;
    public WorkerTool merHarvestingTool;




    public void SetUpBehaviour(GameEntity entity, EC_Movement movement)
    {
        this.entity = entity;
        this.movement = movement;
        nextScanTime = Time.time + UnityEngine.Random.Range(0, scanInterval);
        AssignToIdle();
    }

    public override void OnBehaviourEnter()
    {
        switch (state)
        {
            case WorkerState.Construction:

                constructionState = ConstructionState.Idle;

                break;

            case WorkerState.Harvesting:

                if(ressourceInventory.currentRessourceTransportLoad < ressourceInventory.ressourceTransportLoad)
                {
                    harvestingState = HarvestingState.Idle;
                }
                else
                {
                    harvestingState = HarvestingState.TransportingRessourceToHarvester;
                    Vector3 targetPosition = assignedHarvester.transform.position + (entity.transform.position - assignedHarvester.transform.position).normalized * assignedHarvester.width / 2;
                    movement.MoveTo(targetPosition);
                }

                break;

            case WorkerState.Depositing:

                state = WorkerState.Idle;

                break;


        }
    }

    public override void OnBehaviourExit()
    {
        if (state == WorkerState.Depositing)
        {
            despositionBuilding.GetComponent<IDepositioneable<B_Worker>>().OnWorkerCancelsTasks(this);
        }

        constructionTool.StopAnimation();
        ferHarvestingTool.StopAnimation();
        merHarvestingTool.StopAnimation();
    }

    protected override void Update()
    {
        switch (state)
        {
            case WorkerState.Idle:

                if (Time.time > nextIdleMovementTime)
                {
                    nextIdleMovementTime = Time.time + idleMovementInterval;
                    Vector3 wanderPoint = UnityEngine.Random.insideUnitSphere * 4;
                    wanderPoint.y = entity.transform.position.y;

                    wanderPoint += entity.transform.forward * 4 + entity.transform.position;

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
                                    float currentDistance = (building.transform.position - entity.transform.position).sqrMagnitude;

                                    if (currentDistance < nearestDistance)
                                    {
                                        nearestDistance = currentDistance;
                                        nearestBuildingToConstruct = building;
                                    }
                                }

                                assignedBuildingToBuild = nearestBuildingToConstruct;
                                //we move to a position from where we can build, not to a position inside the building
                                Vector3 targetPosition = nearestBuildingToConstruct.transform.position + (entity.transform.position - nearestBuildingToConstruct.transform.position).normalized * nearestBuildingToConstruct.width / 2;
                                movement.MoveTo(targetPosition);
                                constructionState = ConstructionState.MovingToBuilding;
                            }
                            else if (!standingBesideBaseAndWaiting)
                            {
                                Vector3 positonToMoveTo = UnityEngine.Random.insideUnitSphere * 8;
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

                            if (assignedBuildingToBuild != null)
                            {
                                if (Vector3.Distance(entity.transform.position, assignedBuildingToBuild.transform.position) < assignedBuildingToBuild.width + constructionRange)
                                {
                                    movement.Stop();
                                    constructionState = ConstructionState.Constructing;
                                    constructionTool.StartAnimation();
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

                        if (Time.time > nextConstructionTime)
                        {
                            //check if it istn completed yet
                            if (assignedBuildingToBuild != null)
                            {
                                nextConstructionTime = Time.time + constructionInterval;
                                assignedBuildingToBuild.Construct(constructionPoints);
                            }
                            else
                            {
                                constructionState = ConstructionState.Idle;
                                constructionTool.StopAnimation();

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

                            HashSet<Ressource> ressources = RessourcesManager.Instance.GetRessources(assignedHarvesterType);

                            if (ressources.Count > 0)
                            {
                                standingBesideHarvesterAndWaiting = false;


                                Ressource nearestRessource = null;
                                float nearestDistance = Mathf.Infinity;

                                foreach (Ressource ressource in ressources)
                                {
                                    float currentDistance = (ressource.transform.position - assignedHarvester.transform.position).sqrMagnitude;

                                    if (currentDistance < nearestDistance)
                                    {
                                        nearestDistance = currentDistance;
                                        nearestRessource = ressource;
                                    }
                                }

                                currentSelectedRessource = nearestRessource;
                                Vector3 targetPosition = currentSelectedRessource.transform.position + (entity.transform.position - currentSelectedRessource.transform.position).normalized * currentSelectedRessource.width / 2;
                                movement.MoveTo(targetPosition);
                                harvestingState = HarvestingState.MovingToRessource;
                            }
                            else if (!standingBesideHarvesterAndWaiting)
                            {
                                Vector3 positonToMoveTo = UnityEngine.Random.insideUnitSphere * 5;
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
                                if (Vector3.Distance(entity.transform.position, currentSelectedRessource.transform.position) < currentSelectedRessource.width)
                                {
                                    movement.Stop();
                                    harvestingState = HarvestingState.GatheringRessource;

                                    //activate the tool
                                    if(assignedHarvesterType == RessourceType.fer)
                                    {
                                        ferHarvestingTool.StartAnimation();
                                    }
                                    else if(assignedHarvesterType == RessourceType.mer)
                                    {
                                        merHarvestingTool.StartAnimation();
                                    }

                                    nextRessourceGatheringTime = Time.time + ressourceGatherInterval;
                                }
                            }
                            else
                            {
                                harvestingState = HarvestingState.Idle;
                                ferHarvestingTool.StopAnimation();
                                merHarvestingTool.StopAnimation();


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
                                if (ressourceInventory.currentRessourceTransportLoad + ressourceGatheringPower > ressourceInventory.ressourceTransportLoad)
                                {
                                    ressourceInventory.AddRessource(currentSelectedRessource.type, currentSelectedRessource.TakeRessource(ressourceInventory.ressourceTransportLoad - ressourceInventory.currentRessourceTransportLoad));      
                                }
                                else
                                {
                                    ressourceInventory.AddRessource(currentSelectedRessource.type, currentSelectedRessource.TakeRessource(ressourceGatheringPower));
                                }

                                //if the sack is full, go back
                                if (ressourceInventory.currentRessourceTransportLoad == ressourceInventory.ressourceTransportLoad)
                                {
                                    Vector3 targetPosition = assignedHarvester.transform.position + (entity.transform.position - assignedHarvester.transform.position).normalized * assignedHarvester.width / 2;
                                    movement.MoveTo(targetPosition);
                                    // movement.MoveTo(assignedHarvester.transform.position);
                                    harvestingState = HarvestingState.TransportingRessourceToHarvester;
                                    ferHarvestingTool.StopAnimation();
                                    merHarvestingTool.StopAnimation();

                                }
                            }
                            else
                            {
                                ferHarvestingTool.StopAnimation();
                                merHarvestingTool.StopAnimation();

                                if (ressourceInventory.currentRessourceTransportLoad > 0)
                                {
                                    Vector3 targetPosition = assignedHarvester.transform.position + (entity.transform.position - assignedHarvester.transform.position).normalized * assignedHarvester.width / 2;
                                    movement.MoveTo(targetPosition);
                                    //movement.MoveTo(assignedHarvester.transform.position);
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

                            if (Vector3.Distance(entity.transform.position, assignedHarvester.transform.position) < assignedHarvester.width)
                            {
                                movement.Stop();
                                assignedHarvester.DepotRessource(ressourceInventory.currentRessourceTransportLoad);
                                ressourceInventory.Clear();

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

            case WorkerState.Depositing:

                if (Time.time > nextScanTime)
                {
                    nextScanTime = Time.time + scanInterval;

                    if (despositionBuilding != null)
                    {
                        if (Vector3.Distance(entity.transform.position, despositionBuilding.transform.position) < despositionBuilding.width)
                        {
                            despositionBuilding.GetComponent<IDepositioneable<B_Worker>>().DepositionWorker(this);
                        }
                    }
                    else
                    {
                        AssignToIdle();
                    }
                }


                break;
        }
    }

    public void AssignToConstruction(EC_Base assignedBase)
    {
        if (state == WorkerState.Idle) PlayerManager.Instance.RemoveIdleWorker(this);
        this.assignedBase = assignedBase;
        state = WorkerState.Construction;
        constructionState = ConstructionState.Idle;
        nextScanTime = Time.time;
        ChangeAcessories();     
    }

    public void AssignToHarvesting(EC_HarvestingBuilding harvester)
    {
        if (state == WorkerState.Idle) PlayerManager.Instance.RemoveIdleWorker(this);
        this.assignedHarvester = harvester;
        assignedHarvesterType = harvester.type;
        state = WorkerState.Harvesting;
        harvestingState = HarvestingState.Idle;
        nextScanTime = Time.time;
        ChangeAcessories();


    }

    public void AssignToDeposition(Building depositioneable)
    {
        if (state == WorkerState.Idle) PlayerManager.Instance.RemoveIdleWorker(this);
        despositionBuilding = depositioneable;
        despositionBuilding.GetComponent<IDepositioneable<B_Worker>>().OnWorkerGetsTaksAssigned(this);
        state = WorkerState.Depositing;
        Vector3 targetPosition = despositionBuilding.transform.position + (entity.transform.position - despositionBuilding.transform.position).normalized * despositionBuilding.width / 2;
        movement.MoveTo(targetPosition);
        //movement.MoveTo(despositionBuilding.transform.position);
        nextScanTime = Time.time;

    }

    public void AssignToIdle()
    {
        if(state == WorkerState.Depositing)
        {
            despositionBuilding.GetComponent<IDepositioneable<B_Worker>>().OnWorkerCancelsTasks(this);
        }

        state = WorkerState.Idle;
        nextIdleMovementTime = 0;
        PlayerManager.Instance.AddIdleWorker(this);
        nextIdleMovementTime = UnityEngine.Random.Range(0, idleMovementInterval);
        ChangeAcessories();

    }

    void ChangeAcessories()
    {
        constructionTool.StopAnimation();
        ferHarvestingTool.StopAnimation();
        merHarvestingTool.StopAnimation();

        switch (state)
        {
            case WorkerState.Idle:

                constructionAccesories.SetActive(false);
                ferHarvestingAccesories.SetActive(false);
                merHarvestingAccesories.SetActive(false);

                break;

            case WorkerState.Construction:

                constructionAccesories.SetActive(true);
                ferHarvestingAccesories.SetActive(false);
                merHarvestingAccesories.SetActive(false);

                break;

            case WorkerState.Harvesting:

                if (assignedHarvesterType == RessourceType.fer)
                {
                    constructionAccesories.SetActive(false);
                    ferHarvestingAccesories.SetActive(true);
                    merHarvestingAccesories.SetActive(false);
                }
                else if (assignedHarvesterType == RessourceType.mer)
                {
                    constructionAccesories.SetActive(false);
                    ferHarvestingAccesories.SetActive(false);
                    merHarvestingAccesories.SetActive(true);
                }

                break;
        }
    }

    public override void OnDie()
    {
        PlayerManager.Instance.RemoveIdleWorker(this);
        switch (state)
        {
            case WorkerState.Construction:

                BuildingSystem.Instance.playersBaseLocation.GetComponent<EC_Base>().OnWorkerDies(this);

                break;

            case WorkerState.Harvesting:

                assignedHarvester.OnWorkerDies(this);

                break;

            case WorkerState.Depositing:

                despositionBuilding.GetComponent<IDepositioneable<B_Worker>>().OnWorkerCancelsTasks(this);

                break;
        }
    }
}

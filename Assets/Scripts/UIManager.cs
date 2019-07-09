using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("RessourcePanel")]
    public Text ferRessource;
    public Text merRessource;
    public Text rubithRessource;

    public Text currentPopulation;
    public Text populationLimit;

    [Header("StrategyMenu")]
    public GameObject exitPanel;
    public GameObject buildingPanel;
    public GameObject strategyMenuPanel;

    enum UIMode
    {
        Strategy,
        Action
    }

    enum StrategyUIMode
    {
        Default,
        Building,
        Economy
    }


    StrategyUIMode strategyUIMode = StrategyUIMode.Default;

    public PlayerController playerController;

    HashSet<WorkerAssignerUI> workerAssigners = new HashSet<WorkerAssignerUI>();

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HideWorkerAssigners();
        }
    }

    public void UpdateRessourcesUI(int ferValue, int merValue, int rubithValue)
    {
        ferRessource.text = ferValue.ToString();
        merRessource.text = merValue.ToString();
        rubithRessource.text = rubithValue.ToString();
    }

    public void UpdatePopulationUI(int currentPopulation, int populationLimit)
    {
        this.currentPopulation.text = currentPopulation.ToString();
        this.populationLimit.text = populationLimit.ToString();
    }

    public void OnBuildingButtonClicked()
    {
        buildingPanel.SetActive(true);
        strategyUIMode = StrategyUIMode.Building;
        strategyMenuPanel.SetActive(false);
        exitPanel.SetActive(true);
    }

    public void OnEconomyButtonClicked()
    {
        strategyUIMode = StrategyUIMode.Economy;
        playerController.EnterEconomyMenagementMode();
        ShowWorkerAssigners();
        strategyMenuPanel.SetActive(false);
        exitPanel.SetActive(true);
    }

    public void OnStrategyExitButtonClicked()
    {
        if(strategyUIMode == StrategyUIMode.Economy)
        {
            HideWorkerAssigners();
            playerController.ExitEconomyMenagementMode();

        }
        else if(strategyUIMode == StrategyUIMode.Building)
        {
            buildingPanel.SetActive(false);
            playerController.StopPlaningBuilding();
        }

        strategyMenuPanel.SetActive(true);
        exitPanel.SetActive(false);
        strategyUIMode = StrategyUIMode.Default;
    }

    public void OnBuildingSelectButtonClicked(BuildingPlacerButton button)
    {
        playerController.StartPlaningBuilding(button.buildingData);
    }

    #region manages the worker assigner worldspace UI´s for the economy View
    public void AddWorkerAssigner(WorkerAssignerUI work)
    {
        workerAssigners.Add(work);
    }

    public void RemoveWorkerAssigner(WorkerAssignerUI work)
    {
        workerAssigners.Remove(work);
    }

    public void ShowWorkerAssigners()
    {
        foreach(WorkerAssignerUI assigner in workerAssigners)
        {
            assigner.canvas.enabled = true;
        }
    }

    public void HideWorkerAssigners()
    {
        foreach (WorkerAssignerUI assigner in workerAssigners)
        {
            assigner.canvas.enabled = false;
        }
    }

    #endregion

}

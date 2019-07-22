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
    //public GameObject exitPanel;
    public GameObject buildingPanel;
    public GameObject strategyMenuPanel;

    public ToogleableButton economyButton;
    public ToogleableButton buildingButton;

    public RectTransform baseHPBar;
    public GameObject gameOverPanel;


    enum UIMode
    {
        Strategy,
        Action
    }

   /* enum StrategyUIMode
    {
        Default,
        Building,
        Economy
    }


    StrategyUIMode strategyUIMode = StrategyUIMode.Default;*/

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

    public void OnBuildingButtonActivated()
    {
        ActivateBuildingPanel();
    }

    public void OnBuildingButtonDeactivated()
    {
        playerController.StopPlaningBuilding();
        DeactivateBuildingPanel();
    }

    public void ActivateBuildingPanel()
    {
        buildingPanel.SetActive(true);
        buildingButton.Activate();
    }

    public void DeactivateBuildingPanel()
    {
        buildingButton.Deactivate();
        buildingPanel.SetActive(false);
    }

    public void ToogleBuildingPanel()
    {
        if (buildingPanel.activeSelf)
        {
            DeactivateBuildingPanel();
        }
        else
        {
            ActivateBuildingPanel();
        }
    }



    public void OnEconomyButtonActivated()
    {
        playerController.EnterEconomyMenagementMode();
    }

    public void OnEconomyButtonDeactivated()
    {
        playerController.ExitEconomyMenagementMode();
    }

    public void ActivateEconomyView()
    {
        ShowWorkerAssigners();
        economyButton.Activate();
    }

    public void DeactivateEconomyView()
    {
        economyButton.Deactivate(); //to make sure it changes color;
        HideWorkerAssigners();

    }

    public void OnBuildingSelectButtonClicked(BuildingPlacerButton button)
    {
        playerController.StartPlaningBuilding(button.buildingData);
    }

    public void SetBaseHP(float currentHealth, float maxHealth)
    {
        baseHPBar.localScale = new Vector3(currentHealth/maxHealth, 1f, 1f);
        if (currentHealth <= 0)
        {
            gameOverPanel.SetActive(true);
        }
    }

    #region manages the worker assigner worldspace UI´s for the economy View
    public void AddWorkerAssigner(WorkerAssignerUI work)
    {
        workerAssigners.Add(work);
        if (playerController.economyActive) work.canvas.enabled = true;
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

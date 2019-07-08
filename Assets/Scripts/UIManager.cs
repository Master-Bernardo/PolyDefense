using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text ferRessource;
    public Text merRessource;
    public Text rubithRessource;

    public Text currentPopulation;
    public Text populationLimit;

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
            ShowWorkerAssigners();
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

}

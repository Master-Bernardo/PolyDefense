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

}

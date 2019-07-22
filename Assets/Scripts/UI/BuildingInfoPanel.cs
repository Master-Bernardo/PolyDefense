using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoPanel : MonoBehaviour
{
    public Text buildingName;
    public Text buildingDescription;

    //cost
    public Text ferCost;
    public Text merCost;
    public Text rubithCost;

    public Text constructionPoints;
    public Text healthPoints;

    public void SetUp(BuildingData data)
    {
        buildingName.text = data.buildingName;
        buildingDescription.text = data.description;

        for (int i = 0; i < data.cost.Length; i++)
        {
            if(data.cost[i].type == RessourceType.fer)
            {
                ferCost.text = data.cost[i].value.ToString(); ;
            }
            else if (data.cost[i].type == RessourceType.mer)
            {
                merCost.text = data.cost[i].value.ToString(); ;
            }
            if (data.cost[i].type == RessourceType.rubith)
            {
                rubithCost.text = data.cost[i].value.ToString(); ;
            }
        }

        constructionPoints.text = data.constructionPoints.ToString();
        healthPoints.text = data.healthPoints.ToString();

    }
}


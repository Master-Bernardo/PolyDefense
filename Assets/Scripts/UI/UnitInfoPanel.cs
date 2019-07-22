using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoPanel : MonoBehaviour
{
    public Text buildingName;
    public Text buildingDescription;

    //cost
    public Text ferCost;
    public Text merCost;
    public Text rubithCost;

    public Text recruitmentPoints;
    public Text healthPoints;

    public void SetUp(UnitData data)
    {
        buildingName.text = data.unitName;
        buildingDescription.text = data.description;

        for (int i = 0; i < data.cost.Length; i++)
        {
            if (data.cost[i].type == RessourceType.fer)
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

        recruitmentPoints.text = data.recruitingPoints.ToString();
        healthPoints.text = data.healthPoints.ToString();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * changes the model depending on the construction state
 */
public class BuildingConstructionModelChanger : MonoBehaviour
{
    //percentageCompleted should be value between 0 and 1
    public void ChangeModel(float percentageCompleted)
    {
        transform.localScale = new Vector3(1f, percentageCompleted/2+0.1f, 1f);
    }
}

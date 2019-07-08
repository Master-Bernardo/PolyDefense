using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//just sets the buttons we dont want to not active
[ExecuteInEditMode]
public class WorkerAssignerUIEditorResizer : MonoBehaviour
{
#if UNITY_EDITOR

    public int workerButtons;
    int workerButtonsLastFrame = 0;
    public GameObject[] buttons; //all the buttons except the first one


    private void Update()
    {
        if (workerButtons != workerButtonsLastFrame)
        {
            for (int i = 0; i < workerButtons; i++)
            {
                buttons[i].SetActive(true);
            }
            for (int i = workerButtons; i < buttons.Length; i++)
            {
                buttons[i].SetActive(false);
            }
        }

        workerButtonsLastFrame = workerButtons;
    }
#endif
}

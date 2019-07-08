using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IWorkerAssigneable<T>
{
    int SetWorkerNumber(T newNumber);
}




public class WorkerAssignerUI : MonoBehaviour
{
    //when on of the buttons gets klicked, it calls a function of this assigner- how exactly?

    //there is an arry of buttons
    public Color noWorkerColor;
    public Color workerColor;
    public Image[] images;
    //or every button gives a different int value - one is 0 etc...
    IWorkerAssigneable<int> assignealbe;
    public Ability assigneableAb; //because unity doesnt serialise interfaces, we get it on awake
    public Canvas canvas;

   


    private void Start()
    {
        //set the interface value 
        assignealbe = assigneableAb.GetComponent<IWorkerAssigneable<int>>();
        UIManager.Instance.AddWorkerAssigner(this);


    }

     

    public void SetWorkerNumber(int buttonID)
    {
        int cappedNumber = assignealbe.SetWorkerNumber(buttonID);

        UpdateUI(cappedNumber);
        //Debug.Log(cappedNewWorkerNumber);
        


    }

    //updates the UI according to the current worker situiation
    void UpdateUI(int workersAssigned)
    {
        //set the worker images
        for(int i = 0; i<=workersAssigned; i++)
        {
            images[i].color = workerColor;
        }

        for(int i = workersAssigned; i<images.Length; i++)
        {
            images[i].color = noWorkerColor;
        }

               
    }


    private void OnDestroy()
    {
        UIManager.Instance.RemoveWorkerAssigner(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IWorkerAssigneable<T>
{
    int SetWorkerNumber(T newNumber);

    void OnWorkerDies(B_Worker worker);
}




public class WorkerAssignerUI : MonoBehaviour
{
    //when on of the buttons gets klicked, it calls a function of this assigner- how exactly?

    //there is an arry of buttons
    public Color noWorkerColor;
    public Color workerColor;
    public ClickeableButton[] buttons;
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
         assignealbe.SetWorkerNumber(buttonID);
        //Debug.Log("number before: " + buttonID);
        //Debug.Log("cappedNumber: " + cappedNumber);
        //UpdateUI(cappedNumber);       
    }

    //updates the UI according to the current worker situiation
    public void UpdateUI(int workersAssigned)
    {
        //set the worker images
        for(int i = 0; i<=workersAssigned-1; i++)
        {
            //Debug.Log("paint green");
            buttons[i].SetColor(workerColor);
        }

        for(int i = workersAssigned; i<buttons.Length; i++)
        {
            buttons[i].SetColor(noWorkerColor);
            //Debug.Log("paint white");
        }


    }


    private void OnDestroy()
    {
        UIManager.Instance.RemoveWorkerAssigner(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;



public class BubbleMenuButtonStackable : MonoBehaviour, IPointerClickHandler
{
    int number;
    public Image backgroundImage;
    public Color recruitmentColor;
    public Color normalColor;
    public Image unitIcon;
    public Text textNumber;
    public int unitID;

    public GameObject queueHolderGO;
    IQueueHolder<int,int> queueHolder;
    public int onLmb;
    public int onRmb;
    public int onLmbAndShift;
    public int onRmbAndShift;
    public int onLmbAndStrg;

    /*public UnityEvent OnLmb;
    public UnityEvent OnRmb;
    public UnityEvent OnLmbAndShift;
    public UnityEvent OnRmbAndShift;
    public UnityEvent OnLmbAndStrg;*/

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("queueHolder: " + queueHolder);
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                queueHolder.AddElementToQueue(unitID, onLmb);
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                queueHolder.AddElementToQueue(unitID, onLmbAndStrg);
            }
            else
            {
                queueHolder.AddElementToQueue(unitID, onLmb);
            }
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                queueHolder.AddElementToQueue(unitID, onRmbAndShift);

            }
            else
            {
                queueHolder.AddElementToQueue(unitID, onRmb);
            }
        }
    }

    public void Print(string print)
    {
        Debug.Log(print);
    }

    public void SetUpUI(UnitData data)
    {
        unitIcon.sprite = data.menuImage;
        backgroundImage.color = normalColor;
        queueHolder = queueHolderGO.GetComponent<IQueueHolder<int, int>>();
    }

    public void UpdateUI (int numberOfUnitsInRecruitmentQueue)
    {
        if (numberOfUnitsInRecruitmentQueue > 0)
        {
            textNumber.enabled = true;
            backgroundImage.color = recruitmentColor;
            textNumber.text = numberOfUnitsInRecruitmentQueue.ToString();
        }
        else
        {
            textNumber.enabled = false;
            backgroundImage.color = normalColor;
        }
    }
}

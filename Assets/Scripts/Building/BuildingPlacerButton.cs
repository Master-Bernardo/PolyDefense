using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BuildingPlacerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public BuildingData buildingData;
    public Image image;
    public BuildingInfoPanel infoPanel;

    public Color hoverOnColor;
    Color normalColor;

    public UnityEvent clickEvent;


    private void Awake()
    {
        image.sprite = buildingData.menuImage;
        infoPanel.SetUp(buildingData);
        normalColor = image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverOnColor;
        infoPanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normalColor;
        infoPanel.gameObject.SetActive(false);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickEvent.Invoke();
    }
}

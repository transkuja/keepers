using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.transform.position = transform.position;
        tooltip.GetComponentInChildren<Text>().text = GetComponent<ItemInstance>().ItemContainer.Item.Description;
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        tooltip = GameManager.Instance.Ui.tooltipItem;
    }

}

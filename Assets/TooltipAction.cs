using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TooltipAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {

        tooltip.GetComponentInChildren<Text>().text = GameManager.Instance.GoTarget.GetComponent<Interactable>().Interactions.listActionContainers[transform.GetSiblingIndex()].strName;
        tooltip.transform.position = transform.position;

        Invoke("showTooltip", 0.5f);

    }

    public void showTooltip()
    {
        if (!tooltip.activeSelf)
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip.activeSelf)
        tooltip.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        tooltip = GameManager.Instance.Ui.tooltipAction;
    }
}

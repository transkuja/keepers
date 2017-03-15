using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatsHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.Ui.statsPanelTooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.Ui.statsPanelTooltip.SetActive(false);
    }
}

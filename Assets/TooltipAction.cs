using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TooltipAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject tooltip;


    private bool needToBeShown;
    private float timer;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.GetComponentInChildren<Text>().text = Translater.InteractionName(gameObject.name);
        tooltip.transform.position = transform.position;

        needToBeShown = true;

    }

    public void showTooltip()
    {
        if (!tooltip.activeSelf && needToBeShown)
        {
            tooltip.SetActive(true);
        }
    }
    public void hideTooltip()
    {
        if (tooltip.activeSelf)
        {
            tooltip.SetActive(false);
            timer = 0.0f;

        }
        needToBeShown = false;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hideTooltip();
    }

    // Use this for initialization
    void Start()
    {
        tooltip = GameManager.Instance.Ui.tooltipAction;
        needToBeShown = false;
        timer = 0.0f;
    }

    void Update()
    {
        if (needToBeShown)
        {
            timer += Time.deltaTime;
            {
                if (timer > 1000.0f)
                {
                    hideTooltip();
                }
                if (timer > 1.0f)
                {
                    showTooltip();
                }
            }
        }
    }
}

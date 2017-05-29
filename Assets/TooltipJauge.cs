using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum TypeOfJauge { Action, Mental, Health, Hunger };

public class TooltipJauge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject tooltip;
    [SerializeField]
    private TypeOfJauge typeJauge;

    private bool needToBeShown;
    private float timer;

    public void OnPointerEnter(PointerEventData eventData)
    {

        switch (typeJauge){
            case TypeOfJauge.Action:
                tooltip.GetComponentInChildren<Text>().text = Translater.TooltipText(typeJauge) + GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>().ActionPoints;
                tooltip.transform.position = transform.position;
                break;
            case TypeOfJauge.Mental:
                tooltip.GetComponentInChildren<Text>().text = Translater.TooltipText(typeJauge) + GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.MentalHealthHandler>().CurrentMentalHealth;
                tooltip.transform.position = Input.mousePosition;
                break;
            case TypeOfJauge.Health:
                tooltip.GetComponentInChildren<Text>().text = Translater.TooltipText(typeJauge) + GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Mortal>().CurrentHp;
                tooltip.transform.position = Input.mousePosition;
                break;
            case TypeOfJauge.Hunger:
                tooltip.GetComponentInChildren<Text>().text = Translater.TooltipText(typeJauge) + GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.HungerHandler>().CurrentHunger;
                tooltip.transform.position = Input.mousePosition;
                break;

        }
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
        tooltip = GameManager.Instance.Ui.tooltipJauge;
        needToBeShown = false;
        timer = 0.0f;
    }

    void Update()
    {
        if (needToBeShown)
        {
            timer += Time.deltaTime;
            {
                if ( timer > 1000.0f)
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

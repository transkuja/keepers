using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipJauge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject tooltip;
    private enum TypeOfJauge {Action, Mental, Health, Hunger};
    [SerializeField]
    private TypeOfJauge typeJauge;

    public void OnPointerEnter(PointerEventData eventData)
    {

        switch (typeJauge){
            case TypeOfJauge.Action:
                tooltip.GetComponentInChildren<Text>().text = "PA left : " + GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>().ActionPoints;
                tooltip.transform.position = transform.position;
                break;
            case TypeOfJauge.Mental:
                tooltip.GetComponentInChildren<Text>().text = "Mental : " + GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.MentalHealthHandler>().CurrentMentalHealth;
                tooltip.transform.position = Input.mousePosition;
                break;
            case TypeOfJauge.Health:
                tooltip.GetComponentInChildren<Text>().text = "HP : " + GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Mortal>().CurrentHp;
                tooltip.transform.position = Input.mousePosition;
                break;
            case TypeOfJauge.Hunger:
                tooltip.GetComponentInChildren<Text>().text = "Hunger : " + GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.HungerHandler>().CurrentHunger;
                tooltip.transform.position = Input.mousePosition;
                break;

        }
       
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        tooltip = GameManager.Instance.Ui.tooltipJauge;
    }
}

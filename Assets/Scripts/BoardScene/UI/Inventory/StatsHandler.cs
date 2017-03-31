using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatsHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (GameManager.Instance.ListOfSelectedKeepers.Count < 1) return;
        //KeeperInstance currentSelectedCharacter = GameManager.Instance.ListOfSelectedKeepers[0];


        //// Strengh
        //GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "F: ";
        //GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(0).GetChild(0).GetComponent<Text>().text += currentSelectedCharacter.Keeper.BaseStrength.ToString();
        //if(currentSelectedCharacter.Keeper.BonusStrength > 0)
        //{
        //    GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(0).GetChild(0).GetComponent<Text>().text += " + " + currentSelectedCharacter.Keeper.BonusStrength.ToString();
        //}
        //else if (currentSelectedCharacter.Keeper.BonusStrength < 0)
        //{
        //    GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(0).GetChild(0).GetComponent<Text>().text += " " + currentSelectedCharacter.Keeper.BonusStrength.ToString();
        //}
        //// Defense
        //GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "D: ";
        //GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(1).GetChild(0).GetComponent<Text>().text += currentSelectedCharacter.Keeper.BaseDefense.ToString();
        //if (currentSelectedCharacter.Keeper.BonusDefense > 0)
        //{
        //    GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(1).GetChild(0).GetComponent<Text>().text += " + " + currentSelectedCharacter.Keeper.BonusDefense.ToString();
        //}
        //else if (currentSelectedCharacter.Keeper.BonusDefense < 0)
        //{
        //    GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(1).GetChild(0).GetComponent<Text>().text += " " + currentSelectedCharacter.Keeper.BonusDefense.ToString();
        //}
        //// Spirit
        //GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "S: ";
        //GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(2).GetChild(0).GetComponent<Text>().text += currentSelectedCharacter.Keeper.BaseSpirit.ToString();
        //if (currentSelectedCharacter.Keeper.BonusSpirit > 0)
        //{
        //    GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(2).GetChild(0).GetComponent<Text>().text += " + " + currentSelectedCharacter.Keeper.BonusSpirit.ToString();
        //}
        //else if (currentSelectedCharacter.Keeper.BonusSpirit < 0)
        //{
        //    GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(2).GetChild(0).GetComponent<Text>().text += " " + currentSelectedCharacter.Keeper.BonusSpirit.ToString();
        //}
        //// Intelligence
        //GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "I: ";
        //GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(3).GetChild(0).GetComponent<Text>().text += currentSelectedCharacter.Keeper.BaseIntelligence.ToString();
        //if (currentSelectedCharacter.Keeper.BonusIntelligence > 0)
        //{
        //    GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(3).GetChild(0).GetComponent<Text>().text += " + " + currentSelectedCharacter.Keeper.BonusIntelligence.ToString();
        //}
        //else if (currentSelectedCharacter.Keeper.BonusIntelligence < 0)
        //{
        //    GameManager.Instance.Ui.statsPanelTooltip.transform.GetChild(3).GetChild(0).GetComponent<Text>().text += " " + currentSelectedCharacter.Keeper.BonusIntelligence.ToString();
        //}

        //GameManager.Instance.Ui.statsPanelTooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //GameManager.Instance.Ui.statsPanelTooltip.SetActive(false);
    }
}

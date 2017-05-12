using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuUIQ: MonoBehaviour {

    private MenuManagerQ menuManager;
    public Image startButtonImg;


    void Start()
    {
        menuManager = GetComponent<MenuManagerQ>();
        startButtonImg.enabled = false;
    }

    public void UpdateStartButton()
    {
        // TODO : refaire ça
        switch (menuManager.DeckOfCardsSelected)
        {
            case "deck_01":
                startButtonImg.GetComponentInChildren<Text>().text = menuManager.ListeSelectedKeepers.Count + "/1";
                break;
            case "deck_02":
            case "deck_03":
            case "deck_04":
                startButtonImg.GetComponentInChildren<Text>().text = menuManager.ListeSelectedKeepers.Count + "/3";
                break;
            default:
                startButtonImg.GetComponentInChildren<Text>().text = string.Empty;
                break;
        }

        if (menuManager.ListeSelectedKeepers.Count == 0 || menuManager.CardLevelSelected == -1 || menuManager.DeckOfCardsSelected == string.Empty
            || (menuManager.DeckOfCardsSelected == "deck_04" && menuManager.ListeSelectedKeepers.Count != 3)
            || (menuManager.DeckOfCardsSelected == "deck_02" && menuManager.ListeSelectedKeepers.Count != 3))
        {
            startButtonImg.enabled = false;
        }
        else
        {
            startButtonImg.enabled = true;
        }
        // end TODO
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuUIQ: MonoBehaviour {

    private MenuManagerQ menuManager;

    public GameObject CharacterPanel;
    public Image startButtonImg;
    public Image cardLevelSelectedImg;
    // TODO handle this better
    [SerializeField]
    Sprite[] levelImg;

    void Start()
    {
        menuManager = GetComponent<MenuManagerQ>();
        startButtonImg.enabled = false;
    }


    public void UpdateSelectedKeepers()
    {
        // Clear
        for (int i = 0; i < CharacterPanel.transform.childCount; i++)
        {
            Destroy(CharacterPanel.transform.GetChild(i).gameObject);
        }

        // On selection 
        int nbCharacters = menuManager.ListeSelectedKeepers.Count;
        for (int i = 0; i < nbCharacters; i++)
        {
            PawnInstance currentSelectedCharacter = menuManager.ListeSelectedKeepers[i];

            Sprite associatedSprite = currentSelectedCharacter.Data.AssociatedSprite;
            if (associatedSprite != null)
            {
                GameObject CharacterImage = Instantiate(GameManager.Instance.PrefabUIUtils.prefabMenuSelectedKeeperUI, CharacterPanel.transform);
                CharacterImage.name = currentSelectedCharacter.Data.PawnName + ".Panel";
                CharacterImage.transform.GetChild(0).GetComponent<Image>().sprite = associatedSprite;
                CharacterImage.transform.localScale = Vector3.one;}
        }

        UpdateStartButton();
    }

    public void UpdateCardLevelSelection()
    {
        if(menuManager.CardLevelSelected != -1)
        {
            cardLevelSelectedImg.sprite = levelImg[menuManager.CardLevelSelected - 1];
            cardLevelSelectedImg.enabled = true;
        } else
        {
            cardLevelSelectedImg.enabled = false;
        }

        UpdateStartButton();
    }

    public void UpdateDeckSelection()
    {
        // TODO Update Deck Selection


        UpdateStartButton();
    }

    public void UpdateStartButton()
    {

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
    }
}

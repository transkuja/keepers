using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestDeckLoader;
using QuestLoader; 

public class NewMenuManager : MonoBehaviour {

    private int cardLevelSelected = -1;
    private string deckOfCardsSelected = string.Empty;
    private List<PawnInstance> listeSelectedKeepers;

    public Transform[] questDecksPosition;

    void Start()
    {
        listeSelectedKeepers = new List<PawnInstance>();


        InitDeckOfCards();

        // TMP for debug use when level start
        foreach (string id in NewGameManager.Instance.PawnDataBase.DicPawnDataContainer.Keys)
        {
            Debug.Log(id);
        }

        for (int i = 0; i < NewGameManager.Instance.QuestDataBase.ListQuestData.Count; i++)
        {
            Debug.Log(NewGameManager.Instance.QuestDataBase.ListQuestData[i].idQuest);
        }
        // ENd tmp
    }

    public void InitDeckOfCards()
    {
        for (int i = 0; i < NewGameManager.Instance.QuestDeckDataBase.ListQuestDeck.Count; i++)
        {
            QuestDeckData qdd = NewGameManager.Instance.QuestDeckDataBase.ListQuestDeck[i];

            // Instanciation du deck
            GameObject goDeck = Instantiate(NewGameManager.Instance.PrefabUtils.prefabQuestDeck);
            goDeck.transform.SetParent(questDecksPosition[i], false);

            // Recuperation du component deck of cards for selection in menu
            DeckOfCards deckOfCards = goDeck.GetComponent<DeckOfCards>();
            if (deckOfCards != null && qdd.idQuestDeck != string.Empty)
            {
                deckOfCards.idQuestDeck = qdd.idQuestDeck;
                goDeck.layer = LayerMask.NameToLayer("DeckOfCards");
                if (qdd.materialQuestDeck != null)
                    goDeck.GetComponent<MeshRenderer>().material = qdd.materialQuestDeck;
                else
                    Debug.Log("No material found.");
            }
            else
                Debug.Log("Deck with no id to set on the prefab");
        }

    }

    #region Accessors
    public int CardLevelSelected
    {
        get
        {
            return cardLevelSelected;
        }

        set
        {
            cardLevelSelected = value;
        }
    }

    public string DeckOfCardsSelected
    {
        get
        {
            return deckOfCardsSelected;
        }

        set
        {
            deckOfCardsSelected = value;
        }
    }

    public void AddToSelectedKeepers(PawnInstance pi)
    {
        listeSelectedKeepers.Add(pi);
    }

    public void RemoveFromSelectedKeepers(PawnInstance pi)
    {
        listeSelectedKeepers.Remove(pi);
    }

    public bool ContainsSelectedKeepers(PawnInstance pi)
    {
        return listeSelectedKeepers.Contains(pi);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestDeckLoader;

public class NewMenuManager : MonoBehaviour {

    private int cardLevelSelected = -1;
    private string deckOfCardsSelected = string.Empty;
    private List<PawnInstance> listeSelectedKeepers;

    public Transform[] questDecksPosition;
    public Transform[] keepersPosition;
    public Transform[] cardlevelPosition;

    void Start()
    {
        listeSelectedKeepers = new List<PawnInstance>();

        InitDeckOfCards();
        InitCardLevels();
        InitKeepers();
    }

    public void InitDeckOfCards()
    {
        for (int i = 0; i < GameManager.Instance.QuestDeckDataBase.ListQuestDeck.Count; i++)
        {
            QuestDeckData qdd = GameManager.Instance.QuestDeckDataBase.ListQuestDeck[i];

            // Instanciation du deck
            GameObject goDeck = Instantiate(GameManager.Instance.PrefabUtils.prefabQuestDeck);
            goDeck.transform.SetParent(questDecksPosition[i], false);

            // Recuperation du component deck of cards for selection in menu
            DeckOfCards deckOfCards = goDeck.GetComponent<DeckOfCards>();
            if (deckOfCards != null && qdd.idQuestDeck != string.Empty)
            {
                deckOfCards.idQuestDeck = qdd.idQuestDeck;
                goDeck.layer = LayerMask.NameToLayer("DeckOfCards");
                /*if (qdd.materialQuestDeck != null)
                    goDeck.GetComponent<MeshRenderer>().material = qdd.materialQuestDeck;
                else
                    Debug.Log("No material found.");*/
            }
            else
                Debug.Log("Deck with no id to set on the prefab");
        }
    }

    public void InitCardLevels()
    {
        for (int i = 1; i <= 2; i++)
        {
            // Instanciation du deck
            GameObject goCardLevel = Instantiate(GameManager.Instance.PrefabUtils.prefabLevelCard);
            goCardLevel.transform.SetParent(cardlevelPosition[i-1], false);

            // Recuperation du component card level for selection in menu
            CardLevel cardLevel = goCardLevel.GetComponentInChildren<CardLevel>();
            if (cardLevel != null)
            {
                cardLevel.levelIndex = i;
            }
            else
                Debug.Log("Card with no id to set on the prefab");
        }
    }

    public void InitKeepers()
    {
        int iKeeper = 0;
        foreach (string id in GameManager.Instance.PawnDataBase.DicPawnDataContainer.Keys)
        {
            if (iKeeper > keepersPosition.Length) break;

            if (GameManager.Instance.PawnDataBase.DicPawnDataContainer[id].dicComponentData.ContainsKey(typeof(Behaviour.Keeper)))
            {
                GameObject goKeeper = GameManager.Instance.PawnDataBase.CreatePawn(id, Vector3.zero, Quaternion.Euler(180, 90, -90), keepersPosition[iKeeper]);
                //TMP
                goKeeper.transform.localScale = 5 * Vector3.one;
                //END TMP
                iKeeper++;
            }
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

    public List<PawnInstance> ListeSelectedKeepers
    {
        get
        {
            return listeSelectedKeepers;
        }

        set
        {
            listeSelectedKeepers = value;
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

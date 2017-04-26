using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestDeckLoader;
using QuestLoader; 

public class MenuManagerQ : MonoBehaviour {

    private int cardLevelSelected = -1;
    private string deckOfCardsSelected = string.Empty;
    private List<PawnInstance> listeSelectedKeepers;

    public Transform[] questDecksPosition;
    public Transform[] keepersPosition;
    public Transform[] cardlevelPosition;

    public GameObject GoPrefabLevelCard;
    public GameObject GoPrefabEventCard;
    public GameObject GoPrefabDeck;


    [SerializeField] public List<GameObject> listCardModels = new List<GameObject>();
    [SerializeField] public List<GameObject> listDeckModels = new List<GameObject>();

    [SerializeField] public List<GameObject> listLevelCards = new List<GameObject>();

    public Material matBox;

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
            //GameObject goDeck = Instantiate(GameManager.Instance.PrefabUtils.prefabQuestDeck);
            GameObject goDeck = Instantiate(GoPrefabDeck);
            goDeck.transform.SetParent(questDecksPosition[i].parent, false);
            GameObject.Destroy(questDecksPosition[i].gameObject);

            // Recuperation du component deck of cards for selection in menu
            DeckOfCards deckOfCards = goDeck.GetComponent<DeckOfCards>();
            if (deckOfCards != null && qdd.idQuestDeck != string.Empty)
            {
                deckOfCards.idQuestDeck = qdd.idQuestDeck;
                goDeck.layer = LayerMask.NameToLayer("DeckOfCards");
                goDeck.GetComponent<MeshFilter>().mesh = GetDeckModel(qdd.deckModelName).GetComponent<MeshFilter>().sharedMesh;
                goDeck.GetComponent<MeshRenderer>().material = matBox;
            }
            else
                Debug.Log("Deck with no id to set on the prefab");

            for (int j = 0; j < qdd.secondaryQuests.Count; j++)
            {
                GameObject goTemp = Instantiate(GoPrefabEventCard, goDeck.transform);
                goTemp.GetComponent<MeshFilter>().mesh = GetCardModel(qdd.secondaryQuests[j].cardModelname).GetComponent<MeshFilter>().sharedMesh;
                goTemp.tag = "OpenerContent";
                goTemp.SetActive(false);
            }

            goDeck.AddComponent<Opener>().bOverMode = true;
        }
    }

    public void InitCardLevels()
    {
        for (int i = 1; i <= 2; i++)
        {
            // Instanciation du deck
            GameObject goCardLevel = Instantiate(GoPrefabLevelCard);
            goCardLevel.transform.SetParent(cardlevelPosition[i-1].parent, false);

            GameObject.Destroy(cardlevelPosition[i - 1].gameObject);

            // Recuperation du component card level for selection in menu
            CardLevel cardLevel = goCardLevel.GetComponentInChildren<CardLevel>();
            if (cardLevel != null)
            {
                cardLevel.GetComponent<MeshFilter>().mesh = listLevelCards[i-1].GetComponent<MeshFilter>().sharedMesh;
                cardLevel.GetComponent<MeshRenderer>().material = matBox;
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

    public GameObject GetLevelCardModel(string strName)
    {
        foreach (GameObject goCardModel in listLevelCards)
        {
            if (strName == goCardModel.name)
            {
                return goCardModel;
            }
        }
        Debug.Log("Modele de carte \"" + strName + "\" introuvable");
        return null;
    }

    public GameObject GetCardModel(string strName)
    {
        foreach(GameObject goCardModel in listCardModels)
        {
            if(strName == goCardModel.name)
            {
                return goCardModel;
            }
        }
        Debug.Log("Modele de carte \"" + strName + "\" introuvable");
        return null;
    }

    public GameObject GetDeckModel(string strName)
    {
        foreach (GameObject goDeckModel in listDeckModels)
        {
            if (strName == goDeckModel.name)
            {
                return goDeckModel;
            }
        }
        Debug.Log("Modele de deck \"" + strName + "\" introuvable");
        return null;
    }
    #endregion


}

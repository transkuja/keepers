using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestDeckLoader;

public class MenuManagerQ : MonoBehaviour {

    private int cardLevelSelected = -1;
    private string deckOfCardsSelected = string.Empty;
    private List<PawnInstance> listeSelectedKeepers;

    public Transform questDecksPosition;
    public Transform[] keepersPosition;
    public Transform levelDeckPosition;

    public GameObject GoPrefabLevelCard;
    public GameObject GoPrefabEventCard;
    public GameObject GoPrefabDeck;


    [SerializeField] public List<GameObject> listCardModels = new List<GameObject>();
    [SerializeField] public List<GameObject> listDeckModels = new List<GameObject>();

    [SerializeField] public List<GameObject> listLevelCards = new List<GameObject>();

    void Start()
    {
        listeSelectedKeepers = new List<PawnInstance>();

        InitEventCards();
        InitLevelsCard();
        InitKeepers();
    }

    public void InitEventCards()
    {
      
        for (int i = 0; i < GameManager.Instance.EventDataBase.listEvents.Count; i++)
        {
            GameObject goEventCard = Instantiate(GoPrefabEventCard, questDecksPosition);
            goEventCard.transform.localPosition = Vector3.zero;
            goEventCard.GetComponent<MeshFilter>().mesh = GetCardModel(GameManager.Instance.EventDataBase.listEvents[i].cardModelName).GetComponent<MeshFilter>().sharedMesh;
        }
    }

    public void InitLevelsCard()
    {
        LevelDataBase leveldb = new LevelDataBase();

        for (int i = 0; i < leveldb.listLevels.Count; i++)
        {
            // Instanciation du deck
            GameObject goCardLevel = Instantiate(GoPrefabLevelCard, levelDeckPosition);
            goCardLevel.transform.localPosition = Vector3.zero;

            goCardLevel.GetComponent<MeshFilter>().mesh = GetLevelCardModel(leveldb.listLevels[i].cardModelName).GetComponent<MeshFilter>().sharedMesh;
            goCardLevel.GetComponent<CardLevel>().levelIndex = leveldb.listLevels[i].id;

            for(int j = 0; j< leveldb.listLevels[i].listDeckId.Count; j++)
            {
                GameObject goDeck = Instantiate(GoPrefabDeck, goCardLevel.transform);
                goDeck.transform.localPosition = Vector3.zero;

                QuestDeckData qdd = GameManager.Instance.QuestDeckDataBase.GetQuestDeckDataByID(leveldb.listLevels[i].listDeckId[j]);

                goDeck.GetComponent<MeshFilter>().mesh = GetDeckModel(qdd.deckModelName).GetComponent<MeshFilter>().sharedMesh;

                for(int k = 0; k < qdd.secondaryQuests.Count; k++)
                {
                    GameObject goQuestCard = Instantiate(GoPrefabEventCard, goDeck.transform);
                    goQuestCard.transform.localPosition = Vector3.zero;

                    goQuestCard.GetComponent<MeshFilter>().mesh = GetCardModel(qdd.secondaryQuests[k].cardModelname).GetComponent<MeshFilter>().sharedMesh;
                }
            }
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
                GameObject goKeeper = GameManager.Instance.PawnDataBase.CreatePawn(id, /*Vector3.zero*/ keepersPosition[iKeeper].position, keepersPosition[iKeeper].rotation, null /*keepersPosition[iKeeper]*/);
                OpenerContent oc = goKeeper.AddComponent<OpenerContent>();
                oc.fSpeed = 10;
                oc.AddKeyPose(keepersPosition[iKeeper].position, keepersPosition[iKeeper].rotation);
                oc.AddKeyPose(keepersPosition[iKeeper].position + new Vector3(1.5f,1,0) , keepersPosition[iKeeper].rotation);
                oc.AddKeyPose(keepersPosition[iKeeper].position + new Vector3(3.5f,0,0) , keepersPosition[iKeeper].rotation);
                oc.Init();
                //oc.rd.enabled = true;
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

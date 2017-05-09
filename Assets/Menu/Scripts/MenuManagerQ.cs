using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestDeckLoader;

public class MenuManagerQ : MonoBehaviour {

    [SerializeField]
    GameObject prefabChatox;

    [HideInInspector] public  Dictionary<GameObject, ChatBox> dicPawnChatBox;

    private int cardLevelSelected = -1;
    private string deckOfCardsSelected = string.Empty;
    private List<PawnInstance> listeSelectedKeepers;

    public Transform questDecksPosition;
    public Transform[] keepersPosition;
    public List<Transform> keepersPositionTarget = new List<Transform>();
    public Transform levelDeckPosition;

    public GameObject GoPrefabLevelCard;
    public GameObject GoPrefabEventCard;
    public GameObject GoPrefabQuestCard;
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

        ChatBoxDatabase.Load();
    }

    void Update()
    {

    }

    public void InitEventCards()
    {
      
        for (int i = 0; i < GameManager.Instance.EventDataBase.listEvents.Count; i++)
        {
            if(GameManager.Instance.PersistenceLoader.Pd.dicPersistenceEvents[GameManager.Instance.EventDataBase.listEvents[i].id] == true)
            {
                GameObject goEventCard = Instantiate(GoPrefabEventCard, questDecksPosition);
                goEventCard.transform.localPosition = Vector3.zero;
                goEventCard.GetComponent<MeshFilter>().mesh = GetCardModel(GameManager.Instance.EventDataBase.listEvents[i].cardModelName).GetComponent<MeshFilter>().sharedMesh;
                goEventCard.GetComponent<EventCard>().id = GameManager.Instance.EventDataBase.listEvents[i].id;
            }

        }
    }

    public void InitLevelsCard()
    {
        LevelDataBase leveldb = new LevelDataBase();

        for (int i = 0; i < leveldb.listLevels.Count; i++)
        {
            if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceLevels[leveldb.listLevels[i].id.ToString()] == true)
            {

                // Instanciation des cartes de level
                GameObject goCardLevel = Instantiate(GoPrefabLevelCard, levelDeckPosition);
                goCardLevel.transform.localPosition = Vector3.zero;

                goCardLevel.GetComponent<MeshFilter>().mesh = GetLevelCardModel(leveldb.listLevels[i].cardModelName).GetComponent<MeshFilter>().sharedMesh;
                goCardLevel.GetComponent<CardLevel>().levelIndex = leveldb.listLevels[i].id;

                for (int j = 0; j < leveldb.listLevels[i].listDeckId.Count; j++)    //  Instanciations des decks de quetes
                {
                    if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceDecks[leveldb.listLevels[i].listDeckId[j].ToString()] == true)
                    {

                        GameObject goDeck = Instantiate(GoPrefabDeck, goCardLevel.transform);
                        goDeck.transform.localPosition = Vector3.zero;

                        QuestDeckData qdd = GameManager.Instance.QuestDeckDataBase.GetQuestDeckDataByID(leveldb.listLevels[i].listDeckId[j]);

                        goDeck.GetComponent<MeshFilter>().mesh = GetDeckModel(qdd.deckModelName).GetComponent<MeshFilter>().sharedMesh;

                        goDeck.GetComponent<DeckOfCards>().idQuestDeck = leveldb.listLevels[i].listDeckId[j];

                        // Instantiation de la carte de quete principale
                        GameObject goQuestCard = Instantiate(GoPrefabQuestCard, goDeck.transform);      
                        goQuestCard.transform.localPosition = Vector3.zero;

                        goQuestCard.GetComponent<MeshFilter>().mesh = GetCardModel(qdd.mainQuestCardModel).GetComponent<MeshFilter>().sharedMesh;

                        for (int k = 0; k < qdd.secondaryQuests.Count; k++) // Instantiations des cartes de quete annexe
                        {
                            goQuestCard = Instantiate(GoPrefabQuestCard, goDeck.transform);
                            goQuestCard.transform.localPosition = Vector3.zero;

                            goQuestCard.GetComponent<MeshFilter>().mesh = GetCardModel(qdd.secondaryQuests[k].cardModelname).GetComponent<MeshFilter>().sharedMesh;
                        }
                    }
                }
            }
        }
    }

    public void InitKeepers()
    {
        dicPawnChatBox = new Dictionary<GameObject, ChatBox>();
        int iKeeper = 0;
        foreach (string id in GameManager.Instance.PawnDataBase.DicPawnDataContainer.Keys)
        {
            if (iKeeper > keepersPosition.Length) break;

            if (GameManager.Instance.PawnDataBase.DicPawnDataContainer[id].dicComponentData.ContainsKey(typeof(Behaviour.Keeper)))
            {
                if (GameManager.Instance.PersistenceLoader.Pd.dicPersistencePawns[id] == true) {
                    GameObject goKeeper = GameManager.Instance.PawnDataBase.CreatePawn(id, /*Vector3.zero*/ keepersPosition[iKeeper].position, keepersPosition[iKeeper].rotation, null /*keepersPosition[iKeeper]*/);

                    ChatBox newChatBox = Instantiate(prefabChatox, GetComponentInChildren<Canvas>().transform).GetComponent<ChatBox>();
                    newChatBox.trTarget = goKeeper.transform;
                    newChatBox.SetMode(ChatBox.ChatMode.awaiting);
                    newChatBox.SetEnable(false);
                    dicPawnChatBox.Add(goKeeper, newChatBox);


                    OpenerContent oc = goKeeper.AddComponent<OpenerContent>();

                    int iRandPos = Random.Range(0, keepersPositionTarget.Count);

                    oc.bLocal = false;
                    oc.fSpeed = 5;
                    oc.AddKeyPose(keepersPosition[iKeeper].position, keepersPosition[iKeeper].rotation);

                    oc.AddKeyPose(keepersPosition[iKeeper].position + (keepersPositionTarget[iRandPos].position - keepersPosition[iKeeper].position)/2.0f + new Vector3(0,1,0), keepersPosition[iKeeper].rotation);

                    oc.AddKeyPose(keepersPositionTarget[iRandPos].position, keepersPositionTarget[iRandPos].rotation);

                    keepersPositionTarget.Remove(keepersPositionTarget[iRandPos]);

                    oc.Init();
                    //oc.rd.enabled = true;
                    iKeeper++;
                }
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

    public void SetActiveChatBoxes(bool value)
    {
        foreach (KeyValuePair<GameObject, ChatBox> gc in dicPawnChatBox)
        {
            gc.Value.SetEnable(value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestDeckLoader;
using UnityEngine.UI;

public class MenuManagerQ : MonoBehaviour {

    [SerializeField] GameObject prefabChatox;

    [HideInInspector] public  Dictionary<GameObject, ChatBox> dicPawnChatBox;

    // Utile pour la selection en jeu
    private int cardLevelSelected = -1;
    private string deckOfCardsSelected = string.Empty;
    private List<PawnInstance> listeSelectedKeepers;

    // Postion ou devront aller les perso ou les deck
    public Transform[] keepersPosition;
    public List<Transform> keepersPositionTarget = new List<Transform>();
    public Transform levelDeckPosition;


    //public GameObject GoPrefabLevelCard;
    //public GameObject GoPrefabEventCard;
    //public GameObject GoPrefabQuestCard;
    //public GameObject GoPrefabMainQuestCard;
    //public GameObject GoPrefabSideQuestCard;

    private LevelDataBase leveldb;


    //[SerializeField] public List<GameObject> listLevelCards = new List<GameObject>();

    [Header("Prefab")]
    [SerializeField] GameObject prefabLevelCard;
    [SerializeField] GameObject prefabEventCard;
    [SerializeField] GameObject prefabMainQuestCard;
    [SerializeField] GameObject prefabSideQuestCard;
    [SerializeField] GameObject prefabDeck;

    void Start()
    {
        listeSelectedKeepers = new List<PawnInstance>();
        leveldb = GameManager.Instance.leveldb;

        InitCards();
        InitKeepers();
    }


    public void InitCards()
    {
        //for (int i = 0; i < leveldb.listLevels.Count; i++)
        //{
        //    if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceLevels[leveldb.listLevels[i].id.ToString()] == true)
        //    {
        //        // Instanciation des cartes de level
        //        GameObject goCardLevel = Instantiate(prefabLevelCard, levelDeckPosition);
        //        goCardLevel.transform.localPosition = Vector3.zero;
        //        goCardLevel.GetComponentInChildren<Text>().text = leveldb.listLevels[i].name;
        //        // TODO Maybe add description to level
        //        goCardLevel.GetComponent<CardLevel>().levelIndex = leveldb.listLevels[i].id;

        //        if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceDecks[leveldb.listLevels[i].deckId.ToString()] == true)
        //        {
        //            QuestDeckData qdd = GameManager.Instance.QuestDeckDataBase.GetQuestDeckDataByID(leveldb.listLevels[i].deckId);

        //            GameObject goQuestCard = Instantiate(prefabMainQuestCard, goCardLevel.transform);
        //            goQuestCard.transform.localPosition = Vector3.zero;
        //            goQuestCard.GetComponentInChildren<Text>().text = GameManager.Instance.QuestDeckDataBase.GetDeckByID(leveldb.listLevels[i].deckId).MainQuest;

        //            for (int k = 0; k < qdd.secondaryQuests.Count; k++) // Instantiations des cartes de quete annexe
        //            {
        //                goQuestCard = Instantiate(prefabSideQuestCard, goCardLevel.transform);
        //                goQuestCard.transform.localPosition = Vector3.zero;
        //                goQuestCard.GetComponentInChildren<Text>().text = qdd.secondaryQuests[k].idQuest; // TODO Add real name and description to quests

        //            }


        //            for (int l = 0; l < leveldb.listLevels[i].listEventsId.Count; l++)
        //            {
        //                if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceEvents[GameManager.Instance.EventDataBase.listEvents[l].id] == true)
        //                {
        //                    GameObject goEventCard = Instantiate(prefabEventCard, goCardLevel.transform);
        //                    goEventCard.transform.localPosition = Vector3.zero;
        //                    goEventCard.GetComponent<GlowManagerMenu>().id = leveldb.listLevels[i].listEventsId[l];
        //                    goEventCard.GetComponentInChildren<Text>().text = GameManager.Instance.EventDataBase.GetEventById(leveldb.listLevels[i].listEventsId[l]).id + "\n" +
        //                                                                        GameManager.Instance.EventDataBase.GetEventById(leveldb.listLevels[i].listEventsId[l]).name
        //                                                                        + "\n\n" + GameManager.Instance.EventDataBase.GetEventById(leveldb.listLevels[i].listEventsId[l]).description;
        //                }
        //            }
        //        }
        //    }
        //}
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

                    oc.AddKeyPose(keepersPosition[iKeeper].position + (keepersPositionTarget[iRandPos].position - keepersPosition[iKeeper].position) / 2.0f + new Vector3(0, 1, 0), keepersPosition[iKeeper].rotation);

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

    //public GameObject GetLevelCardModel(string strName)
    //{
    //    foreach (GameObject goCardModel in listLevelCards)
    //    {
    //        if (strName == goCardModel.name)
    //        {
    //            return goCardModel;
    //        }
    //    }
    //    Debug.Log("Modele de carte \"" + strName + "\" introuvable");
    //    return null;
    //}

    //public GameObject GetCardModel(string strName)
    //{
    //    //foreach(GameObject goCardModel in listCardModels)
    //    //{
    //    //    if(strName == goCardModel.name)
    //    //    {
    //    //        return goCardModel;
    //    //    }
    //    //}
    //    //Debug.Log("Modele de carte \"" + strName + "\" introuvable");
    //    //return null;
    //}

    //public GameObject GetDeckModel(string strName)
    //{
    //    //foreach (GameObject goDeckModel in listDeckModels)
    //    //{
    //    //    if (strName == goDeckModel.name)
    //    //    {
    //    //        return goDeckModel;
    //    //    }
    //    //}
    //    //Debug.Log("Modele de deck \"" + strName + "\" introuvable");
    //    //return null;
    //}
    #endregion

    public void SetActiveChatBoxes(bool value)
    {
        foreach (KeyValuePair<GameObject, ChatBox> gc in dicPawnChatBox)
        {
            gc.Value.SetEnable(value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestDeckLoader;
using UnityEngine.UI;
// Temp
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    private LevelDataBase leveldb;
    private MenuUI menuUi;

    private GameObject goDeck;
    private List<GameObject> goCardsLevels;
    private List<GameObject> goCardsInfo;
    private List<List<GameObject>> goCardChildren;

    private Dictionary<GameObject, ChatBox> dicPawnChatBox;

    // Utile pour la selection en jeu
    private int cardLevelSelected = -1;
    private string deckOfCardsSelected = string.Empty;
    private List<PawnInstance> listeSelectedKeepers;

    private bool hasBeenInit;


    [Header("Prefab")]
    [SerializeField]
    GameObject prefabLevelCard;
    [SerializeField]
    GameObject prefabDeck;
    [SerializeField]
    GameObject prefabMainQuestCard;
    [SerializeField]
    GameObject prefabSideQuestCard;
    [SerializeField]
    GameObject prefabEventCard;

    [Header("cardInfo")]
    [SerializeField]
    public List<PawnPrefab> prefabsCardInfo;

    [Header("Chatbox")]
    [SerializeField]
    GameObject prefabChatox;

    private void Awake()
    {
        dicPawnChatBox = new Dictionary<GameObject, ChatBox>();

    }

    void Start()
    {
        listeSelectedKeepers = new List<PawnInstance>();
        goCardChildren = new List<List<GameObject>>();
        goCardsLevels = new List<GameObject>();
        goCardsInfo = new List<GameObject>();
        leveldb = GameManager.Instance.leveldb;
        menuUi = GetComponent<MenuUI>();
        hasBeenInit = false;
        menuUi.UpdateStartButton();
    }

    public void InitCards()
    {
        goDeck = Instantiate(prefabDeck, menuUi.levelDeckPosition);

        for (int i = 0; i < leveldb.listLevels.Count; i++)
        {
            if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceLevels[leveldb.listLevels[i].id.ToString()] == true)
            {
                // Instanciation des cartes de level
                GameObject goCardLevel = Instantiate(prefabLevelCard, goDeck.transform);
                goCardLevel.transform.localPosition = Vector3.zero;
                goCardLevel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = leveldb.listLevels[i].name;
                switch (leveldb.listLevels[i].difficulty){
                    case "easy":
                        goCardLevel.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.green;
                        break;
                    case "hard":
                        goCardLevel.transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Color.red;
                        break;
                    default:
                        break;
                }
                goCardLevel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = leveldb.listLevels[i].difficulty;
                goCardLevel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = leveldb.listLevels[i].nbPawn + " pawns required.";
                // TODO Maybe add description to level
                goCardLevel.GetComponent<CardLevel>().levelIndex = leveldb.listLevels[i].id;
                goCardLevel.SetActive(false);
                goCardsLevels.Add(goCardLevel);

                List<GameObject> cardChildren = new List<GameObject>();

                if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceDecks[leveldb.listLevels[i].deckId.ToString()] == true)
                {
                    QuestDeckData qdd = GameManager.Instance.QuestDeckDataBase.GetQuestDeckDataByID(leveldb.listLevels[i].deckId);

                    GameObject goQuestCard = Instantiate(prefabMainQuestCard, goCardLevel.transform);
                    goQuestCard.transform.localPosition = Vector3.zero;
                    goQuestCard.GetComponentInChildren<Text>().text = "";/* GameManager.Instance.QuestDeckDataBase.GetDeckByID(leveldb.listLevels[i].deckId).DeckName;*/
                    goQuestCard.SetActive(false);
                    cardChildren.Add(goQuestCard);

                    for (int k = 0; k < qdd.secondaryQuests.Count; k++) // Instantiations des cartes de quete annexe
                    {
                        goQuestCard = Instantiate(prefabSideQuestCard, goCardLevel.transform);
                        goQuestCard.transform.localPosition = Vector3.zero;
                        goQuestCard.GetComponentInChildren<Text>().text = "";/*qdd.secondaryQuests[k].idQuest; // TODO Add real name and description to quests*/
                        goQuestCard.SetActive(false);
                        cardChildren.Add(goQuestCard);
                    }


                    for (int l = 0; l < leveldb.listLevels[i].listEventsId.Count; l++)
                    {
                        if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceEvents[GameManager.Instance.EventDataBase.listEvents[l].id] == true)
                        {
                            GameObject goEventCard = Instantiate(prefabEventCard, goCardLevel.transform);
                            goEventCard.transform.localPosition = Vector3.zero;
                            goEventCard.GetComponentInChildren<Text>().text = "";/*GameManager.Instance.EventDataBase.GetEventById(leveldb.listLevels[i].listEventsId[l]).id + "\n" +
                                                                                GameManager.Instance.EventDataBase.GetEventById(leveldb.listLevels[i].listEventsId[l]).description;*/
                            goEventCard.SetActive(false);
                            cardChildren.Add(goEventCard);
                        }
                    }
                }

                GoCardChildren.Add(cardChildren);
            }
        }
    }

    public void InitKeepers()
    {

        int iKeeper = 0;
        foreach (string id in GameManager.Instance.PawnDataBase.DicPawnDataContainer.Keys)
        {
            if (iKeeper > menuUi.keepersPositions.transform.childCount) break;

            if (GameManager.Instance.PawnDataBase.DicPawnDataContainer[id].dicComponentData.ContainsKey(typeof(Behaviour.Keeper)))
            {
                if (GameManager.Instance.PersistenceLoader.Pd.dicPersistencePawns[id] == true)
                {
                    Transform initKeeperTransform = menuUi.keepersPositions.transform.GetChild(iKeeper);
                    GameObject goKeeper = GameManager.Instance.PawnDataBase.CreatePawn(id, Vector3.zero, Quaternion.identity, initKeeperTransform);

                    ChatBox newChatBox = Instantiate(prefabChatox, GetComponentInChildren<Canvas>().transform).GetComponent<ChatBox>();
                    newChatBox.trTarget = goKeeper.transform;
                    newChatBox.SetMode(ChatBox.ChatMode.awaiting);
                    newChatBox.SetEnable(false);
                    dicPawnChatBox.Add(goKeeper, newChatBox);


                    GameObject goCardInfo = Instantiate(getPawnPrefabById(id), menuUi.cardInfoStartingPosition);
                    goCardInfo.transform.localPosition = new Vector3(0, iKeeper * 0.02f, 0);
                    goCardsInfo.Add(goCardInfo);

                    GameManager.Instance.AllKeepersList.Add(goKeeper.GetComponent<PawnInstance>());

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

    public Dictionary<GameObject, ChatBox> DicPawnChatBox
    {
        get
        {
            return dicPawnChatBox;
        }

        set
        {
            dicPawnChatBox = value;
        }
    }

    public LevelDataBase Leveldb
    {
        get
        {
            return leveldb;
        }

        set
        {
            leveldb = value;
        }
    }

    public bool HasBeenInit
    {
        get
        {
            return hasBeenInit;
        }

        set
        {
            hasBeenInit = value;
        }
    }

    public GameObject GoDeck
    {
        get
        {
            return goDeck;
        }

        set
        {
            goDeck = value;
        }
    }

    public List<GameObject> GoCardsLevels
    {
        get
        {
            return goCardsLevels;
        }

        set
        {
            goCardsLevels = value;
        }
    }

    public GameObject PrefabChatox
    {
        get
        {
            return prefabChatox;
        }

        set
        {
            prefabChatox = value;
        }
    }

    public List<GameObject> GoCardsInfo
    {
        get
        {
            return goCardsInfo;
        }

        set
        {
            goCardsInfo = value;
        }
    }

    public List<List<GameObject>> GoCardChildren
    {
        get
        {
            return goCardChildren;
        }

        set
        {
            goCardChildren = value;
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

    public void SetActiveChatBoxes(bool value)
    {
        foreach (KeyValuePair<GameObject, ChatBox> gc in dicPawnChatBox)
        {
            gc.Value.SetEnable(value);
        }
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.buttonClick, 0.5f);

        GameManager.Instance.AllKeepersList.Clear();
        foreach (PawnInstance ki in ListeSelectedKeepers)
        {
            GameManager.Instance.AllKeepersList.Add(ki);
            ki.gameObject.transform.SetParent(GameManager.Instance.transform);

            foreach (KeyValuePair<GameObject, ChatBox> gc in dicPawnChatBox)
            {
                GameObject.Destroy(gc.Value.gameObject);
            }
        }

        GameManager.Instance.DeckSelected = DeckOfCardsSelected;

        if (AudioManager.Instance != null)
        {
            AudioClip toPlay;
            switch (CardLevelSelected)
            {
                case 1:
                    toPlay = AudioManager.Instance.Scene1Clip;
                    break;
                case 2:
                    toPlay = AudioManager.Instance.Scene2Clip;
                    break;
                case 3:
                    toPlay = AudioManager.Instance.Scene3Clip;
                    break;
                case 4:
                    toPlay = AudioManager.Instance.Scene4Clip;
                    break;
                default:
                    toPlay = AudioManager.Instance.menuMusic;
                    break;
            }
            AudioManager.Instance.Fade(toPlay);
        }
        SceneManager.LoadScene(CardLevelSelected);
    }



    public GameObject getPawnPrefabById(string id)
    {
        for (int i = 0; i < prefabsCardInfo.Count; i++)
        {
            if (prefabsCardInfo[i].idPawn == id)
            {
                return prefabsCardInfo[i].prefabPawn;
            }
        }
        return null;
    }
}

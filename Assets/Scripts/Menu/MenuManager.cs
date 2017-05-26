using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestDeckLoader;
using UnityEngine.UI;
using UnityEngine.AI;
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
    private List<string> listeSelectedKeepers;

    private bool hasBeenInit;
    private bool duckhavebringThebox;

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

    // For Lanch Cinematic
    public Transform trVortex;
    public List<GameObject> prefabsMiniatures = new List<GameObject>();
    [HideInInspector] public Miniature currentMiniature = null;
    List<PawnInstance> listPawnSelected = null;
    List<PawnInstance> listPawnJumped = null;
    int nbPawnToWait = 0;
    bool bLauched = false;
    float timerLaunch = 0;

    private void Awake()
    {
        dicPawnChatBox = new Dictionary<GameObject, ChatBox>();
        //duckhavebringThebox = false;
    }
    void Start()
    {
        listeSelectedKeepers = new List<string>();
        goCardChildren = new List<List<GameObject>>();
        goCardsLevels = new List<GameObject>();
        goCardsInfo = new List<GameObject>();
        leveldb = GameManager.Instance.leveldb;
        menuUi = GetComponent<MenuUI>();
        hasBeenInit = false;
        menuUi.UpdateStartButton();
        Cursor.SetCursor(GameManager.Instance.Texture2DUtils.iconeMouse, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if (bLauched)
        {
            updateCinematic();
        }
    }
    public void InitCards()
    {
        goDeck = Instantiate(prefabDeck, menuUi.levelDeckPosition);
        goDeck.transform.localPosition = Vector3.zero;

        for (int i = 0; i < leveldb.listLevels.Count; i++)
        {
            if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceLevels[leveldb.listLevels[i].id.ToString()] == true)
            {
                // Instanciation des cartes de level
                GameObject goCardLevel = Instantiate(prefabLevelCard, goDeck.transform);
                goCardLevel.transform.localPosition = Vector3.zero;
                goCardLevel.transform.localRotation = GoDeck.transform.GetChild(0).rotation;
                goCardLevel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = leveldb.listLevels[i].name;
                switch (leveldb.listLevels[i].difficulty){
                    case "Easy":
                        goCardLevel.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteEasy;
                        break;
                    case "Medium":
                        goCardLevel.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteMedium;
                        break;
                    case "Hard":
                        goCardLevel.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteHard;
                        break;
                    default:
                        goCardLevel.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteTrivial;
                        break;
                }

                goCardLevel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = leveldb.listLevels[i].nbPawn;
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
                    //goQuestCard.GetComponentInChildren<Text>().text = "";/* GameManager.Instance.QuestDeckDataBase.GetDeckByID(leveldb.listLevels[i].deckId).DeckName;*/
                    goQuestCard.SetActive(false);
                    cardChildren.Add(goQuestCard);

                    for (int k = 0; k < qdd.secondaryQuests.Count; k++) // Instantiations des cartes de quete annexe
                    {
                        goQuestCard = Instantiate(prefabSideQuestCard, goCardLevel.transform);
                        goQuestCard.transform.localPosition = Vector3.zero;
              /*          goQuestCard.GetComponentInChildren<Text>().text = ""/*qdd.secondaryQuests[k].idQuest; // TODO Add real name and description to quests*/
                        goQuestCard.SetActive(false);
                        cardChildren.Add(goQuestCard);
                    }

                    for (int l = 0; l < leveldb.listLevels[i].listEventsId.Count; l++)
                    {
                        if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceEvents[GameManager.Instance.EventDataBase.listEvents[l].id] == true)
                        {
                            GameObject goEventCard = Instantiate(prefabEventCard, goCardLevel.transform);
                            goEventCard.transform.localPosition = Vector3.zero;
                        /*    goEventCard.GetComponentInChildren<Text>().text = ""*//*GameManager.Instance.EventDataBase.GetEventById(leveldb.listLevels[i].listEventsId[l]).id + "\n" +
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
                        for (int i = 0; i < menuUi.keepersPositions.transform.childCount; i++)
                        {
                            if (menuUi.keepersPositions.transform.GetChild(i).name == id)
                            {
                                initKeeperTransform = menuUi.keepersPositions.transform.GetChild(i);
                                break;
                            }
                    }
                    GameObject goKeeper = GameManager.Instance.PawnDataBase.CreatePawn(id, Vector3.zero, Quaternion.identity, initKeeperTransform);
                    ChatBox newChatBox = Instantiate(prefabChatox, GetComponentInChildren<Canvas>().transform).GetComponent<ChatBox>();
                    newChatBox.trTarget = goKeeper.transform;
                    newChatBox.SetMode(ChatBox.ChatMode.awaiting);
                    newChatBox.SetEnable(false);
                    dicPawnChatBox.Add(goKeeper, newChatBox);
                    SetActiveChatBoxes(false);

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

    public List<string> ListeSelectedKeepers
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

    public bool DuckhavebringThebox
    {
        get
        {
            return duckhavebringThebox;
        }

        set
        {
            duckhavebringThebox = value;
        }
    }

    public void AddToSelectedKeepers(string pi)
    {
        listeSelectedKeepers.Add(pi);
    }

    public void RemoveFromSelectedKeepers(string pi)
    {
        listeSelectedKeepers.Remove(pi);
    }

    public bool ContainsSelectedKeepers(string pi)
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

    public void LaunchCinematic()
    {
        SetActiveChatBoxes(false);
        //Camera.main.GetComponent<CameraMenu>().ShowMiniature();
        listPawnSelected = new List<PawnInstance>();
        listPawnJumped = new List<PawnInstance>();
        foreach (PawnInstance p in GameObject.FindObjectsOfType<PawnInstance>())
        {
            if (ContainsSelectedKeepers(p.Data.PawnId))
            {
                NavMeshAgent agent = p.GetComponent<NavMeshAgent>();
                agent.enabled = true;
                agent.SetDestination(trVortex.position);
                listPawnSelected.Add(p);
            }
        }
        nbPawnToWait = listPawnSelected.Count;
        menuUi.startButtonImg.gameObject.SetActive(false);
        bLauched = true;
    }

    private void updateCinematic()
    {
        for (int i = 0; i < listPawnSelected.Count; i++)
        {
            if ((listPawnSelected[i].GetComponent<NavMeshAgent>().destination - listPawnSelected[i].transform.position).magnitude < 1.0f)
            {
                listPawnSelected[i].GetComponentInChildren<Animator>().SetTrigger("jumpVortex");
                listPawnJumped.Add(listPawnSelected[i]);
                listPawnSelected.Remove(listPawnSelected[i]);
            }
        }

        for (int i = 0; i < listPawnJumped.Count; i++)
        {
            if (listPawnJumped[i].GetComponent<NavMeshAgent>().remainingDistance < 0.4)
            {
                listPawnJumped.Remove(listPawnJumped[i]);
                nbPawnToWait -= 1;
            }
        }

        if(timerLaunch > 0)
        {

            timerLaunch -= Time.unscaledDeltaTime;
            if(timerLaunch <= 0)
            {
                StartGame();
            }
            else if (timerLaunch <=0.5f)
            {
                currentMiniature.Hide();
            }
        }

        if (nbPawnToWait <= 0 && timerLaunch <= 0)
        {
                timerLaunch = 1.0f;
        }
    }

    public void StartGame()
    {
        Destroy(Camera.main.gameObject.GetComponent<CameraMenu>());
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.buttonClick, 0.5f);

        GameManager.Instance.AllKeeperListId.Clear();
        foreach (string ki in ListeSelectedKeepers)
        {
            GameManager.Instance.AllKeeperListId.Add(ki);

            foreach (KeyValuePair<GameObject, ChatBox> gc in dicPawnChatBox)
            {
                GameObject.Destroy(gc.Value.gameObject);
            }
        }
        Debug.Log(cardLevelSelected);
        if (cardLevelSelected == 1)
        {
            GameManager.Instance.PersistenceLoader.SetPawnUnlocked("swag", false);
            GameManager.Instance.PersistenceLoader.Pd.dicPersistencePawns["swag"] = false;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqfirstmove"] = false;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqtutocombat"] = false;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqmulticharacters"] = false;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqmoraleexplained"] = false;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqlowhunger"] = false;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqlowmorale"] = false;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqashleylowhunger"] = false;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqashleyescort"] = false;
            GameManager.Instance.PersistenceLoader.SetSequenceUnlocked("seqfirstmove", false);
            GameManager.Instance.PersistenceLoader.SetSequenceUnlocked("seqtutocombat", false);
            GameManager.Instance.PersistenceLoader.SetSequenceUnlocked("seqmulticharacters", false);
            GameManager.Instance.PersistenceLoader.SetSequenceUnlocked("seqmoraleexplained", false);
            GameManager.Instance.PersistenceLoader.SetSequenceUnlocked("seqlowhunger", false);
            GameManager.Instance.PersistenceLoader.SetSequenceUnlocked("seqlowmorale", false);
            GameManager.Instance.PersistenceLoader.SetSequenceUnlocked("seqashleylowhunger", false);
            GameManager.Instance.PersistenceLoader.SetSequenceUnlocked("seqashleyescort", false);

            GameManager.Instance.PersistenceLoader.SetLevelUnlocked("4", false);
            GameManager.Instance.PersistenceLoader.SetLevelUnlocked("2", false);
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceLevels["4"] = false;
            GameManager.Instance.PersistenceLoader.Pd.dicPersistenceLevels["2"] = false;
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



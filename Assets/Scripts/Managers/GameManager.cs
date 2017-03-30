using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using QuestSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    #region Debug Variables
    public bool isDebugGameManager;
    #endregion

    private PrisonerInstance prisonerInstanceOld;
    private PawnInstance prisonerInstance;
    private List<KeeperInstance> listOfSelectedKeepersOld = new List<KeeperInstance>();
    private List<PawnInstance> listOfSelectedKeepers= new List<PawnInstance>();

    private Database database = new Database();

    [System.Obsolete]
    private List<KeeperInstance> allKeepersListOld = new List<KeeperInstance>();
    private List<PawnInstance> allKeepersList = new List<PawnInstance>();
    private GameObject goTarget;

    [SerializeField]
    private PrefabUIUtils prefabUtils;
    [SerializeField]
    private SpriteUIUtils spriteUtils;
    [SerializeField]
    private CharactersInitializer characterInitializer;
    [SerializeField]
    private TileManager tileManager;

    private Quest mainQuest;

    private IngameUI ui;
    private IngameScreens gameScreens;

    // quentin Camera
    [SerializeField] CameraManager cameraManager;

    private int nbTurn;

    #region Accessors
    public CameraManager CameraManager
    {
        get
        {
            if (cameraManager == null)
            {
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu")) return null;
                cameraManager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
            }
            return cameraManager;
        }
        set
        {
            cameraManager = value;
        }
    }
    #endregion

    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            database.Init();
           // prisonerInstance = GameObject.Find("Prisoner").GetComponent<PrisonerInstance>();
            nbTurn = 1;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            
        }

        if (isDebugGameManager)
        {
            foreach( KeeperInstance ki in GetComponentsInChildren<KeeperInstance>())
            {
                AllKeepersListOld.Add(ki);
            }
        }


        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // TODO : @Rémi @Rustine : recuperer les positions de départ du prisonnier et des keepers
            //tileManager.Init();
            //Transform[] beginPositionsKeepers = tileManager.beginPositionsKeepers;
            GameObject[] beginPositionsKeepers = new GameObject[allKeepersListOld.Count];
            for (int i = 0; i < allKeepersListOld.Count; i++)
            {
                GameObject beginPositionKeeper = new GameObject();
                beginPositionKeeper.transform.position = Vector3.zero;
                beginPositionsKeepers[i] = beginPositionKeeper;
            }
            //Transform[] beginPositionPrisonnier = tileManager.beginPositionPrisonnier;
            GameObject beginPositionPrisonnier = new GameObject();
            beginPositionPrisonnier.transform.position = Vector3.zero;

            characterInitializer.Init(beginPositionsKeepers, beginPositionPrisonnier);

            foreach(GameObject go in beginPositionsKeepers)
            {
                Destroy(go);
            }
            Destroy(beginPositionPrisonnier);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        List<IQuestObjective> mainObjectives = new List<IQuestObjective>();
        mainObjectives.Add(new PrisonerEscortObjective("Until the end", "Bring Ashley to the end, and ALIVE.", GameObject.FindObjectOfType<Behaviour.Prisoner>().gameObject, TileManager.Instance.EndTile.GetComponent<Tile>()));
        mainQuest = new Quest(new QuestIdentifier(0, gameObject), new QuestText("Main Quest: The last phoque licorne", "", "You're probably wondering why I gathered all of you here today. Well I'll be quick, I want you to bring this wonderful animal to my good and rich friend. Don't worry, you will be rewarded. His name is \"End\", you'll see his flag from pretty far away, head towards it. I'm counting on you, it is extremely important.", "Hint: Don't kill Ashley."), mainObjectives);
    }


    public void ClearListKeeperSelected()
    {
        for (int i = 0; i < listOfSelectedKeepersOld.Count; i++)
        {
            listOfSelectedKeepersOld[i].IsSelected = false;

        }
        listOfSelectedKeepersOld.Clear();
    }

    public void InitializeInGameKeepers()
    {
        foreach (KeeperInstance ki in AllKeepersListOld)
        {
            ki.gameObject.transform.SetParent(transform);
        }
        prisonerInstanceOld.gameObject.transform.SetParent(transform);
                
    }

    public void CheckGameState()
    {
        if (!prisonerInstanceOld.IsAlive)
        {
            Debug.Log("GameOver - Prisoner Died");
            Lose();
        }
        else
        {
            short nbDead = 0;
            foreach (KeeperInstance ki in AllKeepersListOld)
            {
                if (!ki.IsAlive)
                {
                    nbDead++;
                }
            }

            if (nbDead == allKeepersList.Count)
            {
                Debug.Log("GameOver - All Keepers died");
                Lose();
            }
        }

    }

    public void Win()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.winningSound);
        WinScreen.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Lose()
    {
        LoseScreen.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ResetInstance()
    {
        allKeepersList.Clear();
        listOfSelectedKeepersOld.Clear();
        nbTurn = 1;
    }

    /* ------------------------------------------------------------------------------------ */
    /* ------------------------------------- Accessors ------------------------------------ */
    /* ------------------------------------------------------------------------------------ */

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public List<KeeperInstance> ListOfSelectedKeepersOld
    {
        get
        {
            return listOfSelectedKeepersOld;
        }

        set
        {
            listOfSelectedKeepersOld = value;
        }
    }

    public SpriteUIUtils SpriteUtils
    {
        get
        {
            return spriteUtils;
        }

        set
        {
            spriteUtils = value;
        }
    }

    public PrefabUIUtils PrefabUtils
    {
        get
        {
            return prefabUtils;
        }

        set
        {
            prefabUtils = value;
        }
    }

    [System.Obsolete]
    public PrisonerInstance PrisonerInstanceOld
    {
        get
        {
            return prisonerInstanceOld;
        }

        set
        {
            prisonerInstanceOld = value;
        }
    }

    public GameObject GoTarget
    {
        get
        {
            return goTarget;
        }

        set
        {
            goTarget = value;
        }
    }

    public IngameUI Ui
    {
        get
        {
            if(ui == null)
            {
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu")) return null;
                ui = GameObject.Find("IngameUI").GetComponent<IngameUI>();
            }
            return ui;
        }
    }

    public IngameScreens GameScreens
    {
        get
        {
            if (gameScreens == null)
            {
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu")) return null;
                gameScreens = GameObject.Find("IngameScreens").GetComponent<IngameScreens>();
            }
            return gameScreens;
        }
    }

    /// <summary>
    /// Returns the battle results screen
    /// </summary>
    public Transform BattleResultScreen
    {
        get
        {
            if (gameScreens == null)
            {
                gameScreens = GameObject.Find("IngameScreens").GetComponent<IngameScreens>();
            }
            return gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.BattleResultScreens);
        }
    }

    /// <summary>
    /// Returns the select battle characters screen
    /// </summary>
    public Transform SelectBattleCharactersScreen
    {
        get
        {
            if (gameScreens == null)
            {
                gameScreens = GameObject.Find("IngameScreens").GetComponent<IngameScreens>();
            }
            return gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.SelectBattleCharactersScreen);
        }
    }

    public Transform WinScreen
    {
        get
        {
            if (gameScreens == null)
            {
                gameScreens = GameObject.Find("IngameScreens").GetComponent<IngameScreens>();
            }
            return gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.WinScreen);
        }
    }

    public Transform LoseScreen
    {
        get
        {
            if (gameScreens == null)
            {
                gameScreens = GameObject.Find("IngameScreens").GetComponent<IngameScreens>();
            }
            return gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.LoseScreen);
        }
    }

    /// <summary>
    /// Open the selection screen for battle. Takes the tile on which the battle is processed in parameter.
    /// </summary>
    /// <param name="tile">The tile the battle happens on</param>
    public void OpenSelectBattleCharactersScreen(Tile tile)
    {
        Transform screen = SelectBattleCharactersScreen;
        screen.GetComponent<SelectBattleCharactersPanelHandler>().ActiveTile = tile;
        screen.gameObject.SetActive(true);
    }


    public Database Database
    {
        get
        {
            return database;
        }

        set
        {
            database = value;
        }
    }

    public int NbTurn
    {
        get
        {
            return nbTurn;
        }

        set
        {
            nbTurn = value;
        }
    }

    public List<PawnInstance> AllKeepersList
    {
        get
        {
            return allKeepersList;
        }

        set
        {
            allKeepersList = value;
        }
    }

    public List<KeeperInstance> AllKeepersListOld
    {
        get
        {
            return allKeepersListOld;
        }

        set
        {
            allKeepersListOld = value;
        }
    }

    public Quest MainQuest
    {
        get
        {
            return mainQuest;
        }

        set
        {
            mainQuest = value;
        }
    }
    public List<PawnInstance> ListOfSelectedKeepers
    {
        get
        {
            return listOfSelectedKeepers;
        }

        set
        {
            listOfSelectedKeepers = value;
        }
    }

    public PawnInstance PrisonerInstance
    {
        get
        {
            return prisonerInstance;
        }

        set
        {
            prisonerInstance = value;
        }
    }

    public PawnInstance GetFirstSelectedKeeper()
    {
        return listOfSelectedKeepers[0];
    }

    public void AddKeeperToSelectedList(PawnInstance pawn)
    {
        if (pawn.GetComponent<Behaviour.Keeper>() != null)
        {
            ListOfSelectedKeepers.Add(pawn);
        }
        else
        {
            Debug.Log("Can't add a pawn to selected keepers list without the Keeper component.");
        }
    }
}

using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using QuestSystem;
using Behaviour;
using QuestDeckLoader;
using QuestLoader;

public class NewGameManager : MonoBehaviour
{
    private static NewGameManager instance = null;

    #region GameManager children
    [SerializeField]
    private PrefabUIUtils prefabUIUtils;
    [SerializeField]
    private PrefabUtils prefabUtils;
    [SerializeField]
    private SpriteUIUtils spriteUtils;
    [SerializeField]
    private CharactersInitializer characterInitializer;
    // Check with Rémi
       [SerializeField]
        private IngameUI ui;
    #endregion


    #region Debug Variables
    [SerializeField]
    private bool isDebugGameManager;
    #endregion
    
    private Database itemDataBase = new Database();
    private PawnDatabase pawnDataBase = new PawnDatabase();
    private QuestDeckDatabase questDeckDataBase = new QuestDeckDatabase();
    private QuestDatabase questDataBase = new QuestDatabase();

    private TileManager tileManagerReference;
    private CameraManager cameraManagerReference;
    private IngameScreens gameScreens;

    private List<PawnInstance> allKeepersList = new List<PawnInstance>();
    private List<PawnInstance> listOfSelectedKeepers = new List<PawnInstance>();
    private GameObject goTarget;
    private PawnInstance prisonerInstance;

    private int nbTurn;

    private IngameUI ui;
    private IngameScreens gameScreens;

    // quentin Camera
    [SerializeField]
    private CameraManager cameraManager;
    [SerializeField]
    private CharactersInitializer characterInitializer;
    [SerializeField]
    private TileManager tileManager;

    private Quest mainQuest;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // InitAllDatabase
            itemDataBase.Init();
            pawnDataBase.Init();
            questDeckDataBase.Init();
            questDataBase.Init();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (isDebugGameManager)
        {
            foreach (Keeper k in GetComponentsInChildren<Keeper>())
            {
                AllKeepersList.Add(k.GetComponent<PawnInstance>());
            }
        }

        ResetInstance();
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            NewGame();
        }

        DontDestroyOnLoad(gameObject);
    }


    public void NewGame()
    {

        // I NEED A QUEST INITIALIZER
        List<IQuestObjective> mainObjectives = new List<IQuestObjective>();
        mainObjectives.Add(new PrisonerEscortObjective("Until the end", "Bring Ashley to the end, and ALIVE.", GameObject.FindObjectOfType<Behaviour.Prisoner>().gameObject, TileManager.Instance.EndTile.GetComponent<Tile>()));
        mainQuest = new Quest(new QuestIdentifier(0, gameObject), new QuestText("Main Quest: The last phoque licorne", "", "You're probably wondering why I gathered all of you here today. Well I'll be quick, I want you to bring this wonderful animal to my good and rich friend. Don't worry, you will be rewarded. His name is \"End\", you'll see his flag from pretty far away, head towards it. I'm counting on you, it is extremely important.", "Hint: Don't kill Ashley."), mainObjectives);


        // TODO this should not be handled like, especially if there is more prisoner in scene
        prisonerInstance = FindObjectOfType<Behaviour.Prisoner>().GetComponent<PawnInstance>();

        tileManager.Init();
        Transform[] beginPositionsKeepers = tileManager.GetBeginPositions;

        // TODO: retrieve position using main quest 
        GameObject beginPositionPrisonnier = new GameObject();
        beginPositionPrisonnier.transform.position = Vector3.zero;

        characterInitializer.Init(beginPositionsKeepers, beginPositionPrisonnier);

        Destroy(beginPositionPrisonnier);
    }


    public void CheckGameState()
    {
        short nbDead = 0;
        short nbImmortal = 0;
        foreach (PawnInstance pi in AllKeepersList)
        {

            if (pi.GetComponent<Mortal>() == null)
            {
                nbImmortal++;
            }
            else
            {
                if (!pi.GetComponent<Mortal>().IsAlive)
                {
                    nbDead++;
                }
            }
        }

        if (nbDead == allKeepersList.Count - nbImmortal)
        {
            Debug.Log("GameOver - All Keepers died");
            Lose();
        }
        //}

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
        listOfSelectedKeepers.Clear();
        nbTurn = 1;
    }

    public PawnInstance GetFirstSelectedKeeper()
    {
        return listOfSelectedKeepers[0];
    }

    #region Accessors
    public static NewGameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public bool IsDebugGameManager
    {
        get
        {
            return isDebugGameManager;
        }
        set
        {
            isDebugGameManager = value;
        }
    }

    public QuestDeckDatabase QuestDeckDataBase
    {
        get
        {
            return questDeckDataBase;
        }

        set
        {
            questDeckDataBase = value;
        }
    }

    public QuestDatabase QuestDataBase
    {
        get
        {
            return questDataBase;
        }

        set
        {
            questDataBase = value;
        }
    }

    public Database ItemDataBase
    {
        get
        {
            return itemDataBase;
        }

        set
        {
            itemDataBase = value;
        }
    }

    public PawnDatabase PawnDataBase
    {
        get
        {
            return pawnDataBase;
        }

        set
        {
            pawnDataBase = value;
        }
    }

    public SpriteUIUtils SpriteUtils
    {
        get
        {
            return spriteUtils;
        }
    }

    public PrefabUIUtils PrefabUIUtils
    {
        get
        {
            return prefabUIUtils;
        }
    }

    public PrefabUtils PrefabUtils
    {
        get
        {
            return prefabUtils;
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
            if (ui == null)
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

    public void OpenSelectBattleCharactersScreen(Tile tile)
    {
        Transform screen = SelectBattleCharactersScreen;
        screen.GetComponent<SelectBattleCharactersPanelHandler>().ActiveTile = tile;
        screen.gameObject.SetActive(true);
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
    #endregion

    #region Registrations
    // Called once during initialization, launch next step
    public void RegisterTileManager(TileManager _tileManager)
    {
        tileManagerReference = _tileManager;
        ui.gameObject.SetActive(true);

        // Next step, init keepers        
        characterInitializer.InitKeepers(tileManagerReference.GetBeginPositions);
    }

    public void RegisterCameraManager(CameraManager _cameraManager)
    {
        cameraManagerReference = _cameraManager;
    }

    public void RegisterGameScreens(IngameScreens _gameScreens)
    {
        gameScreens = _gameScreens;
    }
    #endregion

    #region Camera facade
    public void UpdateCameraPosition()
    {
        cameraManagerReference.UpdateCameraPosition();
    }
    #endregion

    #region TileManager facade
    public void RegisterMonsterPosition(PawnInstance _monster)
    {
        tileManagerReference.AddMonsterOnTile(_monster);
    }
    #endregion
}

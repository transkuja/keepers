using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    #region Debug Variables
    public bool isDebugGameManager;
    #endregion

    private PrisonerInstance prisonerInstance;
    private List<KeeperInstance> listOfSelectedKeepers = new List<KeeperInstance>();


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
            prisonerInstance = GameObject.Find("Prisoner").GetComponent<PrisonerInstance>();
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
            GameObject[] beginPositionsKeepers = new GameObject[allKeepersList.Count];
            for (int i = 0; i < allKeepersList.Count; i++)
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


    public void ClearListKeeperSelected()
    {
        for (int i = 0; i < listOfSelectedKeepers.Count; i++)
        {
            listOfSelectedKeepers[i].IsSelected = false;

        }
        listOfSelectedKeepers.Clear();
    }

    public void InitializeInGameKeepers()
    {
        foreach (KeeperInstance ki in AllKeepersListOld)
        {
            ki.gameObject.transform.SetParent(transform);
        }
        prisonerInstance.gameObject.transform.SetParent(transform);
                
    }

    public void CheckGameState()
    {
        if (!prisonerInstance.IsAlive)
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
        listOfSelectedKeepers.Clear();
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

    public List<KeeperInstance> ListOfSelectedKeepers
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

    public PrisonerInstance PrisonerInstance
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

}

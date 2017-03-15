using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Boomlagoon.JSON;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private List<KeeperInstance> listOfSelectedKeepers = new List<KeeperInstance>();
    private GameObject goTarget;

    public bool isDebugGameManager;


    public GameObject prefabItemToDrop;

    // TODO: move to ItemManager
    private Database database = new Database();

    private List<KeeperInstance> allKeepersList = new List<KeeperInstance>();
    private PrisonerInstance prisonerInstance;

    private bool characterPanelIngameNeedUpdate = false;
    private bool shortcutPanel_NeedUpdate = true;
    private bool characterPanelMenuNeedUpdate = false;


    private IngameUI ui;
    private IngameScreens gameScreens;

    // quentin Camera
    [SerializeField] CameraManager cameraManager;

    #region Accessors
    public CameraManager CameraManager
    {
        get
        {
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
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            
        }

        if (isDebugGameManager)
        {
            foreach( KeeperInstance ki in GetComponentsInChildren<KeeperInstance>())
            {
                allKeepersList.Add(ki);
            }
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
        foreach (KeeperInstance ki in allKeepersList)
        {
            ki.gameObject.transform.SetParent(transform);
        }
                
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

    public bool SelectedKeeperNeedUpdate
    {
        get
        {
            return characterPanelIngameNeedUpdate;
        }

        set
        {
            characterPanelIngameNeedUpdate = value;
        }
    }

    public bool CharacterPanelMenuNeedUpdate
    {
        get
        {
            return characterPanelMenuNeedUpdate;
        }

        set
        {
            characterPanelMenuNeedUpdate = value;
        }
    }

    public List<KeeperInstance> AllKeepersList
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


    public bool ShortcutPanel_NeedUpdate
    {
        get
        {
            return shortcutPanel_NeedUpdate;
        }

        set
        {
            shortcutPanel_NeedUpdate = value;
        }
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

    public void CheckGameState()
    {
        if(!prisonerInstance.IsAlive)
        {
            Debug.Log("GameOver - Prisoner Died");
            Lose();
        }
        else
        {
            short nbDead = 0;
            foreach (KeeperInstance ki in allKeepersList)
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
        WinScreen.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Lose()
    {
        LoseScreen.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}

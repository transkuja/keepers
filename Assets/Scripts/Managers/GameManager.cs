using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using QuestSystem;
using Behaviour;
using QuestDeckLoader;
using QuestLoader;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    #region GameManager children
    [SerializeField]
    private PrefabUIUtils prefabUIUtils;
    [SerializeField]
    private PrefabUtils prefabUtils;
    [SerializeField]
    private SpriteUIUtils spriteUtils;
    [SerializeField]
    private CharactersInitializer characterInitializer;
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

    private Quest mainQuest;

    private int nbTurn = 1;

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

        ResetInstance();
        if (isDebugGameManager)
        {
            foreach (Keeper k in GetComponentsInChildren<Keeper>())
            {
                AllKeepersList.Add(k.GetComponent<PawnInstance>());
            }
        }

        DontDestroyOnLoad(gameObject);
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
    public static GameManager Instance
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
            return ui;
        }
    }

    public IngameScreens GameScreens
    {
        get
        {
            return gameScreens;
        }
    }

    public Transform BattleResultScreen
    {
        get
        {
            return gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.BattleResultScreens);
        }
    }

    public Transform SelectBattleCharactersScreen
    {
        get
        {
            return gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.SelectBattleCharactersScreen);
        }
    }

    public Transform WinScreen
    {
        get
        {
            return gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.WinScreen);
        }
    }

    public Transform LoseScreen
    {
        get
        {
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
        UpdateCameraPosition(tileManagerReference.BeginTile);
    }

    public void RegisterGameScreens(IngameScreens _gameScreens)
    {
        gameScreens = _gameScreens;
    }
    #endregion

    #region Camera facade
    public void UpdateCameraPosition(PawnInstance cameraTarget)
    {
        cameraManagerReference.UpdateCameraPosition(cameraTarget);
    }

    public void UpdateCameraPosition(Tile cameraTarget)
    {
        cameraManagerReference.UpdateCameraPosition(cameraTarget);
    }

    public float CameraFZoomLerp
    {
        get
        {
            return cameraManagerReference.FZoomLerp;
        }
    }

    public CameraManager CameraManagerReference
    {
        get
        {
            return cameraManagerReference;
        }

        set
        {
            cameraManagerReference = value;
        }
    }
    #endregion

    #region TileManager facade
    public void RegisterMonsterPosition(PawnInstance _monster)
    {
        tileManagerReference.AddMonsterOnTile(_monster);
    }

    public void RegisterKeeperPosition(PawnInstance _keeper)
    {
        tileManagerReference.AddKeeperOnTile(tileManagerReference.BeginTile, _keeper);
    }

    public void RegisterPrisoner(PawnInstance _prisoner)
    {
        prisonerInstance = _prisoner;
        tileManagerReference.RegisterPrisonerPosition(_prisoner);
    }
    #endregion

    public void ClearListKeeperSelected()
    {
        for (int i = 0; i < listOfSelectedKeepers.Count; i++)
        {
            listOfSelectedKeepers[i].GetComponent<Keeper>().IsSelected = false;
        }
        listOfSelectedKeepers.Clear();
    }

    public void AddKeeperToSelectedList(PawnInstance pawn)    {
        if (pawn.GetComponent<Behaviour.Keeper>() != null)
            ListOfSelectedKeepers.Add(pawn);
        else
            Debug.Log("Can't add a pawn to selected keepers list without the Keeper component.");
    }

    public GameObject GetBattleUI()
    {
        return ui.battleUI;
    }
}

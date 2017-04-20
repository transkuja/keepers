using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

using QuestSystem;
using Behaviour;
using QuestDeckLoader;
using QuestLoader;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private GameState currentState;
    #region GameManager children
    [SerializeField]
    private PrefabUIUtils prefabUIUtils;
    [SerializeField]
    private PrefabUtils prefabUtils;
    [SerializeField]
    private SpriteUIUtils spriteUtils;
    [SerializeField]
    private Texture2DUtils texture2DUtils;
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
    private Interactable goTarget;
    private PawnInstance prisonerInstance;

    private Quest mainQuest;

    private int nbTurn = 1;

    // Change game state variables
    private List<NavMeshAgent> pausedAgents = new List<NavMeshAgent>();
    private List<NavMeshAgent> disabledAgents = new List<NavMeshAgent>();
    private List<GameObject> disabledModels = new List<GameObject>();
    private PawnInstance[] currentFighters;

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
        currentState = GameState.InPause;
    }

    public void Lose()
    {
        LoseScreen.gameObject.SetActive(true);
        currentState = GameState.InPause;
    }

    public void ResetInstance()
    {
        allKeepersList.Clear();
        listOfSelectedKeepers.Clear();
        nbTurn = 1;
        currentState = GameState.Normal;
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

    public Interactable GoTarget
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


    public GameState CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            // Exit pause state
            if (currentState == GameState.InPause && value != GameState.InPause)
                ExitPauseStateProcess();
            // Exit battle state
            if (currentState == GameState.InBattle && value == GameState.Normal)
                ExitBattleStateProcess();
            // Enter pause state
            if (value == GameState.InPause && currentState == GameState.Normal)
                SwitchToPauseStateProcess();
            // Enter battle state
            if (value == GameState.InBattle && currentState == GameState.InPause)
            {
                SwitchToBattleStateProcess();
            }

            currentState = value;      
        }
    }

    public GameObject GetBattleUI
    {
        get
        {
            return ui.battleUI;
        }
    }

    public Tile ActiveTile
    {
        get
        {
            return cameraManagerReference.ActiveTile;
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

    public Texture2DUtils Texture2DUtils
    {
        get
        {
            return texture2DUtils;
        }
    }

    public void RegisterGreyTileCameraAdapter(GreyTileCameraAdapter _cameraAdapter)
    {
        cameraManagerReference.greyTileCameraAdapters.Add(_cameraAdapter);
    }
    public void RegisterSelectionPointerCameraAdapter(SelectionPointerCameraAdapter _cameraAdapter)
    {
        cameraManagerReference.selectionPointerCameraAdapters.Add(_cameraAdapter);
    }
    public void RegisterWorldspaceCanvasCameraAdapter(WorldspaceCanvasCameraAdapter _cameraAdapter)
    {
        cameraManagerReference.worldspaceCanvasCameraAdapters.Add(_cameraAdapter);
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

    #region Game states switch functions
    private void SwitchToPauseStateProcess()
    {     
        // Pause keepers
        foreach (PawnInstance pi in allKeepersList)
        {
            NavMeshAgent currentAgent = pi.GetComponent<NavMeshAgent>();
            if (currentAgent != null && currentAgent.isActiveAndEnabled)
            {
                currentAgent.Stop();
                pausedAgents.Add(currentAgent);
            }
        }

        // Pause NPCs
        // If needed, we should register all PNJ on tiles in TileManager so we can handle AI behaviours when the game paused
        // For now we'll only deal with the prisoner
        if (prisonerInstance != null)
        {
            NavMeshAgent prisonerAgent = prisonerInstance.GetComponent<NavMeshAgent>();
            if (prisonerAgent != null && prisonerAgent.isActiveAndEnabled)
            {
                prisonerAgent.Stop();
                pausedAgents.Add(prisonerAgent);
            }
        }

        // Pause monsters
        foreach (Tile tile in tileManagerReference.MonstersOnTile.Keys)
        {
            List<PawnInstance> monsterList = tileManagerReference.MonstersOnTile[tile];
            foreach (PawnInstance pi in monsterList)
            {
                NavMeshAgent currentAgent = pi.GetComponent<NavMeshAgent>();
                if (currentAgent != null && currentAgent.isActiveAndEnabled)
                {
                    currentAgent.Stop();
                    pausedAgents.Add(currentAgent);
                }
            }
        }

    }

    private void ExitPauseStateProcess()
    {
        foreach (NavMeshAgent agent in pausedAgents)
            agent.Resume();
        pausedAgents.Clear();
    }

    public void SetStateToInBattle(PawnInstance[] _fighters)
    {
        currentFighters = _fighters;
        ClearListKeeperSelected();
        CurrentState = GameState.InBattle;
    }

    private void SwitchToBattleStateProcess()
    {
        // Pause keepers
        foreach (PawnInstance pi in allKeepersList)
        {
            bool mustBeDisabled = false;
            for (int i = 0; i < currentFighters.Length && i < 3; i++)
            {
                if (pi == currentFighters[i])
                {
                    mustBeDisabled = true;
                }
            }

            if (mustBeDisabled)
            {
                disabledAgents.Add(pi.GetComponent<NavMeshAgent>());
                pi.GetComponent<NavMeshAgent>().enabled = false;
            }
            else
            {
                NavMeshAgent currentAgent = pi.GetComponent<NavMeshAgent>();
                if (currentAgent != null && currentAgent.isActiveAndEnabled)
                {
                    currentAgent.Stop();
                    pausedAgents.Add(currentAgent);
                    pi.transform.GetChild(0).gameObject.SetActive(false);
                    disabledModels.Add(pi.transform.GetChild(0).gameObject);
                }
            }

            pi.GetComponent<Keeper>().ShowSelectedPanelUI(false);
            ui.ClearActionPanel();
            ui.HideInventoryPanels();
        }

        // Pause NPCs
        // If needed, we should register all PNJ on tiles in TileManager so we can handle AI behaviours when the game paused
        // For now we'll only deal with the prisoner
        if (currentFighters[currentFighters.Length - 1].GetComponent<Prisoner>() != null)
        {
            NavMeshAgent prisonerAgent = currentFighters[currentFighters.Length - 1].GetComponent<NavMeshAgent>();
            if (prisonerAgent != null && prisonerAgent.isActiveAndEnabled)
            {
                disabledAgents.Add(prisonerAgent);
                prisonerAgent.enabled = false;
            }
        }
        else
        {
            if (prisonerInstance != null)
            {
                NavMeshAgent prisonerAgent = prisonerInstance.GetComponent<NavMeshAgent>();
                if (prisonerAgent != null && prisonerAgent.isActiveAndEnabled)
                {
                    prisonerAgent.Stop();
                    pausedAgents.Add(prisonerAgent);
                }
            }
        }

        // Pause monsters
        foreach (Tile tile in tileManagerReference.MonstersOnTile.Keys)
        {
            if (tile != cameraManagerReference.ActiveTile)
            {
                List<PawnInstance> monsterList = tileManagerReference.MonstersOnTile[tile];
                foreach (PawnInstance pi in monsterList)
                {
                    NavMeshAgent currentAgent = pi.GetComponent<NavMeshAgent>();
                    if (currentAgent != null && currentAgent.isActiveAndEnabled)
                    {
                        currentAgent.Stop();
                        pausedAgents.Add(currentAgent);
                    }
                }
            }
            else
            {
                List<PawnInstance> monsterList = tileManagerReference.MonstersOnTile[tile];
                foreach (PawnInstance pi in monsterList)
                {
                    NavMeshAgent currentAgent = pi.GetComponent<NavMeshAgent>();
                    if (currentAgent != null && currentAgent.isActiveAndEnabled)
                    {
                        disabledAgents.Add(currentAgent);
                        currentAgent.enabled = false;
                    }
                }
            }            
        }

    }

    private void ExitBattleStateProcess()
    {
        foreach (NavMeshAgent agent in pausedAgents)
            agent.Resume();
        pausedAgents.Clear();

        foreach (NavMeshAgent agent in disabledAgents)
            agent.enabled = true;
        disabledAgents.Clear();

        foreach (GameObject go in disabledModels)
            go.SetActive(true);
        disabledModels.Clear();
    }


    #endregion

}

public enum GameState
{
    Normal,
    InBattle,
    InPause
}

using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

using QuestSystem;
using Behaviour;
using QuestDeckLoader;

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

    [SerializeField]
    private QuestManager questManagerReference;

    #endregion
    #region Debug Variables

    [SerializeField]
    private bool isDebugGameManager;

    #endregion

    private Database itemDataBase = new Database();
    private PawnDatabase pawnDataBase = new PawnDatabase();
    private QuestDeckDatabase questDeckDataBase = new QuestDeckDatabase();
    private EventDataBase eventDataBase = new EventDataBase();
    private PersistenceLoader persistenceLoader = new PersistenceLoader();
    public LevelDataBase leveldb;


    private QuestsContainer questsContainer = new QuestsContainer();
    private QuestSourceContainer questSources;

    private TileManager tileManagerReference;
    private CameraManager cameraManagerReference;
    private IngameScreens gameScreens;
    private List<PawnInstance> allKeepersList = new List<PawnInstance>();
    private List<PawnInstance> listOfSelectedKeepers = new List<PawnInstance>();

    private List<string> listEventSelected = new List<string>();
    private string deckSelected = string.Empty;

    private Interactable goTarget;
    private PawnInstance prisonerInstance;
    private PawnInstance archerInstance;
    private int nbTurn = 1;

    // Change game state variables
    private List<NavMeshAgent> pausedAgents = new List<NavMeshAgent>();
    private List<GameObject> disabledModels = new List<GameObject>();
    private List<GlowObjectCmd> unregisteredGlows = new List<GlowObjectCmd>();
    private PawnInstance[] currentFighters;
    private List<GameObject> tilePortalsDisabled = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // InitAllDatabase
            itemDataBase.Init();
            pawnDataBase.Init();
            eventDataBase.Init();
            questDeckDataBase.Init();
            questsContainer.Init();
            instance.leveldb = new LevelDataBase();
            ChatBoxDatabase.Load();
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

        if (!prisonerInstance.GetComponent<Mortal>().IsAlive)
        {
            Debug.Log("GameOver - The prisoner is dead");
            Lose();
        }
    }

    public void Win()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.winningSound, 0.05f);
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
        if( SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Menu new"))
        {
            instance.listOfSelectedKeepers.Clear();

        }
        else
        {
            instance.deckSelected = string.Empty;
            instance.listEventSelected.Clear();
            instance.allKeepersList.Clear();
            persistenceLoader.Load();
            instance.archerInstance = null;
        }

        instance.nbTurn = 1;
        GameManager.instance.Ui.UpdateDay();
        instance.currentState = GameState.Normal;
        instance.ui.gameObject.SetActive(false);
        instance.ui.ResetIngameUI();
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

    public Transform OptionsScreen
    {
        get
        {
            return gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.OptionsMenu);
        }
    }

    public void OpenSelectBattleCharactersScreen(Tile tile)
    {
        Transform screen = SelectBattleCharactersScreen;
        screen.GetComponent<SelectBattleCharactersPanelHandler>().ActiveTile = tile;
        screen.gameObject.SetActive(true);
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
            if (currentState == GameState.InBattle && (value == GameState.Normal || value == GameState.InPause))
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

    public CharactersInitializer CharacterInitializer
    {
        get
        {
            return characterInitializer;
        }

        set
        {
            characterInitializer = value;
        }
    }

    #endregion

    #region Registrations
    // Called once during initialization, launch next step
    public void RegisterTileManager(TileManager _tileManager)
    {
        tileManagerReference = _tileManager;
        ui.gameObject.SetActive(true);
        // Next step, init quests     
        RegisterQuestSourceContainer(tileManagerReference.GetComponent<QuestSourceContainer>());
        InitQuests();
    }

    void InitQuests()
    {
        // Give choosen deck ID here
        questManagerReference.Init(GameManager.Instance.DeckSelected);


        // Next step, init keepers 
        characterInitializer.InitKeepers(tileManagerReference.GetBeginPositions);
    }

    public void RegisterCameraManager(CameraManager _cameraManager)
    {
        cameraManagerReference = _cameraManager;
        UpdateCameraPosition(tileManagerReference.BeginTile);
        Ui.GoActionPanelQ.GetComponentInParent<WorldspaceCanvasCameraAdapter>().Init();
    }

    public void RegisterGameScreens(IngameScreens _gameScreens)
    {
        gameScreens = _gameScreens;
    }

    public void RegisterQuestSourceContainer(QuestSourceContainer container)
    {
        questSources = container;
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



    public QuestsContainer QuestsContainer

    {

        get

        {

            return questsContainer;

        }



        set

        {

            questsContainer = value;

        }

    }



    public QuestManager QuestManager

    {

        get

        {

            return questManagerReference;

        }



        set

        {

            questManagerReference = value;

        }

    }



    public QuestSourceContainer QuestSources

    {

        get

        {

            return questSources;

        }



        set

        {

            questSources = value;

        }

    }

    public PawnInstance[] CurrentFighters
    {
        get
        {
            return currentFighters;
        }

        set
        {
            currentFighters = value;
        }
    }

    public EventDataBase EventDataBase
    {
        get
        {
            return eventDataBase;
        }

        set
        {
            eventDataBase = value;
        }
    }

    public List<string> ListEventSelected
    {
        get
        {
            return listEventSelected;
        }

        set
        {
            listEventSelected = value;
        }
    }

    public string DeckSelected
    {
        get
        {
            return deckSelected;
        }

        set
        {
            deckSelected = value;
        }
    }

    public PersistenceLoader PersistenceLoader
    {
        get
        {
            return persistenceLoader;
        }

        set
        {
            persistenceLoader = value;
        }
    }

    public PawnInstance ArcherInstance
    {
        get
        {
            return archerInstance;
        }

        set
        {
            archerInstance = value;
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

    public void UnregisterSelectionPointerCameraAdapter(SelectionPointerCameraAdapter _cameraAdapter)
    {
        cameraManagerReference.selectionPointerCameraAdapters.Remove(_cameraAdapter);
    }

    public void RegisterWorldspaceCanvasCameraAdapter(WorldspaceCanvasCameraAdapter _cameraAdapter)
    {
        if(cameraManagerReference!=null)
        cameraManagerReference.worldspaceCanvasCameraAdapters.Add(_cameraAdapter);
    }

    public void UnregisterWorldspaceCanvasCameraAdapter(WorldspaceCanvasCameraAdapter _cameraAdapter)
    {
        if (cameraManagerReference != null)
            cameraManagerReference.worldspaceCanvasCameraAdapters.Remove(_cameraAdapter);
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

    public List<PawnInstance> GetKeepersOnTile(Tile _tile)
    {
        return tileManagerReference.KeepersOnTile[_tile];
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

    public void AddKeeperToSelectedList(PawnInstance pawn)

    {
        if (pawn.GetComponent<Behaviour.Keeper>() != null)
            ListOfSelectedKeepers.Add(pawn);
        else
            Debug.Log("Can't add a pawn to selected keepers list without the Keeper component.");
    }

    public void ShowMainQuest()
    {
        gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.MainQuestPanel).gameObject.SetActive(true);
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
        {
            if (agent != null && agent.isActiveAndEnabled)
                agent.Resume();
        }
        pausedAgents.Clear();
    }

    public void SetStateToInBattle(PawnInstance[] _fighters)
    {
        currentFighters = _fighters;
        CurrentState = GameState.InBattle;
        ClearListKeeperSelected();
        cameraManagerReference.UpdateCameraPosition(tileManagerReference.CameraPositionForBattle.localPosition);
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
                pi.GetComponent<NavMeshAgent>().enabled = false;
            else
            {
                NavMeshAgent currentAgent = pi.GetComponent<NavMeshAgent>();
                if (currentAgent != null && currentAgent.isActiveAndEnabled)
                {
                    currentAgent.Stop();
                    pausedAgents.Add(currentAgent);
                    pi.transform.GetChild(0).gameObject.SetActive(false);
                    disabledModels.Add(pi.transform.GetChild(0).gameObject);
                    if (pi.GetComponent<GlowObjectCmd>() != null)
                    {
                        GlowController.UnregisterObject(pi.GetComponent<GlowObjectCmd>());
                        unregisteredGlows.Add(pi.GetComponent<GlowObjectCmd>());
                    }
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
                prisonerAgent.enabled = false;
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
                    if (currentAgent != null)
                    {
                        pi.GetComponent<AnimatedPawn>().WasAgentActiveBeforeBattle = currentAgent.isActiveAndEnabled;
                        if (currentAgent.isActiveAndEnabled)
                            currentAgent.enabled = false;                         
                    }
                    if (pi.GetComponent<GlowObjectCmd>() != null)
                    {
                        GlowController.RegisterObject(pi.GetComponent<GlowObjectCmd>());
                    }

                    if (pi.GetComponentInChildren<AggroBehaviour>() != null)
                        pi.GetComponentInChildren<AggroBehaviour>().gameObject.SetActive(false);
                }
            }
        }

        // Mask tile portals
        Transform tilePortals = currentFighters[0].CurrentTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.PortalTriggers);
        for (int i = 0; i < tilePortals.childCount; i++)
        {
            if (tilePortals.GetChild(i).gameObject.activeSelf)
            {
                tilePortalsDisabled.Add(tilePortals.GetChild(i).gameObject);
                tilePortals.GetChild(i).gameObject.SetActive(false);
            }
        }

        // Mask quest reminder button
        gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.QuestReminderButton).gameObject.SetActive(false);

        Transform tileModel = ActiveTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.Model);
        for (int i = 0; i < tileModel.childCount; i++)
        {
            if (tileModel.GetChild(i).name.Equals("Center"))
            {
                tileModel.GetChild(i).gameObject.SetActive(false);
                disabledModels.Add(tileModel.GetChild(i).gameObject);
            }
        }

        // Make escortable disappear
        if (TileManager.Instance.EscortablesOnTile.ContainsKey(ActiveTile))
        {
            foreach (PawnInstance pi in TileManager.Instance.EscortablesOnTile[ActiveTile])
            {
                pi.gameObject.SetActive(false);
                disabledModels.Add(pi.gameObject);
            }
        }

        // Hide item instance
        for (int i = 1; i < ActiveTile.transform.childCount; i++)
        {
            ItemInstance curItem = ActiveTile.transform.GetChild(i).GetComponentInChildren<ItemInstance>();
            if (curItem != null)
            {
                curItem.transform.parent.gameObject.SetActive(false);
                disabledModels.Add(curItem.transform.parent.gameObject);
            }

        }

        Ui.GoActionPanelQ.transform.parent.SetParent(Ui.transform);
    }

    private void ExitBattleStateProcess()
    {
        foreach (NavMeshAgent agent in pausedAgents)
        {
            if (agent != null && agent.isActiveAndEnabled)
                agent.Resume();
        }
        pausedAgents.Clear();

        foreach (GameObject go in disabledModels)
            go.SetActive(true);
        disabledModels.Clear();

        foreach (GlowObjectCmd goc in unregisteredGlows)
            GlowController.RegisterObject(goc);
        unregisteredGlows.Clear();

        // Prevents monster agression when returning to normal state
        foreach (PawnInstance pi in tileManagerReference.KeepersOnTile[ActiveTile])
        {
            if (pi.GetComponent<Fighter>() != null)
                pi.GetComponent<Fighter>().IsTargetableByMonster = false;
        }
        if (prisonerInstance.CurrentTile == ActiveTile)
        {
            if (prisonerInstance.GetComponent<Fighter>() != null)
                prisonerInstance.GetComponent<Fighter>().IsTargetableByMonster = false;
        }

        // Reactivate tile portals
        foreach (GameObject go in tilePortalsDisabled)
            go.SetActive(true);
        tilePortalsDisabled.Clear();

        // Enable quest reminder button
        gameScreens.transform.GetChild(0).GetChild((int)IngameScreensEnum.QuestReminderButton).gameObject.SetActive(true);

        cameraManagerReference.UpdateCameraPositionExitBattle();
    }
    #endregion
}

public enum GameState
{
    Normal,
    InBattle,
    InPause,
    InTuto
}
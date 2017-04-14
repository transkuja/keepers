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

        if (IsDebugGameManager)
        {
            // HERE if is not gameManager
        }
        DontDestroyOnLoad(gameObject);
    }

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

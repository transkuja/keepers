using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using QuestSystem;
using Behaviour;

public class NewGameManager : MonoBehaviour
{
    private static NewGameManager instance = null;

    [SerializeField]
    private PrefabUIUtils prefabUtils;
    [SerializeField]
    private SpriteUIUtils spriteUtils;



    #region Debug Variables
    [SerializeField]
    private bool isDebugGameManager;
    #endregion

    
    private Database itemDataBase = new Database();
    private PawnDatabase pawnDataBase = new PawnDatabase();
    private QuestDeckDatabase questDeckDataBase = new QuestDeckDatabase();
    private QuestDatabase questDataBase = new QuestDatabase();


    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            // InitAllDatabase
            itemDataBase.Init();
            questDeckDataBase.Init();
            questDataBase.Init();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (IsDebugGameManager)
        {
            // HERE if is not
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
    
    public Database Database
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
}

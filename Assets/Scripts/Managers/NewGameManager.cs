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

    [SerializeField]
    private PrefabUIUtils prefabUIUtils;
    [SerializeField]
    private PrefabUtils prefabUtils;
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
}

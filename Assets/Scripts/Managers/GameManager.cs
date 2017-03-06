using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private List<KeeperInstance> listOfSelectedKeepers = new List<KeeperInstance>();
    private GameObject goTarget;

    public bool isDebugGameManager;



    private List<KeeperInstance> allKeepersList = new List<KeeperInstance>();

    
    private bool characterPanelIngameNeedUpdate = false;
    private bool shortcutPanel_NeedUpdate = true;
    private bool characterPanelMenuNeedUpdate = false;


    private IngameUI ui;
    private IngameScreens gameScreens;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
    public void CheckGameOver()
    {
        bool prisonerDied = !TileManager.Instance.prisoner.IsAlive;
        if(prisonerDied)
        {
            Debug.Log("GameOver - Prisoner Died");
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
            }
        }
        
    }
}

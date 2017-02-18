using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private List<KeeperInstance> listOfSelectedKeepers = new List<KeeperInstance>();

    private List<KeeperInstance> allKeepersList = new List<KeeperInstance>();
    
    private bool characterPanelIngameNeedUpdate = true;
    private bool characterPanelMenuNeedUpdate = false;

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

    public bool CharacterPanelIngameNeedUpdate
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
}

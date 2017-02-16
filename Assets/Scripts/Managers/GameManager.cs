using System.Collections;
using System.Collections.Generic;
using Keepers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    // temp for debug
    private bool once = false;

    private List<KeeperInstance> listOfSelectedKeepers = new List<KeeperInstance>();

    // TODO: change it to a list of KeeperInstance @Remi
    private List<Keepers.Selectable> allKeepersList = new List<Keepers.Selectable>();

    private bool characterPanelIngameNeedUpdate = true;
    private bool characterPanelMenuNeedUpdate = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // debug only
        if (!once)
        {
            if (KeeperManager.listKeepers != null)
                Debug.Log("NbPersonnages : " + KeeperManager.listKeepers.Count);

            once = true;
        }
    }

    public void ClearListKeeperSelected()
    {
        for (int i = 0; i < listOfSelectedKeepers.Count; i++)
        {
            listOfSelectedKeepers[i].IsSelected = false;

        }
        listOfSelectedKeepers.Clear();
    }


    /* ------------------------------------------------------------------------------------ */
    /* ------------------------------------------ Accessors ------------------------------- */
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

    public List<Keepers.Selectable> AllKeepersList
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

using UnityEngine;
using System.Collections.Generic;



public class MenuController : MonoBehaviour {

    private MenuManager menuManager;
    private MenuUI menuUI;
    private BoxOpener boxOpener;



    public Transform trLevelCardTarget;

    //public Opener LevelDeck;

    [SerializeField]
    public LayerMask layerToCheck;

    public LayerMask layerControls;


    private LevelDataBase leveldb;

    // Use this for initialization
    void Start()
    {
        menuManager = GetComponent<MenuManager>();
        menuUI = GetComponent<MenuUI>();
        boxOpener = GetComponent<BoxOpener>();
        leveldb = menuManager.Leveldb;
    }

    // Update is called once per frame
    void Update()
    {
        MenuControls();
    }

    

    public void MenuControls()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (boxOpener.IsBoxOpen)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit) == true)
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("CardLevel"))
                    {
                        LevelSelectionControls(hit.transform.gameObject);
                    }
                    else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("KeeperInstance"))
                    {
                        KeeperSelectionControls(hit.transform.gameObject);
                        //FoldEverything();
                    }
                    else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("DeckOfCards"))
                    {
                        DeckSelectionControls(hit.transform.gameObject);
                    }
                    else
                    {
                        //FoldEverything();
                    }
                }
            }

            LayerMask mask = 1 << LayerMask.NameToLayer("BoxLock");
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask);
            if (hit.transform != null)
            {
                boxOpener.BoxControls();
            }
        }
    }


    public void LevelSelectionControls(GameObject hit)
    {

        CardLevel card = hit.transform.gameObject.GetComponent<CardLevel>();
        if (card != null && !menuUI.ACardInfoIsShown && !menuUI.IsACardMoving && !menuUI.IsACardInfoMoving)
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.paperSelectSound, 0.5f);


            if  (card.gameObject == menuUI.LevelCardSelected)
            {
                menuUI.LevelCardSelected.GetComponent<CardLevel>().IsSelected = false;
                menuManager.CardLevelSelected = -1;
                menuManager.DeckOfCardsSelected = string.Empty;

                GameManager.Instance.ListEventSelected.Clear();

            }
            else
            {

                menuUI.LevelCardSelected = card.gameObject;
                menuUI.LevelCardSelected.GetComponent<CardLevel>().IsSelected = true;

                menuManager.CardLevelSelected = card.levelIndex;
                menuManager.DeckOfCardsSelected = leveldb.GetLevelById(card.levelIndex).deckId;

                GameManager.Instance.ListEventSelected.Clear();

                for (int i = 0; i < leveldb.GetLevelById(card.levelIndex).listEventsId.Count; i++)
                {
                    if (GameManager.Instance.PersistenceLoader.Pd.dicPersistenceEvents[leveldb.GetLevelById(card.levelIndex).listEventsId[i]] == true)
                    {
                        GameManager.Instance.ListEventSelected.Add(leveldb.GetLevelById(card.levelIndex).listEventsId[i]);
                    }
                }

            }

            menuUI.UpdateDeckSelected();
            menuUI.UpdateStartButton();
            boxOpener.UpdateLockAspect();
        }
    }

    public void DeckSelectionControls(GameObject hit)
    {
        if(menuUI.cardsInfoAreReady && !menuUI.ACardInfoIsShown && !menuUI.IsACardMoving && !menuUI.IsACardInfoMoving)
        {
            menuUI.UpdateDeckDisplayed();
            foreach (GameObject go in menuManager.GoCardsLevels)
            {
                if (go.transform.parent == menuManager.GoDeck.transform)
                {
                    go.transform.SetParent(null);
                    go.SetActive(true);
                }
            }

            foreach (List<GameObject> goChildren in menuManager.GoCardChildren)
            {

                    foreach (GameObject go in goChildren)

                    {
                        if (menuUI.LevelCardSelected != go.GetComponentInParent<CardLevel>().gameObject)
                        {

                            go.SetActive(false);

                            go.transform.localPosition = new Vector3(0.0f, 0f, 0.0f);
                        }
                    }
       
            }

            //;
        } 
 

 
    }

    public void KeeperSelectionControls(GameObject hit)
    {
        PawnInstance pi = hit.transform.gameObject.GetComponent<PawnInstance>();
        if (pi != null && !menuManager.GoDeck.GetComponent<Deck>().IsOpen && menuManager.CardLevelSelected !=-1)
        {
            if (menuManager.ContainsSelectedKeepers(pi)) // REMOVE
            {
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.deselectSound, 0.25f);
                //pi.GetComponent<OpenerContent>().Hide();
                menuManager.RemoveFromSelectedKeepers(pi);
                if (menuManager.GoDeck.GetComponent<Deck>() != null && !menuManager.GoDeck.GetComponent<Deck>().IsOpen)
                {
                    menuManager.DicPawnChatBox[pi.gameObject].SetMode(ChatBox.ChatMode.awaiting);
                    menuManager.DicPawnChatBox[pi.gameObject].Say(ChatBox.ChatMode.unchosen);
                }
            }
            else    // ADD
            {
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.selectSound, 0.25f);
                //pi.GetComponent<OpenerContent>().Show();

                menuManager.AddToSelectedKeepers(pi);
                if( menuManager.GoDeck.GetComponent<Deck>() != null && !menuManager.GoDeck.GetComponent<Deck>().IsOpen)
                {
                    menuManager.DicPawnChatBox[pi.gameObject].SetMode(ChatBox.ChatMode.picked);
                    menuManager.DicPawnChatBox[pi.gameObject].Say(ChatBox.ChatMode.chosen);
                }

            }

            menuUI.UpdateKeepers(pi, hit.transform.parent);
            pi.transform.SetParent(null);

            boxOpener.UpdateLockAspect();
        } else if (menuManager.CardLevelSelected == -1)

        {



            menuManager.GoDeck.GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(true);

            menuManager.GoDeck.GetComponent<GlowObjectCmd>().enabled = true;
        }
    } 

    //void FoldEverything()
    //{
    //    for (int i = 0; i < LevelDeck.listOpenerSiblings.Count; i++)
    //    {
    //        LevelDeck.listOpenerSiblings[i].Fold();
    //    }
    //    LevelDeck.Fold();
    //}
}

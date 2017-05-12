using UnityEngine;
using System.Collections.Generic;



public class MenuController : MonoBehaviour {

    private MenuManager menuManager;
    private MenuUI menuUI;
    private BoxOpener boxOpener;



    private GameObject levelCardSelected = null;

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
        if (card != null && card.gameObject != levelCardSelected)
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.paperSelectSound, 0.5f);


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

            if (levelCardSelected != null)
            {
                OpenerContent oc = levelCardSelected.GetComponent<OpenerContent>();
                oc.listKeyPose.Clear();
                oc.AddKeyPose(oc.transform.position, oc.transform.rotation);
                oc.AddKeyPose(oc.transform.position + new Vector3(2, 2, 2), Quaternion.Inverse(oc.transform.rotation));
                oc.bNeedShow = true;
                oc.bKill = true;
            }

            OpenerContent newCard = Instantiate(card, card.transform.position, card.transform.rotation).GetComponent<OpenerContent>();
           
            newCard.Init();

            newCard.listKeyPose.Clear();
            newCard.AddKeyPose(card.transform.position, card.transform.rotation);
            newCard.AddKeyPose(trLevelCardTarget.position, trLevelCardTarget.rotation);
            newCard.bNeedShow = true;

            levelCardSelected = newCard.gameObject;

            newCard.GetComponent<Opener>().Reset();

            card.GetComponent<Opener>().Fold();

            GlowController.RegisterObject(newCard.GetComponent<GlowObjectCmd>());

            //menuUI.UpdateCardLevelSelection();
            menuUI.UpdateStartButton();
            boxOpener.UpdateLockAspect();
        }
    }

    public void DeckSelectionControls(GameObject hit)
    {



    }

    public void KeeperSelectionControls(GameObject hit)
    {
        PawnInstance pi = hit.transform.gameObject.GetComponent<PawnInstance>();
        if (pi != null)
        {
            if (menuManager.ContainsSelectedKeepers(pi)) // REMOVE
            {
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.deselectSound, 0.25f);
                //pi.GetComponent<OpenerContent>().Hide();
                menuManager.RemoveFromSelectedKeepers(pi);
                menuManager.DicPawnChatBox[pi.gameObject].SetMode(ChatBox.ChatMode.awaiting);
                menuManager.DicPawnChatBox[pi.gameObject].Say(ChatBox.ChatMode.unchosen);
            }
            else    // ADD
            {
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.selectSound, 0.25f);
                //pi.GetComponent<OpenerContent>().Show();

                menuManager.AddToSelectedKeepers(pi);
                menuManager.DicPawnChatBox[pi.gameObject].SetMode(ChatBox.ChatMode.picked);
                menuManager.DicPawnChatBox[pi.gameObject].Say(ChatBox.ChatMode.chosen);
            }

            menuUI.UpdateKeepers(pi, hit.transform.parent);
            pi.transform.SetParent(null);

            boxOpener.UpdateLockAspect();
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

using UnityEngine;
using System.Collections.Generic;

// Temp
using UnityEngine.SceneManagement;

public class MenuControlsQ : MonoBehaviour {

    private MenuManagerQ menuManager;
    private MenuUIQ menuUI;

    public Transform trLevelCardTarget;
    public Transform trQuestDeckTarget;

    public Opener EventCardSelectedOpener;

    public Opener EventDeck;

    public GameObject prefabLevelCardSelected;
    public GameObject prefabDeckSelected;
    GameObject levelCardSelected = null;
    GameObject deckSelected = null;

    bool bIsOpen = false;
    public Animator animatorBox;
    public Animator animatorCam;
     
    [SerializeField] public LayerMask layerToCheck;

    [SerializeField] Color colorLockOpen;
    [SerializeField] Color colorLockClosed;

    [SerializeField] public LayerMask layerControls;
    public GameObject boxLock;

    // Use this for initialization
    void Start () {
        menuManager = GetComponent<MenuManagerQ>();
        menuUI = GetComponent<MenuUIQ>();

        EventCardSelectedOpener.GetComponent<MeshRenderer>().enabled = false;
        EventCardSelectedOpener.GetComponent<MeshCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        MenuControls();

        BoxControls();
    }

    void UpdateToolTip()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
    }

    public void BoxControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            LayerMask mask = 1 << LayerMask.NameToLayer("BoxLock");
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask);
            if (hit.transform != null)
            {
                
                if ((menuManager.ListeSelectedKeepers.Count == 0 && menuManager.CardLevelSelected == -1 && menuManager.DeckOfCardsSelected == string.Empty))
                {
                    if (bIsOpen)
                    {
                        foreach(Opener o in GameObject.FindObjectsOfType<Opener>())
                        {
                            o.Fold();
                        }
                    }

                    bIsOpen = !bIsOpen;
                    animatorBox.SetBool("bOpen", bIsOpen);
                    animatorCam.SetBool("bOpen", bIsOpen);

                    menuManager.SetActiveChatBoxes(bIsOpen);
                }

            }
        }
    }

    public void MenuControls()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (bIsOpen && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerControls) == true)
            {

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("DeckOfCards"))
                {
                    DeckSelectionControls(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("CardLevel"))
                {
                    LevelSelectionControls(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EventCard"))
                {
                    EventCardsSelectionControls(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("KeeperInstance"))
                {
                    KeeperSelectionControls(hit.transform.gameObject);
                    for (int i = 0; i < EventDeck.listOpenerSiblings.Count; i++)
                    {
                        EventDeck.listOpenerSiblings[i].Fold();
                    }
                    EventDeck.Fold();
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("RuleBook"))
                {
                    RuleBookControls(hit.transform.gameObject);
                }
            }
        }
    }

    public void DeckSelectionControls(GameObject hit)
    {
        DeckOfCards deck = hit.transform.gameObject.GetComponent<DeckOfCards>();
        if (deck != null && deck.gameObject != deckSelected)
        {
            if (menuManager.DeckOfCardsSelected == deck.idQuestDeck)
            {
                menuManager.DeckOfCardsSelected = string.Empty;
            }
            else
            {
                menuManager.DeckOfCardsSelected = deck.idQuestDeck;
            }
            menuUI.UpdateDeckSelection();

            if (deckSelected != null)
            {
                OpenerContent oc = deckSelected.GetComponent<OpenerContent>();
                oc.listKeyPose.Clear();
                oc.AddKeyPose(oc.transform.position, oc.transform.rotation);
                oc.AddKeyPose(oc.transform.position + new Vector3(2,2,2), Quaternion.Inverse(oc.transform.rotation));
                oc.bNeedShow = true;
                oc.bKill = true;
            }

            OpenerContent newDeck = Instantiate(deck.gameObject, deck.transform.position, deck.transform.rotation).GetComponent<OpenerContent>();

            //newDeck.GetComponent<MeshFilter>().mesh = deck.GetComponent<MeshFilter>().sharedMesh;

            newDeck.Init();

            newDeck.listKeyPose.Clear();
            newDeck.AddKeyPose(deck.transform.position, deck.transform.rotation);
            newDeck.AddKeyPose(trQuestDeckTarget.position, trQuestDeckTarget.rotation);
            newDeck.bNeedShow = true;

            newDeck.fSpeed = 5;

            Opener p = newDeck.GetComponent<Opener>();

            p.listOpenerSiblings.Clear();
            for(int i = 0; i < EventDeck.listOpenerSiblings.Count; i++)
            {
                p.listOpenerSiblings.Add(EventDeck.listOpenerSiblings[i]);
                EventDeck.listOpenerSiblings[i].listOpenerSiblings.Add(p);
            }

            deckSelected = newDeck.gameObject;
            UpdateLockAspect();
        }
    }

    public void LevelSelectionControls(GameObject hit)
    {
        CardLevel card = hit.transform.gameObject.GetComponent<CardLevel>();
        if (card != null && card.gameObject != levelCardSelected)
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.paperSelectSound, 0.5f);

            menuManager.CardLevelSelected = card.levelIndex;
            menuManager.DeckOfCardsSelected = menuManager.leveldb.GetLevelById(card.levelIndex).deckId;

            if (deckSelected != null)
            {
                OpenerContent oc = deckSelected.GetComponent<OpenerContent>();
                oc.listKeyPose.Clear();
                oc.AddKeyPose(oc.transform.position, oc.transform.rotation);
                oc.AddKeyPose(oc.transform.position + new Vector3(2, 2, 2), Quaternion.Inverse(oc.transform.rotation));
                oc.bNeedShow = true;
                oc.bKill = true;
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

            OpenerContent newCard = Instantiate(prefabLevelCardSelected, card.transform.position, card.transform.rotation).GetComponent<OpenerContent>();

            newCard.GetComponent<MeshFilter>().mesh = card.GetComponent<MeshFilter>().sharedMesh;

            newCard.Init();

            newCard.listKeyPose.Clear();
            newCard.AddKeyPose(card.transform.position, card.transform.rotation);
            newCard.AddKeyPose(trLevelCardTarget.position, trLevelCardTarget.rotation);
            newCard.bNeedShow = true;

            levelCardSelected = newCard.gameObject;

            GlowController.RegisterObject(newCard.GetComponent<GlowObjectCmd>());

            //menuUI.UpdateCardLevelSelection();
            menuUI.UpdateStartButton();
            UpdateLockAspect();
        }
    }

    public void EventCardsSelectionControls(GameObject hit)
    {
        EventCard ec = hit.transform.GetComponent<EventCard>();
        if (ec.bSelected == true)
        {
            RemoveEventCardFromSelection(hit.transform.gameObject);
        }
        else
        {
            AddEventCardToSelection(hit.transform.gameObject);
        }
        UpdateLockAspect();
    }

    public void KeeperSelectionControls(GameObject hit)
    {
        PawnInstance pi = hit.transform.gameObject.GetComponent<PawnInstance>();
        if (pi != null)
        {
            if (menuManager.ContainsSelectedKeepers(pi)) // REMOVE
            {
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.deselectSound, 0.25f);
                pi.GetComponent<OpenerContent>().Hide();
                menuManager.RemoveFromSelectedKeepers(pi);
                menuManager.dicPawnChatBox[pi.gameObject].SetMode(ChatBox.ChatMode.awaiting);
                menuManager.dicPawnChatBox[pi.gameObject].Say(ChatBox.ChatMode.unchosen);
            }
            else    // ADD
            {
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.selectSound, 0.25f);
                pi.GetComponent<OpenerContent>().Show();

                menuManager.AddToSelectedKeepers(pi);
                menuManager.dicPawnChatBox[pi.gameObject].SetMode(ChatBox.ChatMode.picked);
                menuManager.dicPawnChatBox[pi.gameObject].Say(ChatBox.ChatMode.chosen);
            }
            //menuUI.UpdateSelectedKeepers();
            menuUI.UpdateStartButton();
            UpdateLockAspect();
        }
    }

    public void RuleBookControls(GameObject hit)
    {
        // TODO Rulebook controls
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.buttonClick, 0.5f);

        foreach (PawnInstance ki in menuManager.ListeSelectedKeepers)
        {
            GameManager.Instance.AllKeepersList.Add(ki);
            ki.gameObject.transform.SetParent(GameManager.Instance.transform);

            foreach(KeyValuePair<GameObject, ChatBox> gc in menuManager.dicPawnChatBox)
            {
                GameObject.Destroy(gc.Value.gameObject);
            }
        }

        GameManager.Instance.DeckSelected = menuManager.DeckOfCardsSelected;

        if (AudioManager.Instance != null)
        {
            AudioClip toPlay;
            switch (menuManager.CardLevelSelected)
            {
                case 1:
                    toPlay = AudioManager.Instance.Scene1Clip;
                    break;
                case 2:
                    toPlay = AudioManager.Instance.Scene2Clip;
                    break;
                case 3:
                    toPlay = AudioManager.Instance.Scene3Clip;
                    break;
                case 4:
                    toPlay = AudioManager.Instance.Scene4Clip;
                    break;
                default:
                    toPlay = AudioManager.Instance.menuMusic;
                    break;
            }
            AudioManager.Instance.Fade(toPlay);
        }
        SceneManager.LoadScene(menuManager.CardLevelSelected);
    }

    public void AddEventCardToSelection(GameObject goCard)
    {
        if (EventCardSelectedOpener.listChilds.Count == 0)
        {
            EventCardSelectedOpener.GetComponent<MeshRenderer>().enabled = true;
            EventCardSelectedOpener.GetComponent<MeshCollider>().enabled = true;
        }

        OpenerContent oc = goCard.GetComponent<OpenerContent>();

        oc.GetComponent<EventCard>().bSelected = true;
        oc.transform.parent = EventCardSelectedOpener.transform;
        oc.LoadParent();
        oc.listKeyPose.Clear();
        oc.AddKeyPose(Vector3.zero, Quaternion.identity);
        oc.AddKeyPose(oc.transform.position - EventCardSelectedOpener.transform.position, Quaternion.identity);
        oc.Hide(true);
        oc.bNeedCompute = true;

        GameManager.Instance.ListEventSelected.Add(goCard.GetComponent<EventCard>().id);

        EventDeck.bNeedReload = true;
        EventDeck.LoadChilds();
        if(EventDeck.listChilds.Count == 0)
        {
            EventDeck.GetComponent<MeshRenderer>().enabled = false;
            EventDeck.GetComponent<MeshCollider>().enabled = false;
        }
    }

    public void RemoveEventCardFromSelection(GameObject goCard)
    {
        if (EventDeck.listChilds.Count == 0)
        {
            EventDeck.GetComponent<MeshRenderer>().enabled = true;
            EventDeck.GetComponent<MeshCollider>().enabled = true;
        }

        GameManager.Instance.ListEventSelected.Remove(goCard.GetComponent<EventCard>().id);

        goCard.GetComponent<EventCard>().bSelected = false;

        OpenerContent oc = goCard.GetComponent<OpenerContent>();

        oc.transform.parent = EventDeck.transform;
        oc.LoadParent();
        oc.listKeyPose.Clear();
        oc.AddKeyPose(Vector3.zero, Quaternion.identity);
        oc.AddKeyPose(oc.transform.position - EventDeck.transform.position, Quaternion.identity);
        oc.Hide(true);
        oc.bNeedCompute = true;

        EventCardSelectedOpener.bNeedReload = true;
        EventCardSelectedOpener.LoadChilds();

        if (GameManager.Instance.ListEventSelected.Count == 0)
        {
            EventCardSelectedOpener.GetComponent<MeshRenderer>().enabled = false;
            EventCardSelectedOpener.GetComponent<MeshCollider>().enabled = false;
        }
    }

    public void UpdateLockAspect()
    {
        if ((menuManager.ListeSelectedKeepers.Count == 0 && menuManager.CardLevelSelected == -1 && menuManager.DeckOfCardsSelected == string.Empty))
        {
            boxLock.GetComponent<GlowObjectCmd>().GlowColor = colorLockClosed;
        }
        else
        {
            boxLock.GetComponent<GlowObjectCmd>().GlowColor = colorLockOpen;
        }

    }
}

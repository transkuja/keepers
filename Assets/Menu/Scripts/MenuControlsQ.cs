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
     
    [SerializeField]
    public LayerMask layerToCheck;

    [SerializeField] Color colorLockOpen;
    [SerializeField] Color colorLockClosed;

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

        if (bIsOpen)
        {
            DeckSelectionControls();
            LevelSelectionControls();
            KeeperSelectionControls();
            RuleBookControls();
            EventCardsSelectionControls();
        }

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
            Application.Quit();
    }

    public void DeckSelectionControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            LayerMask DeckOfCardsLayerMask = 1 << LayerMask.NameToLayer("DeckOfCards");
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, DeckOfCardsLayerMask) == true)
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

                    newDeck.GetComponent<MeshFilter>().mesh = deck.GetComponent<MeshFilter>().sharedMesh;

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
        }
    }

    public void LevelSelectionControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            LayerMask cardLevelLayerMask = 1 << LayerMask.NameToLayer("CardLevel");
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, cardLevelLayerMask) == true)
            {
                CardLevel card = hit.transform.gameObject.GetComponent<CardLevel>();
                if (card != null && card.gameObject != levelCardSelected)
                {
                    AudioManager.Instance.PlayOneShot(AudioManager.Instance.paperSelectSound, 0.5f);
                    // TODO Maybe change selection criterias
                    /*if (menuManager.CardLevelSelected == card.levelIndex)
                    {
                        menuManager.CardLevelSelected = -1;
                    }
                    else*/
                    {
                        menuManager.CardLevelSelected = card.levelIndex;
                    }

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

                    menuUI.UpdateCardLevelSelection();
                    UpdateLockAspect();
                }
            }
        }
    }

    public void EventCardsSelectionControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LayerMask mask = 1 << LayerMask.NameToLayer("EventCard");
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask);
            if(hit.transform != null)
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
        }
    }

    public void KeeperSelectionControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            LayerMask keeperLayerMask = 1 << LayerMask.NameToLayer("KeeperInstance"); ;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, keeperLayerMask) == true)
            {
                PawnInstance pi = hit.transform.gameObject.GetComponent<PawnInstance>();
                if (pi != null)
                {
                    if (menuManager.ContainsSelectedKeepers(pi)) // REMOVE
                    {
                        AudioManager.Instance.PlayOneShot(AudioManager.Instance.deselectSound, 0.25f);
                        pi.GetComponent<OpenerContent>().Hide();
                        menuManager.RemoveFromSelectedKeepers(pi);
                        menuManager.dicPawnChatBox[pi.gameObject].SetMode(ChatBox.ChatMode.pickme);
                        menuManager.dicPawnChatBox[pi.gameObject].Say("Oh no ...");
                    }
                    else    // ADD
                    {
                        AudioManager.Instance.PlayOneShot(AudioManager.Instance.selectSound, 0.25f);
                        pi.GetComponent<OpenerContent>().Show();

                        menuManager.AddToSelectedKeepers(pi);
                        menuManager.dicPawnChatBox[pi.gameObject].SetMode(ChatBox.ChatMode.picked);
                        menuManager.dicPawnChatBox[pi.gameObject].Say("Yahouuuu !");
                    }
                    menuUI.UpdateSelectedKeepers();
                    UpdateLockAspect();
                }
            }
        }
    }

    public void RuleBookControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            LayerMask ruleBookLayer = 1 << LayerMask.NameToLayer("RuleBook"); ;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ruleBookLayer) == true)
            {
                Debug.Log("Click on rule book");
            }
        }
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

        Debug.Log(menuManager.DeckOfCardsSelected);
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
                default:
                    toPlay = toPlay = AudioManager.Instance.menuMusic;
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
    }

    public void RemoveEventCardFromSelection(GameObject goCard)
    {
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

        if (GameManager.Instance.ListEventSelected.Count == 0)
        {
            EventCardSelectedOpener.GetComponent<MeshRenderer>().enabled = false;
            EventCardSelectedOpener.GetComponent<MeshCollider>().enabled = false;
        }
    }

    bool FindEventCard(string id)
    {
        for(int i=0; i < GameManager.Instance.ListEventSelected.Count; i++)
        {
            if(string.Compare(GameManager.Instance.ListEventSelected[i],id) == 0)
            {
                return true;
            }
        }
        return false;
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

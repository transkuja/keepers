using UnityEngine;

// Temp
using UnityEngine.SceneManagement;

public class MenuControlsQ : MonoBehaviour {

    private MenuManagerQ menuManager;
    private MenuUIQ menuUI;

    public Transform trLevelCardTarget;
    public Transform trQuestDeckTarget;
    public Opener EventCardSelectedOpener;

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
                bIsOpen = !bIsOpen;
                animatorBox.SetBool("bOpen", bIsOpen);
                animatorCam.SetBool("bOpen", bIsOpen);

                hit.transform.GetComponent<GlowObjectCmd>().GlowColor = bIsOpen ? colorLockOpen : colorLockClosed;
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

                        //GameObject.Destroy(deckSelected);
                    }

                    OpenerContent newDeck = Instantiate(deck.gameObject /*prefabDeckSelected*/, deck.transform.position, deck.transform.rotation).GetComponent<OpenerContent>();

                    newDeck.GetComponent<MeshFilter>().mesh = deck.GetComponent<MeshFilter>().sharedMesh;

                    newDeck.Init();

                    newDeck.listKeyPose.Clear();
                    newDeck.AddKeyPose(deck.transform.position, deck.transform.rotation);
                    newDeck.AddKeyPose(trQuestDeckTarget.position, trQuestDeckTarget.rotation);
                    newDeck.bNeedShow = true;

                    deckSelected = newDeck.gameObject;
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


                    if(levelCardSelected != null)
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
                if (!FindEventCard(hit.transform.gameObject.name))
                {
                    AddEventCardToSelection(hit.transform.gameObject);
                }
                else
                {
                    RemoveEventCardFromSelection(hit.transform.gameObject);
                }
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
                    if (menuManager.ContainsSelectedKeepers(pi))
                    {
                        AudioManager.Instance.PlayOneShot(AudioManager.Instance.deselectSound, 0.25f);
                        pi.GetComponent<OpenerContent>().Hide();
                        menuManager.RemoveFromSelectedKeepers(pi);
                    }
                    else
                    {
                        AudioManager.Instance.PlayOneShot(AudioManager.Instance.selectSound, 0.25f);
                        pi.GetComponent<OpenerContent>().Show();

                        menuManager.AddToSelectedKeepers(pi);
                    }
                    menuUI.UpdateSelectedKeepers();
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
        }

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

    void AddEventCardToSelection(GameObject goCard)
    {
        if (EventCardSelectedOpener.listChilds.Count == 0)
        {
            EventCardSelectedOpener.GetComponent<MeshRenderer>().enabled = true;
            EventCardSelectedOpener.GetComponent<MeshCollider>().enabled = true;
        }

        GameObject newCard = Instantiate(goCard, EventCardSelectedOpener.transform);
        newCard.transform.localPosition = Vector3.zero;

        newCard.name = goCard.name;

        EventCardSelectedOpener.LoadChilds();
        EventCardSelectedOpener.ComputeContentPositions();
        EventCardSelectedOpener.Fold(true);

        GameManager.Instance.ListEventSelected.Add(goCard.name);
    }

    void RemoveEventCardFromSelection(GameObject goCard)
    {
        GameManager.Instance.ListEventSelected.Remove(goCard.name);

        if (EventCardSelectedOpener.listChilds.Count == 0)
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
}

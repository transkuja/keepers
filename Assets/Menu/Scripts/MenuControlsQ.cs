using UnityEngine;

// Temp
using UnityEngine.SceneManagement;

public class MenuControlsQ : MonoBehaviour {

    private MenuManagerQ menuManager;
    private MenuUIQ menuUI;

    public Transform trLevelCardTarget;
    public Transform trQuestDeckTarget;

    public GameObject prefabLevelCardSelected;
    public GameObject prefabDeckSelected;
    GameObject levelCardSelected = null;
    GameObject deckSelected = null;

    bool bIsOpen = false;
    public Animator animatorBox;
    public Animator animatorCam;

    // Use this for initialization
    void Start () {
        menuManager = GetComponent<MenuManagerQ>();
        menuUI = GetComponent<MenuUIQ>();
    }

    // Update is called once per frame
    void Update()
    {
        MenuControls();
        DeckSelectionControls();
        LevelSelectionControls();
        KeeperSelectionControls();
        RuleBookControls();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            bIsOpen = !bIsOpen;
            animatorBox.SetBool("bOpen", bIsOpen);
            animatorCam.SetBool("bOpen", bIsOpen);
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            LayerMask DeckOfCardsLayerMask = 1 << LayerMask.NameToLayer("DeckOfCards");
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, DeckOfCardsLayerMask) == true)
            {
                DeckOfCards deck = hit.transform.gameObject.GetComponent<DeckOfCards>();
                if (deck != null)
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

                    OpenerContent newDeck = Instantiate(prefabDeckSelected, deck.transform.position, deck.transform.rotation).GetComponent<OpenerContent>();

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            LayerMask cardLevelLayerMask = 1 << LayerMask.NameToLayer("CardLevel"); ;
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

                    menuUI.UpdateCardLevelSelection();
                }
            }
        }
    }

    public void KeeperSelectionControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
}

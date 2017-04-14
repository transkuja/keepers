using UnityEngine;

// Temp
using UnityEngine.SceneManagement;

public class NewMenuControls : MonoBehaviour {

    private NewMenuManager menuManager;
    private NewMenuUI menuUI;

    // Use this for initialization
    void Start () {
        menuManager = GetComponent<NewMenuManager>();
        menuUI = GetComponent<NewMenuUI>();
    }

    // Update is called once per frame
    void Update()
    {
        MenuControls();
        DeckSelectionControls();
        LevelSelectionControls();
        KeeperSelectionControls();
        RuleBookControls();
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
                DeckOfCards card = hit.transform.gameObject.GetComponent<DeckOfCards>();
                if (card != null)
                {
                    if (menuManager.DeckOfCardsSelected == card.idQuestDeck)
                    {
                        menuManager.DeckOfCardsSelected = string.Empty;
                    }
                    else
                    {
                        menuManager.DeckOfCardsSelected = card.idQuestDeck;
                    }
                    menuUI.UpdateDeckSelection();
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
                if (card != null)
                {
                    AudioManager.Instance.PlayOneShot(AudioManager.Instance.paperSelectSound, 0.5f);
                    if (menuManager.CardLevelSelected == card.levelIndex)
                    {
                        menuManager.CardLevelSelected = -1;
                    }
                    else
                    {
                        menuManager.CardLevelSelected = card.levelIndex;
                    }
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
                        menuManager.RemoveFromSelectedKeepers(pi);
                    }
                    else
                    {
                        AudioManager.Instance.PlayOneShot(AudioManager.Instance.selectSound, 0.25f);
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
            NewGameManager.Instance.ListOfSelectedKeepers.Add(ki);
            ki.gameObject.transform.SetParent(NewGameManager.Instance.transform);
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

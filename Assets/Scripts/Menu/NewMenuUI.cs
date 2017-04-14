using UnityEngine;
using UnityEngine.UI;

public class NewMenuUI : MonoBehaviour {

    private NewMenuManager menuManager;

    public GameObject CharacterPanel;
    public Image startButtonImg;
    public Image cardLevelSelectedImg;
    // TODO handle this better
    [SerializeField]
    Sprite[] levelImg;

    void Start()
    {
        menuManager = GetComponent<NewMenuManager>();
        startButtonImg.enabled = false;
    }

    public void Update()
    {
        UpdateStartButton();
    }

    public void UpdateSelectedKeepers()
    {
        // Clear
        for (int i = 0; i < CharacterPanel.transform.childCount; i++)
        {
            Destroy(CharacterPanel.transform.GetChild(i).gameObject);
        }

        // On selection 
        int nbCharacters = menuManager.ListeSelectedKeepers.Count;
        for (int i = 0; i < nbCharacters; i++)
        {
            PawnInstance currentSelectedCharacter = menuManager.ListeSelectedKeepers[i];

            Sprite associatedSprite = currentSelectedCharacter.Data.AssociatedSprite;
            if (associatedSprite != null)
            {
                GameObject CharacterImage = Instantiate(NewGameManager.Instance.PrefabUIUtils.prefabMenuSelectedKeeperUI, CharacterPanel.transform);
                CharacterImage.name = currentSelectedCharacter.Data.PawnName + ".Panel";
                CharacterImage.transform.GetChild(0).GetComponent<Image>().sprite = associatedSprite;
                CharacterImage.transform.localScale = Vector3.one;}
        }
    }

    public void UpdateCardLevelSelection()
    {
        if(menuManager.CardLevelSelected != -1)
        {
            cardLevelSelectedImg.sprite = levelImg[menuManager.CardLevelSelected - 1];
            cardLevelSelectedImg.enabled = true;
        } else
        {
            cardLevelSelectedImg.enabled = false;
        }
    }

    public void UpdateDeckSelection()
    {
        Debug.Log("Deck Selected");
    }

    public void UpdateStartButton()
    {
        if (menuManager.ListeSelectedKeepers.Count == 0 || menuManager.CardLevelSelected == -1)
        {
            startButtonImg.enabled = false;
        }
        else
        {
            startButtonImg.enabled = true;
        }
    }
}

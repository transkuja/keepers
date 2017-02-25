using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class IngameUI : MonoBehaviour
{
    // CharacterPanel
    [Header("Character Panel")]
    public GameObject CharacterPanel;
    public GameObject baseCharacterImage;

    // Turn Panel
    [Header("Turn Panel")]
    public GameObject TurnPanel;
    public GameObject TurnButton;
    public float buttonRotationSpeed = 1.0f;

    // CharacterPanel
    [Header("Action Panel")]
    public GameObject goActionPanelQ;
    public GameObject baseActionImage;

    public bool isTurnEnding = false;

    public void Awake()
    {
        UpdateCharacterPanelUI();
    }

    public void Update()
    {
        if (GameManager.Instance != null )
        {
            if (GameManager.Instance.CharacterPanelIngameNeedUpdate)
            {
                UpdateCharacterPanelUI();
            }
        }
    }

    void UpdateCharacterPanelUI()
    {
        if (GameManager.Instance == null){ return; }
        if (CharacterPanel == null) { return; }

        for (int i = 0; i < CharacterPanel.transform.childCount; i++)
        {
            Destroy(CharacterPanel.transform.GetChild(i).gameObject);
        }

        int nbCaracters = GameManager.Instance.AllKeepersList.Count;
        for (int i = 0; i < nbCaracters; i++)
        {
            KeeperInstance currentSelectedCharacter = GameManager.Instance.AllKeepersList[i];

            Sprite associatedSprite = currentSelectedCharacter.Keeper.AssociatedSprite;
            if (associatedSprite != null)
            {
                GameObject goKeeper = Instantiate(baseCharacterImage, CharacterPanel.transform);

                goKeeper.name = currentSelectedCharacter.Keeper.CharacterName + ".Panel";
                goKeeper.transform.GetChild(0).GetComponent<Image>().sprite = associatedSprite;
                goKeeper.transform.localScale = Vector3.one;

                // Stats
                // HP
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponentInChildren<Text>().text = "HP: " + currentSelectedCharacter.Keeper.Hp.ToString();
                // MP
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponentInChildren<Text>().text = "MP: " + currentSelectedCharacter.Keeper.Mp.ToString();
                // Strengh
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponentInChildren<Text>().text = "S: " + currentSelectedCharacter.Keeper.Strength.ToString();
                // Defense
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.GetComponentInChildren<Text>().text = "D: " + currentSelectedCharacter.Keeper.Defense.ToString();
                // Intelligence
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(4).gameObject.GetComponentInChildren<Text>().text = "I: " + currentSelectedCharacter.Keeper.Intelligence.ToString();
                // Spirit
                goKeeper.transform.GetChild(0).GetChild(0).GetChild(5).gameObject.GetComponentInChildren<Text>().text = "S: " + currentSelectedCharacter.Keeper.Spirit.ToString();

                // Status
                // Hunger
                goKeeper.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponentInChildren<Text>().text = "H: " + currentSelectedCharacter.Keeper.ActualHunger.ToString();
                // MentalHealth
                goKeeper.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponentInChildren<Text>().text = "MH: " + currentSelectedCharacter.Keeper.ActualMentalHealth.ToString();
            }
        }

        GameManager.Instance.CharacterPanelIngameNeedUpdate = false;
    }


    public void UpdateActionPanelUIQ(List<ActionContainer> listActionContainers)
    {
        if (GameManager.Instance == null) { return; }
        if (goActionPanelQ == null) { return; }

        // Clear
        if (goActionPanelQ.GetComponentsInChildren<Image>().Length > 0)
        {
            foreach (Image ActionPanel in goActionPanelQ.GetComponentsInChildren<Image>())
            {
                Destroy(goActionPanelQ.gameObject);
            }
        }

        // Actions
        Debug.Log("Nb actions = " + listActionContainers.Count);
        for (int i = 0; i < listActionContainers.Count; i++)
        {
            Debug.Log("Num action = " + i);
            GameObject goAction = Instantiate(baseActionImage, goActionPanelQ.transform);
            goAction.name = listActionContainers[i].strName;

            goAction.GetComponent<RectTransform>().position = (Input.mousePosition);

            Button btn = goAction.GetComponent<Button>();

            int n = i;
            btn.onClick.AddListener(() => { listActionContainers[n].action(); });

            btn.GetComponentInChildren<Text>().text = listActionContainers[i].strName;
            //btn.onClick.AddListener(() => { action(n); });
        }
    }

    public void action(int index)
    {
        GameManager.Instance.listOfActions[index].action.Invoke();

        // Handle click on a actionnable
        GameManager.Instance.listOfActions.Clear();

        // Handle click on anything else
        GameManager.Instance.ActionPanelNeedUpdate = true;
    }

    // TODO: @Rémi bouton à corriger (on ne doit pas pouvoir cliquer 2x de suite)
    public void EndTurn()
    {
        if (!isTurnEnding)
        {
            AnimateButtonOnClick();
            EventManager.EndTurnEvent();
        }
    }
    
    
    private void AnimateButtonOnClick()
    {
        // Activation de l'animation au moment du click
        Animator anim_button = TurnButton.GetComponent<Animator>();
  
        anim_button.speed = buttonRotationSpeed;
        anim_button.enabled = true;
        isTurnEnding = false;
    }  
}

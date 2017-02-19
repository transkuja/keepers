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
    public GameObject ActionPanel;
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

            if (GameManager.Instance.ActionPanelNeedUpdate)
            {
                UpdateActionPanelUI();
            }
        }
    }

    void UpdateCharacterPanelUI()
    {
        if (GameManager.Instance == null){ return; }
        if (CharacterPanel == null) { return; }

        // Clear
        if (CharacterPanel.GetComponentsInChildren<Image>().Length > 0)
        {
            foreach (Image characterImage in CharacterPanel.GetComponentsInChildren<Image>())
            {
                Destroy(characterImage.gameObject);
            }
        }

        // On selection
        float margeX = 60.0f;
        float margeY = 70.0f;
        float offsetY = 20.0f;
  
        int nbCaracters = GameManager.Instance.AllKeepersList.Count;
        for (int i = 0; i < nbCaracters; i++)
        {
            KeeperInstance currentSelectedCharacter = GameManager.Instance.AllKeepersList[i];

            Sprite associatedSprite = currentSelectedCharacter.Keeper.AssociatedSprite;
            if (associatedSprite != null)
            {
                GameObject goKeeper = Instantiate(baseCharacterImage, CharacterPanel.transform);
                goKeeper.name = currentSelectedCharacter.Keeper.CharacterName;
                goKeeper.GetComponent<Image>().sprite = associatedSprite;

                float value = margeY + (offsetY * (i)) + ((goKeeper.GetComponent<Image>().rectTransform.rect.height) * (i));
                goKeeper.transform.localPosition = new Vector3(value, -margeX, 0.0f);
                goKeeper.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }

        GameManager.Instance.CharacterPanelIngameNeedUpdate = false;
    }

    void UpdateActionPanelUI()
    {
        if (GameManager.Instance == null) { return; }
        if (ActionPanel == null) { return; }

        // Clear
        if (ActionPanel.GetComponentsInChildren<Image>().Length > 0)
        {
            foreach (Image ActionPanel in ActionPanel.GetComponentsInChildren<Image>())
            {
                Destroy(ActionPanel.gameObject);
            }
        }


        float margeX = 0.0f;
        float margeY = 0.0f;
        float offsetY = 10.0f;

        // Actions
        for (int i = 0; i < GameManager.Instance.listOfActions.Count; i++)
        {
            GameObject goAction = Instantiate(baseActionImage, ActionPanel.transform);
            goAction.name = GameManager.Instance.listOfActions[i].ActionName;
            goAction.GetComponent<Image>().sprite = GameManager.Instance.listOfActions[i].ActionSprite;

            // Wait what !
            int n = i;
            GameManager.Instance.listOfActions[i].TypeAction = GameManager.Instance.listOfActions[i].TypeAction;
            goAction.AddComponent<Button>().onClick.AddListener(() => { action(n); });

            float value = margeY + (offsetY * (i)) + ((goAction.GetComponent<Image>().rectTransform.rect.height) * (i));
            goAction.transform.localPosition = new Vector3(margeX, +value, 0.0f);
            goAction.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        GameManager.Instance.ActionPanelNeedUpdate = false;
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

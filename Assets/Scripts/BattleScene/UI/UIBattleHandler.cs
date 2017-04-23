using UnityEngine;
using UnityEngine.UI;
using Behaviour;
using System.Collections.Generic;

public enum BattleUIButtons { SkillsPanel, ThrowDice, EscapeButton, SkillName, CharactersPanel }
public enum UIBattleState { WaitForDiceThrow, DiceRolling, WaitForDiceThrowValidation, Actions, SkillsOpened, TargetSelection, Disabled }
public enum CharactersPanelChildren { Avatar, LifeBar, Attributes }
public enum AttributesChildren { Attack, Defense, Magic, Support }
public enum LifeBarChildren { Remaining, Text }

public class UIBattleHandler : MonoBehaviour {

    private bool[] occupiedCharacterPanelIndex;

    [SerializeField]
    private GameObject skillsButtons;
    [SerializeField]
    private GameObject throwDiceButton;
    [SerializeField]
    private GameObject escapeBattleButton;
    [SerializeField]
    private GameObject skillName;
    [SerializeField]
    private GameObject charactersPanel;

    [Header("Hidden in battle")]
    [SerializeField]
    private GameObject endTurnButton;
    [SerializeField]
    private GameObject shortcutButton;

    private Dictionary<PawnInstance, Transform> associatedCharacterPanel = new Dictionary<PawnInstance, Transform>();

    public GameObject SkillName
    {
        get
        {
            return skillName;
        }

        set
        {
            skillName = value;
        }
    }

    public bool[] OccupiedCharacterPanelIndex
    {
        get
        {
            return occupiedCharacterPanelIndex;
        }

        set
        {
            occupiedCharacterPanelIndex = value;
        }
    }

    void OnEnable()
    {
        if (endTurnButton == null)
            Debug.LogWarning("End turn button reference not set in UIBattleHandler.");
        else
            endTurnButton.SetActive(false);

        if (shortcutButton == null)
            Debug.LogWarning("Shortcut button reference not set in UIBattleHandler (top left button).");
        else
            shortcutButton.SetActive(false);

        BattleHandler.EnableMonstersLifeBars();
        ChangeState(UIBattleState.WaitForDiceThrow);
    }

    void OnDisable()
    {
        if (endTurnButton == null)
            Debug.LogWarning("End turn button reference not set in UIBattleHandler.");
        else
            endTurnButton.SetActive(true);

        if (shortcutButton == null)
            Debug.LogWarning("Shortcut button reference not set in UIBattleHandler (top left button).");
        else
            shortcutButton.SetActive(true);

        BattleHandler.DisableMonstersLifeBars();
        associatedCharacterPanel.Clear();
        ChangeState(UIBattleState.Disabled);
    }

    public void PressAttackButton()
    {
        // Retrieve current character from CharacterManager to process attack
    }

    public void PressSkillsButton()
    {
        // Retrieve skills list and print them on UI (disable all buttons, enable Skills buttons)
        ChangeState(UIBattleState.SkillsOpened);

    }

    public void PressGuardButton()
    {
        // Retrieve current character from CharacterManager to process guard
    }

    public void ChangeState(UIBattleState newState)
    {
        switch(newState)
        {
            case UIBattleState.WaitForDiceThrow:
                throwDiceButton.GetComponent<Button>().interactable = true;
                throwDiceButton.GetComponent<ThrowDiceButtonFeedback>().enabled = true;
                throwDiceButton.transform.parent.GetComponent<Image>().enabled = true;

                escapeBattleButton.GetComponent<Button>().interactable = true;
                throwDiceButton.SetActive(true);
                escapeBattleButton.SetActive(true);
                charactersPanel.SetActive(true);
                break;

            case UIBattleState.Actions:
                throwDiceButton.GetComponent<Button>().interactable = false;
                throwDiceButton.GetComponent<ThrowDiceButtonFeedback>().enabled = false;
                throwDiceButton.transform.parent.GetComponent<Image>().enabled = false;
                escapeBattleButton.GetComponent<Button>().interactable = false;
                break;

            case UIBattleState.Disabled:
                throwDiceButton.SetActive(false);
                escapeBattleButton.SetActive(false);
                charactersPanel.SetActive(false);
                break;

        }
    }

    public void EscapeBattle()
    {
        BattleHandler.HandleBattleDefeat();
        GameManager.Instance.CurrentState = GameState.Normal;
    }

    public void CharacterPanelInit(PawnInstance _pawnInstanceForInit)
    {
        int initIndex = 0;
        for (int i = 0; i < 3; i++)
        {
            if (_pawnInstanceForInit.GetComponent<Prisoner>())
            {
                initIndex = 2;
                break;
            }
            if (occupiedCharacterPanelIndex[i] == false)
            {
                initIndex = i;
                break;
            }
        }

        Transform characterPanel = charactersPanel.transform.GetChild(initIndex).GetChild(0);
        characterPanel.GetChild((int)CharactersPanelChildren.Avatar).GetComponent<Image>().sprite = _pawnInstanceForInit.Data.AssociatedSprite;

        Mortal mortalComponent = _pawnInstanceForInit.GetComponent<Mortal>();
        Image lifeBarImg = characterPanel.GetChild((int)CharactersPanelChildren.LifeBar).GetChild((int)LifeBarChildren.Remaining).GetComponent<Image>();
        lifeBarImg.fillAmount = mortalComponent.CurrentHp / (float)mortalComponent.Data.MaxHp;
        if (lifeBarImg.fillAmount < 0.33f)
        {
            lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spriteOrangeLifeBar;
        }
        else
        {
            lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spriteGreenLifeBar;
        }
        characterPanel.GetChild((int)CharactersPanelChildren.LifeBar).GetChild((int)LifeBarChildren.Text).GetComponent<Text>().text = mortalComponent.CurrentHp + " / " + mortalComponent.Data.MaxHp;

        Fighter fighterComponent = _pawnInstanceForInit.GetComponent<Fighter>();

        characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetChild((int)AttributesChildren.Attack).GetComponentInChildren<Text>().text = fighterComponent.PhysicalSymbolStored.ToString();
        characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetChild((int)AttributesChildren.Defense).GetComponentInChildren<Text>().text = fighterComponent.DefensiveSymbolStored.ToString();
        characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetChild((int)AttributesChildren.Magic).GetComponentInChildren<Text>().text = fighterComponent.MagicalSymbolStored.ToString();
        characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetChild((int)AttributesChildren.Support).GetComponentInChildren<Text>().text = fighterComponent.SupportSymbolStored.ToString();

        occupiedCharacterPanelIndex[initIndex] = true;
        associatedCharacterPanel.Add(_pawnInstanceForInit, characterPanel);
    }

    public void UpdateLifeBar(Mortal _toUpdate)
    {
        foreach (Transform child in _toUpdate.transform)
        {
            if (child.CompareTag("FeedbackTransform"))
            {
                GameObject lifeBar = child.GetChild(0).GetChild(1).gameObject;
                Image lifeBarImg = lifeBar.transform.GetChild((int)LifeBarChildren.Remaining).GetComponent<Image>();
                lifeBarImg.fillAmount = _toUpdate.CurrentHp / (float)_toUpdate.MaxHp;
                if (lifeBarImg.fillAmount < 0.33f)
                {
                    lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spriteOrangeLifeBar;
                }
                else
                {
                    lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spriteGreenLifeBar;
                }
                break;
            }
        }
    }

    public void UpdateAttributesStocks(Fighter _toUpdate)
    {
        Transform attributes = associatedCharacterPanel[_toUpdate.GetComponent<PawnInstance>()].GetChild((int)CharactersPanelChildren.Attributes);
        attributes.GetChild((int)AttributesChildren.Attack).GetComponentInChildren<Text>().text = _toUpdate.PhysicalSymbolStored.ToString();
        attributes.GetChild((int)AttributesChildren.Defense).GetComponentInChildren<Text>().text = _toUpdate.DefensiveSymbolStored.ToString();
        attributes.GetChild((int)AttributesChildren.Magic).GetComponentInChildren<Text>().text = _toUpdate.MagicalSymbolStored.ToString();
        attributes.GetChild((int)AttributesChildren.Support).GetComponentInChildren<Text>().text = _toUpdate.SupportSymbolStored.ToString();
    }

    public void UpdateCharacterLifeBar(Mortal _toUpdate)
    {
        Transform panelToUpdate = associatedCharacterPanel[_toUpdate.GetComponent<PawnInstance>()];
        Image lifeBarImg = panelToUpdate.GetChild((int)CharactersPanelChildren.LifeBar).GetChild((int)LifeBarChildren.Remaining).GetComponent<Image>();
        lifeBarImg.fillAmount = _toUpdate.CurrentHp / (float) _toUpdate.Data.MaxHp;
        if (lifeBarImg.fillAmount < 0.33f)
        {
            lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spriteOrangeLifeBar;
        }
        else
        {
            lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spriteGreenLifeBar;
        }
        panelToUpdate.GetChild((int)CharactersPanelChildren.LifeBar).GetChild((int)LifeBarChildren.Text).GetComponent<Text>().text = _toUpdate.CurrentHp + " / " + _toUpdate.Data.MaxHp;
    }
}

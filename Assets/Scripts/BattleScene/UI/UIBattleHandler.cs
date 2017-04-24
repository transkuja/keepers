using UnityEngine;
using UnityEngine.UI;
using Behaviour;
using System.Collections.Generic;
using System;

public enum BattleUIChildren { SkillsPanel, ThrowDice, EscapeButton, SkillName, CharactersPanel }
public enum UIBattleState { WaitForDiceThrow, DiceRolling, WaitForDiceThrowValidation, Actions, SkillsOpened, TargetSelection, Disabled }
public enum CharactersPanelChildren { Avatar, LifeBar, Attributes }
public enum AttributesChildren { Attack, Defense, Magic, Support }
public enum LifeBarChildren { Remaining, Text }
public enum SkillButtonChildren { SkillName, Atk, Def, Mag, Support }

public class UIBattleHandler : MonoBehaviour {

    private bool[] occupiedCharacterPanelIndex;

    [SerializeField]
    private GameObject skillsPanels;
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
    private Dictionary<PawnInstance, Transform> associatedSkillsPanel = new Dictionary<PawnInstance, Transform>();

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

    public Transform GetCharacterPanelIndex(PawnInstance _fromPawn)
    {
        return associatedCharacterPanel[_fromPawn];
    }

    public Transform GetSkillsPanelIndex(PawnInstance _fromPawn)
    {
        return associatedSkillsPanel[_fromPawn];
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
        associatedSkillsPanel.Clear();
        foreach (Transform characterPan in associatedCharacterPanel.Values)
        {
            characterPan.gameObject.SetActive(false);
        }
        for (int i = 0; i < charactersPanel.transform.childCount; i++)
            charactersPanel.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
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
        characterPanel.GetChild((int)CharactersPanelChildren.Avatar).GetChild(0).GetComponent<Image>().sprite = _pawnInstanceForInit.Data.AssociatedSprite;

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
        characterPanel.gameObject.SetActive(true);
        associatedCharacterPanel.Add(_pawnInstanceForInit, characterPanel);
        SkillsPanelInit(_pawnInstanceForInit, initIndex);
    }

    private void SkillsPanelInit(PawnInstance _pawnInstanceForInit, int _index)
    {
        Transform skillsPanel = skillsPanels.transform.GetChild(_index);
        Fighter fighterComponent = _pawnInstanceForInit.GetComponent<Fighter>();

        if (fighterComponent.BattleSkills != null && fighterComponent.BattleSkills.Count > 0)
        {
            for (int i = 0; i < fighterComponent.BattleSkills.Count && i < 4; i++)
            {
                Transform currentSkill = skillsPanel.GetChild(i);
                currentSkill.GetChild((int)SkillButtonChildren.SkillName).GetComponent<SkillContainer>().SkillData = fighterComponent.BattleSkills[i];
                currentSkill.GetChild((int)SkillButtonChildren.SkillName).GetComponent<Text>().text = fighterComponent.BattleSkills[i].SkillName;
                currentSkill.GetComponent<SkillDescriptionUI>().SkillDescription = fighterComponent.BattleSkills[i].Description;
                foreach (Face face in fighterComponent.BattleSkills[i].Cost)
                {
                    if (face.Type == FaceType.Physical)
                        currentSkill.GetChild((int)SkillButtonChildren.Atk).GetComponentInChildren<Text>().text = face.Value.ToString();

                    if (face.Type == FaceType.Defensive)
                        currentSkill.GetChild((int)SkillButtonChildren.Def).GetComponentInChildren<Text>().text = face.Value.ToString();

                    if (face.Type == FaceType.Magical)
                        currentSkill.GetChild((int)SkillButtonChildren.Mag).GetComponentInChildren<Text>().text = face.Value.ToString();

                    if (face.Type == FaceType.Support)
                        currentSkill.GetChild((int)SkillButtonChildren.Support).GetComponentInChildren<Text>().text = face.Value.ToString();
                }
                currentSkill.gameObject.SetActive(true);
            }
        }
        associatedSkillsPanel.Add(_pawnInstanceForInit, skillsPanel);
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

    public void UpdateAvatar(PawnInstance _toUpdate, bool _enableHighlightFeedback)
    {
        if (!associatedCharacterPanel.ContainsKey(_toUpdate))
            return;

        if (_toUpdate.GetComponent<Fighter>().HasPlayedThisTurn)
        {
                        
        }
        else
        {
            associatedCharacterPanel[_toUpdate].GetChild((int)CharactersPanelChildren.Avatar).GetChild(0).GetComponent<Image>().sprite = _toUpdate.Data.AssociatedSprite;
        }

        associatedCharacterPanel[_toUpdate].GetChild((int)CharactersPanelChildren.Avatar).GetComponent<Image>().enabled = _enableHighlightFeedback;
    }

    public void UpdateAttributesStocks(Fighter _toUpdate)
    {
        Transform attributes = associatedCharacterPanel[_toUpdate.GetComponent<PawnInstance>()].GetChild((int)CharactersPanelChildren.Attributes);
        Transform atkAttribute = attributes.GetChild((int)AttributesChildren.Attack);
        Transform defAttribute = attributes.GetChild((int)AttributesChildren.Defense);
        Transform magAttribute = attributes.GetChild((int)AttributesChildren.Magic);
        Transform supAttribute = attributes.GetChild((int)AttributesChildren.Support);

        int diffAtk = _toUpdate.PhysicalSymbolStored - Int32.Parse(atkAttribute.GetComponentInChildren<Text>().text);
        if (diffAtk < 0)
        {
            GameObject atkAscFeedback = Instantiate(GameManager.Instance.PrefabUIUtils.attributesAscFeedback, atkAttribute);
            atkAscFeedback.transform.localPosition = Vector3.zero;
            atkAscFeedback.GetComponent<AttributesAscFeedback>().FeedbackValue(diffAtk);
        }

        int diffDef = _toUpdate.DefensiveSymbolStored - Int32.Parse(defAttribute.GetComponentInChildren<Text>().text);
        if (diffDef < 0)
        {
            GameObject defAscFeedback = Instantiate(GameManager.Instance.PrefabUIUtils.attributesAscFeedback, defAttribute);
            defAscFeedback.transform.localPosition = Vector3.zero;
            defAscFeedback.GetComponent<AttributesAscFeedback>().FeedbackValue(diffDef);
        }

        int diffMag = _toUpdate.MagicalSymbolStored - Int32.Parse(magAttribute.GetComponentInChildren<Text>().text);
        if (diffMag < 0)
        {
            GameObject magAscFeedback = Instantiate(GameManager.Instance.PrefabUIUtils.attributesAscFeedback, magAttribute);
            magAscFeedback.transform.localPosition = Vector3.zero;
            magAscFeedback.GetComponent<AttributesAscFeedback>().FeedbackValue(diffMag);
        }

        int diffSup = _toUpdate.SupportSymbolStored - Int32.Parse(supAttribute.GetComponentInChildren<Text>().text);
        if (diffSup < 0)
        {
            GameObject supAscFeedback = Instantiate(GameManager.Instance.PrefabUIUtils.attributesAscFeedback, supAttribute);
            supAscFeedback.transform.localPosition = Vector3.zero;
            supAscFeedback.GetComponent<AttributesAscFeedback>().FeedbackValue(diffSup);
        }

        atkAttribute.GetComponentInChildren<Text>().text = _toUpdate.PhysicalSymbolStored.ToString();
        defAttribute.GetComponentInChildren<Text>().text = _toUpdate.DefensiveSymbolStored.ToString();
        magAttribute.GetComponentInChildren<Text>().text = _toUpdate.MagicalSymbolStored.ToString();
        supAttribute.GetComponentInChildren<Text>().text = _toUpdate.SupportSymbolStored.ToString();
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

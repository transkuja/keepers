using UnityEngine;
using UnityEngine.UI;
using Behaviour;
using System.Collections.Generic;
using System;

public enum BattleUIChildren { SkillsPanel, ThrowDice, EscapeButton, SkillName, CharactersPanel }
public enum UIBattleState { WaitForDiceThrow, DiceRolling, WaitForDiceThrowValidation, Actions, SkillsOpened, TargetSelection, Disabled }
public enum CharactersPanelChildren { Avatar, LifeBar, Attributes }
public enum AttributesChildren { Attack, Defense, Magic }
public enum LifeBarChildren { Remaining, Text }
public enum SkillButtonChildren { SkillName, Atk, Def, Mag }
public enum SkillPanelChildren { Skill1, Skill2, Skill3, Skill4, CloseButton, CharacterAvatar }


public class UIBattleHandler : MonoBehaviour {

    private bool[] occupiedCharacterPanelIndex;
    private Color customGrey = new Color(0.6784f, 0.6784f, 0.6784f, 1);

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
    private Dictionary<Transform, PawnInstance> associatedCharacterPanelReversed = new Dictionary<Transform, PawnInstance>();

    private Dictionary<PawnInstance, Transform> associatedSkillsPanel = new Dictionary<PawnInstance, Transform>();

    public bool isCharactersPanelLocked = false;

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

    public GameObject SkillsPanels
    {
        get
        {
            return skillsPanels;
        }

        set
        {
            skillsPanels = value;
        }
    }

    public GameObject CharactersPanel
    {
        get
        {
            return charactersPanel;
        }

        set
        {
            charactersPanel = value;
        }
    }

    public Transform GetCharacterPanelIndex(PawnInstance _fromPawn)
    {
        return associatedCharacterPanel[_fromPawn];
    }

    public PawnInstance GetCharacterFromPanelIndex(Transform _fromPanel)
    {
        return associatedCharacterPanelReversed[_fromPanel];
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
        ResetUIBattle();
    }

    public void PressSkillsButton()
    {
        // Retrieve skills list and print them on UI (disable all buttons, enable Skills buttons)
        ChangeState(UIBattleState.SkillsOpened);

    }

    public void ChangeState(UIBattleState newState)
    {
        switch(newState)
        {
            case UIBattleState.WaitForDiceThrow:
                throwDiceButton.GetComponent<Button>().interactable = true;
                throwDiceButton.GetComponent<ThrowDiceButtonFeedback>().enabled = true;
                // Better without feedback? Activate in tuto
                //throwDiceButton.transform.parent.GetComponent<Image>().enabled = true;

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
        BattleHandler.ApplyEscapePenalty();
        BattleHandler.HandleBattleDefeat();
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
        characterPanel.GetChild((int)CharactersPanelChildren.Avatar).GetChild(0).GetComponent<Image>().sprite = _pawnInstanceForInit.Data.AssociatedSpriteForBattle;

        Mortal mortalComponent = _pawnInstanceForInit.GetComponent<Mortal>();
        Image lifeBarImg = characterPanel.GetChild((int)CharactersPanelChildren.LifeBar).GetChild((int)LifeBarChildren.Remaining).GetComponent<Image>();
        lifeBarImg.fillAmount = mortalComponent.CurrentHp / (float)mortalComponent.Data.MaxHp;
        lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spritePlayerGreenLifeBar;

        if (lifeBarImg.fillAmount < 0.33f)
        {
            lifeBarImg.color = Color.red;
        }
        else
        {
            lifeBarImg.color = Color.green;
        }
        characterPanel.GetChild((int)CharactersPanelChildren.LifeBar).GetChild((int)LifeBarChildren.Text).GetComponent<Text>().text = mortalComponent.CurrentHp + " / " + mortalComponent.Data.MaxHp;

        Fighter fighterComponent = _pawnInstanceForInit.GetComponent<Fighter>();

        characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetChild((int)AttributesChildren.Attack).GetComponentInChildren<Text>().text = fighterComponent.PhysicalSymbolStored.ToString();
        characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetChild((int)AttributesChildren.Defense).GetComponentInChildren<Text>().text = fighterComponent.DefensiveSymbolStored.ToString();
        characterPanel.GetChild((int)CharactersPanelChildren.Attributes).GetChild((int)AttributesChildren.Magic).GetComponentInChildren<Text>().text = fighterComponent.MagicalSymbolStored.ToString();

        occupiedCharacterPanelIndex[initIndex] = true;
        characterPanel.gameObject.SetActive(true);
        associatedCharacterPanel.Add(_pawnInstanceForInit, characterPanel);
        associatedCharacterPanelReversed.Add(characterPanel, _pawnInstanceForInit);
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

                SkillBattle fighterCurSkill;
                bool isDepressed = _pawnInstanceForInit.GetComponent<MentalHealthHandler>() != null && _pawnInstanceForInit.GetComponent<MentalHealthHandler>().IsDepressed;
                if (isDepressed && fighterComponent.DepressedSkills.Count == fighterComponent.BattleSkills.Count)
                    fighterCurSkill = fighterComponent.DepressedSkills[i];
                else
                    fighterCurSkill = fighterComponent.BattleSkills[i];

                currentSkill.GetChild((int)SkillButtonChildren.SkillName).GetComponent<SkillContainer>().SkillData = fighterCurSkill;
                currentSkill.GetChild((int)SkillButtonChildren.SkillName).GetComponentInChildren<Text>().text = fighterCurSkill.SkillName;
                currentSkill.GetComponent<SkillDescriptionUI>().SkillData = fighterCurSkill;

                currentSkill.GetChild((int)SkillButtonChildren.Atk).GetComponentInChildren<Text>().text = "0";
                currentSkill.GetChild((int)SkillButtonChildren.Def).GetComponentInChildren<Text>().text = "0";
                currentSkill.GetChild((int)SkillButtonChildren.Mag).GetComponentInChildren<Text>().text = "0";

                foreach (Face face in fighterCurSkill.Cost)
                {
                    if (face.Type == FaceType.Physical)
                        currentSkill.GetChild((int)SkillButtonChildren.Atk).GetComponentInChildren<Text>().text = face.Value.ToString();

                    if (face.Type == FaceType.Defensive)
                        currentSkill.GetChild((int)SkillButtonChildren.Def).GetComponentInChildren<Text>().text = face.Value.ToString();

                    if (face.Type == FaceType.Magical)
                        currentSkill.GetChild((int)SkillButtonChildren.Mag).GetComponentInChildren<Text>().text = face.Value.ToString();
                }
                currentSkill.gameObject.SetActive(true);
            }
        }

        skillsPanel.GetChild((int)SkillPanelChildren.CharacterAvatar).GetComponent<Image>().sprite = _pawnInstanceForInit.Data.AssociatedSpriteForBattle;
        associatedSkillsPanel.Add(_pawnInstanceForInit, skillsPanel);
    }

    public void TutoReloadSkillPanel(PawnInstance _pawnInstanceForReload)
    {
        Transform panelToReload = associatedSkillsPanel[_pawnInstanceForReload];
        Fighter fighterComponent = _pawnInstanceForReload.GetComponent<Fighter>();

        for (int i = 0; i < 4; i++)
            panelToReload.GetChild(i).gameObject.SetActive(false);

        if (fighterComponent.BattleSkills != null && fighterComponent.BattleSkills.Count > 0)
        {
            for (int i = 0; i < fighterComponent.BattleSkills.Count && i < 4; i++)
            {
                Transform currentSkill = panelToReload.GetChild(i);
                SkillBattle fighterCurSkill;
                bool isDepressed = _pawnInstanceForReload.GetComponent<MentalHealthHandler>() != null && _pawnInstanceForReload.GetComponent<MentalHealthHandler>().IsDepressed;
                if (isDepressed && fighterComponent.DepressedSkills.Count == fighterComponent.BattleSkills.Count)
                    fighterCurSkill = fighterComponent.DepressedSkills[i];
                else
                    fighterCurSkill = fighterComponent.BattleSkills[i];

                currentSkill.GetChild((int)SkillButtonChildren.SkillName).GetComponent<SkillContainer>().SkillData = fighterCurSkill;
                currentSkill.GetChild((int)SkillButtonChildren.SkillName).GetComponentInChildren<Text>().text = fighterCurSkill.SkillName;
                currentSkill.GetComponent<SkillDescriptionUI>().SkillData = fighterCurSkill;
                foreach (Face face in fighterCurSkill.Cost)
                {
                    if (face.Type == FaceType.Physical)
                        currentSkill.GetChild((int)SkillButtonChildren.Atk).GetComponentInChildren<Text>().text = face.Value.ToString();

                    if (face.Type == FaceType.Defensive)
                        currentSkill.GetChild((int)SkillButtonChildren.Def).GetComponentInChildren<Text>().text = face.Value.ToString();

                    if (face.Type == FaceType.Magical)
                        currentSkill.GetChild((int)SkillButtonChildren.Mag).GetComponentInChildren<Text>().text = face.Value.ToString();
                }
                currentSkill.gameObject.SetActive(true);
            }
        }
        panelToReload.GetChild((int)SkillPanelChildren.CharacterAvatar).GetComponent<Image>().sprite = _pawnInstanceForReload.Data.AssociatedSpriteForBattle;
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
                lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spriteMonsterGreenLifeBar;

                if (lifeBarImg.fillAmount < 0.33f)
                {
                    lifeBarImg.color = Color.red;
                }
                else
                {
                    lifeBarImg.color = Color.green;
                }
                break;
            }
        }
    }

    public void UpdateAvatar(PawnInstance _toUpdate, bool _enableHighlightFeedback)
    {
        if (GameManager.Instance.CurrentState == GameState.InTuto)
            return;

        bool enableHighlightFeedback = false;
        if (!associatedCharacterPanel.ContainsKey(_toUpdate))
            return;

        if (!BattleHandler.HasDiceBeenThrown)
        {
            enableHighlightFeedback = false;
        }
        else
        {
            if (_toUpdate.GetComponent<Fighter>().HasPlayedThisTurn)
            {
                enableHighlightFeedback = false;
            }
            else
            {
                if (GameManager.Instance.ListOfSelectedKeepers != null && GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                    enableHighlightFeedback = !(GameManager.Instance.GetFirstSelectedKeeper() == _toUpdate);
                else
                    enableHighlightFeedback = true;
                associatedCharacterPanel[_toUpdate].GetChild((int)CharactersPanelChildren.Avatar).GetChild(0).GetComponent<Image>().sprite = _toUpdate.Data.AssociatedSpriteForBattle;
            }
        }
        associatedCharacterPanel[_toUpdate].GetChild((int)CharactersPanelChildren.Avatar).GetComponent<Image>().enabled = enableHighlightFeedback;
        associatedCharacterPanel[_toUpdate].GetChild((int)CharactersPanelChildren.Avatar).GetComponentInChildren<Button>().interactable = enableHighlightFeedback && !isCharactersPanelLocked;
        associatedCharacterPanel[_toUpdate].GetChild((int)CharactersPanelChildren.Avatar).GetComponentInParent<Button>().interactable = enableHighlightFeedback && !isCharactersPanelLocked;
    }

    public void UpdateAttributesStocks(Fighter _toUpdate)
    {
        Transform attributes = associatedCharacterPanel[_toUpdate.GetComponent<PawnInstance>()].GetChild((int)CharactersPanelChildren.Attributes);
        Transform atkAttribute = attributes.GetChild((int)AttributesChildren.Attack);
        Transform defAttribute = attributes.GetChild((int)AttributesChildren.Defense);
        Transform magAttribute = attributes.GetChild((int)AttributesChildren.Magic);

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

        atkAttribute.GetComponentInChildren<Text>().text = _toUpdate.PhysicalSymbolStored.ToString();
        defAttribute.GetComponentInChildren<Text>().text = _toUpdate.DefensiveSymbolStored.ToString();
        magAttribute.GetComponentInChildren<Text>().text = _toUpdate.MagicalSymbolStored.ToString();

        float atkFillAmount = (_toUpdate.PhysicalSymbolStored / (float)Fighter.StockMaxValue);
        atkAttribute.GetComponentInChildren<Image>().fillAmount = atkFillAmount;
        if (atkFillAmount == 1.0f)
            atkAttribute.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteFillAtkFull;
        else
            atkAttribute.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteFillAtk;

        float defFillAmount = (_toUpdate.DefensiveSymbolStored / (float)Fighter.StockMaxValue);
        defAttribute.GetComponentInChildren<Image>().fillAmount = defFillAmount;
        if (defFillAmount == 1.0f)
            defAttribute.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteFillDefFull;
        else
            defAttribute.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteFillDef;

        float magFillAmount = (_toUpdate.MagicalSymbolStored / (float)Fighter.StockMaxValue);
        magAttribute.GetComponentInChildren<Image>().fillAmount = magFillAmount;
        if (magFillAmount == 1.0f)
            magAttribute.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteFillMagicFull;
        else
            magAttribute.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteFillMagic;
    }

    private void ResetAttributesGauges()
    {
        foreach (PawnInstance pi in associatedCharacterPanel.Keys)
        {
            Transform attributes = associatedCharacterPanel[pi].GetChild((int)CharactersPanelChildren.Attributes);
            Transform atkAttribute = attributes.GetChild((int)AttributesChildren.Attack);
            Transform defAttribute = attributes.GetChild((int)AttributesChildren.Defense);
            Transform magAttribute = attributes.GetChild((int)AttributesChildren.Magic);
            
            atkAttribute.GetComponentInChildren<Text>().text = "0";
            defAttribute.GetComponentInChildren<Text>().text = "0";
            magAttribute.GetComponentInChildren<Text>().text = "0";

            atkAttribute.GetComponentInChildren<Image>().fillAmount = 0.0f;        
            atkAttribute.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteFillAtk;

            defAttribute.GetComponentInChildren<Image>().fillAmount = 0.0f;
            defAttribute.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteFillDef;

            magAttribute.GetComponentInChildren<Image>().fillAmount = 0.0f;
            magAttribute.GetComponentInChildren<Image>().sprite = GameManager.Instance.SpriteUtils.spriteFillMagic;
        }
    }

    public void UpdateCharacterLifeBar(Mortal _toUpdate)
    {
        Transform panelToUpdate = associatedCharacterPanel[_toUpdate.GetComponent<PawnInstance>()];
        Image lifeBarImg = panelToUpdate.GetChild((int)CharactersPanelChildren.LifeBar).GetChild((int)LifeBarChildren.Remaining).GetComponent<Image>();
        lifeBarImg.fillAmount = _toUpdate.CurrentHp / (float) _toUpdate.Data.MaxHp;
        lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spritePlayerGreenLifeBar;

        if (lifeBarImg.fillAmount < 0.33f)
        {
            lifeBarImg.color = Color.red;
        }
        else
        {
            lifeBarImg.color = Color.green;
        }
        panelToUpdate.GetChild((int)CharactersPanelChildren.LifeBar).GetChild((int)LifeBarChildren.Text).GetComponent<Text>().text = _toUpdate.CurrentHp + " / " + _toUpdate.Data.MaxHp;
    }

    public void CloseSkillsPanel()
    {
        GameManager.Instance.ClearListKeeperSelected();
        Cursor.SetCursor(GameManager.Instance.Texture2DUtils.iconeMouse, Vector2.zero, CursorMode.Auto);
        BattleHandler.ActivateFeedbackSelection(true, false);
        BattleHandler.DeactivateFeedbackSelection(false, true);
    }

    private void ResetUIBattle()
    {
        // Restore board state UI
        if (endTurnButton == null)
            Debug.LogWarning("End turn button reference not set in UIBattleHandler.");
        else
            endTurnButton.SetActive(true);

        if (shortcutButton == null)
            Debug.LogWarning("Shortcut button reference not set in UIBattleHandler (top left button).");
        else
            shortcutButton.SetActive(true);

        BattleHandler.DisableMonstersLifeBars();

        // Clear association tables and characters panel
        associatedSkillsPanel.Clear();
        foreach (Transform characterPan in associatedCharacterPanel.Values)
        {
            characterPan.gameObject.SetActive(false);
        }
        for (int i = 0; i < charactersPanel.transform.childCount; i++)
            charactersPanel.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);

        ResetAttributesGauges();

        associatedCharacterPanel.Clear();
        associatedCharacterPanelReversed.Clear();

        // Reset skills panel
        for (int i = 0; i < skillsPanels.transform.childCount; i++)
        {
            Transform currentSkillsPanel = skillsPanels.transform.GetChild(i);

            for (int j = 0; j < 4; j++)
            {
                Transform currentSkill = currentSkillsPanel.GetChild(j);
                currentSkill.GetChild((int)SkillButtonChildren.SkillName).GetComponent<SkillContainer>().SkillData = null;
                currentSkill.GetChild((int)SkillButtonChildren.SkillName).GetComponentInChildren<Text>().text = "";
                currentSkill.GetComponent<SkillDescriptionUI>().SkillData = null;

                currentSkill.GetChild((int)SkillButtonChildren.Atk).GetComponentInChildren<Text>().text = "0";
                currentSkill.GetChild((int)SkillButtonChildren.Def).GetComponentInChildren<Text>().text = "0";
                currentSkill.GetChild((int)SkillButtonChildren.Mag).GetComponentInChildren<Text>().text = "0";

                currentSkill.GetChild((int)SkillButtonChildren.Atk).GetComponentInChildren<Text>().color = customGrey;
                currentSkill.GetChild((int)SkillButtonChildren.Def).GetComponentInChildren<Text>().color = customGrey;
                currentSkill.GetChild((int)SkillButtonChildren.Mag).GetComponentInChildren<Text>().color = customGrey;

                currentSkill.gameObject.SetActive(false);
            }
        }

        // Disable UI battle
        ChangeState(UIBattleState.Disabled);
    }

    public void LockCharactersPanelButtons()
    {
        for (int i = 0; i < charactersPanel.transform.childCount; i++)
        {
            Button[] buttons = charactersPanel.transform.GetChild(i).GetComponentsInChildren<Button>();
            for (int j = 0; j < buttons.Length; j++)
            {
                buttons[j].interactable = false;
            }
        }
        isCharactersPanelLocked = true;
    }

    public void UnlockCharactersPanelButtons()
    {
        isCharactersPanelLocked = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleUIButtons { MainButtons, Skills, ThrowDice, ValidateThrow }
public enum UIBattleState { WaitForDiceThrow, DiceRolling, WaitForDiceThrowValidation, Actions, SkillsOpened, TargetSelection, Disabled }

public class UIBattleHandler : MonoBehaviour {

    [SerializeField]
    private GameObject mainButtons;
    [SerializeField]
    private GameObject skillsButtons;
    [SerializeField]
    private GameObject throwDiceButton;
    [SerializeField]
    private GameObject sendDiceResultsButton;
    [SerializeField]
    private GameObject targetSelectionInfo;

    [Header("Hidden in battle")]
    [SerializeField]
    private GameObject endTurnButton;
    [SerializeField]
    private GameObject shortcutButton;

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

    public void PressRunButton()
    {
        // Run process here
    }

    public void ChangeState(UIBattleState newState)
    {
        switch(newState)
        {
            case UIBattleState.Actions:
                mainButtons.SetActive(true);
                skillsButtons.SetActive(false);
                sendDiceResultsButton.SetActive(false);
                throwDiceButton.SetActive(false);
                break;
            case UIBattleState.Disabled:
                mainButtons.SetActive(false);
                skillsButtons.SetActive(false);
                sendDiceResultsButton.SetActive(false);
                throwDiceButton.SetActive(false);
                break;
            case UIBattleState.SkillsOpened:
                mainButtons.SetActive(false);
                skillsButtons.SetActive(true);
                break;
            case UIBattleState.WaitForDiceThrow:
                mainButtons.SetActive(false);
                skillsButtons.SetActive(false);
                sendDiceResultsButton.SetActive(false);
                throwDiceButton.SetActive(true);
                break;
            case UIBattleState.WaitForDiceThrowValidation:
                sendDiceResultsButton.SetActive(true);
                break;
            case UIBattleState.DiceRolling:
                mainButtons.SetActive(false);
                skillsButtons.SetActive(false);
                throwDiceButton.SetActive(false);
                break;
            case UIBattleState.TargetSelection:
                targetSelectionInfo.SetActive(true);
                break;

        }
    }

    public GameObject SendDiceResultsButton
    {
        get
        {
            return sendDiceResultsButton;
        }

        set
        {
            sendDiceResultsButton = value;
        }
    }
}

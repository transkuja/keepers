using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleHandler : MonoBehaviour {

    [SerializeField]
    private GameObject mainButtons;
    [SerializeField]
    private GameObject skillsButtons;
    [SerializeField]
    private GameObject itemsButtons;
    [SerializeField]
    private GameObject switchButtons;

    public void PressAttackButton()
    {
        // Retrieve current character from CharacterManager to process attack
    }

    public void PressSkillsButton()
    {
        // Retrieve skills list and print them on UI (disable all buttons, enable Skills buttons)
        mainButtons.SetActive(false);
        skillsButtons.SetActive(true);

    }

    public void PressItemsButton()
    {
        // Retrieve items from GameManager? (disable all buttons, enable battle Items buttons)
        mainButtons.SetActive(false);
        itemsButtons.SetActive(true);
    }

    public void PressGuardButton()
    {
        // Retrieve current character from CharacterManager to process guard
    }

    public void PressSwitchButton()
    {
        // Retrieve characters on same tile from some Manager (disable all buttons, enable Switch buttons with Character names on it)
        mainButtons.SetActive(false);
        switchButtons.SetActive(true);

    }

    public void PressRunButton()
    {
        // Run process here
    }

}

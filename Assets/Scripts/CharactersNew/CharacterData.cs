using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData {

    [SerializeField]
    private string characterName = "CharacterBob";

    [SerializeField]
    private bool[] behaviours;

    [Header("UI")]
    [SerializeField]
    private Sprite associatedSprite;

    [SerializeField]
    private Sprite deadSprite;

    [SerializeField]
    public int inventorySlots = 4;

    [Header("Stats")]
    // TODO Change all these fields
    #region Ideas
    // int specialDicesUse = 3;
    // int numberOfDice = 4;
    // int diceMinValue = 1;
    // int diceMaxValue = 6;
    // DiceType diceType;
    // enum DiceType = { Offensive, Defensive, Support } => define DiceTypes + what they are used for
    #endregion
    [SerializeField]
    private int maxHp = 100;
    [SerializeField]
    private int maxMp = 50;
    [SerializeField]
    private short baseStrength = 5;
    [SerializeField]
    private short bonusStrength = 5;
    [SerializeField]
    private short baseDefense = 5;
    [SerializeField]
    private short bonusDefense = 5;
    [SerializeField]
    private short baseIntelligence = 5;
    [SerializeField]
    private short bonusIntelligence = 5;
    [SerializeField]
    private short baseSpirit = 5;
    [SerializeField]
    private short bonusSpirit = 5;

    [Header("Status")]
    [SerializeField]
    [Range(50, 120)]
    private short maxHunger = 100;

    [SerializeField]
    private short maxMentalHealth = 100;

    [SerializeField]
    private short maxActionPoints = 3;

    [Header("Battle data")]
    [SerializeField]
    private List<SkillBattle> battleSkills;

}

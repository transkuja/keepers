using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PawnData {

    [SerializeField]
    private string pawnName = "Jean-Joel";

    [SerializeField]
    private bool[] behaviours;

    // TODO externalize these fields in UI dedicated components
    [Header("UI")]
    [SerializeField]
    private Sprite associatedSprite;

    [SerializeField]
    private Sprite deadSprite;

    [SerializeField]
    private int inventorySlots = 4;

    // TODO externalize these fields in status dedicated components
    [Header("Status")]
    [SerializeField]
    [Range(50, 120)]
    private short maxHunger = 100;

    [SerializeField]
    private short maxMentalHealth = 100;

    [SerializeField]
    private short maxActionPoints = 3;

    [Header("Stats")]
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

    // TODO externalize these fields in battle dedicated components
    // TODO Change all these fields
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
    
    // TODO externalize this field in battle dedicated components
    [Header("Battle data")]
    [SerializeField]
    private List<SkillBattle> battleSkills;

    [SerializeField]
    private List<string> possibleDrops;

    #region Accessors
    public string PawnName
    {
        get
        {
            return pawnName;
        }

        set
        {
            pawnName = value;
        }
    }

    public int MaxHp
    {
        get
        {
            return maxHp;
        }

        set
        {
            maxHp = value;
        }
    }

    [System.Obsolete("This accessor will be removed when the real battle system will be implemented.")]
    public int MaxMp
    {
        get
        {
            return maxMp;
        }

        set
        {
            maxMp = value;
        }
    }

    [System.Obsolete("This accessor will be removed when the real battle system will be implemented.")]
    public short BaseStrength
    {
        get
        {
            return baseStrength;
        }

        set
        {
            baseStrength = value;
        }
    }

    [System.Obsolete("This accessor will be removed when the real battle system will be implemented.")]
    public short BonusStrength
    {
        get
        {
            return bonusStrength;
        }

        set
        {
            bonusStrength = value;
        }
    }

    [System.Obsolete("This accessor will be removed when the real battle system will be implemented.")]
    public short BaseDefense
    {
        get
        {
            return baseDefense;
        }

        set
        {
            baseDefense = value;
        }
    }

    [System.Obsolete("This accessor will be removed when the real battle system will be implemented.")]
    public short BonusDefense
    {
        get
        {
            return bonusDefense;
        }

        set
        {
            bonusDefense = value;
        }
    }

    [System.Obsolete("This accessor will be removed when the real battle system will be implemented.")]
    public short BaseIntelligence
    {
        get
        {
            return baseIntelligence;
        }

        set
        {
            baseIntelligence = value;
        }
    }

    [System.Obsolete("This accessor will be removed when the real battle system will be implemented.")]
    public short BonusIntelligence
    {
        get
        {
            return bonusIntelligence;
        }

        set
        {
            bonusIntelligence = value;
        }
    }

    [System.Obsolete("This accessor will be removed when the real battle system will be implemented.")]
    public short BaseSpirit
    {
        get
        {
            return baseSpirit;
        }

        set
        {
            baseSpirit = value;
        }
    }

    [System.Obsolete("This accessor will be removed when the real battle system will be implemented.")]
    public short BonusSpirit
    {
        get
        {
            return bonusSpirit;
        }

        set
        {
            bonusSpirit = value;
        }
    }

    public List<SkillBattle> BattleSkills
    {
        get
        {
            return battleSkills;
        }

        set
        {
            battleSkills = value;
        }
    }

    public bool[] Behaviours
    {
        get
        {
            return behaviours;
        }

        set
        {
            behaviours = value;
        }
    }

    public Sprite AssociatedSprite
    {
        get
        {
            return associatedSprite;
        }

        set
        {
            associatedSprite = value;
        }
    }

    public Sprite DeadSprite
    {
        get
        {
            return deadSprite;
        }

        set
        {
            deadSprite = value;
        }
    }

    public int InventorySlots
    {
        get
        {
            return inventorySlots;
        }

        set
        {
            inventorySlots = value;
        }
    }

    public short MaxHunger
    {
        get
        {
            return maxHunger;
        }

        set
        {
            maxHunger = value;
        }
    }

    public short MaxMentalHealth
    {
        get
        {
            return maxMentalHealth;
        }

        set
        {
            maxMentalHealth = value;
        }
    }

    public short MaxActionPoints
    {
        get
        {
            return maxActionPoints;
        }

        set
        {
            maxActionPoints = value;
        }
    }

    public List<string> PossibleDrops
    {
        get
        {
            return possibleDrops;
        }

        set
        {
            possibleDrops = value;
        }
    }
    #endregion


}

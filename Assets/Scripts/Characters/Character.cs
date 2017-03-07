using System.Collections.Generic;
using UnityEngine;

/*
 * Contains common attributes for Keepers and Monsters
 *
 */
public class Character {

    [SerializeField]
    private string characterName = "CharacterBob";

    [Header("Stats")]

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
    private List<SkillBattle> battleSkills;

    private List<GameObject> goListCharacterFollowing = new List<GameObject>();

    #region Accessors
    public string CharacterName
    {
        get { return characterName; }
        set { characterName = value; }
    }

   
    public List<SkillBattle> BattleSkills
    {
        get { return battleSkills; }
        set { battleSkills = value; }
    }

    public List<GameObject> GoListCharacterFollowing
    {
        get
        {
            return goListCharacterFollowing;
        }

        set
        {
            goListCharacterFollowing = value;
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
    public short BaseStrength
    {
        get
        {
            return baseStrength;
        }

        private set { }
    }

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

    public short BaseDefense
    {
        get
        {
            return baseDefense;
        }

        private set { }

    }

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

    public short BaseIntelligence
    {
        get
        {
            return baseIntelligence;
        }
        private set { }

    }

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

    public short BaseSpirit
    {
        get
        {
            return baseSpirit;
        }
        private set { }

    }

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

    public short GetEffectiveStrength()
    {
        short result = baseStrength;
        result += bonusStrength;
        return result;
    }

    public short GetEffectiveIntelligence()
    {
        short result = baseIntelligence;
        result += bonusIntelligence;
        return result;
    }

    public short GetEffectiveDefense()
    {
        short result = baseDefense;
        result += bonusDefense;
        return result;
    }

    public short GetEffectiveSpirit()
    {
        short result = baseSpirit;
        result += bonusSpirit;
        return result;
    }
    #endregion
}

/*
 * Contains definition of battle skills 
 * 
 */
[System.Serializable]
public class SkillBattle
{

    private int damage;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
}


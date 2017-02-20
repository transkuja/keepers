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
    private int hp = 100;
    [SerializeField]
    private int mp = 50;
    [SerializeField]
    private short strength = 5;
    [SerializeField]
    private short defense = 5;
    [SerializeField]
    private short intelligence = 5;
    [SerializeField]
    private short spirit = 5;
    private List<SkillBattle> battleSkills;

    private List<GameObject> goListCharacterFollowing = new List<GameObject>();

    public string CharacterName
    {
        get { return characterName; }
        set { characterName = value; }
    }

    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public int Mp
    {
        get { return mp; }
        set { mp = value; }
    }

    public short Strength
    {
        get { return strength; }
        set { strength = value; }
    }

    public short Defense
    {
        get { return defense; }
        set { defense = value; }
    }

    public short Intelligence
    {
        get { return intelligence; }
        set { intelligence = value; }
    }

    public short Spirit
    {
        get { return spirit; }
        set { spirit = value; }
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


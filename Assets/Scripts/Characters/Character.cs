using System.Collections.Generic;

/*
 * Contains common attributes for Keepers and Monsters
 *
 */
public class Character {
    private string name;

    private int hp = 100;
    private int mp = 50;
    private short strength = 5;
    private short defense = 5;
    private short intelligence = 5;
    private short spirit = 5;
    private List<SkillBattle> battleSkills;

    public string Name
    {
        get { return name; }
        set { name = value; }
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

}

/*
 * Contains definition of battle skills 
 * 
 */
public class SkillBattle
{

    private int damage;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
}
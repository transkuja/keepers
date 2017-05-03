using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Behaviour;

public enum TargetType { FriendSingle, FoeSingle, FriendAll, FoeAll, Self }

/*
 * Contains definition of battle skills 
 */
[System.Serializable]
public class SkillBattle {

    public SkillBattle depressedVersion;

    [SerializeField]
    private Fighter skillUser;

    [SerializeField]
    private int damage;
    [SerializeField]
    private string skillName;
    [SerializeField]
    private string description;
    [SerializeField]
    private List<Face> cost = new List<Face>();
    [SerializeField]
    TargetType targetType;
    [SerializeField]
    BattleBoeuf[] boeufs;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public List<Face> Cost
    {
        get
        {
            return cost;
        }

        set
        {
            cost = value;
        }
    }

    public TargetType TargetType
    {
        get
        {
            return targetType;
        }

        set
        {
            targetType = value;
        }
    }

    public string SkillName
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

    public Fighter SkillUser
    {
        get
        {
            return skillUser;
        }

        set
        {
            skillUser = value;
        }
    }

    public SkillBattle DepressedVersion
    {
        get
        {
            return depressedVersion;
        }

        set
        {
            depressedVersion = value;
        }
    }

    public SkillBattle()
    {

    }

    public SkillBattle(SkillBattle _origin)
    {
        if (_origin == null)
        {
            new SkillBattle();
        }
        else
        {
            skillUser = _origin.skillUser;
            damage = _origin.damage;

            skillName = _origin.skillName;
            description = _origin.description;
            cost = _origin.cost;
            targetType = _origin.targetType;

            depressedVersion = new SkillBattle(_origin.depressedVersion);
        }
    }

    public bool CanUseSkill()
    {
        foreach (Face f in cost)
        {
            if (f.Type == FaceType.Physical && skillUser.PhysicalSymbolStored < f.Value)
                return false;
            if (f.Type == FaceType.Magical && skillUser.MagicalSymbolStored < f.Value)
                return false;

            if (f.Type == FaceType.Defensive && skillUser.DefensiveSymbolStored < f.Value)
                return false;
        }
        return true;
    }

    public void UseSkill(PawnInstance _target)
    {
        foreach (Face f in cost)
        {
            if (f.Type == FaceType.Physical)
                skillUser.PhysicalSymbolStored -= f.Value;
            if (f.Type == FaceType.Magical)
                skillUser.MagicalSymbolStored -= f.Value;

            if (f.Type == FaceType.Defensive)
                skillUser.DefensiveSymbolStored -= f.Value;
        }

        GameObject skillNameUI = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName;
        skillNameUI.transform.GetComponentInChildren<Text>().text = skillName;
        skillNameUI.SetActive(true);

        int effectiveDamage = damage;

        foreach (BattleBoeuf boeuf in skillUser.EffectiveBoeufs)
        {
            if (boeuf.BoeufType == BoeufType.Damage)
                effectiveDamage += boeuf.EffectValue;
        }

        if (targetType == TargetType.FoeAll)
        {
            for (int i = 0; i < BattleHandler.CurrentBattleMonsters.Length; i++)
            {
                Fighter curFighter = BattleHandler.CurrentBattleMonsters[i].GetComponent<Fighter>();
                curFighter.IsWaitingForDmgFeedback = true;
                curFighter.IsWaitingForSkillPanelToClose = true;
                curFighter.PendingDamage = effectiveDamage;
                curFighter.EffectiveBoeufs.AddRange(boeufs);
            }
        }
        _target.GetComponent<Fighter>().IsWaitingForDmgFeedback = true;
        _target.GetComponent<Fighter>().IsWaitingForSkillPanelToClose = true;
        _target.GetComponent<Fighter>().PendingDamage = effectiveDamage;
        _target.GetComponent<Fighter>().EffectiveBoeufs.AddRange(boeufs);

        BattleHandler.IsWaitingForSkillEnd = true;
    }
}


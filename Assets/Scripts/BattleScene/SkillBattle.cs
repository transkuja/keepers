using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Behaviour;
using System;

public enum TargetType { FriendSingle, FoeSingle, FriendAll, FoeAll, Self }
public enum SkillType { Physical, Magical, Defensive }

/*
 * Contains definition of battle skills 
 */
[System.Serializable]
public class SkillBattle {
    public int effectiveAttackValue = 3;

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
    SkillType skillType;

    [SerializeField]
    BattleBoeuf[] boeufs;

    [SerializeField]
    bool isMeantToHeal = false;
    [SerializeField]
    bool isMeantToBuff = false;

    public int Damage
    {
        get {
            if (skillName == "Attack")
                return StandardAtkDmg(skillUser);

            return damage;
        }
        set { damage = value; }
    }

    private int StandardAtkDmg(Fighter _skillUser)
    {
        int attackDamage = 0;

        for (int i = 0; i < _skillUser.LastThrowResult.Length; i++)
        {
            // Apply attack calculation
            if (_skillUser.LastThrowResult[i].Type == FaceType.Physical)
            {
                attackDamage += (effectiveAttackValue * _skillUser.LastThrowResult[i].Value);
            }
            else
            {
                attackDamage += 1;
            }
        }

        return attackDamage;
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

    public SkillType SkillType
    {
        get
        {
            return skillType;
        }

        set
        {
            skillType = value;
        }
    }

    public bool IsMeantToHeal
    {
        get
        {
            return isMeantToHeal;
        }

        set
        {
            isMeantToHeal = value;
        }
    }

    public bool IsMeantToBuff
    {
        get
        {
            return isMeantToBuff;
        }

        set
        {
            isMeantToBuff = value;
        }
    }

    public BattleBoeuf[] Boeufs
    {
        get
        {
            return boeufs;
        }

        set
        {
            boeufs = value;
        }
    }

    public SkillBattle()
    {

    }

    public SkillBattle(string _id)
    {
        if (_id == "default")
        {
            damage = 0;

            skillName = "Attack";
            description = "Damage based on current dice roll.";
            cost = new List<Face>();
            targetType = TargetType.FoeSingle;
        }
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
            boeufs = _origin.boeufs;
            skillType = _origin.skillType;
            isMeantToHeal = _origin.isMeantToHeal;
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

    private void ConsumeCost()
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
    }

    private void ApplySkillEffectOnTarget(PawnInstance _target, int _effectiveDamage)
    {
        Fighter curTargetFighter;
        if (_target.GetComponent<Mortal>() == null || _target.GetComponent<Mortal>().CurrentHp <= 0)
            return;

        curTargetFighter = _target.GetComponent<Fighter>();
        int curEffDmg = _effectiveDamage;

        curTargetFighter.IsWaitingForDmgFeedback = true;
        curTargetFighter.IsWaitingForSkillPanelToClose = true;

        curTargetFighter.PendingDamage = ((isMeantToHeal) ? -curEffDmg : curEffDmg);
        if (boeufs != null)
            curTargetFighter.EffectiveBoeufs.AddRange(boeufs);
    }

    public void UseSkill()
    {
        ConsumeCost();

        GameObject skillNameUI = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName;
        skillNameUI.transform.GetComponentInChildren<Text>().text = skillName;
        skillNameUI.SetActive(true);

        int effectiveDamage = Damage;

        foreach (BattleBoeuf boeuf in skillUser.EffectiveBoeufs)
        {
            if (boeuf.BoeufType == BoeufType.Damage)
                effectiveDamage += boeuf.EffectValue;
        }

        if (targetType == TargetType.FoeAll)
        {
            for (int i = 0; i < BattleHandler.CurrentBattleMonsters.Length; i++)
            {
                ApplySkillEffectOnTarget(BattleHandler.CurrentBattleMonsters[i], effectiveDamage);
            }
             BattleHandler.ExpectedAnswers = BattleHandler.CurrentBattleMonsters.Length;
        }
        else if (targetType == TargetType.FriendAll)
        {
            for (int i = 0; i < BattleHandler.CurrentBattleKeepers.Length; i++)
            {
                ApplySkillEffectOnTarget(BattleHandler.CurrentBattleKeepers[i], effectiveDamage);
            }
            if (BattleHandler.isPrisonerOnTile)
                ApplySkillEffectOnTarget(GameManager.Instance.PrisonerInstance, effectiveDamage);
            BattleHandler.ExpectedAnswers = BattleHandler.CurrentBattleKeepers.Length + ((BattleHandler.isPrisonerOnTile) ? 1 : 0);
        }
        else if (targetType == TargetType.Self)
        {
            ApplySkillEffectOnTarget(skillUser.GetComponent<PawnInstance>(), effectiveDamage);
            BattleHandler.ExpectedAnswers = 1;
        }
        else
        {
            Debug.LogError("UseSkill without parameters should not be called for single target skills other than Self.");
        }

        BattleHandler.IsWaitingForSkillEnd = true;
    }

    public void UseSkill(PawnInstance _target)
    {
        ConsumeCost();

        GameObject skillNameUI = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName;
        skillNameUI.transform.GetComponentInChildren<Text>().text = skillName;
        skillNameUI.SetActive(true);

        int effectiveDamage = Damage;

        foreach (BattleBoeuf boeuf in skillUser.EffectiveBoeufs)
        {
            if (boeuf.BoeufType == BoeufType.Damage)
                effectiveDamage += boeuf.EffectValue;
        }

        ApplySkillEffectOnTarget(_target, effectiveDamage);
        BattleHandler.ExpectedAnswers = 1;
        PlayAttackAnimation(_target);
        BattleHandler.IsWaitingForSkillEnd = true;
    }

    private void PlayAttackAnimation(PawnInstance _target)
    {
        NavMeshAgent agent = skillUser.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        skillUser.transform.LookAt(_target.transform);
        agent.SetDestination(_target.transform.position);
    }
}


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Behaviour;
using System;
using Random = UnityEngine.Random;

public enum TargetType { FriendSingle, FoeSingle, FriendAll, FoeAll, Self }
public enum SkillType { Physical, Magical, Defensive }

/*
 * Contains definition of battle skills 
 */
[System.Serializable]
public class SkillBattle {
    [HideInInspector]
    public int effectiveAttackValue = 4;
    [SerializeField]
    private int characterSkillIndex;

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

    [SerializeField]
    GameObject battleAnimation;

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
                attackDamage += 3;
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

    public int CharacterSkillIndex
    {
        get
        {
            return characterSkillIndex;
        }

        set
        {
            characterSkillIndex = value;
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
            battleAnimation = GameManager.Instance.PrefabUtils.baseAttackAnimation;
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
            isMeantToBuff = _origin.isMeantToBuff;
            battleAnimation = _origin.battleAnimation;
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
        {
            for (int i = 0; i < boeufs.Length; i++)
            {
                bool boeufMustBeAdded = true;
                Fighter tmpTargetFighter = (boeufs[i].BoeufTarget == BoeufTarget.SameAsAttack) ? curTargetFighter : skillUser.GetComponent<Fighter>();

                if (boeufs[i].Duration == 0 && boeufs[i].BoeufType == BoeufType.IncreaseStocks && boeufs[i].BoeufTarget == BoeufTarget.Self)
                {
                    int value;
                    if (boeufs[i].EffectValue == 0)
                    {
                        skillUser.ConsecutiveShots++;
                        value = skillUser.ConsecutiveShots;
                    }
                    else
                    {
                        value = boeufs[i].EffectValue;
                    }

                    skillUser.transform.gameObject.AddComponent<DieFeedback>();

                    for (int j = 0; j < boeufs[i].SymbolsAffected.Length; j++)
                    {
                        if (boeufs[i].SymbolsAffected[j] == FaceType.Physical)
                            skillUser.PhysicalSymbolStored += value;
                        if (boeufs[i].SymbolsAffected[j] == FaceType.Magical)
                            skillUser.MagicalSymbolStored += value;
                        if (boeufs[i].SymbolsAffected[j] == FaceType.Defensive)
                            skillUser.DefensiveSymbolStored += value;

                        skillUser.transform.GetComponent<DieFeedback>().PopFeedback(new Face(boeufs[i].SymbolsAffected[j], value), skillUser.GetComponent<PawnInstance>());
                    }
                    continue;
                }

                for (int j = 0; j < tmpTargetFighter.EffectiveBoeufs.Count; j++)
                {
                    if (boeufs[i].BoeufType == tmpTargetFighter.EffectiveBoeufs[j].BoeufType)
                    {
                        if ((boeufs[i].EffectValue > 0 && tmpTargetFighter.EffectiveBoeufs[j].EffectValue > 0 && boeufs[i].EffectValue <= tmpTargetFighter.EffectiveBoeufs[j].EffectValue) 
                            || (boeufs[i].EffectValue < 0 && tmpTargetFighter.EffectiveBoeufs[j].EffectValue < 0 && boeufs[i].EffectValue >= tmpTargetFighter.EffectiveBoeufs[j].EffectValue))
                        {
                            tmpTargetFighter.RemoveBoeuf(tmpTargetFighter.EffectiveBoeufs[j]);
                            break;
                        }
                        else
                        {
                            if ((boeufs[i].EffectValue > 0 && tmpTargetFighter.EffectiveBoeufs[j].EffectValue > 0)
                                    || (boeufs[i].EffectValue < 0 && tmpTargetFighter.EffectiveBoeufs[j].EffectValue < 0))
                            {
                                boeufMustBeAdded = false;
                                break;
                            }
                        }
                    }
                }
                if (boeufMustBeAdded) tmpTargetFighter.AddBoeuf(new BattleBoeuf(boeufs[i]));
            }          
        }
    }

    public void UseSkill()
    {
        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().LockCharactersPanelButtons();
        ConsumeCost();
        if (skillName.Contains("Rapid"))
            skillUser.HasPlayedARapidSkill = true;

        GameObject skillNameUI = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName;
        skillNameUI.transform.GetComponentInChildren<Text>().text = skillName;
        if (skillUser.GetComponent<LuckBased>() != null && skillUser.GetComponent<LuckBased>().isSkillFeedbackPending)
            skillUser.GetComponent<LuckBased>().FeedbackLuckForSkillBattle();
        skillNameUI.SetActive(true);

        int effectiveDamage = Damage;

        if (!isMeantToHeal && !isMeantToBuff)
        {
            foreach (BattleBoeuf boeuf in skillUser.EffectiveBoeufs)
            {
                if (boeuf.BoeufType == BoeufType.Damage)
                    effectiveDamage += boeuf.EffectValue;
            }
            effectiveDamage = Mathf.Max(0, effectiveDamage);
        }

        if (targetType == TargetType.FoeAll)
        {
            UseSkillOnAllFoes(effectiveDamage);
            if (skillName.Contains("Rainbow Beam"))
            {
                int tmpTargets = BattleHandler.ExpectedAnswers;
                UseSkillOnAllAllies(effectiveDamage);
                BattleHandler.ExpectedAnswers += tmpTargets;
            }
        }
        else if (targetType == TargetType.FriendAll)
        {
            UseSkillOnAllAllies(effectiveDamage);
        }
        else if (targetType == TargetType.Self)
        {
            if (battleAnimation != null)
            {
                PawnInstance[] tab = new PawnInstance[1];
                BattleHandler.CurrentSkillAnimDuration = battleAnimation.GetComponent<IBattleAnimation>().GetAnimationTime();
                tab[0] = skillUser.GetComponent<PawnInstance>();
                battleAnimation.GetComponent<IBattleAnimation>().SetTargets(tab);
                battleAnimation.GetComponent<IBattleAnimation>().Play();
            }
            else
            {
                BattleHandler.CurrentSkillAnimDuration = 0.5f;
            }
            ApplySkillEffectOnTarget(skillUser.GetComponent<PawnInstance>(), effectiveDamage);
            BattleHandler.ExpectedAnswers = 1;
            if (skillName == "Splash")
            {
                skillUser.GetComponent<AnimatedPawn>().Anim.SetTrigger("dance");
                BattleHandler.CurrentSkillAnimDuration = 1.2f;
            }
            else if (skillName == "Unicorn Pride")
            {
                skillUser.GetComponent<AnimatedPawn>().Anim.SetTrigger("dance");
                BattleHandler.CurrentSkillAnimDuration = 1.2f;
            }
        }
        else
        {
            Debug.LogError("UseSkill without parameters should not be called for single target skills other than Self.");
        }

        BattleHandler.IsWaitingForSkillEnd = true;
    }

    private void UseSkillOnAllFoes(int _effectiveDamage)
    {
        BattleHandler.ExpectedAnswers = BattleHandler.CurrentBattleMonsters.Length;
        if (battleAnimation != null)
        {
            if (battleAnimation.GetComponent<IBattleAnimation>().GetTargetType() == TargetType.FoeSingle)
            {
                List<PawnInstance> target = new List<PawnInstance>();
                target.Add(BattleHandler.CurrentBattleMonsters[0]);
                battleAnimation.GetComponent<IBattleAnimation>().SetTargets(target.ToArray());
            }
            else
            {
                List<PawnInstance> targets = new List<PawnInstance>();
                foreach (PawnInstance m in BattleHandler.CurrentBattleMonsters)
                {
                    if (m.GetComponent<Mortal>().CurrentHp > 0)
                        targets.Add(m);
                }
                battleAnimation.GetComponent<IBattleAnimation>().SetTargets(targets.ToArray());
            }

            BattleHandler.CurrentSkillAnimDuration = battleAnimation.GetComponent<IBattleAnimation>().GetAnimationTime();
            battleAnimation.GetComponent<IBattleAnimation>().Play();
        }
        else
        {
            BattleHandler.CurrentSkillAnimDuration = 0.5f;
        }
        for (int i = 0; i < BattleHandler.CurrentBattleMonsters.Length; i++)
        {
            if (BattleHandler.CurrentBattleMonsters[i].GetComponent<Mortal>().CurrentHp > 0)
            {
                if (skillName.Contains("Rainbow Beam"))
                {
                    BattleHandler.CurrentBattleMonsters[i].GetComponent<Fighter>().isPendingSkillsAshleys = true;
                    ApplySkillEffectOnTarget(BattleHandler.CurrentBattleMonsters[i], Random.Range(-3, 4));

                }
                else
                    ApplySkillEffectOnTarget(BattleHandler.CurrentBattleMonsters[i], _effectiveDamage);
            }
            else
                BattleHandler.ExpectedAnswers--;
        }
    }

    private void UseSkillOnAllAllies(int _effectiveDamage)
    {
        BattleHandler.ExpectedAnswers = BattleHandler.CurrentBattleKeepers.Length;
        if (battleAnimation != null)
        {
            List<PawnInstance> targets = new List<PawnInstance>();
            foreach (PawnInstance k in BattleHandler.CurrentBattleKeepers)
            {
                if (k.GetComponent<Mortal>().CurrentHp <= 0)
                    targets.Add(k);
            }
            battleAnimation.GetComponent<IBattleAnimation>().SetTargets(targets.ToArray());
            battleAnimation.GetComponent<IBattleAnimation>().SetTargets(BattleHandler.CurrentBattleKeepers);
            BattleHandler.CurrentSkillAnimDuration = battleAnimation.GetComponent<IBattleAnimation>().GetAnimationTime();
            battleAnimation.GetComponent<IBattleAnimation>().Play();
        }
        else
        {
            BattleHandler.CurrentSkillAnimDuration = 0.5f;
        }
        for (int i = 0; i < BattleHandler.CurrentBattleKeepers.Length; i++)
        {
            if (BattleHandler.CurrentBattleKeepers[i].GetComponent<Mortal>().CurrentHp > 0)
            {
                if (skillName.Contains("Rainbow Beam"))
                {
                    BattleHandler.CurrentBattleKeepers[i].GetComponent<Fighter>().isPendingSkillsAshleys = true;
                    ApplySkillEffectOnTarget(BattleHandler.CurrentBattleKeepers[i], Random.Range(-3, 4));
                }
                else
                    ApplySkillEffectOnTarget(BattleHandler.CurrentBattleKeepers[i], _effectiveDamage);
            }
            else
                BattleHandler.ExpectedAnswers--;
        }
        if (BattleHandler.isPrisonerOnTile)
        {
            if (skillName.Contains("Rainbow Beam"))
            {
                GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().isPendingSkillsAshleys = true;
                ApplySkillEffectOnTarget(GameManager.Instance.PrisonerInstance, Random.Range(-3, 4));
            }
            else
                ApplySkillEffectOnTarget(GameManager.Instance.PrisonerInstance, _effectiveDamage);

            BattleHandler.ExpectedAnswers++;
        }
    }

    public void UseSkill(PawnInstance _target)
    {
        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().LockCharactersPanelButtons();
        ConsumeCost();
        if (skillName.Contains("Rapid"))
            skillUser.HasPlayedARapidSkill = true;

        GameObject skillNameUI = GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName;
        skillNameUI.transform.GetComponentInChildren<Text>().text = skillName;
        if (skillUser.GetComponent<LuckBased>() != null && skillUser.GetComponent<LuckBased>().isSkillFeedbackPending)
            skillUser.GetComponent<LuckBased>().FeedbackLuckForSkillBattle();
        skillNameUI.SetActive(true);

        int effectiveDamage = Damage;

        if (!isMeantToHeal && !isMeantToBuff)
        {
            foreach (BattleBoeuf boeuf in skillUser.EffectiveBoeufs)
            {
                if (boeuf.BoeufType == BoeufType.Damage)
                    effectiveDamage += boeuf.EffectValue;
            }
            effectiveDamage = Mathf.Max(0, effectiveDamage);
        }
        if (battleAnimation != null)
        {
            PawnInstance[] tab = new PawnInstance[1];
            tab[0] = _target.GetComponent<PawnInstance>();
            BattleHandler.CurrentSkillAnimDuration = battleAnimation.GetComponent<IBattleAnimation>().GetAnimationTime();
            battleAnimation.GetComponent<IBattleAnimation>().SetTargets(tab);
            battleAnimation.GetComponent<IBattleAnimation>().Play();
        }
        else
        {
            BattleHandler.CurrentSkillAnimDuration = 0.5f;
        }
        ApplySkillEffectOnTarget(_target, effectiveDamage);
        BattleHandler.ExpectedAnswers = 1;
        skillUser.GetComponent<AnimatedPawn>().Anim.SetTrigger("doClassicAtk");
        BattleHandler.IsWaitingForSkillEnd = true;
        
    }
}


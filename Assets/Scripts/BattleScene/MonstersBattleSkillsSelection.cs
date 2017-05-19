using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;

public delegate SkillBattle SkillDecisionAlgo(Fighter _fighter);

public class MonstersBattleSkillsSelection  {
    enum DuckySkills { NormalCoin, CoinCoin, StrongCoin, WeakCoin };
    enum BirdSkills { Nosedive, WingsOfFury };
    enum BunnySkills { CarrotPunch, Burrow };

    bool arrivedInNewTierThisTurn = false;

    public SkillDecisionAlgo GetDecisionAlgorithm(string pawnId)
    {
        if (pawnId.Contains("bunny")) pawnId = "bunny";
        switch (pawnId)
        {
            case "ducky":
                return DuckySkillDecisionAlgo;
            case "bird":
                return BirdSkillDecisionAlgo;
            case "bunny":
                return BunnySkillDecisionAlgo;
            default:
                return DefaultSkillDecisionAlgo;
        }
    }

    SkillBattle DefaultSkillDecisionAlgo(Fighter _fighter)
    {
        List<SkillBattle> skills = _fighter.BattleSkills;
        return skills[Random.Range(0, skills.Count)];
    }

    SkillBattle BirdSkillDecisionAlgo(Fighter _fighter)
    {
        List<SkillBattle> skills = _fighter.BattleSkills;

        Debug.Log(BattleHandler.NbTurn);
        if (_fighter.EffectiveBoeufs == null || _fighter.EffectiveBoeufs.Count == 0)
            return skills[(int)BirdSkills.WingsOfFury];
        
        return skills[(int)BirdSkills.Nosedive];
    }

    SkillBattle DuckySkillDecisionAlgo(Fighter _fighter)
    {
        List<SkillBattle> skills = _fighter.BattleSkills;
        Mortal mortal = _fighter.GetComponent<Mortal>();
        // si CurrentHp < 20%
        if (mortal.CurrentHp < mortal.Data.MaxHp / 5.0f)
        {
            if (!arrivedInNewTierThisTurn)
            {
                arrivedInNewTierThisTurn = true;
                return skills[(int)DuckySkills.StrongCoin];
            }

            return skills[(int)DuckySkills.CoinCoin];
        }
        else if (mortal.CurrentHp < mortal.Data.MaxHp / 2.5f)
        {
            if (BattleHandler.NbTurn % 5 == 1)
                return skills[(int)DuckySkills.StrongCoin];

            if (Random.Range(0, 10) > 4)
                return skills[(int)DuckySkills.CoinCoin];
            return skills[(int)DuckySkills.NormalCoin];
        }
        else if (mortal.CurrentHp < mortal.Data.MaxHp / 1.25f)
        {
            if (BattleHandler.NbTurn % 5 == 1)
                return skills[(int)DuckySkills.StrongCoin];

            if (BattleHandler.NbTurn % 2 == 0)
                return skills[(int)DuckySkills.WeakCoin];
            else
            {
                if (Random.Range(0, 10) > 6)
                    return skills[(int)DuckySkills.CoinCoin];
                return skills[(int)DuckySkills.NormalCoin];
            }
        }
        else
        {
            if (BattleHandler.NbTurn % 3 != 0)
                return skills[(int)DuckySkills.WeakCoin];
            else
            {
                if (Random.Range(0, 10) > 7)
                    return skills[(int)DuckySkills.CoinCoin];
                return skills[(int)DuckySkills.NormalCoin];
            }
        }
    }

    SkillBattle BunnySkillDecisionAlgo(Fighter _fighter)
    {
        List<SkillBattle> skills = _fighter.BattleSkills;

        if (BattleHandler.NbTurn % 2 == 0)
            return skills[(int)BunnySkills.Burrow];

        return skills[(int)BunnySkills.CarrotPunch];
    }

}

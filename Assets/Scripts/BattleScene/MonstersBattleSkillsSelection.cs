using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;

public delegate SkillBattle SkillDecisionAlgo(Fighter _fighter);

public class MonstersBattleSkillsSelection  {
    enum DuckySkills { NormalCoin, CoinCoin, StrongCoin, WeakCoin };

    public SkillDecisionAlgo GetDecisionAlgorithm(string pawnId)
    {
        switch (pawnId)
        {
            case "ducky":
                return DuckySkillDecisionAlgo;
            default:
                return DefaultSkillDecisionAlgo;
        }
    }

    SkillBattle DefaultSkillDecisionAlgo(Fighter _fighter)
    {
        List<SkillBattle> skills = _fighter.BattleSkills;
        return skills[Random.Range(0, skills.Count)];
    }


    SkillBattle DuckySkillDecisionAlgo(Fighter _fighter)
    {
        List<SkillBattle> skills = _fighter.BattleSkills;
        Mortal mortal = _fighter.GetComponent<Mortal>();
        // si CurrentHp < 20%
        if (mortal.CurrentHp < mortal.Data.MaxHp / 5.0f)
        {
            if (BattleHandler.NbTurn % 5 == 1)
                return skills[(int)DuckySkills.StrongCoin];

            if (Random.Range(0, 10) > 6)
                return skills[(int)DuckySkills.CoinCoin];
            return skills[(int)DuckySkills.NormalCoin];
        }
        else if (mortal.CurrentHp < mortal.Data.MaxHp / 2.0f)
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
}

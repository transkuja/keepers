using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LuckResult { Luck, BadLuck, Common }

namespace Behaviour
{
    public class LuckBased : MonoBehaviour
    {
        private int sucessiveLuck = 0;
        private int sucessiveBadLuck = 0;
        private List<SkillBattle> physicalSkills = new List<SkillBattle>();
        private List<SkillBattle> defensiveSkills = new List<SkillBattle>();
        private List<SkillBattle> magicalSkills = new List<SkillBattle>();

        // Skills
        // Physical
        
            // Defensive

            // Magical

        private void Start()
        {
            SkillBattle physicalSkill1 = new SkillBattle();
            physicalSkill1.Damage = 25;
            physicalSkill1.Description = "TODO";
            physicalSkill1.SkillName = "TODO";
            physicalSkill1.TargetType = TargetType.FoeSingle;
            physicalSkill1.SkillUser = GetComponent<Fighter>();
            physicalSkill1.Cost = new List<Face>();
            physicalSkill1.Cost.Add(new Face(FaceType.Physical, 2));
            physicalSkills.Add(physicalSkill1);

            SkillBattle physicalSkill2 = new SkillBattle();
            physicalSkill2.Damage = 25;
            physicalSkill2.Description = "TODO";
            physicalSkill2.SkillName = "TODO";
            physicalSkill2.TargetType = TargetType.FoeSingle;
            physicalSkill2.SkillUser = GetComponent<Fighter>();
            physicalSkill2.Cost = new List<Face>();
            physicalSkill2.Cost.Add(new Face(FaceType.Physical, 2));
            physicalSkill2.Boeufs = new BattleBoeuf[2];
            physicalSkill2.Boeufs[0].BoeufType = BoeufType.Damage;
            physicalSkill2.Boeufs[0].Duration = 4;
            physicalSkill2.Boeufs[0].EffectValue = -5;
            physicalSkill2.Boeufs[1].BoeufType = BoeufType.Defense;
            physicalSkill2.Boeufs[1].Duration = 4;
            physicalSkill2.Boeufs[1].EffectValue = -10;
            physicalSkills.Add(physicalSkill2);

            SkillBattle physicalSkill3 = new SkillBattle();
            physicalSkill3.Damage = 12;
            physicalSkill3.Description = "TODO";
            physicalSkill3.SkillName = "TODO";
            physicalSkill3.TargetType = TargetType.FoeSingle;
            physicalSkill3.SkillUser = GetComponent<Fighter>();
            physicalSkill3.Cost = new List<Face>();
            physicalSkill3.Cost.Add(new Face(FaceType.Physical, 2));
            physicalSkill3.Boeufs = new BattleBoeuf[2];
            physicalSkill3.Boeufs[0].BoeufType = BoeufType.Damage;
            physicalSkill3.Boeufs[0].Duration = 4;
            physicalSkill3.Boeufs[0].EffectValue = 10;
            physicalSkill3.Boeufs[1].BoeufType = BoeufType.Defense;
            physicalSkill3.Boeufs[1].Duration = 4;
            physicalSkill3.Boeufs[1].EffectValue = 10;
            physicalSkills.Add(physicalSkill3);
        }

        public LuckResult RollDice()
        {
            int randDie = Random.Range(0, 4);
            if (randDie == 0)
            {
                sucessiveLuck++;
                if (sucessiveLuck > 3)
                {
                    sucessiveLuck = 0;
                    sucessiveBadLuck++;
                    return LuckResult.BadLuck;
                }
                return LuckResult.Luck;
            }
            else if (randDie == 1)
            {
                sucessiveBadLuck++;
                if (sucessiveLuck > 3)
                {
                    sucessiveBadLuck = 0;
                    sucessiveLuck++;
                    return LuckResult.Luck;
                }
                return LuckResult.BadLuck;
            }
            else
                return LuckResult.Common;
        }

        public void HandleLuckForActionPoints()
        {
            switch (RollDice())
            {
                case LuckResult.Luck:
                    GetComponent<Keeper>().Data.MaxActionPoint = 4;
                    break;
                case LuckResult.BadLuck:
                    GetComponent<Keeper>().Data.MaxActionPoint = 2;
                    break;
                default:
                    GetComponent<Keeper>().Data.MaxActionPoint = 3;
                    break;
            }
        }

        public int HandleLuckForTalk()
        {
            switch (RollDice())
            {
                case LuckResult.Luck:
                    return 25;
                case LuckResult.BadLuck:
                    return -30;
                default:
                    return 15;
            }
        }

        public void HandleLuckForHarvest(ItemContainer _item)
        {
            switch (RollDice())
            {
                case LuckResult.Luck:
                    if (_item != null)
                        _item.Quantity = (int)(_item.Quantity * 1.5f);
                    break;
                case LuckResult.BadLuck:
                    if (_item != null)
                        _item.Quantity = (int)(_item.Quantity / 2.0f);
                    break;
                default:
                    break;
            }
        }

        public void HandleLuckForTileDiscovery(Tile _tile)
        {
            List<PawnInstance> monstersOnTile = TileManager.Instance.MonstersOnTile[_tile];
            bool effectApplied = false;
            switch (RollDice())
            {
                case LuckResult.Luck:
                    foreach (PawnInstance pi in monstersOnTile)
                    {
                        if (pi.GetComponent<Monster>() != null && pi.GetComponent<Monster>().GetMType == MonsterType.Common)
                        {
                            pi.GetComponent<Mortal>().CurrentHp = 0;
                            effectApplied = true;
                            break;
                        }
                    }
                    if (!effectApplied) sucessiveLuck--;
                    break;
                case LuckResult.BadLuck:
                    if (monstersOnTile.Count < 3)
                    {
                        PrefabUtils pus = GameManager.Instance.PrefabUtils;
                        GameObject monsterToPopPrefab = pus.getMonsterPrefabById("wolf");
                        if (_tile.Type == TileType.Beach)
                        {
                            monsterToPopPrefab = pus.getMonsterPrefabById("snake");
                        }
                        else if (_tile.Type == TileType.Plain)
                        {
                            monsterToPopPrefab = pus.getMonsterPrefabById("jacob");
                        }
                        else if (_tile.Type == TileType.Snow)
                        {
                            monsterToPopPrefab = pus.getMonsterPrefabById("snowwolf");
                        }
                        else if (_tile.Type == TileType.Forest)
                        {
                            monsterToPopPrefab = pus.getMonsterPrefabById("bird");
                        }
                        else if (_tile.Type == TileType.Desert)
                        {
                            monsterToPopPrefab = pus.getMonsterPrefabById("wolf");
                        }
                        else if (_tile.Type == TileType.China)
                        {
                            monsterToPopPrefab = pus.listMonstersPrefab[Random.Range(0, pus.listMonstersPrefab.Count)].prefabPawn;
                        }

                        Instantiate(monsterToPopPrefab,
                            GameManager.Instance.GetFirstSelectedKeeper().CurrentTile.transform.position,
                            Quaternion.identity, GameManager.Instance.GetFirstSelectedKeeper().CurrentTile.transform);
                        effectApplied = true;
                    }
                    if (!effectApplied) sucessiveBadLuck--;
                    break;
                default:
                    break;
            }
        }

        public SkillBattle HandleLuckForSkills(SkillBattle _skill)
        {
            int randDieForSkills = Random.Range(0, 3);
            if (_skill.SkillType == SkillType.Physical)
                return physicalSkills[randDieForSkills];
            else if (_skill.SkillType == SkillType.Magical)
                return magicalSkills[randDieForSkills];
            else
                return defensiveSkills[randDieForSkills];
        }
    }
}

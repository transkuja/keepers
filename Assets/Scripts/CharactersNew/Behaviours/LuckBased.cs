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
    }
}

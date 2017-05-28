using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LuckResult { Luck, BadLuck, Common }

namespace Behaviour
{
    public class LuckBased : MonoBehaviour
    {
        private int sucessiveLuck = 0;
        private int sucessiveBadLuck = 0;
        [SerializeField]
        private List<SkillBattle> physicalSkills = new List<SkillBattle>();
        [SerializeField]
        private List<SkillBattle> defensiveSkills = new List<SkillBattle>();
        [SerializeField]
        private List<SkillBattle> magicalSkills = new List<SkillBattle>();
        public Fighter fighterComponent;

        private void Start()
        {
            if (fighterComponent == null)
                Debug.LogWarning("Fighter reference missing.");

            foreach (SkillBattle sb in physicalSkills)
            {
                if (sb.SkillUser == null)
                    sb.SkillUser = fighterComponent;
            }
            foreach (SkillBattle sb in defensiveSkills)
            {
                if (sb.SkillUser == null)
                    sb.SkillUser = fighterComponent;
            }
            foreach (SkillBattle sb in magicalSkills)
            {
                if (sb.SkillUser == null)
                    sb.SkillUser = fighterComponent;
            }
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
                if (sucessiveBadLuck > 3)
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
            Keeper keeper = GetComponent<Keeper>();
            switch (RollDice())
            {
                case LuckResult.Luck:
                    keeper.Data.MaxActionPoint = 4;
                    keeper.UpdateActionPoint(4);
                    FeedbackLuckAP(true);
                    break;
                case LuckResult.BadLuck:
                    keeper.Data.MaxActionPoint = 2;
                    keeper.UpdateActionPoint(2);
                    FeedbackLuckAP(false);
                    break;
                default:
                    keeper.Data.MaxActionPoint = 3;
                    keeper.UpdateActionPoint(3);
                    break;
            }
            keeper.UpdateActionPointsUI();
        }

        private void FeedbackLuckAP(bool isGoodLuck)
        {
            GameObject feedback = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, GameManager.Instance.Ui.transform.GetChild(0));
            if (GameManager.Instance.ListOfSelectedKeepers != null && GameManager.Instance.ListOfSelectedKeepers.Count > 0 && GetComponent<PawnInstance>() == GameManager.Instance.ListOfSelectedKeepers[0])
            {
                feedback.transform.position = GetComponent<Keeper>().SelectedActionPointsUI.transform.position + Vector3.right * (150 * Screen.width / 1920.0f) + Vector3.up * (50 * Screen.height / 1080.0f);
            }
            else
            {
                if (!GameManager.Instance.Ui.goShortcutKeepersPanel.activeSelf)
                    GameManager.Instance.Ui.ToggleShortcutPanel();
                feedback.transform.SetParent(GetComponent<Keeper>().ShorcutUI.transform);
                feedback.transform.localPosition = Vector3.zero;
            }
            feedback.AddComponent<CatsFeedback>();
            feedback.GetComponent<CatsFeedback>().SendSprite((isGoodLuck) ? GameManager.Instance.SpriteUtils.luckat : GameManager.Instance.SpriteUtils.badLuckat);
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
                    FeedbackLuckHarvest(true);
                    break;
                case LuckResult.BadLuck:
                    if (_item != null)
                        _item.Quantity = (int)(_item.Quantity / 2.0f);
                    FeedbackLuckHarvest(false);
                    break;
                default:
                    break;
            }
        }

        private void FeedbackLuckHarvest(bool isGoodLuck)
        {
            GameObject feedback = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabImageUI, GetComponent<Interactable>().Feedback.GetChild(0));
            feedback.transform.localScale = Vector3.one;
            feedback.GetComponent<RectTransform>().sizeDelta = Vector2.one * 0.75f;

            feedback.AddComponent<CatsFeedback>();
            feedback.GetComponent<CatsFeedback>().SendSprite((isGoodLuck) ? GameManager.Instance.SpriteUtils.luckat : GameManager.Instance.SpriteUtils.badLuckat);
            feedback.transform.localPosition = Vector3.zero;
            feedback.transform.localPosition += Vector3.up * 0.35f;
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
            if (_skill.SkillName == "Attack" || _skill.SkillName == "Great Power")
                return _skill;
             
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

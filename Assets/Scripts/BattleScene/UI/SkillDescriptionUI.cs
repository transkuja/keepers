using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDescriptionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject descriptionPanel;
    SkillBattle skillData;

    public SkillBattle SkillData
    {
        get
        {
            return skillData;
        }

        set
        {
            skillData = value;
        }
    }

    private void Start()
    {
        if (descriptionPanel == null)
        {
            descriptionPanel = Instantiate(GameManager.Instance.PrefabUIUtils.skillDescriptionPanel, GameManager.Instance.Ui.transform.GetChild(0));
        }
        descriptionPanel.SetActive(false);
        descriptionPanel.GetComponentInChildren<Text>().text = skillData.Description;

        if (skillData.TargetType == TargetType.FoeAll)
        {
            descriptionPanel.GetComponentInChildren<Text>().text += "\nTarget: All enemies";
        }
        else if (skillData.TargetType == TargetType.FoeSingle)
            descriptionPanel.GetComponentInChildren<Text>().text += "\nTarget: One enemy";
        else if (skillData.TargetType == TargetType.FriendSingle)
            descriptionPanel.GetComponentInChildren<Text>().text += "\nTarget: One ally";
        else if (skillData.TargetType == TargetType.FoeAll)
            descriptionPanel.GetComponentInChildren<Text>().text += "\nTarget: All allies";
        else
            descriptionPanel.GetComponentInChildren<Text>().text += "\nTarget: Self";

        if (skillData.IsMeantToHeal)
            descriptionPanel.GetComponentInChildren<Text>().text += "\n\nHeal value: " + skillData.Damage;
        else if (skillData.Damage > 0)
        {
            if (skillData.SkillName.Equals("Attack"))
            {
                descriptionPanel.GetComponentInChildren<Text>().text += "\n\nDamage: " + skillData.effectiveAttackValue + " damage for each swords on dice, 3 damage for other faces";
            }
            else
                descriptionPanel.GetComponentInChildren<Text>().text += "\n\nDamage: " + skillData.Damage;
        }

        if (skillData.Boeufs != null && skillData.Boeufs.Length > 0)
        {
            descriptionPanel.GetComponentInChildren<Text>().text += "\nEffect: ";
            // Big fat ugly hardcoded effect description lol
            if (skillData.SkillName.Contains("Rapid"))
                descriptionPanel.GetComponentInChildren<Text>().text += "The more you shoot in a turn, the more magical power you gain.";
            else
                for (int i = 0; i < skillData.Boeufs.Length; i++)
                {
                    BattleBoeuf curBoeuf = skillData.Boeufs[i];
                    if (curBoeuf.BoeufType == BoeufType.Damage)
                    {
                        descriptionPanel.GetComponentInChildren<Text>().text += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                        descriptionPanel.GetComponentInChildren<Text>().text += "damage dealt by " + Mathf.Abs(curBoeuf.EffectValue);
                    }
                    else if (curBoeuf.BoeufType == BoeufType.Defense)
                    {
                        descriptionPanel.GetComponentInChildren<Text>().text += (curBoeuf.EffectValue > 0) ? "Reduce " : "Increase ";
                        descriptionPanel.GetComponentInChildren<Text>().text += "damage taken by " + Mathf.Abs(curBoeuf.EffectValue);
                    }
                    else if (curBoeuf.BoeufType == BoeufType.Aggro)
                    {

                    }
                    else if (curBoeuf.BoeufType == BoeufType.CostReduction)
                    {
                        descriptionPanel.GetComponentInChildren<Text>().text += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                        descriptionPanel.GetComponentInChildren<Text>().text += "chances of being targeted by " + Mathf.Abs(curBoeuf.EffectValue) + " percent";
                    }
                    else if (curBoeuf.BoeufType == BoeufType.IncreaseStocks)
                    {
                        descriptionPanel.GetComponentInChildren<Text>().text += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                        for (int j = 0; j < curBoeuf.SymbolsAffected.Length; j++)
                        {
                            if (curBoeuf.SymbolsAffected[j] == FaceType.Physical)
                                descriptionPanel.GetComponentInChildren<Text>().text += "physical ";
                            else if (curBoeuf.SymbolsAffected[j] == FaceType.Defensive)
                                descriptionPanel.GetComponentInChildren<Text>().text += "defensive ";
                            else
                                descriptionPanel.GetComponentInChildren<Text>().text += "magic ";
                        }
                        descriptionPanel.GetComponentInChildren<Text>().text += "gauges by " + Mathf.Abs(curBoeuf.EffectValue);
                    }
                    descriptionPanel.GetComponentInChildren<Text>().text += " for " + (curBoeuf.Duration - 1) + " turns. ";
                }
        }


        descriptionPanel.transform.localPosition = GameManager.Instance.PrefabUIUtils.skillDescriptionPanel.transform.localPosition;
        descriptionPanel.transform.localScale = Vector3.one;
    }

    private void OnEnable()
    {
        if (descriptionPanel == null)
        {
            descriptionPanel = Instantiate(GameManager.Instance.PrefabUIUtils.skillDescriptionPanel, GameManager.Instance.Ui.transform.GetChild(0));
        }
        descriptionPanel.SetActive(false);
        descriptionPanel.GetComponentInChildren<Text>().text = skillData.Description;

        if (skillData.TargetType == TargetType.FoeAll)
        {
            descriptionPanel.GetComponentInChildren<Text>().text += "\nTarget: All enemies";
        }
        else if (skillData.TargetType == TargetType.FoeSingle)
            descriptionPanel.GetComponentInChildren<Text>().text += "\nTarget: One enemy";
        else if (skillData.TargetType == TargetType.FriendSingle)
            descriptionPanel.GetComponentInChildren<Text>().text += "\nTarget: One ally";
        else if (skillData.TargetType == TargetType.FoeAll)
            descriptionPanel.GetComponentInChildren<Text>().text += "\nTarget: All allies";
        else
            descriptionPanel.GetComponentInChildren<Text>().text += "\nTarget: Self";

        if (skillData.IsMeantToHeal)
            descriptionPanel.GetComponentInChildren<Text>().text += "\n\nHeal value: " + skillData.Damage;
        else if (skillData.Damage > 0)
        {
            if (skillData.SkillName.Equals("Attack"))
            {
                descriptionPanel.GetComponentInChildren<Text>().text += "\n\nDamage: " + skillData.effectiveAttackValue + " damage for each swords on dice, 1 damage for other faces";
            }
            else
                descriptionPanel.GetComponentInChildren<Text>().text += "\n\nDamage: " + skillData.Damage;
        }

        if (skillData.Boeufs != null && skillData.Boeufs.Length > 0)
        {
            descriptionPanel.GetComponentInChildren<Text>().text += "\nEffect" + ((skillData.Boeufs.Length > 1) ? "s" : "") + ": ";
            // Big fat ugly hardcoded effect description lol (and I copy paste it, yay)
            if (skillData.SkillName.Contains("Rapid"))
                descriptionPanel.GetComponentInChildren<Text>().text += "The more you shoot in a turn, the more magical power you gain.";
            else
                for (int i = 0; i < skillData.Boeufs.Length; i++)
                {
                    BattleBoeuf curBoeuf = skillData.Boeufs[i];
                    if (curBoeuf.BoeufType == BoeufType.Damage)
                    {
                        descriptionPanel.GetComponentInChildren<Text>().text += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                        descriptionPanel.GetComponentInChildren<Text>().text += "damage dealt by " + Mathf.Abs(curBoeuf.EffectValue);
                    }
                    else if (curBoeuf.BoeufType == BoeufType.Defense)
                    {
                        descriptionPanel.GetComponentInChildren<Text>().text += (curBoeuf.EffectValue > 0) ? "Reduce " : "Increase ";
                        descriptionPanel.GetComponentInChildren<Text>().text += "damage taken by " + Mathf.Abs(curBoeuf.EffectValue);
                    }
                    else if (curBoeuf.BoeufType == BoeufType.Aggro)
                    {

                    }
                    else if (curBoeuf.BoeufType == BoeufType.CostReduction)
                    {
                        descriptionPanel.GetComponentInChildren<Text>().text += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                        descriptionPanel.GetComponentInChildren<Text>().text += "chances of being targeted by " + Mathf.Abs(curBoeuf.EffectValue) + " percent";
                    }
                    else if (curBoeuf.BoeufType == BoeufType.IncreaseStocks)
                    {
                        descriptionPanel.GetComponentInChildren<Text>().text += (curBoeuf.EffectValue < 0) ? "Reduce " : "Increase ";
                        for (int j = 0; j < curBoeuf.SymbolsAffected.Length; j++)
                        {
                            if (curBoeuf.SymbolsAffected[j] == FaceType.Physical)
                                descriptionPanel.GetComponentInChildren<Text>().text += "physical ";
                            else if (curBoeuf.SymbolsAffected[j] == FaceType.Defensive)
                                descriptionPanel.GetComponentInChildren<Text>().text += "defensive ";
                            else
                                descriptionPanel.GetComponentInChildren<Text>().text += "magic ";
                        }
                        descriptionPanel.GetComponentInChildren<Text>().text += "gauges by " + Mathf.Abs(curBoeuf.EffectValue);
                    }
                    descriptionPanel.GetComponentInChildren<Text>().text += " for " + curBoeuf.Duration + " turns.\n";
                }
        }

        descriptionPanel.transform.localPosition = GameManager.Instance.PrefabUIUtils.skillDescriptionPanel.transform.localPosition;
        descriptionPanel.transform.localScale = Vector3.one;
    }

    private void OnDisable()
    {
        Destroy(descriptionPanel);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.SetActive(false);
    }
}

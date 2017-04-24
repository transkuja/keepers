using UnityEngine;
using UnityEngine.UI;

public class SkillContainer : MonoBehaviour {
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

    private void OnEnable()
    {
        // TODO: fix this, user should be set on skill
        Behaviour.Fighter fighterComponent = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Fighter>();
        GetComponent<Button>().interactable = skillData.CanUseSkill(fighterComponent);
        if (GetComponent<Button>().interactable == true)
            GetComponent<Text>().color = Color.white;
        else
            GetComponent<Text>().color = Color.grey;

        Text atkTxt = transform.parent.GetChild((int)SkillButtonChildren.Atk).GetComponentInChildren<Text>();
        Text magTxt = transform.parent.GetChild((int)SkillButtonChildren.Mag).GetComponentInChildren<Text>();
        Text defTxt = transform.parent.GetChild((int)SkillButtonChildren.Def).GetComponentInChildren<Text>();
        Text supTxt = transform.parent.GetChild((int)SkillButtonChildren.Support).GetComponentInChildren<Text>();

        foreach (Face face in skillData.Cost)
        {
            if (face.Type == FaceType.Physical)
            {
                if (face.Value == 0)
                    atkTxt.color = Color.white;
                else
                {
                    if (face.Value > fighterComponent.PhysicalSymbolStored)
                        atkTxt.color = Color.red;
                    else if (face.Value <= fighterComponent.PhysicalSymbolStored)
                        atkTxt.color = Color.green;
                }
            }

            if (face.Type == FaceType.Magical)
            {
                if (face.Value == 0)
                    magTxt.color = Color.white;
                else
                {
                    if (face.Value > fighterComponent.MagicalSymbolStored)
                        magTxt.color = Color.red;
                    else if (face.Value <= fighterComponent.MagicalSymbolStored)
                        magTxt.color = Color.green;
                }
            }

            if (face.Type == FaceType.Defensive)
            {
                if (face.Value == 0)
                    defTxt.color = Color.white;
                else
                {
                    if (face.Value > fighterComponent.DefensiveSymbolStored)
                        defTxt.color = Color.red;
                    else if (face.Value <= fighterComponent.DefensiveSymbolStored)
                        defTxt.color = Color.green;
                }
            }

            if (face.Type == FaceType.Support)
            {
                if (face.Value == 0)
                    supTxt.color = Color.white;
                else
                {
                    if (face.Value > fighterComponent.SupportSymbolStored)
                        supTxt.color = Color.red;
                    else if (face.Value <= fighterComponent.SupportSymbolStored)
                        supTxt.color = Color.green;
                }
            }
        }
    }

    public void UseLinkedSkill()
    {
        if (skillData.TargetType == TargetType.Foe)
        {
            GameManager.Instance.Ui.mouseFollower.SetActive(true);
            GameManager.Instance.Ui.mouseFollower.GetComponent<MouseFollower>().ExpectedTarget(TargetType.Foe);
            BattleHandler.ActivateFeedbackSelection(false, true);
        }
        else if (skillData.TargetType == TargetType.Friend)
        {
            GameManager.Instance.Ui.mouseFollower.SetActive(true);
            GameManager.Instance.Ui.mouseFollower.GetComponent<MouseFollower>().ExpectedTarget(TargetType.Friend);
            BattleHandler.ActivateFeedbackSelection(true, false);
        }
        // TODO: find an other way
        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(GameManager.Instance.GetFirstSelectedKeeper()).gameObject.SetActive(false);
        BattleHandler.WaitForSkillConfirmation(skillData, GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Fighter>());
    }
}

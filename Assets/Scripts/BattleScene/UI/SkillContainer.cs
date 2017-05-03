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
        GetComponent<Button>().interactable = skillData.CanUseSkill();
        if (GetComponent<Button>().interactable == true)
            GetComponent<Text>().color = Color.white;
        else
            GetComponent<Text>().color = Color.grey;

        Text atkTxt = transform.parent.GetChild((int)SkillButtonChildren.Atk).GetComponentInChildren<Text>();
        Text magTxt = transform.parent.GetChild((int)SkillButtonChildren.Mag).GetComponentInChildren<Text>();
        Text defTxt = transform.parent.GetChild((int)SkillButtonChildren.Def).GetComponentInChildren<Text>();

        foreach (Face face in skillData.Cost)
        {
            if (face.Type == FaceType.Physical)
            {
                if (face.Value == 0)
                    atkTxt.color = Color.white;
                else
                {
                    if (face.Value > skillData.SkillUser.PhysicalSymbolStored)
                        atkTxt.color = Color.red;
                    else if (face.Value <= skillData.SkillUser.PhysicalSymbolStored)
                        atkTxt.color = Color.green;
                }
            }

            if (face.Type == FaceType.Magical)
            {
                if (face.Value == 0)
                    magTxt.color = Color.white;
                else
                {
                    if (face.Value > skillData.SkillUser.MagicalSymbolStored)
                        magTxt.color = Color.red;
                    else if (face.Value <= skillData.SkillUser.MagicalSymbolStored)
                        magTxt.color = Color.green;
                }
            }

            if (face.Type == FaceType.Defensive)
            {
                if (face.Value == 0)
                    defTxt.color = Color.white;
                else
                {
                    if (face.Value > skillData.SkillUser.DefensiveSymbolStored)
                        defTxt.color = Color.red;
                    else if (face.Value <= skillData.SkillUser.DefensiveSymbolStored)
                        defTxt.color = Color.green;
                }
            }
        }
    }

    public void UseLinkedSkill()
    {
        if (skillData.TargetType == TargetType.FoeSingle || skillData.TargetType == TargetType.FoeAll)
        {
            GameManager.Instance.Ui.mouseFollower.SetActive(true);
            GameManager.Instance.Ui.mouseFollower.GetComponent<MouseFollower>().ExpectedTarget(TargetType.FoeSingle);
            BattleHandler.ActivateFeedbackSelection(false, true);
        }
        else if (skillData.TargetType == TargetType.FriendSingle)
        {
            GameManager.Instance.Ui.mouseFollower.SetActive(true);
            GameManager.Instance.Ui.mouseFollower.GetComponent<MouseFollower>().ExpectedTarget(TargetType.FriendSingle);
            BattleHandler.ActivateFeedbackSelection(true, false);
        }
        // TODO: find an other way
        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(skillData.SkillUser.GetComponent<PawnInstance>()).gameObject.SetActive(false);
        BattleHandler.WaitForSkillConfirmation(skillData);
    }
}

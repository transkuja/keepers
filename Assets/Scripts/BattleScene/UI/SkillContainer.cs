using UnityEngine;

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
            GameManager.Instance.Ui.mouseFollower.GetComponent<MouseFollower>().ExpectedTarget(TargetType.Friend);
            GameManager.Instance.Ui.mouseFollower.SetActive(true);
            BattleHandler.ActivateFeedbackSelection(true, false);
        }
        // TODO: find an other way
        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(GameManager.Instance.GetFirstSelectedKeeper()).gameObject.SetActive(false);
        BattleHandler.WaitForSkillConfirmation(skillData);
    }
}

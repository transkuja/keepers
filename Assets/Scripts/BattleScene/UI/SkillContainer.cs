using UnityEngine;
using UnityEngine.UI;

public class SkillContainer : MonoBehaviour {
    SkillBattle skillData;
    private Color customGrey = new Color(0.6784f, 0.6784f, 0.6784f, 1);
    private Color customRed = new Color(0.75f, 0, 0, 1);
    private Color customGreen = new Color(0, 0.75f, 0, 1);

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
            GetComponentInChildren<Text>().color = Color.black;
        else
            GetComponentInChildren<Text>().color = Color.grey;

        Text atkTxt = transform.parent.GetChild((int)SkillButtonChildren.Atk).GetComponentInChildren<Text>();
        Text magTxt = transform.parent.GetChild((int)SkillButtonChildren.Mag).GetComponentInChildren<Text>();
        Text defTxt = transform.parent.GetChild((int)SkillButtonChildren.Def).GetComponentInChildren<Text>();

        foreach (Face face in skillData.Cost)
        {
            if (face.Type == FaceType.Physical)
            {
                if (face.Value == 0)
                    atkTxt.color = customGrey;
                else
                {
                    if (face.Value > skillData.SkillUser.PhysicalSymbolStored)
                        atkTxt.color = customRed;
                    else if (face.Value <= skillData.SkillUser.PhysicalSymbolStored)
                        atkTxt.color = customGreen;
                }
            }

            if (face.Type == FaceType.Magical)
            {
                if (face.Value == 0)
                    magTxt.color = customGrey;
                else
                {
                    if (face.Value > skillData.SkillUser.MagicalSymbolStored)
                        magTxt.color = customRed;
                    else if (face.Value <= skillData.SkillUser.MagicalSymbolStored)
                        magTxt.color = customGreen;
                }
            }

            if (face.Type == FaceType.Defensive)
            {
                if (face.Value == 0)
                    defTxt.color = customGrey;
                else
                {
                    if (face.Value > skillData.SkillUser.DefensiveSymbolStored)
                        defTxt.color = customRed;
                    else if (face.Value <= skillData.SkillUser.DefensiveSymbolStored)
                        defTxt.color = customGreen;
                }
            }
        }
    }

    public void UseLinkedSkill()
    {
        if (skillData.TargetType == TargetType.FoeSingle || skillData.TargetType == TargetType.FriendSingle)
        {
            if (skillData.SkillType == SkillType.Physical)
                Cursor.SetCursor(GameManager.Instance.Texture2DUtils.attackCursor, Vector2.zero, CursorMode.Auto);
            else if (skillData.SkillType == SkillType.Defensive)
                Cursor.SetCursor(GameManager.Instance.Texture2DUtils.buffCursor, Vector2.zero, CursorMode.Auto);
            else if (skillData.SkillType == SkillType.Magical)
                Cursor.SetCursor(GameManager.Instance.Texture2DUtils.magicCursor, Vector2.zero, CursorMode.Auto);

            if (skillData.TargetType == TargetType.FoeSingle)
                BattleHandler.ActivateFeedbackSelection(false, true);
            else
                BattleHandler.ActivateFeedbackSelection(true, false);

            // TODO: find an other way
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(skillData.SkillUser.GetComponent<PawnInstance>()).gameObject.SetActive(false);
            BattleHandler.WaitForSkillConfirmation(skillData);
        }
        else
        {
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(skillData.SkillUser.GetComponent<PawnInstance>()).gameObject.SetActive(false);
            BattleHandler.WaitForSkillConfirmation(skillData);
            skillData.UseSkill();
        }

    }
}

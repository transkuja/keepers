using UnityEngine;
using Behaviour;

public class MouseClickedOnIngameElt : MonoBehaviour {

	void Start () {
        TutoManager.MouseClicked = false;
        if (TutoManager.s_instance.PlayingSequence.CurrentState == SequenceState.WaitingForSkillUse)
        {
            TutoManager.SecondMouseClicked = false;
        }
	}
	
    void OnMouseDown()
    {
        if (TutoManager.s_instance.PlayingSequence.CurrentState != SequenceState.WaitingForSkillUse)
        {
            TutoManager.MouseClicked = true;
            if (GetComponent<Keeper>() != null)
            {
                Keeper clickedKeeper = GetComponent<Keeper>();
                GameManager.Instance.AddKeeperToSelectedList(GetComponent<PawnInstance>());
                clickedKeeper.IsSelected = true;

                if (TutoManager.s_instance.StateBeforeTutoStarts == GameState.InBattle)
                {
                    GameManager.Instance.ClearListKeeperSelected();
                    GameManager.Instance.AddKeeperToSelectedList(GetComponent<PawnInstance>());
                    clickedKeeper.IsSelected = true;
                    GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(GetComponent<PawnInstance>()).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (TutoManager.MouseClicked == true)
            {
                TutoManager.SecondMouseClicked = true;
                BattleHandler.PendingSkill.UseSkill(GetComponent<PawnInstance>());
                GameManager.Instance.Ui.mouseFollower.SetActive(false);
            }
        }
    }
}

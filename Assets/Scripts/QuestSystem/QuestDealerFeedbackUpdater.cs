using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestDealerFeedbackUpdater : MonoBehaviour {
    Quest quest;
    public void Init(Quest _quest)
    {
        quest = _quest;
        foreach(IQuestObjective obj in quest.Objectives)
            obj.OnComplete += UpdateQuestState;
        ActivateQuestAvailableFeedback();
    }

	void UpdateQuestState () {
        if(quest.CheckIfComplete())
        {
            ActivateQuestCompleteFeedback();
        }
    }

    public void ActivateQuestCompleteFeedback()
    {

    }

    public void ActivateQuestAvailableFeedback()
    {

    }

    public void ActivateQuestWaitingFeedback()
    {

    }

    public void DisableFeedbacks()
    {

    }
}

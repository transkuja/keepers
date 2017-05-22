using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestDealerFeedbackUpdater : MonoBehaviour {
    Quest quest;
    public GameObject feedbackContainer;
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
    
    public void ActivateQuestAvailableFeedback()
    {
        feedbackContainer.transform.GetChild(0).gameObject.SetActive(true);
        feedbackContainer.transform.GetChild(1).gameObject.SetActive(false);
        feedbackContainer.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void ActivateQuestWaitingFeedback()
    {
        feedbackContainer.transform.GetChild(0).gameObject.SetActive(false);
        feedbackContainer.transform.GetChild(1).gameObject.SetActive(true);
        feedbackContainer.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void ActivateQuestCompleteFeedback()
    {
        feedbackContainer.transform.GetChild(0).gameObject.SetActive(false);
        feedbackContainer.transform.GetChild(1).gameObject.SetActive(false);
        feedbackContainer.transform.GetChild(2).gameObject.SetActive(true);
    }


    public void DisableFeedbacks()
    {
        feedbackContainer.transform.GetChild(0).gameObject.SetActive(false);
        feedbackContainer.transform.GetChild(1).gameObject.SetActive(false);
        feedbackContainer.transform.GetChild(2).gameObject.SetActive(false);
    }
}

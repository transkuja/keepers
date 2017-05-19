using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestUtility {
    public static void AcceptQuest(Quest quest)
    {
        quest.Init();
        GameManager.Instance.QuestManager.ActiveQuests.Add(quest);
        GameObject.FindObjectOfType<QuestReminder>().Refresh();
    }

    public static void CompleteQuest(Quest quest)
    {
        quest.Complete();
        GameManager.Instance.QuestManager.ActiveQuests.Remove(quest);
        GameManager.Instance.QuestManager.CompletedQuests.Add(quest);

    }
}

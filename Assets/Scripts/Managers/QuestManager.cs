using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestManager : MonoBehaviour
{
    public QuestDeck CurrentQuestDeck;

    public List<Quest> AvailableQuests;
    public List<Quest> ActiveQuests;
    public Quest MainQuest;

    void Start()
    {
        CurrentQuestDeck = new QuestDeck();
        CurrentQuestDeck.Id = 0;
    }

    public void Init()
    {
        ActiveQuests = new List<Quest>();
        AvailableQuests = new List<Quest>();
        // if temporaire
        if(CurrentQuestDeck != null)
        {
            MainQuest = GameManager.Instance.QuestsContainer.FindQuestByID(CurrentQuestDeck.MainQuest);
            foreach (string questID in CurrentQuestDeck.SideQuests)
            {
                AvailableQuests.Add(GameManager.Instance.QuestsContainer.FindQuestByID(questID));
            }
        }
        
    }

    public Quest GetQuestByID(string id)
    {
        foreach (Quest quest in AvailableQuests)
        {
            if (quest.Identifier.ID == id)
                return quest;
        }
        return null;
    }
}

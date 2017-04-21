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

    }

    public void Init()
    {
        //TODO: Should be set according to what has been selected in the menu
        CurrentQuestDeck = GameManager.Instance.QuestDeckDataBase.GetDeckByID("deck_02");

        ActiveQuests = new List<Quest>();
        AvailableQuests = new List<Quest>();

        if(CurrentQuestDeck != null)
        {
            //Should only be the case if we lauch from the scene
            if(!GameManager.Instance.QuestsContainer.isInitialized)
            {
                GameManager.Instance.QuestsContainer.Init();
            }
            
            MainQuest = GameManager.Instance.QuestsContainer.FindQuestByID(CurrentQuestDeck.MainQuest);

            foreach (string questID in CurrentQuestDeck.SideQuests)
            {
                AvailableQuests.Add(GameManager.Instance.QuestsContainer.FindQuestByID(questID));
            }
        }
        else
        {
            Debug.Log("QuestDeck Id is null");
        }
        
    }

    public Quest GetQuestByID(string id)
    {
        Quest _quest = AvailableQuests.Find(x => x.Identifier.ID == id);
        if (_quest != null)
            return _quest;

        _quest = ActiveQuests.Find(x => x.Identifier.ID == id);
        if (_quest != null)
            return _quest;

        if (MainQuest.Identifier.ID == id)
            return MainQuest;
        
        return null;
    }
}

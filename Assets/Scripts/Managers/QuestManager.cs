using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestManager : MonoBehaviour
{
    public bool isDebugQuestManager;
    [SerializeField]
    private string questDeckToLoadDebug;

    public QuestDeck CurrentQuestDeck;

    public List<Quest> Quests;
    public List<Quest> ActiveQuests;
    public List<Quest> CompletedQuests;
    public Quest MainQuest;

    void Start()
    {

    }

    public void Init(string questDeckID)
    {
        if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto)
        {
            CurrentQuestDeck = GameManager.Instance.QuestDeckDataBase.GetDeckByID("deck_01");
            if (TutoManager.s_instance.GetComponent<SeqFirstMove>().AlreadyPlayed == false)
                CurrentQuestDeck.SideQuests.Clear();
        }
        else
        {
            if (questDeckID == "deck_03")
                CurrentQuestDeck.SideQuests.Clear();

            CurrentQuestDeck = GameManager.Instance.QuestDeckDataBase.GetDeckByID(questDeckID);
            if(isDebugQuestManager)
                CurrentQuestDeck = GameManager.Instance.QuestDeckDataBase.GetDeckByID(questDeckToLoadDebug);
        }
    
        ActiveQuests = new List<Quest>();
        CompletedQuests = new List<Quest>();
        Quests = new List<Quest>();

        if(CurrentQuestDeck != null)
        {

            //Should only be the case if we launch from the scene
            if(!GameManager.Instance.QuestsContainer.isInitialized)
            {
                GameManager.Instance.QuestsContainer.Init();
            }
            
            MainQuest = new Quest(GameManager.Instance.QuestsContainer.FindQuestByID(CurrentQuestDeck.MainQuest));

            foreach (string questID in CurrentQuestDeck.SideQuests)
            {
                Quests.Add(new Quest(GameManager.Instance.QuestsContainer.FindQuestByID(questID)));
            }
        }
        else
        {
            Debug.Log("QuestDeck Id is null");
        }
        
    }

    public Quest GetQuestByID(string id)
    {
        Quest _quest = Quests.Find(x => x.Identifier.ID == id);
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

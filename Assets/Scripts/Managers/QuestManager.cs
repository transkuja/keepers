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
        // FIX: holy shit. Init tutoData should be done before this init
        //if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto && GameManager.Instance.PersistenceLoader.Pd.dicPersistenceSequences["seqfirstmove"] == false)
        //{
        //    CurrentQuestDeck = GameManager.Instance.QuestDeckDataBase.GetDeckByID("deck_01");
        //    CurrentQuestDeck.SideQuests.Clear();
        //}
        if (isDebugQuestManager || GameManager.Instance.IsDebugGameManager)

            CurrentQuestDeck = GameManager.Instance.QuestDeckDataBase.GetDeckByID(questDeckToLoadDebug);
        else
        {
            Debug.LogWarning("T'as pas lancé depuis le menu et t'as ni coché isDebugQuestManager ou pire encore ... le isDebugGameManager. Troudbal.");
            CurrentQuestDeck = GameManager.Instance.QuestDeckDataBase.GetDeckByID(questDeckID);
        }


        if (questDeckID == "deck_03" || questDeckID == "deck_01")
            CurrentQuestDeck.SideQuests.Clear();

           
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestsContainer {
    private List<Quest> quests;
    public bool isInitialized = false;

    public List<Quest> Quests
    {
        get
        {
            return quests;
        }

        set
        {
            quests = value;
        }
    }

    public void Init()
    {
        // Level 1
        quests = new List<Quest>();

        List<IQuestObjective> mainObjectives = new List<IQuestObjective>();
        mainObjectives.Add(new PrisonerEscortObjective("Bring Ashley to The End, and ALIVE.", "Bring Ashley to The End, and ALIVE.", null, null));
        quests.Add(new Quest(new QuestIdentifier("main_quest_01", ""), 
                            new QuestText("The last unicorn seal", 
                            "", 
                            "You're probably wondering why I gathered all of you here today. Well I'll be quick, I want you to bring this wonderful animal, Ashley, to my good and rich friend." + 
                            "Don't worry, you will be rewarded. His name is \"The End\", you'll see his flag from pretty far away, head towards it. I'm counting on you, it is extremely important.", 
                            "",
                            "Hint: Don't kill Ashley."), 
                            mainObjectives));


        List<IQuestObjective> objectives = new List<IQuestObjective>();

        objectives.Add(new KillMonsterObjective(Translater.SideQuestsText("level2", 0, QuestTexts.ObjectiveTitle),
                                                Translater.SideQuestsText("level2", 0, QuestTexts.ObjectiveDesc), 
                                                "rabbit_jacob_01"));

        quests.Add(new Quest(new QuestIdentifier("side_quest_01", "pnj_01"),
                                         new QuestText(Translater.SideQuestsText("level2", 0, QuestTexts.Title),
                                                        Translater.SideQuestsText("level2", 0, QuestTexts.Desc),
                                                        //"Oh hey fellow adventurer! You seem tough, and I really think you could help me. You see, a really scary monster is coming closer to the village and the inhabitants are getting stressed out. I can't fix the problem alone, and if we do nothing, I can't imagine what will happen to these people. Please, go to the north east and kill this beast! You will be rewarded, don't worry.", 
                                                        Translater.SideQuestsText("level2", 0, QuestTexts.Dialog),
                                                        Translater.SideQuestsText("level2", 0, QuestTexts.EndDialog),
                                                        Translater.SideQuestsText("level2", 0, QuestTexts.Hint)),
                                         objectives));

        // Level 2

        // Level 3

        // Level 4

        mainObjectives = new List<IQuestObjective>();
        mainObjectives.Add(new PrisonerEscortObjective("Until The End", "Bring Ashley to The End, and ALIVE.", null, null));
        quests.Add(new Quest(new QuestIdentifier("main_quest_04", ""),
                            new QuestText("Main Quest: The last phoque licorne",
                            "",
                            "You're probably wondering why I gathered all of you here today. Well I'll be quick, I want you to bring this wonderful animal, Ashley, to my good and rich friend." +
                            "Don't worry, you will be rewarded. His name is \"The End\", you'll see his flag from pretty far away, head towards it. I'm counting on you, it is extremely important.",
                            "",
                            "Hint: Don't kill Ashley."),
                            mainObjectives));


        objectives.Clear();
        objectives = new List<IQuestObjective>();


        objectives.Add(new MultipleEscortObjective(Translater.SideQuestsText("level4", 0, QuestTexts.ObjectiveTitle),
                                                Translater.SideQuestsText("level4", 0, QuestTexts.ObjectiveDesc),
                                                "duckling", 3, null));

        quests.Add(new Quest(new QuestIdentifier("side_quest_ducklings_01", "ducky"),
                                         new QuestText(Translater.SideQuestsText("level4", 0, QuestTexts.Title),
                                                        Translater.SideQuestsText("level4", 0, QuestTexts.Desc),
                                                        Translater.SideQuestsText("level4", 0, QuestTexts.Dialog),
                                                        Translater.SideQuestsText("level4", 0, QuestTexts.EndDialog),
                                                        Translater.SideQuestsText("level4", 0, QuestTexts.Hint)),
                                         objectives));

        isInitialized = true;
    }

    public Quest FindQuestByID(string id)
    {
        if(quests == null)
        {
            Debug.Log("Quest database has not been initialized. Plese use the Init function before trying to access quests.");
            return null;
        }

        return quests.Find(x => x.Identifier.ID == id);
    }
}

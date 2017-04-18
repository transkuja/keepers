using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class MainQuests {
    public List<Quest> quests;
    
    public void Init()
    {
        quests = new List<Quest>();
        List<IQuestObjective> mainObjectives = new List<IQuestObjective>();
        mainObjectives.Add(new PrisonerEscortObjective("Until The End", "Bring Ashley to The End, and ALIVE.", GameObject.FindObjectOfType<Behaviour.Prisoner>().gameObject, TileManager.Instance.EndTile));
        GameManager.Instance.MainQuest = new Quest(new QuestIdentifier("main_quest_0", ""), new QuestText("Main Quest: The last phoque licorne", "", "You're probably wondering why I gathered all of you here today. Well I'll be quick, I want you to bring this wonderful animal to my good and rich friend. Don't worry, you will be rewarded. His name is \"The End\", you'll see his flag from pretty far away, head towards it. I'm counting on you, it is extremely important.", "Hint: Don't kill Ashley."), mainObjectives);
    }
}

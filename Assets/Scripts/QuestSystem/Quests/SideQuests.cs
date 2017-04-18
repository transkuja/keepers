using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class SideQuests {
    public List<Quest> quests;

    public void Init()
    {
        quests = new List<Quest>();

        List<IQuestObjective> objectives = new List<IQuestObjective>();
        objectives.Add(new KillMonsterObjective("Kill the Jacob the SCARY MONSTER", 
                                                "Eliminate the scary monster menacing the village. That guy said it should be near this hidden passage...", 
                                                "rabbit_jacob_01" , 
                                                TileManager.Instance.EndTile));

        GameManager.Instance.MainQuest = new Quest(new QuestIdentifier("side_quest_01", "pnj_01"),
                                         new QuestText("Scary spooky monster must die", 
                                                        "A Frightful monster is menacing the village. Find it and persuade it not to come close again.", 
                                                        //"Oh hey fellow adventurer! You seem tough, and I really think you could help me. You see, a really scary monster is coming closer to the village and the inhabitants are getting stressed out. I can't fix the problem alone, and if we do nothing, I can't imagine what will happen to these people. Please, ", 
                                                        "Hey you! Kill that big thing over there! *points at a hidden passge between two bushes*",
                                                        "Hint: The target should be on a surrounding tile. Don't go alone."),
                                         objectives);


    }
}

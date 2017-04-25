using UnityEngine;
using System;
using UnityEngine.Events;
namespace QuestSystem
{
    public delegate void InitEvent();
    public interface IQuestObjective
    {
        InitEvent OnInit { get; set; }

        // We must give an ID to every Quest Objective class (static because it belongs to the class)
        // So we can know what to call when loading the quest from JSON
        int ID { get; }
        string Title { get; }
        string Description { get; }
        bool IsComplete { get; }

        void UpdateProgress();
        void CheckProgress();

        IQuestObjective GetCopy();

        //Needs to be called to link to the Event Manager
        void Init();
    }
}


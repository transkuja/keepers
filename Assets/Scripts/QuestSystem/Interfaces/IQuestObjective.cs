using UnityEngine;
using System;

namespace QuestSystem
{
    public interface IQuestObjective
    { 
        string Title { get; }
        string Description { get; }
        bool IsComplete { get; }

        void UpdateProgress();
        void CheckProgress();
    }
}


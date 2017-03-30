using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    [Serializable]
    public class Quest
    {
        // Events
        public delegate void QuestEvent();
        public QuestEvent OnQuestComplete;
        public QuestEvent OnQuestUpdate;
        public QuestEvent OnQuestFail;

        [SerializeField]
        // Contains the source object, and the quest ID  ///chain quest and the next quest is blank, chainquestiD
        private QuestIdentifier identifier;

        [SerializeField]
        // Contains the Name, DescriptionSummary, Quest Hint, Quest Dialog
        private QuestText information;

        [SerializeField]
        private QuestReward reward;

        public Quest(QuestIdentifier ident, QuestText info, List<IQuestObjective> obj, QuestReward _reward = null)
        {
            identifier = ident;
            information = info;
            objectives = obj;
            reward = _reward;

        }

        // Objectives
        private List<IQuestObjective> objectives;

        public QuestText Information
        {
            get
            {
                return information;
            }

            set
            {
                information = value;
            }
        }

        public QuestIdentifier Identifier
        {
            get
            {
                return identifier;
            }

            set
            {
                identifier = value;
            }
        }

        public List<IQuestObjective> Objectives
        {
            get
            {
                return objectives;
            }

            set
            {
                objectives = value;
            }
        }

        // Rewards

        public bool CheckAndComplete()
        {
            for (int i = 0; i < objectives.Count; i++)
            {
                objectives[i].CheckProgress();
                if (!objectives[i].IsComplete)
                {
                    return false;
                }
            }
            if (OnQuestComplete != null)
                OnQuestComplete();
            return true;
        }

        // Check Completion
        private bool IsComplete()
        {
            for (int i = 0; i < objectives.Count; i++)
            {
                if (!objectives[i].IsComplete)
                {
                    return false;
                }
            }
            if (OnQuestComplete != null)
                OnQuestComplete();
            return true;
        }
    }
}

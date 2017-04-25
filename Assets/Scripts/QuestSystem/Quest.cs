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
        public QuestEvent OnQuestInit;
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

        public Quest()
        { }

        public void Reset(QuestIdentifier ident, QuestText info, List<IQuestObjective> obj, QuestReward _reward = null)
        {
            identifier = ident;
            information = info;
            objectives = obj;
            reward = _reward;
        }

        public void Init()
        {
            for(int i = 0; i < objectives.Count; i++)
            {
                objectives[i].Init();
            }
            if (OnQuestInit != null)
                OnQuestInit();
        }

        public void ResetQuestObjectives(List<IQuestObjective> obj, QuestReward _reward = null)
        {
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

        // Check objectives and completion
        public bool CheckAndComplete() // Check
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

        public bool CheckIfComplete() //Check, pas d'event fire
        {
            for (int i = 0; i < objectives.Count; i++)
            {
                objectives[i].CheckProgress();
                if (!objectives[i].IsComplete)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Complete() //Pas de check
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

        // Check Completion
        public bool IsComplete() //Pas de check, pas d'event fire
        {
            for (int i = 0; i < objectives.Count; i++)
            {
                if (!objectives[i].IsComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

using UnityEngine;
using System;

namespace QuestSystem
{
    [Serializable]
    public class QuestIdentifier
    {
        [SerializeField]
        private string id;
        //private int chainQuestID;
        [SerializeField]
        private string sourceID;


        /*public int ChainQuestID
        {
            get
            {
                return chainQuestID;
            }
        }*/

        public string ID
        {
            get
            {
                return id;
            }
        }

        public string SourceID
        {
            get
            {
                return sourceID;
            }
        }

        public QuestIdentifier(string _id, string _sourceID)
        {
            id = _id;
            sourceID = _sourceID;
        }

        public QuestIdentifier(QuestIdentifier qi)
        {
            id = qi.id;
            sourceID = qi.sourceID;
        }
    }
}

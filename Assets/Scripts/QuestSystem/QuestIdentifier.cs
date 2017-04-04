using UnityEngine;
using System;

namespace QuestSystem
{
    [Serializable]
    public class QuestIdentifier
    {
        [SerializeField]
        private int id;
        //private int chainQuestID;
        [SerializeField]
        private GameObject source;


        /*public int ChainQuestID
        {
            get
            {
                return chainQuestID;
            }
        }*/

        public int ID
        {
            get
            {
                return id;
            }
        }

        public GameObject Source
        {
            get
            {
                return source;
            }
        }

        public QuestIdentifier(int _id, GameObject _source)
        {
            id = _id;
            source = _source;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestDeck
    {
        int id;
        string deckName;
        List<Quest> quests;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

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

        public string DeckName
        {
            get
            {
                return deckName;
            }

            set
            {
                deckName = value;
            }
        }
    }
}

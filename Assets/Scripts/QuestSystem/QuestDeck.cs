using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestDeck
    {
        string id;
        string deckName;
        string deckModelName;
        string mainQuest;
        List<string> sideQuests;
        string deckModelName;

        public string Id
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

        public List<string> SideQuests
        {
            get
            {
                return sideQuests;
            }

            set
            {
                sideQuests = value;
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

        public string MainQuest
        {
            get
            {
                return mainQuest;
            }

            set
            {
                mainQuest = value;
            }
        }

        public string DeckModelName
        {
            get
            {
                return deckModelName;
            }

            set
            {
                deckModelName = value;
            }
        }

        public QuestDeck()
        {
            sideQuests = new List<string>();
        }
    }
}

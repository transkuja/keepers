using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Boomlagoon.JSON;

namespace QuestSystem
{
    public class QuestDeckDatabase
    {
        public class QuestAssociationDealer
        {
            public string idQuest;
            public string idQuestDealer;

            public QuestAssociationDealer()
            {
                // For tests
                idQuest = "defaultIdQuest";
                idQuestDealer = "defaultIdQuestDealer";
            }
        }

        public class QuestDeckDataContainer
        {
            public string idQuestDeck;
            public string nameQuestDeck;
            public Sprite spriteQuestDeck;
            public string idMainQuest;
            public List<QuestAssociationDealer> secondaryQuests;

            public QuestDeckDataContainer()
            {
                // For tests
                idQuestDeck = "defaultId";
                nameQuestDeck = "defaultQuestDeckName";
                spriteQuestDeck = null;
                idMainQuest = "defaultIdMainQuest";

                // Not for tests
                secondaryQuests = new List<QuestAssociationDealer>();
            }
        }

        Dictionary<string, QuestDeckDataContainer> dicQuestDeck;

        // Use this for initialization
        public QuestDeckDatabase()
        {
            dicQuestDeck = new Dictionary<string, QuestDeckDataContainer>();
        }

        public void Init()
        {
            string pathBase = Application.dataPath + "/../Data";

            string fileContent = File.ReadAllText(pathBase + "/questDecks.json");
            JSONObject json = JSONObject.Parse(fileContent);

            JSONArray questDeckArray = json["QuestDecks"].Array;

            foreach (JSONValue questDeck in questDeckArray)
            {
                QuestDeckDataContainer newQuestDeckDataContainer = new QuestDeckDataContainer();
                foreach (KeyValuePair<string, JSONValue> deckQuestEntry in questDeck.Obj)
                {
                    switch (deckQuestEntry.Key)
                    {
                        // ROOT DATA
                        case "id":
                            newQuestDeckDataContainer.idQuestDeck = deckQuestEntry.Value.Str;
                            break;
                        case "name":
                            newQuestDeckDataContainer.nameQuestDeck = deckQuestEntry.Value.Str;
                            break;
                        case "sprite":
                            // if null -> didn't the corresponding sprite
                            newQuestDeckDataContainer.spriteQuestDeck = Resources.Load(deckQuestEntry.Value.Str) as Sprite;
                            break;
                        case "main":
                            newQuestDeckDataContainer.idMainQuest = deckQuestEntry.Value.Str;
                            break;
                        case "secondary":
                            JSONArray secondaryQuestArray = deckQuestEntry.Value.Array;
                            foreach (JSONValue secondaryQuest in secondaryQuestArray)
                            {
                                QuestAssociationDealer association = new QuestAssociationDealer();
                                foreach (KeyValuePair<string, JSONValue> secondaryQuestId in questDeck.Obj)
                                {
                                    switch (secondaryQuestId.Key)
                                    {
                                        // SECONDARY QUEST DATA
                                        case "idQuest":
                                          association.idQuest = deckQuestEntry.Value.Str;
                                            break;
                                        case "idQuestDealer":
                                            association.idQuestDealer = deckQuestEntry.Value.Str;
                                            break;
                                    }
                                }
                                newQuestDeckDataContainer.secondaryQuests.Add(association);
                            }
                            break;
                    }
                }
                dicQuestDeck.Add(newQuestDeckDataContainer.idQuestDeck, newQuestDeckDataContainer);
            }
        }
        #region Accessors
        public Dictionary<string, QuestDeckDataContainer> DicQuestDeck
        {
            get
            {
                return dicQuestDeck;
            }

            set
            {
                dicQuestDeck = value;
            }
        }
        #endregion
    }
}
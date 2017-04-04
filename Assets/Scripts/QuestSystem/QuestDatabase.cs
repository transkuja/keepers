using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Boomlagoon.JSON;

namespace QuestSystem
{
    public class QuestDatabase
    {

        public class InformationQuestData
        {
            public string descriptionSummary;
            public string dialog;
            public string hint;
        }

        public class QuestDataContainer
        {
            public string idQuest;
            public string nameQuest;
            public InformationQuestData informationsQuest;
            public string idQuestDealer;
            // Reward

            public QuestDataContainer()
            {
                informationsQuest = new InformationQuestData();
            }
        }


        Dictionary<string, QuestDataContainer> dicQuestDataContainer;

        // Use this for initialization
        public QuestDatabase()
        {
            dicQuestDataContainer = new Dictionary<string, QuestDataContainer>();
        }

        public void Init()
        {
            string pathBase = Application.dataPath + "/../Data";

            string fileContent = File.ReadAllText(pathBase + "/quests.json");
            JSONObject json = JSONObject.Parse(fileContent);

            JSONArray questsArray = json["Quests"].Array;

            foreach (JSONValue value in questsArray)
            {
                QuestDataContainer newQuestDataContainer = new QuestDataContainer();
                foreach (KeyValuePair<string, JSONValue> questEntry in value.Obj)
                {
                    switch (questEntry.Key)
                    {
                        // ROOT DATA
                        case "id":
                            newQuestDataContainer.idQuest = questEntry.Value.Str;
                            break;
                        case "name":
                            newQuestDataContainer.nameQuest = questEntry.Value.Str;
                            break;
                        case "information":
                            foreach (KeyValuePair<string, JSONValue> informationEntry in value.Obj)
                            {
                                switch (informationEntry.Key)
                                {
                                    // ROOT DATA
                                    case "description":
                                        newQuestDataContainer.informationsQuest.descriptionSummary = informationEntry.Value.Str;
                                        break;
                                    case "dialog":
                                        newQuestDataContainer.informationsQuest.dialog = informationEntry.Value.Str;
                                        break;
                                    case "hint":
                                        newQuestDataContainer.informationsQuest.hint = informationEntry.Value.Str;
                                        break;
                                }
                            }
                            break;
                    }
                }
                dicQuestDataContainer.Add(newQuestDataContainer.idQuest, newQuestDataContainer);
            }
        }
    }
}
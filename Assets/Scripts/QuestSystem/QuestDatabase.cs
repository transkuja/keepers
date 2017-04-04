using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Boomlagoon.JSON;

namespace QuestLoader
{
    public class QuestDatabase
    {

        public class InformationQuestData
        {
            public string descriptionSummary;
            public string dialog;
            public string hint;
        }

        public class QuestData
        {
            public string idQuest;
            public string nameQuest;
            public InformationQuestData informationsQuest;
            public string idQuestDealer;
            // Reward

            public QuestData()
            {
                informationsQuest = new InformationQuestData();
            }
        }


        List<QuestData> listQuest;

        // Use this for initialization
        public QuestDatabase()
        {
            listQuest = new List<QuestData>();
        }

        public void Init()
        {
            string pathBase = Application.dataPath + "/../Data";

            string fileContent = File.ReadAllText(pathBase + "/quests.json");
            JSONObject json = JSONObject.Parse(fileContent);

            JSONArray questsArray = json["Quests"].Array;

            foreach (JSONValue value in questsArray)
            {
                QuestData newQuestDataContainer = new QuestData();
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
                listQuest.Add(newQuestDataContainer);
            }
        }

        #region Accessors
        public List<QuestData> ListQuestData
        {
            get
            {
                return listQuest;
            }
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Boomlagoon.JSON;

public class LevelDataBase {

    public class Level
    {
        public int id;
        public string name;
        public string cardModelName;
        public List<string> listDeckId;

        public Level()
        {
            listDeckId = new List<string>();
        }
    }

    public List<Level> listLevels;

    public LevelDataBase()
    {
        listLevels = new List<Level>();
        Load();
    }


    public void Load()
    {
        string pathBase = Application.dataPath + "/../Data";

        string fileContent = File.ReadAllText(pathBase + "/levels.json");
        JSONObject json = JSONObject.Parse(fileContent);

        JSONArray levelArray = json["Levels"].Array;

        foreach (JSONValue level in levelArray)
        {
            Level newLevel = new Level();

            foreach (KeyValuePair<string, JSONValue> levelEntry in level.Obj)
            {
                switch (levelEntry.Key)
                {
                    // ROOT DATA
                    case "id":
                        newLevel.id = (int)levelEntry.Value.Number;
                        break;
                    case "name":
                        newLevel.name = levelEntry.Value.Str;
                        break;
                    case "cardModelName":
                        newLevel.cardModelName = levelEntry.Value.Str;
                        break;
                    case "questDecks":
                        JSONArray questDecksArray = levelEntry.Value.Array;
                        foreach (JSONValue questDeck in questDecksArray)
                        {
                            foreach (KeyValuePair<string, JSONValue> questEntry in questDeck.Obj)
                            {
                                switch (questEntry.Key)
                                {
                                    // SECONDARY QUEST DATA
                                    case "id":
                                        newLevel.listDeckId.Add(questEntry.Value.Str);
                                        break;
                                }
                            }
                        }
                        break;
                }
            }
            listLevels.Add(newLevel);
        }
    }

    public Level GetLevelById(int id)
    {
        return listLevels.Find(x => x.id == id);
    }
}

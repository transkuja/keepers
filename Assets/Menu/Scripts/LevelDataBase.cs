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
        public string deckId;
        public string nbPawn;
        public string difficulty;
        public List<string> listEventsId;

        public Level()
        {
            listEventsId = new List<string>();
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
        string path = Path.Combine(Application.streamingAssetsPath, "levels.json");
        string fileContents;

        if (path.Contains("://"))
        {
            WWW www = new WWW(path);
            fileContents = www.text;
        }
        else
            fileContents = File.ReadAllText(path);

        JSONObject json = JSONObject.Parse(fileContents);

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
                    case "questDeckId":
                        newLevel.deckId = levelEntry.Value.Str;
                        break;
                    case "nbPawn":
                        newLevel.nbPawn = levelEntry.Value.Str;
                        break;
                    case "difficulty":
                        newLevel.difficulty = levelEntry.Value.Str;
                        break;
                    case "events":
                        JSONArray Array = levelEntry.Value.Array;
                        foreach (JSONValue entry in Array)
                        {
                            newLevel.listEventsId.Add(entry.Str);
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

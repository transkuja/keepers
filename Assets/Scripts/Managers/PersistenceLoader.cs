using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Boomlagoon.JSON;

public class PersistenceData
{
    public Dictionary<string, bool> dicPersistencePawns;
    public Dictionary<string, bool> dicPersistenceLevels;
    public Dictionary<string, bool> dicPersistenceEvents;
    public Dictionary<string, bool> dicPersistenceDecks;
    public Dictionary<string, bool> dicPersistenceSequences;
    public List<bool> dicPersistenceOptions;


    public PersistenceData()
    {
        dicPersistencePawns = new Dictionary<string, bool>();
        dicPersistenceLevels = new Dictionary<string, bool>();
        dicPersistenceEvents = new Dictionary<string, bool>();
        dicPersistenceDecks = new Dictionary<string, bool>();
        dicPersistenceSequences = new Dictionary<string, bool>();
        dicPersistenceOptions = new List<bool>();
    }
}

public class PersistenceLoader {

    private PersistenceData pd;

    private JSONObject json;

    public PersistenceLoader()
    {
        pd = new PersistenceData();
    }


    #region Accessors
    public PersistenceData Pd
    {
        get
        {
            return pd;
        }

        set
        {
            pd = value;
        }
    }
    #endregion

    public void Load()
    {
        string pathBase = Application.persistentDataPath;
        string fileContents;

        if (!File.Exists(pathBase + "/sauvegarde.json"))
        {
            string path = Path.Combine(Application.streamingAssetsPath, "sauvegardeReset.json");

            if (path.Contains("://"))
            {
                WWW www = new WWW(path);
                fileContents = www.text;
            }
            else
                fileContents = File.ReadAllText(path);

            File.WriteAllText(pathBase + "/sauvegarde.json", fileContents);
        }
        else
            fileContents = File.ReadAllText(pathBase + "/sauvegarde.json");

        json = JSONObject.Parse(fileContents);

        JSONArray pawnArray = json["Pawns"].Array;
        foreach (JSONValue value in pawnArray)
        {
            string id = String.Empty;
            bool isUnlocked = false;
            foreach (KeyValuePair<string, JSONValue> pawnEntry in value.Obj)
            {
                switch (pawnEntry.Key)
                {
                    // ROOT DATA
                    case "id":
                        id = pawnEntry.Value.Str;
                        break;
                    case "unlocked":
                        isUnlocked = pawnEntry.Value.Boolean;
                        break;
                }
            }
            if (!pd.dicPersistencePawns.ContainsKey(id)){
                pd.dicPersistencePawns.Add(id, isUnlocked);
            }
    
        }

        JSONArray levelsArray = json["Levels"].Array;
        foreach (JSONValue value in levelsArray)
        {
            string id = String.Empty;
            bool isUnlocked = false;
            foreach (KeyValuePair<string, JSONValue> levelEntry in value.Obj)
            {
                switch (levelEntry.Key)
                {
                    // ROOT DATA
                    case "id":
                        id = levelEntry.Value.Str;
                        break;
                    case "unlocked":
                        isUnlocked = levelEntry.Value.Boolean;
                        break;
                }
            }
            if (!pd.dicPersistenceLevels.ContainsKey(id))
            {
                pd.dicPersistenceLevels.Add(id, isUnlocked);
            }
        }

        JSONArray deckArray = json["Decks"].Array;
        foreach (JSONValue value in deckArray)
        {
            string id = String.Empty;
            bool isUnlocked = false;
            foreach (KeyValuePair<string, JSONValue> deckEntry in value.Obj)
            {
                switch (deckEntry.Key)
                {
                    // ROOT DATA
                    case "id":
                        id = deckEntry.Value.Str;
                        break;
                    case "unlocked":
                        isUnlocked = deckEntry.Value.Boolean;
                        break;
                }
            }
            if (!pd.dicPersistenceDecks.ContainsKey(id))
            {
                pd.dicPersistenceDecks.Add(id, isUnlocked);
            }
        }

        JSONArray eventsArray = json["Events"].Array;
        foreach (JSONValue value in eventsArray)
        {
            string id = String.Empty;
            bool isUnlocked = false;
            foreach (KeyValuePair<string, JSONValue> eventEntry in value.Obj)
            {
                switch (eventEntry.Key)
                {
                    // ROOT DATA
                    case "id":
                        id = eventEntry.Value.Str;
                        break;
                    case "unlocked":
                        isUnlocked = eventEntry.Value.Boolean;
                        break;
                }
            }
            if (!pd.dicPersistenceEvents.ContainsKey(id))
            {
                pd.dicPersistenceEvents.Add(id, isUnlocked);
            }
        }

        JSONObject CameraObject = json["Camera"].Obj;
        bool isFollowingKeeper = false;
        foreach (KeyValuePair<string, JSONValue> optionEntry in CameraObject)
        {
            switch (optionEntry.Key)
            {
                // ROOT DATA
                case "isFollowingKeeper":
                    isFollowingKeeper = optionEntry.Value.Boolean;
                    if (!pd.dicPersistenceOptions.Contains(isFollowingKeeper))
                    {
                        pd.dicPersistenceOptions.Add(isFollowingKeeper);
                    }
                    break;
            }
        }

        JSONArray sequencesArray = json["Sequences"].Array;
        foreach (JSONValue value in sequencesArray)
        {
            string id = String.Empty;
            bool alreadyPlayed = false;
            foreach (KeyValuePair<string, JSONValue> eventEntry in value.Obj)
            {
                switch (eventEntry.Key)
                {
                    // ROOT DATA
                    case "id":
                        id = eventEntry.Value.Str;
                        break;
                    case "alreadyPlayed":
                        alreadyPlayed = eventEntry.Value.Boolean;
                        break;
                }
            }
            if (!pd.dicPersistenceSequences.ContainsKey(id))
            {
                pd.dicPersistenceSequences.Add(id, alreadyPlayed);
            }
        }
    }

    public void SetPawnUnlocked(string key, bool isUnlocked)
    {
        SetUnlocked("Pawns", key, "unlocked", isUnlocked);
    }

    public void SetLevelUnlocked(string key, bool isUnlocked)
    {
        SetUnlocked("Levels", key, "unlocked", isUnlocked);
    }

    public void SetDeckUnlocked(string key, bool isUnlocked)
    {
        SetUnlocked("Decks", key, "unlocked", isUnlocked);

    }

    public void SetEventUnlocked(string key, bool isUnlocked)
    {
        SetUnlocked("Events", key, "unlocked", isUnlocked);

    }

    public void SetSequenceUnlocked(string id, bool alreadyPlayed)
    {
        SetUnlocked("Sequences", id, "alreadyPlayed",  alreadyPlayed);
    }

    public void SetOptionUnlocked(bool isActive)
    {
        SetOption("Camera", "isFollowingKeeper", isActive);
    }

    private void SetUnlocked(string what, string id, string secondKey, bool isUnlocked)
    {
        JSONArray array = json[what].Array;
        bool isRightKey;
        foreach (JSONValue value in array)
        {
            isRightKey = false;
            foreach (KeyValuePair<string, JSONValue> pawnEntry in value.Obj)
            {

                if (pawnEntry.Key == "id")
                {
                    if (pawnEntry.Value.Str == id)
                        isRightKey = true;
                }

                if (isRightKey == true && pawnEntry.Key == secondKey)
                {
        
                     pawnEntry.Value.Boolean = isUnlocked;
                     break;
                }
            }
        }
        string pathBase = Application.persistentDataPath;
        File.WriteAllText(pathBase + "/sauvegarde.json", json.ToString());

        return;
    }

    private void SetOption(string what, string id, bool isUnlocked)
    {

        JSONObject CameraObject = json[what].Obj;
        foreach (KeyValuePair<string, JSONValue> optionEntry in CameraObject)
        {
            if (optionEntry.Key == id)
            {
                optionEntry.Value.Boolean = isUnlocked;
                break;
            }
        }

        string pathBase = Application.persistentDataPath;
        File.WriteAllText(pathBase + "/sauvegarde.json", json.ToString());

        return;
    }
}




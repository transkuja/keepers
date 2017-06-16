using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Boomlagoon.JSON;

public static class ChatBoxDatabase
{
    public static List<string>[] tabEmotes;
    public static List<string>[] tabEmotesFR;

    public static List<string>[] TabEmotes
    {
        get
        {
            if (Translater.CurrentLanguage == LanguageEnum.FR) return tabEmotesFR;
            else            
                return tabEmotes;            
        }
    }

    public static void Load()
    {
        tabEmotes = new List<string>[5] {new List<string>(),new List<string>(),new List<string>(),new List<string>(), new List<string>() };

        string pathBase = Application.dataPath + "/../Data";

        string fileContent = File.ReadAllText(pathBase + "/chatbox.json");
        JSONObject json = JSONObject.Parse(fileContent);

        JSONArray Array = json["Awaiting"].Array;
        foreach (JSONValue entry in Array)
        {
            tabEmotes[0].Add(entry.Str);
        }

        Array = json["Chosen"].Array;
        foreach (JSONValue entry in Array)
        {
            tabEmotes[1].Add(entry.Str);
        }

        Array = json["Picked"].Array;
        foreach (JSONValue entry in Array)
        {
            tabEmotes[2].Add(entry.Str);
        }

        Array = json["Unchosen"].Array;
        foreach (JSONValue entry in Array)
        {
            tabEmotes[3].Add(entry.Str);
        }

        tabEmotesFR = new List<string>[5] { new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>() };

        Array = json["AwaitingFr"].Array;
        foreach (JSONValue entry in Array)
        {
            tabEmotesFR[0].Add(entry.Str);
        }

        Array = json["ChosenFr"].Array;
        foreach (JSONValue entry in Array)
        {
            tabEmotesFR[1].Add(entry.Str);
        }

        Array = json["PickedFr"].Array;
        foreach (JSONValue entry in Array)
        {
            tabEmotesFR[2].Add(entry.Str);
        }

        Array = json["UnchosenFr"].Array;
        foreach (JSONValue entry in Array)
        {
            tabEmotesFR[3].Add(entry.Str);
        }

        Array = json["WhoAmI"].Array;
        foreach (JSONValue entry in Array)
        {
            tabEmotes[4].Add(entry.Str);
            tabEmotesFR[4].Add(entry.Str);
        }
    }
}

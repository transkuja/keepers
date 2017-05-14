using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Boomlagoon.JSON;

public static class ChatBoxDatabase
{
    public static List<string>[] tabEmotes;

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

        Array = json["WhoAmI"].Array;
        foreach (JSONValue entry in Array)
        {
            tabEmotes[4].Add(entry.Str);
        }
    }
}

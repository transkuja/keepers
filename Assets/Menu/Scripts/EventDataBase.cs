using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Boomlagoon.JSON;

public class EventDataBase
{

    public class Event
    {
        public string id;
        public string name;
        public string description;
        public string cardModelName;
    }

    public List<Event> listEvents;

    public EventDataBase()
    {
        listEvents = new List<Event>();
    }

    public void Init()
    {
        string pathBase = Application.dataPath + "/../Data";

        string fileContent = File.ReadAllText(pathBase + "/events.json");
        JSONObject json = JSONObject.Parse(fileContent);

        JSONArray EventArray = json["Events"].Array;

        foreach (JSONValue evt in EventArray)
        {
            Event newEvent = new Event();

            foreach (KeyValuePair<string, JSONValue> levelEntry in evt.Obj)
            {
                switch (levelEntry.Key)
                {
                    // ROOT DATA
                    case "id":
                        newEvent.id = levelEntry.Value.Str;
                        break;
                    case "name":
                        newEvent.name = levelEntry.Value.Str;
                        break;
                    case "description":
                        newEvent.description = levelEntry.Value.Str;
                        break;
                    case "cardModelName":
                        newEvent.cardModelName = levelEntry.Value.Str;
                        break;
                }
            }
            listEvents.Add(newEvent);
        }
    }

    public Event GetEventById(string id)
    {
        return listEvents.Find(x => x.id == id);
    }
}

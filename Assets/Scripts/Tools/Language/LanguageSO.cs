using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Boomlagoon.JSON;

[CreateAssetMenu(fileName="Language.asset", menuName="Language Database")]
public class LanguageSO : ScriptableObject {

    [System.Serializable]
    private struct KeyValueStore
    {
        public string key;
        public string value;
    }

    [SerializeField] private List<KeyValueStore> entries;

    private static UnityEvent updateEvent = new UnityEvent();
    public static UnityEvent UpdateEvent {
        get { return updateEvent; }
    }

    private static LanguageSO activeDatabase = null;
    private static LanguageSO Database {
        get {
            if (activeDatabase == null) {
                LoadDatabase("EN");
            }
            return activeDatabase;
        }
    }

    public static void LoadDatabase(string language)
    {
        activeDatabase = Resources.Load<LanguageSO>(language);
        updateEvent.Invoke();
    }

    public static string GetText(string key)
    {
        LanguageSO db = Database;
        if (db != null) {
            foreach (KeyValueStore kv in db.entries)
            {
                if (kv.key == key)
                    return kv.value;
            }
        }
        return string.Empty;
    }

#if UNITY_EDITOR
    public bool Import(JSONObject source)
    {
        if (source.ContainsKey("locale") == false
                || source["locale"].Type != JSONValueType.String
                || Regex.IsMatch(source["locale"].Str, @"^[a-z]{2}(_[A-Z]{2})?$") == false
                || source.ContainsKey("keys") == false
                || source["keys"].Type != JSONValueType.Object)
            return false;

        entries = new List<KeyValueStore>();
        foreach (KeyValuePair<string, JSONValue> entry in source["keys"].Obj)
        {
            if (entry.Value.Type == JSONValueType.String)
                entries.Add(new KeyValueStore()
                {
                    key = entry.Key,
                    value = entry.Value.Str
                });
        }

        UnityEditor.EditorUtility.SetDirty(this);

        return true;
    }
#endif

}

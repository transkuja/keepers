using UnityEngine;
using UnityEditor;
using System.IO;
using Boomlagoon.JSON;

public static class LanguageImporter {

    [MenuItem("Tools/Import language...")]
	static void ImportLanguage()
    {
        string path = EditorUtility.OpenFilePanel("Select language JSON file", "", "json");
        if (path != string.Empty)
        {
            try {
                string fileContents = File.ReadAllText(path);
                JSONObject json = JSONObject.Parse(fileContents);
                if ((Selection.activeObject as LanguageSO).Import(json))
                    EditorUtility.DisplayDialog("Conclusion", "Yes!", "ok");
                else
                    EditorUtility.DisplayDialog("Conclusion", "Nope :(", "ok");
            }
            catch
            {
                EditorUtility.DisplayDialog("Error", "An error has occured", "ok");
            }
        }
    }

    [MenuItem("Tools/Import language...", true)]
    static bool CanImportLanguage()
    {
        if (Selection.objects.Length == 1)
            return Selection.activeObject is LanguageSO;
        else
            return false;
    }

}

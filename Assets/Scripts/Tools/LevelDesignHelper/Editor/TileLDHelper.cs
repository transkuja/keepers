using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class TileLDHelper : EditorWindow {
    int width = 6;
    int height = 6;
    float space = 0;
    float spaceX = 0.0f;
    float spaceZ = 0.0f;
    GameObject[] tiles = null;
    GameObject HelperObject = null;
    HelperRoot helperRoot = null;
    GameObject selectedObject = null;
    GameObject emptyTilePrefab = null;
    GameObject[] selectedObjects = null;
    TileType selectedType = TileType.None; //The type selected by the user
    TileType currentType = TileType.None; //The current loaded type (update when selectedType is changed)
    GameObject[] models = null;
    int selectedModelIndex = 0;

    bool createdHelper = false;

    bool toggleGeneration = true;
    bool toggleManagement = false;

    //Error messages
    bool errorLoadNoComponent = false;
    bool errorLoadNoSelection = false;

    [MenuItem("Level Design/Level creation Helper...")]
    public static void ShowWindow()
    {
        TileLDHelper ld = EditorWindow.GetWindow(typeof(TileLDHelper), false) as TileLDHelper;
        ld.OnSelectionChange();
        ld.Show();
    }

    void OnSelectionChange()
    {
        Object[] selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel | SelectionMode.ExcludePrefab);
        selectedObjects = new GameObject[selection.Length];
        for(int i = 0; i < selection.Length; i++)
        {
            selectedObjects[i] = selection[i] as GameObject;
        }
        selectedObject = Selection.activeObject as GameObject;
        Repaint();
    }

	void OnGUI()
    {
        
        if (HelperObject == null)
        {
            EditorGUILayout.HelpBox("No Helper selected.\nIf you want to create a new one, use \"Grid Generation\" tab.\nIf you want to load an existing helper, select it in the hierarchy and click \"Load Helper\"", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("Helper Root \" " + HelperObject.name + " \" selected.", MessageType.Info);
        }
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Load Helper", GUILayout.Height(30), GUILayout.Width(100)))
        {
            OnSelectionChange();
            if(selectedObject == null)
            {
                errorLoadNoSelection = true;
            }
            else
            {
                errorLoadNoSelection = false;
                if (selectedObject.GetComponent<HelperRoot>() != null)
                {
                    errorLoadNoComponent = false;
                    createdHelper = false;
                    LoadHelper();
                }
                else
                {
                    errorLoadNoComponent = true;
                    HelperObject = null;
                    helperRoot = null;
                }
            }
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (errorLoadNoSelection)
        {
            EditorGUILayout.HelpBox("Select an object in the Hierarchy", MessageType.Error);
        }
        if (errorLoadNoComponent)
        {
            EditorGUILayout.HelpBox("The selected object must have a \"Helper Root\" component.", MessageType.Warning);
        }
        toggleGeneration = EditorGUILayout.Foldout(toggleGeneration, "Grid Generation", true);
        if(toggleGeneration)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(50);
            EditorGUILayout.BeginVertical();
            //GUILayout.Label("Grid Generation", EditorStyles.boldLabel);
            width = EditorGUILayout.IntField("Columns Count", width);
            height = EditorGUILayout.IntField("Rows Count", height);
            GUILayout.Space(10);
            GUILayout.Label("Space between");

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            spaceX = EditorGUILayout.FloatField("Each column", spaceX);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            spaceZ = EditorGUILayout.FloatField("Each row", spaceZ);
            GUILayout.EndHorizontal();

            width = width < 1 ? 1 : width;
            height = height < 1 ? 1 : height;
            space = space < 0.0f ? 0.0f : space;
            spaceX = spaceX < 0.0f ? 0.0f : spaceX;
            spaceZ = spaceZ < 0.0f ? 0.0f : spaceZ;

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Generate Helper Tiles", GUILayout.Height(30), GUILayout.Width(200)))
            {
                GenerateTiles();
                createdHelper = true;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUILayout.Space(50);
            EditorGUILayout.EndHorizontal();
            if(createdHelper)
                EditorGUILayout.HelpBox("The Helper has been successfully generated.\nHowever, you will need to select it in the hierarchy and press \"Load Helper\" to use it.", MessageType.Warning);
        }
        
        GUILayout.Space(5);
        toggleManagement = EditorGUILayout.Foldout(toggleManagement, "Tile Editing", true);
        if(toggleManagement)
        {
            bool validSelection = true;
            if(selectedObjects == null)
            {
                EditorGUILayout.HelpBox("Select one or more tile to edit their properties.", MessageType.Info);
                GUI.enabled = false;
            }
            else
            {
                foreach (GameObject go in selectedObjects)
                {
                    Tile t = go.GetComponentInParent<Tile>();
                    if (t == null)
                    {
                        validSelection = false;
                    }
                }
                if (!validSelection)
                {
                    EditorGUILayout.HelpBox("Warning: One or more of your selected objects and their parent does not have a Tile component.", MessageType.Warning);
                    GUI.enabled = false;
                }
                else
                {
                    GUI.enabled = true;
                }
            }
            selectedType = (TileType)EditorGUILayout.EnumPopup("Tile Type",selectedType);
            if(selectedType != currentType)
            {
                currentType = selectedType;
                UpdateModels();
                selectedModelIndex = 0;
            }
            List<string> choices= new List<string>();
            choices.Add("None");
            if(models != null)
                foreach(GameObject go in models)
                {
                    choices.Add(go.name);
                }
            selectedModelIndex = EditorGUILayout.Popup("Model to apply", selectedModelIndex, choices.ToArray());

            if(GUILayout.Button("Apply"))
            {
                if(selectedModelIndex == 0 && selectedType == TileType.None)
                {
                    if (emptyTilePrefab == null)
                        LoadEmptyTilePrefab();
                    for (int i = 0; i < selectedObjects.Length; i++)
                    {
                        if (selectedObjects[i].GetComponentInParent<Tile>() != null)
                        {
                            UpdateTile(selectedObjects[i].GetComponentInParent<Tile>(), currentType, emptyTilePrefab);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < selectedObjects.Length; i++)
                    {
                        if (selectedObjects[i].GetComponentInParent<Tile>() != null)
                        {
                            UpdateTile(selectedObjects[i].GetComponentInParent<Tile>(), currentType, (selectedModelIndex - 1 >= 0) ? models[selectedModelIndex - 1] : null);
                        }
                    }
                }
            }

            GUI.enabled = true;
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (HelperObject == null)
                GUI.enabled = false;
            if (GUILayout.Button("Link all Tiles", GUILayout.Height(30), GUILayout.Width(200)))
            {
                LinkTiles();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
        }
        GUI.enabled = true;
        Tile.ShowLinks = GUILayout.Toggle(Tile.ShowLinks, "Show Tiles Links");
        if (HelperObject == null)
            GUI.enabled = false;
        GUILayout.Space(15);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Clean up", GUILayout.Height(30), GUILayout.Width(100)))
        {
            Undo.FlushUndoRecordObjects();
            Undo.SetCurrentGroupName("UndoCleanUp");
            CleanUp();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.HelpBox("Information Warning: the \"TilePrefab\" object must stay the first child of its parent (a Tile). Same goes for the model object, which must stay the first child of the \"TilePrefab\" object.", MessageType.Info);
    }

    void LoadEmptyTilePrefab()
    {
        string tilePath = "Assets/Prefabs/Tiles/TilePrefab.prefab";
        emptyTilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(tilePath);
    }

    void LoadHelper()
    {
        HelperObject = selectedObject;
        helperRoot = HelperObject.GetComponent<HelperRoot>();
        tiles = new GameObject[HelperObject.transform.childCount];
        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = HelperObject.transform.GetChild(i).gameObject;
        }
    }

    void UpdateTile(Tile tile, TileType type, GameObject model = null)
    {
        Undo.RecordObject(tile, "Changing model " + tile.name);
        tile.Type = type;
        Vector3 prevPos = tile.transform.position;
        if(model != null)
        {
            if(model == emptyTilePrefab)
            {
                for (int i = 0; i < tile.transform.GetChild(0).childCount; i++)
                {
                    Transform child = tile.transform.GetChild(0).GetChild(i);
                    if (child.name == "TileModel" || child.name == "Model")
                    {
                        prevPos = child.position;
                        Undo.DestroyObjectImmediate(child.gameObject);
                    }
                }
                GameObject go = PrefabUtility.InstantiatePrefab(model) as GameObject;
                GameObject go2 = go.transform.GetChild(0).gameObject;

                go2.name = "Model";
                go.transform.parent = tile.transform.GetChild(0);
                go2.transform.parent = go.transform.parent;
                go2.transform.SetAsFirstSibling();
                DestroyImmediate(go);
                go2.transform.position = prevPos;
                Undo.RegisterCreatedObjectUndo(go2, "Created Model " + tile.name);
            }
            else
            {
                for (int i = 0; i < tile.transform.GetChild(0).childCount; i++)
                {
                    Transform child = tile.transform.GetChild(0).GetChild(i);
                    if (child.name == "TileModel" || child.name == "Model")
                    {
                        prevPos = child.position;
                        Undo.DestroyObjectImmediate(child.gameObject);
                    }
                }
                GameObject go = PrefabUtility.InstantiatePrefab(model) as GameObject;
                go.name = "TileModel";
                go.transform.parent = tile.transform.GetChild(0);
                go.transform.SetAsFirstSibling();
                go.transform.position = prevPos;
                Undo.RegisterCreatedObjectUndo(go, "Created Model " + tile.name);
            }
        }
    }

    void UpdateModels()
    {
        switch(currentType)
        {
            case TileType.Mountain:
                models = GetAtPath<GameObject>("Prefabs/Tiles/Mountain") as GameObject[];
                break;
            case TileType.Plain:
                models = GetAtPath<GameObject>("Prefabs/Tiles/Plain") as GameObject[];
                break;
            case TileType.Snow:
                models = GetAtPath<GameObject>("Prefabs/Tiles/Snow") as GameObject[];
                break;
            case TileType.Beach:
                models = GetAtPath<GameObject>("Prefabs/Tiles/Beach") as GameObject[];
                break;
        }
    }

    void GenerateTiles()
    {
        EditorUtility.DisplayProgressBar("Generation", "Setting Up...", 0.0f);
        space += 2.0f;
        tiles = new GameObject[width * height * 2];
        GameObject root = new GameObject("Helper Tiles", typeof(HelperRoot));
        Undo.RegisterCreatedObjectUndo(root, "Created Helper Tiles");
        HelperRoot hr = root.GetComponent<HelperRoot>();
        hr.Width = width;
        hr.Height = height;
        if (emptyTilePrefab == null)
            LoadEmptyTilePrefab();
        GameObject prefab = emptyTilePrefab;
        Vector3 size = prefab.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size;
        float baseSpaceX = spaceX;
        float baseSpaceZ = spaceZ;
        //We apply all the size transformations to match what we see in the scene
        if (prefab.transform.GetChild(0).localRotation.eulerAngles.y == 90.0f)
        {
            
            size.x *= prefab.transform.localScale.z; // Le x et le z sont inversés car le mesh utilisé pour l'instant est mal orienté
            size.y *= prefab.transform.localScale.y;
            size.z *= prefab.transform.localScale.x;

            size.x *= prefab.transform.GetChild(0).localScale.z;
            size.y *= prefab.transform.GetChild(0).localScale.y;
            size.z *= prefab.transform.GetChild(0).localScale.x;

            size.x *= prefab.transform.GetChild(0).GetChild(0).localScale.z;
            size.y *= prefab.transform.GetChild(0).GetChild(0).localScale.y;
            size.z *= prefab.transform.GetChild(0).GetChild(0).localScale.x;
            spaceX += size.z;
            spaceZ += size.x;
        }
        else
        {
            size.x *= prefab.transform.localScale.x;
            size.y *= prefab.transform.localScale.y;
            size.z *= prefab.transform.localScale.z;

            size.x *= prefab.transform.GetChild(0).localScale.x;
            size.y *= prefab.transform.GetChild(0).localScale.y;
            size.z *= prefab.transform.GetChild(0).localScale.z;

            size.x *= prefab.transform.GetChild(0).GetChild(0).localScale.x;
            size.y *= prefab.transform.GetChild(0).GetChild(0).localScale.y;
            size.z *= prefab.transform.GetChild(0).GetChild(0).localScale.z;
            spaceX += size.x;
            spaceZ += size.z;
        }

        if (prefab == null)
            Debug.Log("No prefab");
        for(int j = 0; j < height * 2; j++)
        {
            for(int i = 0; i < width; i++)
            {
                EditorUtility.DisplayProgressBar("Generation", "Generating Tiles...", (float)((j*width+i+1.0f))/(width*height*2) );
                tiles[j * width + i] = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                Vector3 pos = Vector3.zero;
                GameObject tile = new GameObject("Tile " + (i+1) +" / " + (j+1), typeof(Tile));
                
                tile.transform.parent = root.transform;
                if (j%2 == 0)
                {
                    pos.x = i * spaceX*1.5f;
                    pos.z = j * spaceZ/2.0f;
                }
                else
                {
                    pos.x = i * spaceX*1.5f + spaceX*0.75f;
                    pos.z = j * spaceZ/2.0f + spaceZ*0.0f;
                }
                tile.transform.position = pos;
                tiles[j * width + i].transform.position = pos;
                tiles[j * width + i].transform.parent = tile.transform;
            }
        }
        EditorUtility.DisplayProgressBar("Generation", "Done", 1.0f);
        space -= 2.0f;
        spaceX = baseSpaceX;
        spaceZ = baseSpaceZ;

        EditorUtility.ClearProgressBar();
        DestroyImmediate(emptyTilePrefab);
        helperRoot = null;
        HelperObject = null;
        tiles = null;
    }

    void LinkTiles()
    {
        for(int j = 0; j < helperRoot.Height*2; j++)
        {
            for(int i = 0; i < helperRoot.Width; i++)
            {
                if(isTileEmpty(tiles[j*helperRoot.Width + i]) == false)
                {
                    Tile tile = tiles[j * helperRoot.Width + i].GetComponent<Tile>();
                    //North
                    if ((j >= helperRoot.Height * 2 - 2 && j%2 == 1) || (j >= helperRoot.Height * 2 - 3 && j % 2 == 0))
                    {
                        tile.North = null;
                    }
                    else
                    {
                        if (isTileEmpty(tiles[(j+2) * helperRoot.Width + i]) == false)
                        {
                            tile.North = tiles[(j + 2) * helperRoot.Width + i].GetComponent<Tile>();
                        }
                        else
                        {
                            tile.North = null;
                        }
                    }
                    //North East
                    if ((j >= helperRoot.Height * 2 - 1)
                        || (i >= helperRoot.Width - 1 && j % 2 == 1))
                    {
                        tile.North_East = null;
                    }
                    else
                    {
                        if(j%2 == 0)
                        {
                            if (isTileEmpty(tiles[(j + 1) * helperRoot.Width + i]) == false)
                            {
                                tile.North_East = tiles[(j + 1) * helperRoot.Width + i].GetComponent<Tile>();
                            }
                            else
                            {
                                tile.North_East = null;
                            }
                        }
                        else
                        {
                            if (isTileEmpty(tiles[(j + 1) * helperRoot.Width + i + 1]) == false)
                            {
                                tile.North_East = tiles[(j + 1) * helperRoot.Width + i + 1].GetComponent<Tile>();
                            }
                            else
                            {
                                tile.North_East = null;
                            }
                        }
                        
                    }
                    //South East
                    if ((j == 0) || (i >= helperRoot.Width - 1 && j % 2 == 1))
                    {
                        tile.South_East = null;
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            if (isTileEmpty(tiles[(j - 1) * helperRoot.Width + i]) == false)
                            {
                                tile.South_East = tiles[(j - 1) * helperRoot.Width + i].GetComponent<Tile>();
                            }
                            else
                            {
                                tile.South_East = null;
                            }
                        }
                        else
                        {
                            if (isTileEmpty(tiles[(j - 1) * helperRoot.Width + i + 1]) == false)
                            {
                                tile.South_East = tiles[(j - 1) * helperRoot.Width + i + 1].GetComponent<Tile>();
                            }
                            else
                            {
                                tile.South_East = null;
                            }
                        }

                    }

                    //South
                    if ((j == 0 || j == 1))
                    {
                        tile.Neighbors[(int)Direction.South] = null;
                    }
                    else
                    {
                        if (isTileEmpty(tiles[(j - 2) * helperRoot.Width + i]) == false)
                        {
                            tile.Neighbors[(int)Direction.South] = tiles[(j - 2) * helperRoot.Width + i].GetComponent<Tile>();
                        }
                        else
                        {
                            tile.Neighbors[(int)Direction.South] = null;
                        }
                    }
                    //South West
                    if ((j == 0) || (i == 0 && j % 2 == 0))
                    {
                        tile.South_West = null;
                    }
                    else
                    {
                        if (j % 2 == 1)
                        {
                            if (isTileEmpty(tiles[(j - 1) * helperRoot.Width + i]) == false)
                            {
                                tile.South_West = tiles[(j - 1) * helperRoot.Width + i].GetComponent<Tile>();
                            }
                            else
                            {
                                tile.South_West = null;
                            }
                        }
                        else
                        {
                            if (isTileEmpty(tiles[(j - 1) * helperRoot.Width + i - 1]) == false)
                            {
                                tile.South_West = tiles[(j - 1) * helperRoot.Width + i - 1].GetComponent<Tile>();
                            }
                            else
                            {
                                tile.South_West = null;
                            }
                        }

                    }
                    //North West
                    if ((j >= helperRoot.Height * 2 - 1)
                        || (i == 0 && j%2 == 0))
                    {
                        tile.North_West = null;
                    }
                    else
                    {
                        if (j % 2 == 1)
                        {
                            if (isTileEmpty(tiles[(j + 1) * helperRoot.Width + i]) == false)
                            {
                                tile.North_West = tiles[(j + 1) * helperRoot.Width + i].GetComponent<Tile>();
                            }
                            else
                            {
                                tile.Neighbors[(int)Direction.North_West] = null;
                            }
                        }
                        else
                        {
                            if (isTileEmpty(tiles[(j + 1) * helperRoot.Width + i - 1]) == false)
                            {
                                tile.North_West = tiles[(j + 1) * helperRoot.Width + i - 1].GetComponent<Tile>();
                            }
                            else
                            {
                                tile.North_West = null;
                            }
                        }
                    }
                }
            }
        }
    }

    void CleanUp()
    {
        EditorUtility.DisplayProgressBar("Clean Up", "Starting Cleaning process...", 0.0f);
        for (int i = 0; i < tiles.Length; i++)
        {
            Undo.RecordObject(tiles[i].transform, "CleanUp transform tile " + i);
            EditorUtility.DisplayProgressBar("Clean Up", "Cleaning up...", (i+1)/(tiles.Length));
            if (isTileEmpty(tiles[i]))
            {
                Undo.DestroyObjectImmediate(tiles[i]);
            }
        }

        EditorUtility.DisplayProgressBar("Clean Up", "Done", 1.0f);
        HelperObject = null;
        helperRoot = null;
        tiles = null;
        EditorUtility.ClearProgressBar();
        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }

    bool isTileEmpty(GameObject tile)
    {
        if(tile.transform.childCount == 1 && tile.transform.GetChild(0).GetChild(0).name == "Model")
        {
            return true;
        }
        if(tile.transform.childCount == 0)
        {
            return true;
        }
        return false;
    }

    public static T[] GetAtPath<T>(string path)
    {

        ArrayList al = new ArrayList();
        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
        foreach (string fileName in fileEntries)
        {
            string name = fileName;
            name = fileName.Replace('\\', '/');
            int index = name.LastIndexOf("/");
            string localPath = "Assets/" + path;
            if (index > 0)
                localPath += name.Substring(index);
            
            Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));
            if (t != null)
                al.Add(t);
        }
        T[] result = new T[al.Count];
        for (int i = 0; i < al.Count; i++)
            result[i] = (T)al[i];

        return result;
    }
}

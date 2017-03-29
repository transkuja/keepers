using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabUIUtils : MonoBehaviour {

    // KeeperPanel
    [Header("Character")]
    //public GameObject goSelectedKeeperPanel;
    // Inventory
    public GameObject PrefabInventaireUI;
    public GameObject PrefabPrisonerFeedingUI;
    public GameObject PrefabSlotUI;
    public GameObject PrefabItemUI;

    public GameObject prefabItemToDrop;

    // ActionPanel
    [Header("Action UI")]
    public GameObject WorldSpaceUIprefab;
    public GameObject PrefabActionUI;
    public GameObject PrefabActionPoint;

    // ShortcutPanel
    [Header("Shortcut UI")]
    public GameObject PrefabSelectedCharacterUI;

    // Utils 
    public GameObject PrefabImageUI;

    [Header("Quest")]
    public GameObject PrefabContentQuestUI;


    [Header("Panneau")]
    public GameObject PrefabContentPanneauUI;


}

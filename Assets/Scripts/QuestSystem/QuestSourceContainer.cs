using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using System;

public class QuestSourceContainer : MonoBehaviour {

    public QuestSource[] Sources;

    public QuestSource FindSourceByID(string id)
    {
        return Array.Find<QuestSource>(Sources, x => x.ID == id);
    }
}

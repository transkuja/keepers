using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using System;

public class MultipleEscortObjective : IQuestObjective {
    InitEvent onInit;
    private string title;
    private string description;
    private bool isComplete;

    // ---Not in use yet----
    //  // We must give an ID to every Quest Objective class (static because it belongs to the class)
    //  // So we can know what to call when loading the quest from JSON
    private static int id = 0;
    // ---------------------

    public string escortablePawnID;
    public int amountToBring;
    public int amountBrought = 0;
    public Tile destination;

    public MultipleEscortObjective(string _title, string desc, string _escortablePawnID, int _amount, Tile _destination, bool complete = false)
    {
        title = _title;
        description = desc;
        escortablePawnID = _escortablePawnID;
        amountToBring = _amount;
        destination = _destination;
        isComplete = complete;
    }

    public string Title
    {
        get
        {
            return title;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }

    public bool IsComplete
    {
        get
        {
            return isComplete;
        }
    }

    public int ID
    {
        get
        {
            return id;
        }
    }

    InitEvent IQuestObjective.OnInit
    {
        get
        {
            return onInit;
        }

        set
        {
            onInit = value;
        }
    }

    public void CheckProgress()
    {
        UpdateProgress();
        if (amountBrought >= amountToBring)
        {
            isComplete = true;
        }
        else
        {
            isComplete = false;
        }
    }

    public void UpdateProgress()
    {
        Debug.Log(destination);
        List<PawnInstance> pi = TileManager.Instance.EscortablesOnTile[destination].FindAll(x => x.Data.PawnId == escortablePawnID);
        if(pi != null)
            amountBrought = pi.Count;
    }

    public void Init()
    {
        if (onInit != null)
        {
            onInit();
        }
    }

    public void Unregister()
    {
    }

    public IQuestObjective GetCopy()
    {
        return new MultipleEscortObjective(title, description, escortablePawnID, amountToBring, destination);
    }
}

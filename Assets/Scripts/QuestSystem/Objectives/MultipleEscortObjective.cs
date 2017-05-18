using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using System;

public class MultipleEscortObjective : IQuestObjective {
    InitEvent onInit;
    CompleteEvent onComplete;
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

    public CompleteEvent OnComplete
    {
        get
        {
            return onComplete;
        }

        set
        {
            onComplete = value;
        }
    }

    public void CheckProgress()
    {
        UpdateProgress();
        if (amountBrought >= amountToBring)
        {
            EventManager.OnPawnMove -= UpdateProgress;
            isComplete = true;
        }
        else
        {
            isComplete = false;
        }
    }

    public void CheckProgressWithEvent()
    {
        UpdateProgress();
        if (amountBrought >= amountToBring)
        {
            EventManager.OnPawnMove -= UpdateProgress;
            isComplete = true;
            if (onComplete != null)
                onComplete();
        }
        else
        {
            isComplete = false;
        }
    }

    public void UpdateProgress()
    {
        //List<PawnInstance> pi = TileManager.Instance.EscortablesOnTile[destination].FindAll(x => x.Data.PawnId == escortablePawnID);
        List<PawnInstance> escortOnTile = new List<PawnInstance>();
        if(TileManager.Instance.EscortablesOnTile.ContainsKey(destination))
        {
            foreach (PawnInstance p in TileManager.Instance.EscortablesOnTile[destination])
            {
                if (p.Data.PawnId == escortablePawnID)
                {
                    escortOnTile.Add(p);
                }
            }
        }
        amountBrought = escortOnTile.Count;
    }

    public void UpdateProgress(PawnInstance pi, Tile tile)
    {
        if(pi.Data.PawnId == escortablePawnID && tile == destination)
        {
            CheckProgressWithEvent();
        }
    }

    public void Init()
    {
        if (onInit != null)
        {
            onInit();
        }
        EventManager.OnPawnMove += UpdateProgress;
    }

    public void Unregister()
    {
    }

    public IQuestObjective GetCopy()
    {
        return new MultipleEscortObjective(title, description, escortablePawnID, amountToBring, destination);
    }
}

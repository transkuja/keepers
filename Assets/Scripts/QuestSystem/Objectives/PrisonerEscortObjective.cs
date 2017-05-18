using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using System;

public class PrisonerEscortObjective : IQuestObjective
{
    InitEvent onInit;
    CompleteEvent onComplete;
    private string title;
    private string description;
    private bool isComplete;

    // We must give an ID to every Quest Objective class (static because it belongs to the class)
    // So we can know what to call when loading the quest from JSON
    private static int id = 0;

    public GameObject prisoner;
    public Tile destination;

    public PrisonerEscortObjective(string _title, string desc, GameObject _prisoner, Tile dest, bool complete = false)
    {
        title = _title;
        description = desc;
        prisoner = _prisoner;
        isComplete = complete;
        destination = dest;
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
        if (prisoner.GetComponent<PawnInstance>().CurrentTile == destination)
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
        if (prisoner.GetComponent<PawnInstance>().CurrentTile == destination)
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
        
    }

    public void UpdateProgress(PawnInstance pi, Tile t)
    {
        if(pi.gameObject == prisoner && t == destination)
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

    public IQuestObjective GetCopy()
    {
        return new PrisonerEscortObjective(title, description, prisoner, destination, isComplete);
    }
}

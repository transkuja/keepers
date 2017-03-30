using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using System;

public class PrisonerEscortObjective : IQuestObjective
{
    private string title;
    private string description;
    private bool isComplete;

    private GameObject prisoner;
    private Tile destination;

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

    public void CheckProgress()
    {
        if(prisoner.GetComponent<PawnInstance>().CurrentTile == destination)
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
        
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using System;

public class PrisonerEscortObjective : IQuestObjective
{
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

    public void Init()
    {
        
    }
}
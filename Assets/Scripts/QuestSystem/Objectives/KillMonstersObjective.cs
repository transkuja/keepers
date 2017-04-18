using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using Behaviour;
using System;

public class KillMonstersObjective : IQuestObjective
{
    InitEvent onInit;
    private string title;
    private string description;
    private bool isComplete;

    // We must give an ID to every Quest Objective class (static because it belongs to the class)
    // So we can know what to call when loading the quest from JSON
    private static int id = 0;

    public string monsterTypeID;
    public int amountToKill;
    public int amountKilled = 0;

    public KillMonstersObjective(string _title, string desc, string _monsterTypeID, int _amount, bool complete = false)
    {
        title = _title;
        description = desc;
        monsterTypeID = _monsterTypeID;
        amountToKill = _amount;
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
        if(amountKilled >= amountToKill)
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

    public void UpdateProgress(Monster m)
    {
        if(m.MonsterTypeID == monsterTypeID)
        {
            amountKilled++;
            if (amountKilled >= amountToKill)
                CheckProgress();
        }
    }

    public void Init()
    {
        EventManager.OnMonsterDie += UpdateProgress;
        if(onInit != null)
        {
            onInit();
        }
    }

    public void Unregister()
    {
        EventManager.OnMonsterDie -= UpdateProgress;
    }
}

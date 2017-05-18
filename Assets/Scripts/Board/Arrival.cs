﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrival : MonoBehaviour {

    void Start () {
        InterationImplementer.Add(new Interaction(CantEndGame), 0, "Can't End Game", GameManager.Instance.SpriteUtils.spriteEndAction);
        EventManager.OnPawnMove += UpdateAvailability;
	}

    void UpdateAvailability(PawnInstance pi, Tile t)
    {
        PrisonerEscortObjective objective = (PrisonerEscortObjective)(GameManager.Instance.QuestManager.MainQuest.Objectives[0]);
        if (pi == objective.prisoner && t == objective.destination)
        {
            InterationImplementer.Remove("Can't End Game");
            InterationImplementer.Add(new Interaction(ClickEnd), 0, "End Game", GameManager.Instance.SpriteUtils.spriteEndAction);
        }
        else
        {
            if(InterationImplementer.listActionContainers.Find(x => x.strName == "End Game") != null)
            {
                InterationImplementer.Remove("End Game");
                InterationImplementer.Add(new Interaction(CantEndGame), 0, "Can't End Game", GameManager.Instance.SpriteUtils.spriteEndAction);
            }
        }
    }

    public void CantEndGame(int i = -1)
    {
        GameManager.Instance.CameraManagerReference.UpdateCameraPosition(((PrisonerEscortObjective)(GameManager.Instance.QuestManager.MainQuest.Objectives[0])).prisoner.GetComponent<PawnInstance>());
    }

    public void ClickEnd(int i = -1)
    {

        bool completed = GameManager.Instance.QuestManager.MainQuest.CheckIfComplete();
        if(completed)
        {
            EventManager.OnPawnMove -= UpdateAvailability;
            GameManager.Instance.Ui.GoActionPanelQ.transform.parent.SetParent(GameManager.Instance.Ui.transform);
            GameManager.Instance.QuestManager.MainQuest.Complete();
        }
        else
        {
            //TODO: Add feedback here to say that the objective is not complete
        }
    }

    public InteractionImplementer InterationImplementer
    {
        get
        {
            return GetComponent<Interactable>().Interactions;
        }
    }
}

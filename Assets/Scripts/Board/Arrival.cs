using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrival : MonoBehaviour {

    void Start () {
        InterationImplementer.Add(new Interaction(CantEndGame), 0, "Can't End Game", Translater.EndActionSpriteSwap());
        EventManager.OnPawnMove += UpdateAvailability;
	}

    void UpdateAvailability(PawnInstance pi, Tile t)
    {
        PrisonerEscortObjective objective = (PrisonerEscortObjective)(GameManager.Instance.QuestManager.MainQuest.Objectives[0]);
        if (pi == objective.prisoner.GetComponent<PawnInstance>() && t == objective.destination)
        {
            InterationImplementer.Remove("Can't End Game");
            InterationImplementer.Add(new Interaction(ClickEnd), 0, "End Game", Translater.EndActionSpriteSwap());
        }
        else
        {
            if(InterationImplementer.listActionContainers.Find(x => x.strName == "End Game") != null)
            {
                InterationImplementer.Remove("End Game");
                InterationImplementer.Add(new Interaction(CantEndGame), 0, "Can't End Game", Translater.EndActionSpriteSwap());
            }
        }
    }

    public void CantEndGame(int i = -1)
    {
        PawnInstance pi = ((PrisonerEscortObjective)(GameManager.Instance.QuestManager.MainQuest.Objectives[0])).prisoner.GetComponent<PawnInstance>();
        GameManager.Instance.CameraManagerReference.UpdateCameraPosition(pi);
        pi.GetComponent<GlowObjectCmd>().BlinkForSeconds(4.0f);
    }

    public void ClickEnd(int i = -1)
    {

        bool completed = GameManager.Instance.QuestManager.MainQuest.CheckIfComplete();
        if(completed)
        {

            EventManager.OnPawnMove -= UpdateAvailability;
            GameManager.Instance.Ui.GoActionPanelQ.transform.parent.SetParent(GameManager.Instance.Ui.transform);
            GameManager.Instance.QuestManager.MainQuest.Complete();
            QuestReminder.BNeedRefresh = true;
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

    public void OnDestroy()
    {
        EventManager.OnPawnMove = null;
    }
}

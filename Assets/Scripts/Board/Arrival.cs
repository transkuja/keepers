using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrival : MonoBehaviour {

    void Start () {
        InterationImplementer.Add(new Interaction(ClickEnd), 0, "End Game", GameManager.Instance.SpriteUtils.spriteEndAction);  
	}

    public void ClickEnd(int i = -1)
    {

        bool completed = GameManager.Instance.QuestManager.MainQuest.CheckIfComplete();
        if(completed)
        {
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

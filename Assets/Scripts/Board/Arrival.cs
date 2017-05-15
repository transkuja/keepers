using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrival : MonoBehaviour {

    void Start () {
        InterationImplementer.Add(new Interaction(ClickEnd), 0, "End Game", GameManager.Instance.SpriteUtils.spriteEndAction);  
	}

    public void EndGame(int i = -1)
    {
        GameManager.Instance.CheckGameState();

        if (TileManager.Instance.PrisonerTile == TileManager.Instance.EndTile)
        {
            Debug.Log("You win");
        }
        else
        {
            Debug.Log("Nope");
        }
    }

    public void ClickEnd(int i = -1)
    {
        

        GameManager.Instance.Ui.GoActionPanelQ.transform.parent.SetParent(GameManager.Instance.Ui.transform);
        GameManager.Instance.QuestManager.MainQuest.CheckAndComplete();
    }

    public InteractionImplementer InterationImplementer
    {
        get
        {
            return GetComponent<Interactable>().Interactions;
        }
    }
}

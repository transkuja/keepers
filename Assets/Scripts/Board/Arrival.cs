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
        // Temporary, these will be set by the QuestInitializer I guess
        ((PrisonerEscortObjective)GameManager.Instance.MainQuest.Objectives[0]).prisoner = GameManager.Instance.PrisonerInstance.gameObject;
        ((PrisonerEscortObjective)GameManager.Instance.MainQuest.Objectives[0]).destination = TileManager.Instance.EndTile;


        GameManager.Instance.MainQuest.CheckAndComplete();
    }

    public InteractionImplementer InterationImplementer
    {
        get
        {
            return GetComponent<Interactable>().Interactions;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrival : MonoBehaviour {

    InteractionImplementer interationImplementer;

    [SerializeField]
    private Tile tileArrival;

    // Use this for initialization
    void Start () {
        interationImplementer = new InteractionImplementer();
        GameManager.Instance.MainQuest.CheckAndComplete();
        GameManager.Instance.MainQuest.OnQuestComplete += EndGameQuest;
        interationImplementer.Add(new Interaction(ClickEnd), 0, "End Game", GameManager.Instance.SpriteUtils.spriteEndAction);  
	}



    public void EndGame(int i = -1)
    {
        GameManager.Instance.CheckGameState();

        if (TileManager.Instance.PrisonerTile == tileArrival)
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
        ((PrisonerEscortObjective)GameManager.Instance.MainQuest.Objectives[0]).destination = tileArrival;


        GameManager.Instance.MainQuest.CheckAndComplete();
    }

    void EndGameQuest()
    {
        GameManager.Instance.MainQuest.OnQuestComplete -= EndGameQuest;
        GameManager.Instance.Win();
    }

    public InteractionImplementer InterationImplementer
    {
        get
        {
            return interationImplementer;
        }
    }
}

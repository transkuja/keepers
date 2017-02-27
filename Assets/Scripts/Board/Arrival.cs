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
        interationImplementer.Add(new Interaction(EndGame), "End Game", null);  
	}

    public void EndGame(int i = -1)
    {
        if(TileManager.Instance.PrisonerTile == tileArrival)
        {
            Debug.Log("You win");
        }
        else
        {
            Debug.Log("Nope");
        }

    }

    public InteractionImplementer InterationImplementer
    {
        get
        {
            return interationImplementer;
        }
    }
}

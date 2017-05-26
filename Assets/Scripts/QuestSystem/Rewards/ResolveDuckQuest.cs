using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class ResolveDuckQuest : MonoBehaviour {

    bool initialized = false;
	// Use this for initialization
	void Start () {
        initialized = false;
        
	}
	
	// Update is called once per frame
	void Update () {
		if(!initialized)
        {
            Quest toGive = GetComponent<Behaviour.QuestDealer>().QuestToGive;
            if(toGive != null)
            {
                toGive.OnQuestComplete += CompleteQuest;
                initialized = true;
            }
        }
	}

    void CompleteQuest()
    {
        GetComponent<Behaviour.QuestDealer>().QuestToGive.OnQuestComplete -= CompleteQuest;
        List<PawnInstance> pi = TileManager.Instance.EscortablesOnTile[GetComponentInParent<Tile>()].FindAll(x => x.Data.PawnId == "duckling");
        foreach(PawnInstance p in pi)
        {
            if (p.GetComponent<Behaviour.Escortable>().escort != null)
            {
                p.GetComponent<Behaviour.Escortable>().UnEscort();
            }          
            StartCoroutine(p.GetComponent<Behaviour.AnimatedPawn>().DanceOfDeath());
        }
        StartCoroutine(GetComponent<Behaviour.AnimatedPawn>().DanceOfDeath());
    }
}

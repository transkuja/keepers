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
            Quest toGive = GetComponent<Behaviour.QuestDealer>().questToGive;
            if(toGive != null)
            {
                toGive.OnQuestComplete += CompleteQuest;
                initialized = true;
            }
        }
	}

    void CompleteQuest()
    {
        GetComponent<Behaviour.QuestDealer>().questToGive.OnQuestComplete -= CompleteQuest;
        List<PawnInstance> pi = TileManager.Instance.EscortablesOnTile[GetComponentInParent<Tile>()].FindAll(x => x.Data.PawnId == "duckling");
        foreach(PawnInstance p in pi)
        {
            if (p.GetComponent<Behaviour.Escortable>().escort != null)
            {
                p.GetComponent<Behaviour.Escortable>().UnEscort();
            }
            
            if (p.GetComponent<Behaviour.Mortal>().DeathParticles != null)
            {
                ParticleSystem ps = Instantiate(p.GetComponent<Behaviour.Mortal>().DeathParticles, p.transform.parent);
                ps.transform.position = p.transform.position;
                ps.Play();
                Destroy(ps.gameObject, ps.main.duration);
            }
            p.gameObject.SetActive(false);
        }

        if (GetComponent<Behaviour.Mortal>().DeathParticles != null)
        {
            ParticleSystem ps = Instantiate(GetComponent<Behaviour.Mortal>().DeathParticles, transform.parent);
            ps.transform.position = transform.position;
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration);
            gameObject.SetActive(false);
        }

    }
}

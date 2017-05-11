using UnityEngine;
using System.Collections;
using Behaviour;

public class TriggerOnCristals : MonoBehaviour {
    private bool triggerActif;

    public TriggerOnCristals monCopain;

    private Material mat;
    private Color storedColor;

    public bool TriggerActif
    {
        get
        {
            return triggerActif;
        }

        set
        {
            triggerActif = value;
            if(triggerActif == true)
            {
                if (monCopain.TriggerActif)
                {
                    GetComponent<PathBlocker>().enabled = false;
                    monCopain.GetComponent<PathBlocker>().enabled = false;
                }
            } 
   
        }
    }

    public void Start()
    {
        mat = this.GetComponentInChildren<MeshRenderer>().material;
		triggerActif = false;

		storedColor = mat.GetColor ("_EmissionColor");
    }
		
	// Quand le papillon ou le double se trouve a l'interieur
	public void OnTriggerEnter(Collider col){
        if (col.GetComponentInParent<Keeper>() != null && col.isTrigger)
        {
            if(!TriggerActif)
            {
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.crystalOnSound, 0.5f);
                TriggerActif = true;
            }
		}
	}

    public void OnTriggerExit(Collider col)
    {
        if (col.GetComponentInParent<Keeper>() != null && col.isTrigger)
        {
            TriggerActif = false;
        }
    }

    public void Update()
    {
        // Hell yeah
        if (GameManager.Instance.CurrentState == GameState.InBattle || GameManager.Instance.CurrentState == GameState.InTuto)
            GetComponentInChildren<MeshRenderer>().enabled = false;
        else
            GetComponentInChildren<MeshRenderer>().enabled = true;

        if (!triggerActif) {
			mat.SetColor ("_EmissionColor", storedColor);
		} else {
			mat.SetColor ("_EmissionColor", Color.blue);
            //GetComponentInChildren<ParticleSystem>().main.startColor.color = Color.blue;
		}

        transform.Rotate(Vector3.up, 0.5f);
    }

}

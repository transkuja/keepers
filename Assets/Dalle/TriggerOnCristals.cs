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
            TriggerActif = true;
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
		if (!triggerActif) {
			mat.SetColor ("_EmissionColor", storedColor);
		} else {
			mat.SetColor ("_EmissionColor", Color.blue);
		}

        transform.Rotate(Vector3.up, 0.5f);
    }

}

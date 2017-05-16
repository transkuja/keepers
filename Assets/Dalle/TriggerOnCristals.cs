using UnityEngine;
using System.Collections.Generic;
using Behaviour;

public class TriggerOnCristals : MonoBehaviour {
    [SerializeField]
    private bool triggerActif;

    private List<Keeper> currentlyInTrigger;

    private bool bCompleted = false;

    public TriggerOnCristals monCopain;

    public Color unactiveColor;
    public Color activeColor;
    public ParticleSystem particles;

    private List<Material> mat;
    private Color storedColor;

    [SerializeField] GameObject[] tabPathBlockerVisuals;

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

                    tabPathBlockerVisuals[0].GetComponent<Animator>().SetBool("bHalfOpened", true);
                    tabPathBlockerVisuals[0].GetComponent<Animator>().SetBool("bFullyOpened", true);
                    if (tabPathBlockerVisuals[1].GetComponent<Animator>().isInitialized == true)
                    {
                        tabPathBlockerVisuals[1].GetComponent<Animator>().SetBool("bHalfOpened", true);
                        tabPathBlockerVisuals[1].GetComponent<Animator>().SetBool("bFullyOpened", true);
                    }
                    bCompleted = true;
                    monCopain.bCompleted = true;

                    foreach (Material m in mat)
                    {
                        m.SetColor("_EmissionColor", activeColor);
                    }
                    if (!particles.emission.enabled)
                    {
                        ParticleSystem.EmissionModule em = particles.emission;
                        em.enabled = true;
                    }

                    foreach (Material m in monCopain.mat)
                    {
                        m.SetColor("_EmissionColor", activeColor);
                    }
                    if (!monCopain.particles.emission.enabled)
                    {
                        ParticleSystem.EmissionModule em = monCopain.particles.emission;
                        em.enabled = true;
                    }
                }
                else
                {
                    tabPathBlockerVisuals[0].GetComponent<Animator>().SetBool("bHalfOpened", true);
                    if (tabPathBlockerVisuals[1].GetComponent<Animator>().isInitialized == true)
                    {
                        tabPathBlockerVisuals[1].GetComponent<Animator>().SetBool("bHalfOpened", true);
                    }
                }
            }
            else
            {
                if (!monCopain.triggerActif)
                {
                    tabPathBlockerVisuals[0].GetComponent<Animator>().SetBool("bHalfOpened", false);
                    if (tabPathBlockerVisuals[1].GetComponent<Animator>().isInitialized == true)
                    {
                        tabPathBlockerVisuals[1].GetComponent<Animator>().SetBool("bHalfOpened", false);
                    }
                }
            }
            // TODO Add sounds for feedback
        }
    }

    public void Start()
    {
        currentlyInTrigger = new List<Keeper>();
        mat = new List<Material>();
        foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            mat.Add(mr.material);
		triggerActif = false;
        particles = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.EmissionModule em = particles.emission;
        em.enabled = false;
    }
		
	public void OnTriggerEnter(Collider col){
        if (col.GetComponentInParent<Keeper>() != null)
        {
            currentlyInTrigger.Add(col.GetComponentInParent<Keeper>());
            if (!TriggerActif)
            {
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.crystalOnSound, 0.5f);
                TriggerActif = true;
            }
		}
	}

    public void OnTriggerExit(Collider col)
    {
        if (col.GetComponentInParent<Keeper>() != null)
        {
            currentlyInTrigger.Remove(col.GetComponentInParent<Keeper>());
            if(currentlyInTrigger.Count <= 0)
                TriggerActif = false;
        }
    }

    public void Update()
    {
        // Hell yeah
        if (GameManager.Instance.CurrentState == GameState.InBattle || GameManager.Instance.CurrentState == GameState.InTuto)
            transform.GetChild(0).gameObject.SetActive(false);
        else
            transform.GetChild(0).gameObject.SetActive(true);

        if (!bCompleted)
        {
            if (!triggerActif) {
                foreach(Material m in mat)
                {
                    m.SetColor("_EmissionColor", unactiveColor);
                }
                if (particles.emission.enabled)
                {
                    ParticleSystem.EmissionModule em = particles.emission;
                    em.enabled = false;
                }
            } else {
                foreach (Material m in mat)
                {
                    m.SetColor("_EmissionColor", activeColor);
                }
                if (!particles.emission.enabled)
                {
                    ParticleSystem.EmissionModule em = particles.emission;
                    em.enabled = true;
                }
            }
        }

        transform.Rotate(Vector3.up, 0.5f);
    }
}

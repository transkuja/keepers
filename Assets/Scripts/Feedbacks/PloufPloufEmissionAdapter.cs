using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PloufPloufEmissionAdapter : MonoBehaviour {
    ParticleSystem ps;
    NavMeshAgent agent;
    float emissionMin = 0.0f;
    float emissionMax = 10.0f;
    public bool play = false;
    ParticleSystem.EmissionModule module;
	void Start () {
        ps = GetComponent<ParticleSystem>();
        agent = GetComponentInParent<NavMeshAgent>();
        module = ps.emission;
	}
	

	void Update () {
        if (play)
        {
            if (!ps.isPlaying)
                ps.Play();
            module.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Clamp(agent.velocity.magnitude * 4, emissionMin, emissionMax));
        }
        else
        {
            if (!ps.isStopped)
                ps.Stop();
        }
        
	}
}

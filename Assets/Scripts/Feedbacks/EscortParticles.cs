using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Particle = UnityEngine.ParticleSystem.Particle;

[ExecuteInEditMode]
public class EscortParticles : MonoBehaviour {
    ParticleSystem ps;
    bool emitting;
    public Transform target;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        emitting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(emitting)
        {
            Particle[] parts = new Particle[ps.main.maxParticles];
            ps.GetParticles(parts);
            if (parts.GetUpperBound(0) > 0)
            {
                for (int i = 0; i < parts.GetUpperBound(0); i++)
                {
                    parts[i].position = Vector3.Lerp(parts[i].position, target.position + Vector3.up/4.0f, Time.deltaTime * 4.0f);
                }
            }
            else
            {
                emitting = false;
            }
            ps.SetParticles(parts, parts.GetLength(0));
        }
	}

    public void EmitTowards(Transform _target)
    {
        target = _target;
        emitting = true;
        ps.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleChain : MonoBehaviour {
    [SerializeField]
    ParticleSystem toChain;
    ParticleSystem ps;

    bool hasBeenPlayed = false;
    bool hasBeenChained = false;

	void Start () {
        hasBeenPlayed = false;
        hasBeenChained = false;
        ps = GetComponent<ParticleSystem>();
        toChain.Stop();
	}

	void Update () {
		if(ps.isPlaying && hasBeenPlayed == false)
        {
            hasBeenPlayed = true;
        }

        if(hasBeenPlayed)
        {
            if(ps.isStopped && !hasBeenChained)
            {
                toChain.Play();
                AudioSource audioSource = toChain.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Play();
                }
                hasBeenChained = true;
            }
        }
	}
}

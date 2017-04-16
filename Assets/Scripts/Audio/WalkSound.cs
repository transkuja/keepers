using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkSound : MonoBehaviour {

    AudioSource source;

	void Start () {
        source = GetComponent<AudioSource>();
        source.clip = AudioManager.Instance.walkSound;
	}

    public void PlaySound()
    {
        if(source != null)
        {
            source.PlayOneShot(source.clip);
            source.volume = GetComponentInParent<NavMeshAgent>().velocity.magnitude;
            source.volume = Mathf.Min(source.volume, AudioManager.Instance.VolumeFXs);
        }
    }
}

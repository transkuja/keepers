using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkSound : MonoBehaviour {

    AudioSource source;
    [SerializeField]
    AnimationCurve blendCurve;

	void Start () {
        source = GetComponent<AudioSource>();
        source.clip = AudioManager.Instance.walkSound;
	}

    public void PlaySound()
    {
        if(source != null)
        {
            source.maxDistance = 20;
            source.spatialBlend = 1.0f;
            if(blendCurve.keys.Length > 0)
                source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, blendCurve);
            source.pitch = Random.Range(0.5f, 1.5f);
            source.PlayOneShot(source.clip);
            source.volume = GetComponentInParent<NavMeshAgent>().velocity.magnitude;
            source.volume = Mathf.Min(source.volume, AudioManager.Instance.VolumeFXs);
        }
    }

}

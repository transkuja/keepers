using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFXOnce : MonoBehaviour {
    AudioSource source;
    [SerializeField]
    float volume = 1.0f;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        StartCoroutine(playSoundWithDelay(source.clip, 0.1f));
    }
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator playSoundWithDelay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.PlayOneShot(clip, AudioManager.Instance.VolumeFXs * volume);
    }
}

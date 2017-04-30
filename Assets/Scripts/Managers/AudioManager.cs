using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    private static AudioManager s_instance = null;


    public AudioSource sourceMusic;

    public AudioSource sourceFX;

    public AudioClip menuMusic;

    public AudioClip Scene1Clip;

    public AudioClip Scene2Clip;

    public AudioClip walkSound;

    public AudioClip selectSound;

    public AudioClip deselectSound;

    public AudioClip paperSelectSound;

    public AudioClip buttonClick;
    // Temp
    [SerializeField]
    public AudioClip battleSound;

    public AudioClip winningSound;

    private AudioClip musicToPlay;

    [SerializeField]
    private float volumeMusic = 0.5f;
    [SerializeField]
    private float volumeFXs = 1.0f;

    public static AudioManager Instance
    {
        get
        {
            return s_instance;
        }
    }

    public float VolumeMusic
    {
        get
        {
            return volumeMusic;
        }

        set
        {
            volumeMusic = value;
            sourceMusic.volume = volumeMusic;
        }
    }

    public float VolumeFXs
    {
        get
        {
            return volumeFXs;
        }

        set
        {
            volumeFXs = value;
            sourceFX.volume = volumeFXs;
        }
    }

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
            transform.GetChild(0).GetComponent<AudioSource>().clip = menuMusic;
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start ()
    {
        sourceMusic.Play();
    }

    private bool isFading = false;
    private float timerFade = 0.0f;

    public void Fade(AudioClip _music)
    {
        if (sourceMusic.clip != _music)
        {
            musicToPlay = _music;
            timerFade = 1.0f;
            isFading = true;
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(isFading)
        {
            timerFade -= Time.deltaTime;
            if(timerFade < 0)
            {
                timerFade = 0.0f;
                sourceMusic.volume = volumeMusic;
                sourceMusic.clip = musicToPlay;
                sourceMusic.Play();
                isFading = false;
            }
            else
            {
                sourceMusic.volume = timerFade * volumeMusic;
            }
        }
        else
        {
            sourceMusic.volume = volumeMusic;
        }

        
	}

    public void PlayOneShot(AudioClip clip)
    {
        if (clip == winningSound)
            sourceFX.PlayOneShot(clip, 0.02f);
        else
            sourceFX.PlayOneShot(clip, volumeFXs);
    }

    public void PlayOneShot(AudioClip clip, float volumeMultiplier)
    {
        if (clip == winningSound)
            sourceFX.PlayOneShot(clip, 0.02f * volumeMultiplier);
        else
            sourceFX.PlayOneShot(clip, volumeFXs * volumeMultiplier);
    }
}

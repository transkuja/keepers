using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    private static AudioManager s_instance = null;

    [SerializeField]
    public AudioSource sourceMusic;
    [SerializeField]
    public AudioSource sourceFX;
    [SerializeField]
    public AudioClip themeMusic;
    [SerializeField]
    public AudioClip ingameMusic;

    // Temp
    [SerializeField]
    public AudioClip battleSound;

    private AudioClip musicToPlay;

    [SerializeField]
    private float volumeMusic = 1.0f;
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
        }
        else if (s_instance != this)
            Destroy(gameObject);

        
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

    public void playBattleSound()
    {
        sourceFX.PlayOneShot(battleSound, volumeFXs);
    }
}

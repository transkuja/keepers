using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    private static AudioManager s_instance = null;


    public AudioSource sourceMusic;

    public AudioSource sourceFX;

    public AudioClip menuMusic;

    public AudioClip Scene1Clip;

    public AudioClip Scene2Clip;
    public AudioClip Scene3Clip;
    public AudioClip Scene4Clip;

    public AudioClip thowDiceSound;

    public AudioClip walkSound;

    public AudioClip selectSound;

    public AudioClip deselectSound;

    public AudioClip paperSelectSound;

    public AudioClip buttonClick;

    public AudioClip interactionButtonClick;

    public AudioClip battleSound;

    public AudioClip winningSound;
    public AudioClip winningMusic;

    public AudioClip crystalOnSound;
    public AudioClip quackSound;

    public AudioClip commonBattleMusic;

    public AudioClip miniBossMusic;

    public AudioClip epicFightMusic;

    private AudioClip musicToPlay;
    private AudioClip previousMusic;

    public AudioClip luckyCatSound;
    public AudioClip viciousCatSound;

    [SerializeField]
    private float volumeMusic = 0.5f;
    [SerializeField]
    private float volumeFXs = 1.0f;

    // Very important audio feature, do not remove
    float duckTimer = 0.2f;
    int step = 0;
    bool isDuckChatting = false;

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

    public void Fade(AudioClip _music, float timer = 1.0f)
    {
        if (sourceMusic.clip != _music)
        {
            musicToPlay = _music;
            timerFade = timer;
            isFading = true;
        }

    }

    public void PlayBattleMusic(Behaviour.MonsterType monsterType)
    {
        previousMusic = sourceMusic.clip;
        if (monsterType == Behaviour.MonsterType.Epic)
        {
            sourceMusic.clip = epicFightMusic;
        }
        else if (monsterType == Behaviour.MonsterType.Miniboss)
            sourceMusic.clip = miniBossMusic;
        else
            sourceMusic.clip = commonBattleMusic;

        sourceMusic.Play();
    }

    public void StopBattleMusic()
    {
        Fade(previousMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        isFading = false;
        sourceMusic.clip = clip;
        sourceMusic.volume = volumeMusic;
        sourceMusic.Play();
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

        if (isDuckChatting)
        {
            duckTimer -= Time.deltaTime;
            if (duckTimer <= 0.0f)
            {
                PlayOneShot(quackSound);
                if (step == 1) duckTimer = 0.4f;
                if (step == 2) duckTimer = 0.7f;
                if (step == 3) duckTimer = 0.2f;
                if (step == 4) duckTimer = 0.5f;
                if (step == 5) duckTimer = 0.2f;
                if (step == 6) isDuckChatting = false;
                step++;
            }
        }
	}

    public void PlayOneShot(AudioClip clip)
    {
        sourceFX.PlayOneShot(clip, volumeFXs);
    }

    public void PlayOneShot(AudioClip clip, float volumeMultiplier)
    {
        sourceFX.PlayOneShot(clip, volumeFXs * volumeMultiplier);
    }

    public void PlayButtonClickSound()
    {
        PlayOneShot(interactionButtonClick);
    }

    public void PlayButtonClickSound(int i = 0)
    {
        PlayOneShot(interactionButtonClick, 0.5f);
    }

    public void PlayDuckChatting()
    {
        isDuckChatting = true;
        duckTimer = 0.2f;
        step = 0;
        PlayOneShot(quackSound);
    }
}

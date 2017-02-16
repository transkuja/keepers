using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinematique))]
public class CinematiqueManager : MonoBehaviour {

    private static CinematiqueManager s_instance = null;

    private Cinematique Cinematique;
    public bool isPlaying = false;

    void Awake()
    {
        if (s_instance == null)
            s_instance = this;
        else if (s_instance != this)
            Destroy(gameObject);

        Cinematique = GetComponent<Cinematique>();
    }

    public static CinematiqueManager Instance
    {
        get
        {
            return s_instance;
        }
    }

    void Start()
    {
        Cinematique.PlayDelayedCinematique();
    }

}

public abstract class Cinematique : MonoBehaviour {
    public float delay = 0.0f;

    public abstract void PlayDelayedCinematique();
    public abstract void PlayCinematique();
}
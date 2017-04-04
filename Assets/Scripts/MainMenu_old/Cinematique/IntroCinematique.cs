using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCinematique : Cinematique {

    [Header("Camera")]
    public GameObject MainCamera;
    public GameObject CinematiqueCamera;
    public float CameraSpeed = 1;

    [Header("Box")]
    public GameObject TopBox;
    public GameObject BottomBox;
    public float BoxMouvementSpeed = 1;

    public override void PlayDelayedCinematique()
    {
        CinematiqueManager.Instance.isPlaying = true;
        Invoke("PlayCinematique", delay);
    }

    public override void PlayCinematique()
    {
        // Activation de l'animation de la camera
        MainCamera.SetActive(false);
        CinematiqueCamera.SetActive(true);
        CinematiqueCamera.GetComponent<Animator>().speed = CameraSpeed;

        // Activation de l'animation d'ouverture de la boîte
        Animator anim_box = TopBox.GetComponent<Animator>();
        anim_box.speed = BoxMouvementSpeed;
        anim_box.enabled = true;
    }
}

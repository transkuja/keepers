using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ParticlesBattleAnimation : MonoBehaviour, IBattleAnimation {

    PawnInstance[] targets;
    [SerializeField]
    float duration = 1.0f;
    [SerializeField]
    ParticleSystem particles;
    [SerializeField]
    TargetType targetType;

    public float GetAnimationTime()
    {
        return duration;
    }

    public void Play()
    {
        foreach (PawnInstance go in targets)
        {
            ParticleSystem ps = Instantiate<ParticleSystem>(particles, go.transform);
            ps.transform.localPosition = new Vector3(0, ps.transform.position.y, 0);
            ps.transform.parent = go.transform.parent;
            ps.Play();
            AudioSource audioSource = ps.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
            Destroy(ps.gameObject, duration*3);
        }
        
    }

    public void SetTargets(PawnInstance[] gos)
    {
        targets = gos;
    }

    public TargetType GetTargetType()
    {
        return targetType;
    }
}

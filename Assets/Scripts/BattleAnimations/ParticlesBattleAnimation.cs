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
        Debug.Log("Play ");
        foreach (PawnInstance go in targets)
        {
            ParticleSystem ps = Instantiate<ParticleSystem>(particles, go.transform);
            ps.transform.localPosition = Vector3.zero;
            ps.transform.parent = go.transform.parent;
            ps.Play();
            Destroy(ps.gameObject, duration);
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

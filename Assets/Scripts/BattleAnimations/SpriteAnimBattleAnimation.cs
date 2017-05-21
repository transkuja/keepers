using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SpriteAnimBattleAnimation : MonoBehaviour, IBattleAnimation {

    PawnInstance[] targets;
    [SerializeField]
    float duration;
    [SerializeField]
    GameObject spriteWithAnimator;
    [SerializeField]
    TargetType targetType;
    [SerializeField]
    AudioClip toPlay;
    [SerializeField]
    float volumeMultiplier = 1.0f;

    public float GetAnimationTime()
    {
        return duration;
    }

    public void Play()
    {
        if(toPlay != null)
        {
            GameObject go = new GameObject("Audio", typeof(AudioSource), typeof(VolumeAdapter));
            AudioSource source = go.GetComponent<AudioSource>();
            go.GetComponent<VolumeAdapter>().multiplier = volumeMultiplier;
            source.clip = toPlay;
            source.Play();
            Destroy(source.gameObject, source.clip.length + 0.1f);
        }
        
        foreach(PawnInstance go in targets)
        {
            GameObject g = Instantiate(spriteWithAnimator);
            Canvas canvas = go.GetComponentInChildren<Canvas>();
            g.transform.SetParent(canvas.transform);
            g.transform.localPosition = Vector3.zero - canvas.transform.parent.transform.position.y/2.0f * Vector3.up;
            g.transform.localScale = Vector3.one;
            duration = g.GetComponent<SpriteAnimator>().GetAnimationTime();
            g.GetComponent<SpriteAnimator>().Play();
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

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SpriteAnimBattleAnimation : MonoBehaviour, IBattleAnimation {

    PawnInstance[] targets;
    [SerializeField]
    float duration;
    [SerializeField]
    GameObject spriteWithAnimator;
    [SerializeField]
    TargetType targetType;

    public float GetAnimationTime()
    {
        return duration;
    }

    public void Play()
    {
        foreach(PawnInstance go in targets)
        {
            GameObject g = Instantiate<GameObject>(spriteWithAnimator, go.transform);
            Destroy(g, GetAnimationTime());
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

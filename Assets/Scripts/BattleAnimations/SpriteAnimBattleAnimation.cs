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

    public float GetAnimationTime()
    {
        return duration;
    }

    public void Play()
    {
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

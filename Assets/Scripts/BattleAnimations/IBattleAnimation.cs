using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleAnimation {
    void Play();
    float GetAnimationTime();

    TargetType GetTargetType();
    void SetTargets(PawnInstance[] gos);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationButtonClick : MonoBehaviour {

    public IngameUI canvasMabite;
    public Animator test;

    // Update is called once per frame
    public void HandleAnimation() {
        canvasMabite.isTurnEnding = false;
        test.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationButtonClick : MonoBehaviour {

    public IngameUI ingameUI;

    // Update is called once per frame
    public void HandleAnimation() {
        ingameUI.isTurnEnding = false;
        GetComponent<Animator>().enabled = false;
    }
}

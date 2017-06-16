using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnim : MonoBehaviour {
    [SerializeField]
    AnimationCurve opacity;
    [SerializeField]
    float animationDuration = 1.0f;
    Image sprite;
    float timer = 0.0f;
	void Start () {
        timer = 0.0f;
        sprite = GetComponent<Image>();	
	}

	void Update () {
        timer += Time.deltaTime / (animationDuration+0.00001f); //Prevent dividing by zero
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, opacity.Evaluate(timer));
	}
}

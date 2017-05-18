using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JumpingQuestFeedback : MonoBehaviour {
    [SerializeField]
    AnimationCurve curve;
    [SerializeField]
    float strength = 0.5f;
    [SerializeField]
    float speed = 1.0f;
    Vector3 startPos;
    float timer = 0.0f;
	void Start () {
        timer = 0.0f;
        startPos = transform.localPosition;
	}

	void Update () {
        timer += Time.deltaTime * speed;
        if(timer > 1.0f)
        {
            timer -= 1.0f;
        }
        transform.localPosition = startPos + Vector3.up * curve.Evaluate(timer) * strength;
	}
}

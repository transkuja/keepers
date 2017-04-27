using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlecheQuiBouge : MonoBehaviour {
    public Vector3 PointToPoint;
    public float magnitude = 25;
    public float speed = 3;
    private float timer = 0.0f;
    public float distanceOffset = 20.0f;
	// Use this for initialization
	void Start () {
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.unscaledDeltaTime;
        transform.position = transform.up * distanceOffset + PointToPoint + transform.up * ((Mathf.Sin(timer * speed)+1)/2) * magnitude;

        if(timer > Mathf.PI * 2)
        {
            timer -= Mathf.PI * 2;
        }
	}
}

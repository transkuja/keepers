using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlecheQuiBouge : MonoBehaviour {
    public Vector3 PointToPoint;
    public float magnitude = 25;
    public float speed = 3;
    private float timer = 0.0f;
    public float distanceOffset = 20.0f;
    private float referenceWidth = 1920.0f;

	void Start () {
        timer = 0.0f;
        GetComponent<RectTransform>().sizeDelta = new Vector2((Screen.width * 1.2f) / 10.0f, (Screen.width *1.2f) / 10.0f);
        distanceOffset *= (Screen.width / referenceWidth);
    }
	
	void Update () {
        timer += Time.unscaledDeltaTime;
        transform.position = transform.up * distanceOffset + PointToPoint + transform.up * ((Mathf.Sin(timer * speed)+1)/2) * magnitude;

        if(timer > Mathf.PI * 2)
        {
            timer -= Mathf.PI * 2;
        }
	}
}

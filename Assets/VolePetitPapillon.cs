using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolePetitPapillon : MonoBehaviour {

    float i = 0.0f;
    float SineSpeedX;
    float SineSpeedY;
    float SineDistanceX;
    float SineDistanceY;
    float offsetX;
    float offsetY;
    Vector3 startPos;
    // Use this for initialization
    void Start()
    {
        i = 0.0f;
        startPos = transform.position;
        SineSpeedX = Random.Range(50.0f, 80.0f);
        SineSpeedY = Random.Range(50.0f, 80.0f);
        SineDistanceX = Random.Range(0.5f, 1.0f);
        SineDistanceY = Random.Range(0.5f, 1.0f);
        offsetX = Random.Range(-2.0f, 2.0f);
        offsetY = Random.Range(-2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        i += Time.deltaTime;
        Vector3 targetPos;
        targetPos.x = startPos.x + Mathf.Sin(i * Mathf.PI * SineSpeedX / 180) * SineDistanceX + offsetX;
        targetPos.y = startPos.y + Mathf.Sin(i * Mathf.PI * SineSpeedY / 180) * SineDistanceY + offsetY;
        targetPos.z = startPos.z;

        transform.position = targetPos;
    }
}

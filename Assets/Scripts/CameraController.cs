using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    float minFov = 15f;
    float maxFov = 90f;
    float sensitivity = 10f;

    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    // Update is called once per frame
    void Update () {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
        {
            float fov = Camera.main.fieldOfView;
            fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Camera.main.fieldOfView = fov;
       }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    dragOrigin = Input.mousePosition;
        //    return;
        //}

        //if (!Input.GetMouseButton(0)) return;

        //Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        //Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

        //transform.Translate(move, Space.World);
    }
}

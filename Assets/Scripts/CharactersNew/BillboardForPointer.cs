using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardForPointer : MonoBehaviour {

    public void RecalculateActionCanvas()
    {
        if(GameManager.Instance != null && GameManager.Instance.CameraManagerReference != null)
            transform.localScale = new Vector3(1,1,1) + new Vector3(1,1,1) * 2 * (1 - GameManager.Instance.CameraFZoomLerp);
    }

    void Update()
    {
        RecalculateActionCanvas();
    }
}

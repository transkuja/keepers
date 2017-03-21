using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardForPointer : MonoBehaviour {

    public void RecalculateActionCanvas()
    {
        transform.localScale = new Vector3(1,1,1) + new Vector3(1,1,1) * 2 * (1 - GameManager.Instance.CameraManager.FZoomLerp);
    }

    void Update()
    {
        RecalculateActionCanvas();
    }
}

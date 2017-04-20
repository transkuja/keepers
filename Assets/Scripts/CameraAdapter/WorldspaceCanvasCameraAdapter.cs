using UnityEngine;

public class WorldspaceCanvasCameraAdapter : MonoBehaviour {

    Vector3 v3ScaleRef;

    void Start()
    {
        GameManager.Instance.RegisterWorldspaceCanvasCameraAdapter(this);
        v3ScaleRef = transform.localScale;
        this.RecalculateActionCanvas(Camera.main);
    }

    public void RecalculateActionCanvas(Camera camera)
    {
        if (GameManager.Instance != null && camera != null)
        {
            transform.rotation = camera.transform.rotation;
            //transform.GetChild(1).localPosition = Vector3.zero;
            transform.localScale = v3ScaleRef + new Vector3(0.5f, 0.5f, 0.5f) * (1 - GameManager.Instance.CameraFZoomLerp);
        }
    }
}

using UnityEngine;

public class WorldspaceCanvasCameraAdapter : MonoBehaviour {

    void Start()
    {
        GameManager.Instance.RegisterWorldspaceCanvasCameraAdapter(this);
    }

    public void RecalculateActionCanvas(Camera camera)
    {
        if (GameManager.Instance != null && camera != null)
        {
            transform.rotation = camera.transform.rotation;
            //transform.GetChild(1).localPosition = Vector3.zero;
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f) + new Vector3(0.5f, 0.5f, 0.5f) * (1 - GameManager.Instance.CameraFZoomLerp);
        }
    }
}

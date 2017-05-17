using UnityEngine;

public class WorldspaceCanvasCameraAdapter : MonoBehaviour {

    Vector3 v3ScaleRef;
    public bool isForInteractions = false;

    void Start()
    {
        if (!isForInteractions)
        {
            GameManager.Instance.RegisterWorldspaceCanvasCameraAdapter(this);
            v3ScaleRef = transform.localScale;
            this.RecalculateActionCanvas(Camera.main);
        }
    }

    public void Init()
    {
        if (isForInteractions)
        {
            GameManager.Instance.Ui.ClearActionPanel();
            GameManager.Instance.RegisterWorldspaceCanvasCameraAdapter(this);
            v3ScaleRef = new Vector3(0.2f,0.2f,0.2f);
            this.RecalculateActionCanvas(Camera.main);
        }
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

    private void OnDestroy()
    {
        if (GameManager.Instance.CameraManagerReference != null)
            GameManager.Instance.UnregisterWorldspaceCanvasCameraAdapter(this);

    }
}

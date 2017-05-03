
using UnityEngine;

public class SelectionPointerCameraAdapter : MonoBehaviour {

    void Start()
    {
        GameManager.Instance.RegisterSelectionPointerCameraAdapter(this);
    }

    public void RecalculateOrientationAndScale()
    {
        if(GameManager.Instance != null && GameManager.Instance.CameraManagerReference != null)
            transform.localScale = new Vector3(1,1,1) + new Vector3(1,1,1) * 2 * (1 - GameManager.Instance.CameraFZoomLerp);
    }

    void OnDestroy()
    {
        GameManager.Instance.UnregisterSelectionPointerCameraAdapter(this);
    }

}

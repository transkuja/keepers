using UnityEngine;

public class GreyTileCameraAdapter : MonoBehaviour {

    void Start()
    {
        GameManager.Instance.RegisterGreyTileCameraAdapter(this);
    }

    public void RecalculateOrientation(Camera camera)
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
            camera.transform.rotation * Vector3.up);
    }

}

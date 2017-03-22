using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardForWorldSpaceUI : MonoBehaviour {

    public Camera m_Camera;

    void Start()
    {
        m_Camera = Camera.main;
    }

    public void RecalculateActionCanvas()
    {
        if (transform.GetChild(0).childCount > 0)
        {
            transform.rotation = m_Camera.transform.rotation;


            if (GameManager.Instance.GoTarget.GetComponent<BoxCollider>() != null)
            {
                Vector3 size = GameManager.Instance.GoTarget.GetComponent<BoxCollider>().bounds.size;
                Vector3 V1 = new Vector3(0, Mathf.Max(size.y, size.z), 0);
                transform.localPosition = V1 + (Vector3.up * (1 - GameManager.Instance.CameraManager.FZoomLerp));

            }
            else if (GameManager.Instance.GoTarget.GetComponentInChildren<MeshCollider>() != null)
            {
                Vector3 size = GameManager.Instance.GoTarget.GetComponentInChildren<MeshCollider>().bounds.size;
                Vector3 V1 = new Vector3(0, Mathf.Max(size.y, size.z), 0);
                transform.localPosition = V1 + (Vector3.up * (1 - GameManager.Instance.CameraManager.FZoomLerp));
            }
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f)  + new Vector3(0.5f,0.5f,0.5f)  * (1 - GameManager.Instance.CameraManager.FZoomLerp);
        }
    }

    void Update()
    {
        RecalculateActionCanvas();
    }
}

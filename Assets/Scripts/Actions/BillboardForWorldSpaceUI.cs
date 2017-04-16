using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardForWorldSpaceUI : MonoBehaviour {

    public Camera m_Camera;

    void Start()
    {
        m_Camera = Camera.main;
    }

    // TODO change this shit: ça doit être fait par l'accesseur du camera lerp
    public void RecalculateActionCanvas()
    {
        //
        if (GameManager.Instance != null && m_Camera != null)
        {
            transform.rotation = m_Camera.transform.rotation;
            transform.localPosition = Vector3.zero;
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f) + new Vector3(0.5f, 0.5f, 0.5f) * (1 - GameManager.Instance.CameraFZoomLerp);
        }


        //if (transform.GetChild(0).childCount > 0)
        //{
        //    transform.rotation = m_Camera.transform.rotation;
        //    transform.localPosition = Vector3.zero;
        //    //if (GameManager.Instance.GoTarget != null)
        //    //{
        //        //if (GameManager.Instance.GoTarget.GetComponent<BoxCollider>() != null)
        //        //{
        //        //    Vector3 size = GameManager.Instance.GoTarget.GetComponent<BoxCollider>().bounds.size;
        //        //    Vector3 V1 = new Vector3(0, Mathf.Max(size.y, size.z), 0);
        //        //    transform.localPosition = V1 + (Vector3.up * (1 - GameManager.Instance.CameraFZoomLerp));

        //        //}
        //        //else if (GameManager.Instance.GoTarget.GetComponentInChildren<MeshCollider>() != null)
        //        //{
        //        //    Vector3 size = GameManager.Instance.GoTarget.GetComponentInChildren<MeshCollider>().bounds.size;
        //        //    Vector3 V1 = new Vector3(0, size.y, 0);
        //        //    transform.localPosition = V1 + (Vector3.up * (1.1f - GameManager.Instance.CameraFZoomLerp));
        //        //    ///////////////////////////// ! \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        //        //    // TODO WARNING BIG CHIASSOUILLE fix for protoprout fix quickly after ask Rémi
        //        //    if (GameManager.Instance.GoTarget.GetComponentInParent<PawnInstance>() != null &&
        //        //        GameManager.Instance.GoTarget.GetComponentInParent<PawnInstance>().Data.PawnName == "Grekhan")
        //        //    {
        //        //        transform.localPosition -= Vector3.up * 0.2f;
        //        //    }
        //        //    ///////////////////////////// ! \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        //        //}
        //    //}

        //    transform.localScale = new Vector3(0.2f, 0.2f, 0.2f)  + new Vector3(0.5f,0.5f,0.5f)  * (1 - GameManager.Instance.CameraFZoomLerp);
        //}
    }

    void Update()
    {
        RecalculateActionCanvas();
    }
}

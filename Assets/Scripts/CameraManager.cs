using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initial -0.03, 1.08, -3.07
/// </summary>
public class CameraManager : MonoBehaviour {

    enum eCameraState
    {
        close,
        far,
        fromCloseToFar,
        fromFarToClose
    }

    struct Transformation
    {
        public Quaternion rotation;
        public Vector3 posititon;
    }

    // *********************************
    // Camera parameters
    [Header("Camera Controls")]
    [SerializeField]
    float dragSpeed = 2;
    Vector3 dragOrigin;

    float minFov = 15f;
    float maxFov = 90f;
    float sensitivity = 10f;

    eCameraState camState = eCameraState.close;
    [SerializeField]
    float fTransitionSpeed = 1;
    [SerializeField]
    float fPosYClose = 1;
    [SerializeField]
    float fPosYFar = 3;
    float fTransitionLerp = 0;
    Quaternion quatOrientationClose;
    Quaternion quatOrientationFar;
    Transformation origin, target;
    // *********************************

    Tile activeTile;
    Vector3 positionFromATileClose;
    Vector3 positionFar;
    bool isUpdateNeeded = false;
    Vector3 oldPosition;
    float lerpParameter = 0.0f;

    public void Start()
    {
        fTransitionLerp = 0;

        positionFromATileClose = transform.position;

        GameObject goCameraFarRef = GameObject.Find("CameraFarRef");
        quatOrientationFar = goCameraFarRef.transform.rotation;
        Destroy(goCameraFarRef);

        Vector3 v3NewPos = transform.position;
        v3NewPos.y = fPosYClose;
        transform.position = v3NewPos;
        quatOrientationClose = transform.rotation;
        camState = eCameraState.close;
    }

    public void UpdateCameraPosition(KeeperInstance selectedKeeper)
    {
        isUpdateNeeded = true;
        oldPosition = transform.position;
        activeTile = TileManager.Instance.GetTileFromKeeper[selectedKeeper];
    }

    public void UpdateCameraPosition()
    {
        isUpdateNeeded = true;
        oldPosition = transform.position;
        activeTile = TileManager.Instance.PrisonerTile;
    }

    void Update()
    {
        if (isUpdateNeeded)
        {
            lerpParameter += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(oldPosition, positionFromATileClose + activeTile.transform.position, Mathf.Min(lerpParameter, 1.0f));
            if (lerpParameter >= 1.0f)
            {
                isUpdateNeeded = false;
                oldPosition = Vector3.zero;
                lerpParameter = 0.0f;
            }
        }

        CameraControls();

        if(camState == eCameraState.fromCloseToFar || camState == eCameraState.fromFarToClose)
        {
            UpdateCamModeTransition();
        }
    }

    private void CameraControls()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToogleCamMode();
        }

        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
            {
                float fov = Camera.main.fieldOfView;
                fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
                fov = Mathf.Clamp(fov, minFov, maxFov);
                Camera.main.fieldOfView = fov;
            }                                                                                                                                                                                            
            return;
        }

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 distance = Camera.main.transform.forward * 3.0f;
            Vector3 point = Camera.main.transform.position + distance;
            Vector3 perp = new Vector3(pos.y, -pos.x, 0);
            Vector3 rotateAroundAxis = perp.x * Camera.main.transform.right + perp.y * Vector3.up;
            float rotateDegrees = Mathf.Rad2Deg * Mathf.Atan(pos.magnitude / distance.magnitude);
            // TODO: Clamp
            Camera.main.transform.RotateAround(point, rotateAroundAxis, rotateDegrees);
        }
        else
        {
            //Vector3 move = new Vector3(-pos.x, -pos.y, 0);
            //Camera.main.transform.Translate(move.normalized * dragSpeed, Space.Self);
            Vector3 move = new Vector3(-pos.x, 0, -pos.y);
            Camera.main.transform.Translate(move.normalized * dragSpeed * Time.unscaledDeltaTime, Space.World);
        }

        dragOrigin = Input.mousePosition;
    }

    private void ToogleCamMode()
    {
        if (camState == eCameraState.close || camState == eCameraState.far)
        {
            fTransitionLerp = 0.0f;
        }
        else if (camState == eCameraState.fromFarToClose || camState == eCameraState.fromCloseToFar)
        {
            fTransitionLerp = 1.0f - fTransitionLerp;
        }

        if (camState == eCameraState.close || camState == eCameraState.fromFarToClose)
        {
            origin.rotation = quatOrientationClose;
            origin.posititon = transform.position;
            origin.posititon.y = fPosYClose;

            target.rotation = quatOrientationFar;
            target.posititon = transform.position;
            target.posititon.y = fPosYFar;

            camState = eCameraState.fromCloseToFar;
        }
        else if (camState == eCameraState.far || camState == eCameraState.fromCloseToFar)
        {
            origin.rotation = quatOrientationFar;
            origin.posititon = transform.position;
            origin.posititon.y = fPosYFar;

            target.rotation = quatOrientationClose;
            target.posititon = transform.position;
            target.posititon.y = fPosYClose;

            camState = eCameraState.fromFarToClose;
        }
    }

    private void UpdateCamModeTransition()
    {
        fTransitionLerp += fTransitionSpeed * Time.unscaledDeltaTime;

        if (fTransitionLerp < 1.0f)
        {
            transform.position = Vector3.Lerp(origin.posititon, target.posititon, fTransitionLerp);
            transform.rotation = Quaternion.Lerp(origin.rotation, target.rotation, fTransitionLerp);
        }
        else
        {
            transform.position = target.posititon;
            transform.rotation = target.rotation;

            switch (camState)
            {
                case eCameraState.fromCloseToFar:
                    camState = eCameraState.far;
                    break;
                case eCameraState.fromFarToClose:
                    camState = eCameraState.close;
                    break;
            }
        }
    }
}

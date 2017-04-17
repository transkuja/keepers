using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initial -0.03, 1.08, -3.07
/// </summary>
public class CameraManager : MonoBehaviour {

    enum eZoomState
    {
        idle = 0,
        forward = 1,
        backward = -1
    }

    struct Transformation
    {
        public Quaternion rotation;
        public Vector3 position;
    }

    // *********************************
    // Camera parameters
    [Header("Camera Controls")]

    // Camera Zoom
    eZoomState zoomState = eZoomState.idle;
    [SerializeField] float fZoomSpeed = 1;
    [SerializeField] float fLerpNotch = 0.1f;
    Transformation tFar, tClose;
    float fZoomLerp = 1;
    float fZoomLerpOrigin = 1;
    float fLerpTarget = 1;

    // Camera Drag
    [SerializeField] float fDragFactor = 0.1f;
    [SerializeField]
    float fKeySpeed = 5.0f;
    bool bIsDraging = false;
    Vector3 v3DragOrigin;

    // Camera adapters
    public List<GreyTileCameraAdapter> greyTileCameraAdapters = new List<GreyTileCameraAdapter>();
    public List<SelectionPointerCameraAdapter> selectionPointerCameraAdapters = new List<SelectionPointerCameraAdapter>();
    public List<WorldspaceCanvasCameraAdapter> worldspaceCanvasCameraAdapters = new List<WorldspaceCanvasCameraAdapter>();

    // *********************************
    enum CameraBound
    {
        North,
        East,
        South,
        West
    }


    [SerializeField]
    Transform cameraBounds;

    Tile activeTile;
    Vector3 positionFromATileClose;
    bool isUpdateNeeded = false;
    Vector3 oldPosition;
    float lerpParameter = 0.0f;

    #region Accessors
    public float FZoomLerp
    {
        get
        {
            return fZoomLerp;
        }

        private set
        {
            fZoomLerp = value;
            foreach (GreyTileCameraAdapter ca in greyTileCameraAdapters)
                ca.RecalculateOrientation(Camera.main);

            foreach (SelectionPointerCameraAdapter ca in selectionPointerCameraAdapters)
                ca.RecalculateOrientationAndScale();

            foreach (WorldspaceCanvasCameraAdapter ca in worldspaceCanvasCameraAdapters)
                ca.RecalculateActionCanvas(Camera.main);
        }
    }

    public Tile ActiveTile
    {
        get
        {
            return activeTile;
        }

        set
        {
            activeTile = value;
        }
    }
    #endregion

    public void Start()
    {
        FZoomLerp = 0;

        positionFromATileClose = transform.position;

        GameObject goCameraCloseRef = GameObject.Find("CameraCloseRef");
        tClose.rotation = goCameraCloseRef.transform.rotation;
        tClose.position = goCameraCloseRef.transform.position;
        Destroy(goCameraCloseRef);

        GameObject goCameraFarRef = GameObject.Find("CameraFarRef");
        tFar.rotation = goCameraFarRef.transform.rotation;
        tFar.position = goCameraFarRef.transform.position;
        Destroy(goCameraFarRef);

        transform.position = tClose.position;
        transform.rotation = tClose.rotation;

        zoomState = eZoomState.idle;
        FZoomLerp = 1;
        fLerpTarget = 1;
        fZoomLerpOrigin = 1;

        GameManager.Instance.RegisterCameraManager(this);
    }
    public void UpdateCameraPosition(PawnInstance pi)
    {
        isUpdateNeeded = true;
        oldPosition = transform.position;

        activeTile = pi.CurrentTile;
    }

    public void UpdateCameraPosition(Tile targetTile)
    {
        isUpdateNeeded = true;
        oldPosition = transform.position;
        activeTile = targetTile;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.Normal)
        {
            if (isUpdateNeeded)
            {
                lerpParameter += Time.deltaTime;

                Vector3 v3NewPos = Vector3.Lerp(oldPosition, positionFromATileClose + activeTile.transform.position + Vector3.back, Mathf.Min(lerpParameter, 1.0f));

                v3NewPos.y = transform.position.y;
                transform.localPosition = v3NewPos;

                v3NewPos.y = tClose.position.y;
                tClose.position = v3NewPos;

                v3NewPos.y = tFar.position.y;
                tFar.position = v3NewPos;

                if (lerpParameter >= 1.0f)
                {
                    isUpdateNeeded = false;
                    oldPosition = Vector3.zero;
                    lerpParameter = 0.0f;
                }

                //Vector3 pos = transform.position;
                //pos.x = Mathf.Clamp(pos.x, cameraBounds.GetChild((int)CameraBound.West).position.x, cameraBounds.GetChild((int)CameraBound.East).position.x);
                //pos.z = Mathf.Clamp(pos.z, cameraBounds.GetChild((int)CameraBound.South).position.z, cameraBounds.GetChild((int)CameraBound.North).position.z);
                //transform.position = pos;
            }

            Controls();

            if (zoomState != eZoomState.idle)
            {
                UpdateCamZoom();
            }
        }
    }

    private void Controls()
    {
        ControlZoom();

        ControlDrag();

        ControlKeys();
    }

    private void ControlZoom()
    {
        float fScrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (fScrollValue != 0)
        {
            if ((fScrollValue > 0 && fZoomLerp < 1) || (fScrollValue < 0 && fZoomLerp > 0))
            {
                switch (zoomState)
                {
                    case eZoomState.idle:
                        fZoomLerpOrigin = fZoomLerp;
                        fLerpTarget = fZoomLerp;
                        break;

                    case eZoomState.forward:
                        if (fScrollValue < 0)
                        {
                            fZoomLerpOrigin = fZoomLerp;
                            fLerpTarget = fZoomLerp;
                        }
                        break;
                    case eZoomState.backward:
                        if (fScrollValue > 0)
                        {
                            fZoomLerpOrigin = fZoomLerp;
                            fLerpTarget = fZoomLerp;
                        }
                        break;
                    default:
                        break;
                }

                fLerpTarget += fLerpNotch * (fScrollValue * 10);

                if (fLerpTarget > 1)
                {
                    fLerpTarget = 1;
                }
                else if (fLerpTarget < 0)
                {
                    fLerpTarget = 0;
                }

                if (fScrollValue > 0)
                {
                    zoomState = eZoomState.forward;
                }
                else
                {
                    zoomState = eZoomState.backward;
                }


            }
        }
    }

    void OnDrawGizmos()
    {
        float north = cameraBounds.GetChild((int)CameraBound.North).position.z;
        float south = cameraBounds.GetChild((int)CameraBound.South).position.z;
        float east = cameraBounds.GetChild((int)CameraBound.East).position.x;
        float west = cameraBounds.GetChild((int)CameraBound.West).position.x;

        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(new Vector3(west, 0, north), new Vector3(east, 0, north));
        Gizmos.DrawLine(new Vector3(east, 0, north), new Vector3(east, 0, south));
        Gizmos.DrawLine(new Vector3(east, 0, south), new Vector3(west, 0, south));
        Gizmos.DrawLine(new Vector3(west, 0, south), new Vector3(west, 0, north));
    }

    private void ControlKeys()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            Vector3 v3IncrementPos = new Vector3( Input.GetAxisRaw("Horizontal")* fKeySpeed * Time.deltaTime, 0, Input.GetAxisRaw("Vertical") * fKeySpeed * Time.deltaTime);
            if (!((tClose.position + v3IncrementPos).z > cameraBounds.GetChild((int)CameraBound.South).position.z &&
               (tClose.position + v3IncrementPos).z < cameraBounds.GetChild((int)CameraBound.North).position.z))
            {
                v3IncrementPos.z = 0.0f;
            }

            transform.position += v3IncrementPos;
            tClose.position += v3IncrementPos;
            tFar.position += v3IncrementPos;

            v3DragOrigin = Input.mousePosition;
        }
       
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, cameraBounds.GetChild((int)CameraBound.West).position.x, cameraBounds.GetChild((int)CameraBound.East).position.x);
        //pos.z = Mathf.Clamp(pos.z, cameraBounds.GetChild((int)CameraBound.South).position.z, cameraBounds.GetChild((int)CameraBound.North).position.z);
        transform.position = pos;

        pos = tClose.position;
        pos.x = Mathf.Clamp(pos.x, cameraBounds.GetChild((int)CameraBound.West).position.x, cameraBounds.GetChild((int)CameraBound.East).position.x);
        tClose.position = pos;

        pos = tFar.position;
        pos.x = Mathf.Clamp(pos.x, cameraBounds.GetChild((int)CameraBound.West).position.x, cameraBounds.GetChild((int)CameraBound.East).position.x);
        tFar.position = pos;
    }

    private void ControlDrag()
    {
        if (Input.GetMouseButtonDown(2) && !bIsDraging)
        {
            v3DragOrigin = Input.mousePosition;

            bIsDraging = true;
        }
        else if (Input.GetMouseButton(2) && bIsDraging)
        {

            Vector3 v3DragDiff = v3DragOrigin - Input.mousePosition;

            Vector3 v3IncrementPos = new Vector3(v3DragDiff.x * fDragFactor, 0, v3DragDiff.y * fDragFactor);
            if(!((tClose.position + v3IncrementPos).z > cameraBounds.GetChild((int)CameraBound.South).position.z &&
               (tClose.position + v3IncrementPos).z < cameraBounds.GetChild((int)CameraBound.North).position.z))
            {
                v3IncrementPos.z = 0.0f;
            }

            transform.position += v3IncrementPos;
            tClose.position += v3IncrementPos;
            tFar.position += v3IncrementPos;

            v3DragOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2) && bIsDraging)
        {
            bIsDraging = false;
        }
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, cameraBounds.GetChild((int)CameraBound.West).position.x, cameraBounds.GetChild((int)CameraBound.East).position.x);
        //pos.z = Mathf.Clamp(pos.z, cameraBounds.GetChild((int)CameraBound.South).position.z, cameraBounds.GetChild((int)CameraBound.North).position.z);
        transform.position = pos;

        pos = tClose.position;
        pos.x = Mathf.Clamp(pos.x, cameraBounds.GetChild((int)CameraBound.West).position.x, cameraBounds.GetChild((int)CameraBound.East).position.x);
        tClose.position = pos;

        pos = tFar.position;
        pos.x = Mathf.Clamp(pos.x, cameraBounds.GetChild((int)CameraBound.West).position.x, cameraBounds.GetChild((int)CameraBound.East).position.x);
        tFar.position = pos;
    }

    private void UpdateCamZoom()
    {
        FZoomLerp = fZoomLerp + (fLerpTarget - fZoomLerpOrigin) * fZoomSpeed * Time.unscaledDeltaTime;

        if((zoomState == eZoomState.forward && fZoomLerp > fLerpTarget) || (zoomState == eZoomState.backward && fZoomLerp < fLerpTarget))
        {
            FZoomLerp = fLerpTarget;
            zoomState = eZoomState.idle;
        }

        transform.position = Vector3.Lerp(tFar.position, tClose.position, fZoomLerp);
        transform.rotation = Quaternion.Lerp(tFar.rotation, tClose.rotation, fZoomLerp);
        
    }
}

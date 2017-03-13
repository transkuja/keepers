﻿using System.Collections;
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
    bool bIsDraging = false;
    Vector3 v3DragOrigin;
    // *********************************

    Tile activeTile;
    Vector3 positionFromATileClose;
    bool isUpdateNeeded = false;
    Vector3 oldPosition;
    float lerpParameter = 0.0f;

    public void Start()
    {
        fZoomLerp = 0;

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
        fZoomLerp = 1;
        fLerpTarget = 1;
        fZoomLerpOrigin = 1;
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

            Vector3 v3NewPos = Vector3.Lerp(oldPosition, positionFromATileClose + activeTile.transform.position, Mathf.Min(lerpParameter, 1.0f));

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
        }

        Controls();

        if(zoomState != eZoomState.idle)
        {
            UpdateCamZoom();
        }
    }

    private void Controls()
    {
        ControlZoom();

        ControlDrag();
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

            transform.position += v3IncrementPos;
            tClose.position += v3IncrementPos;
            tFar.position += v3IncrementPos;

            v3DragOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(2) && bIsDraging)
        {
            bIsDraging = false;
        }
    }

    private void UpdateCamZoom()
    {
        fZoomLerp = fZoomLerp + (fLerpTarget - fZoomLerpOrigin) * fZoomSpeed * Time.unscaledDeltaTime;

        if((zoomState == eZoomState.forward && fZoomLerp > fLerpTarget) || (zoomState == eZoomState.backward && fZoomLerp < fLerpTarget))
        {
                fZoomLerp = fLerpTarget;
                zoomState = eZoomState.idle;
        }

        transform.position = Vector3.Lerp(tFar.position, tClose.position, fZoomLerp);
        transform.rotation = Quaternion.Lerp(tFar.rotation, tClose.rotation, fZoomLerp);
    }
}
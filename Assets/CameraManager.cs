using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Initial -0.03, 1.08, -3.07
/// </summary>
public class CameraManager : MonoBehaviour {

    Tile activeTile;
    Vector3 positionFromATileClose;
    Vector3 positionFar;
    bool isUpdateNeeded = false;
    Vector3 oldPosition;
    float lerpParameter = 0.0f;

    public void Start()
    {
        positionFromATileClose = transform.position;
    }

    public void UpdateCameraPosition(KeeperInstance selectedKeeper)
    {
        isUpdateNeeded = true;
        oldPosition = transform.position;
        activeTile = TileManager.Instance.GetTileFromKeeper[selectedKeeper];
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
    }
}

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

    public void Start()
    {
        positionFromATileClose = transform.position;
    }

    public void UpdateCameraPosition(KeeperInstance selectedKeeper)
    {
        activeTile = TileManager.Instance.GetTileFromKeeper[selectedKeeper];
        transform.SetParent(activeTile.transform);
        transform.localPosition = positionFromATileClose;
        transform.SetParent(null);
    }
}

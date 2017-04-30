﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Behaviour;

public class RightMouseClickExpected : MonoBehaviour {

    public string TargetExpected;

    void Start()
    {
        TutoManager.MouseClicked = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) == true)
                {
                    Tile tileHit = hitInfo.collider.gameObject.GetComponentInParent<Tile>();

                    if (tileHit != null && hitInfo.collider.GetComponentInParent<PawnInstance>() == null && TargetExpected == "Tile")
                    {
                        if (tileHit == GameManager.Instance.AllKeepersList[0].CurrentTile)
                        {
                            TutoManager.MouseClicked = true;
                            GameManager.Instance.AllKeepersList[0].GetComponent<AnimatedPawn>().TriggerRotation(hitInfo.point);
                        }
                    }
                    else if (TargetExpected == "Portal" && hitInfo.collider.GetComponentInParent<TilePassage>() != null)
                    {
                        TutoManager.MouseClicked = true;
                        TilePassage tp = hitInfo.collider.gameObject.GetComponent<TilePassage>();
                        GameManager.Instance.AllKeepersList[0].GetComponent<AnimatedPawn>().TriggerRotation(hitInfo.collider.transform.position);

                        if (tp.GetComponentInParent<Tile>() == GameManager.Instance.AllKeepersList[0].CurrentTile)
                            tp.HandleClick();
                    }
                }
            }
        }
    }
}
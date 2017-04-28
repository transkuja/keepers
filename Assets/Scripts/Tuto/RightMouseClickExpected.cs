using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Behaviour;

public class RightMouseClickExpected : MonoBehaviour {

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
                    if (tileHit != null && hitInfo.collider.GetComponentInParent<PawnInstance>() == null)
                    {
                        if (tileHit == GameManager.Instance.AllKeepersList[0].CurrentTile)
                        {
                            TutoManager.MouseClicked = true;
                            GameManager.Instance.AllKeepersList[0].GetComponent<AnimatedPawn>().TriggerRotation(hitInfo.point);
                        }
                    }
                }
            }
        }
    }
}

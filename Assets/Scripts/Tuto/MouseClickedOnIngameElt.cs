using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;

public class MouseClickedOnIngameElt : MonoBehaviour {

	void Start () {
        TutoManager.MouseClicked = false;	
	}
	
    void OnMouseDown()
    {
        TutoManager.MouseClicked = true;
        if (GetComponent<Keeper>() != null)
        {
            GameManager.Instance.AddKeeperToSelectedList(GetComponent<PawnInstance>());
            GetComponent<Keeper>().IsSelected = true;
        }
    }
}

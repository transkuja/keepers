using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickedOnIngameElt : MonoBehaviour {

	void Start () {
        TutoManager.MouseClicked = false;	
	}
	
    void OnMouseDown()
    {
        TutoManager.MouseClicked = true;
    }
}

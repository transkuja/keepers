using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpenerControls : MonoBehaviour {

    BoxOpener box;

        public void Start()
    {
        box = GameObject.FindObjectOfType<MenuManager>().GetComponent<BoxOpener>();
    }

    public void OnMouseDown()
    {
        if(! box.BoxIsReady)
        box.BoxControls();
    }

}

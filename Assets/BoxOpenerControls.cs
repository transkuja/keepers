using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpenerControls : MonoBehaviour {

    BoxOpener box;
    MenuManager menuManager;

        public void Start()
    {
        box = GameObject.FindObjectOfType<MenuManager>().GetComponent<BoxOpener>();
        menuManager = GameObject.FindObjectOfType<MenuManager>();
    }

    public void OnMouseDown()
    {
        if(!box.BoxIsReady && menuManager.DuckhavebringThebox)
        box.BoxControls();
    }

}

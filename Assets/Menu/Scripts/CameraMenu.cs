using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour {

    [SerializeField] MenuManager menuManager;

    public void ShowMiniature()
    {
        menuManager.currentMiniature.Show(true);
    }
}

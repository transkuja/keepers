using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

    private bool isOpen = false;

    public bool IsOpen
    {
        get
        {
            return isOpen;
        }

        set
        {
            isOpen = value;
        }
    }
}

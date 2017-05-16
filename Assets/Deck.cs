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


    public void OnMouseOver()

    {
        if (GetComponent<GlowObjectCmd>().IsBlinking)
        {
            GetComponent<GlowObjectCmd>().ActivateBlinkBehaviour(false);
            GetComponent<GlowObjectCmd>().UpdateColor(true);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

    private bool isOpen = false;

    void Start()
    {
        Material mat = GetComponent<MeshRenderer>().material;
        mat.mainTexture = Translater.DeckTexture();
        mat.SetTexture("_EmissionMap", Translater.DeckTexture());
    }

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

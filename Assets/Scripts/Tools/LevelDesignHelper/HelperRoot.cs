using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperRoot : MonoBehaviour {
    [SerializeField]
    int width;
    [SerializeField]
    int height;

    public int Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }
}

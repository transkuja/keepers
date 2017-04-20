using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture2DUtils : MonoBehaviour {

    [Header("Die faces textures")]
    public Texture2D attackX1;
    public Texture2D attackX2;
    public Texture2D attackX3;
    public Texture2D defenseX1;
    public Texture2D defenseX2;
    public Texture2D defenseX3;
    public Texture2D magicX1;
    public Texture2D magicX2;
    public Texture2D magicX3;
    public Texture2D supportX1;
    public Texture2D supportX2;
    public Texture2D supportX3;

    public Texture2D GetTextureFromFaceData(Face faceData)
    {
        if (faceData.Type == FaceType.Physical)
        {
            if (faceData.Value == 1)
                return attackX1;
            if (faceData.Value == 2)
                return attackX2;
            if (faceData.Value == 3)
                return attackX3;
        }

        if (faceData.Type == FaceType.Magical)
        {
            if (faceData.Value == 1)
                return magicX1;
            if (faceData.Value == 2)
                return magicX2;
            if (faceData.Value == 3)
                return magicX3;
        }

        if (faceData.Type == FaceType.Defensive)
        {
            if (faceData.Value == 1)
                return defenseX1;
            if (faceData.Value == 2)
                return defenseX2;
            if (faceData.Value == 3)
                return defenseX3;
        }

        if (faceData.Type == FaceType.Support)
        {
            if (faceData.Value == 1)
                return supportX1;
            if (faceData.Value == 2)
                return supportX2;
            if (faceData.Value == 3)
                return supportX3;
        }

        Debug.LogWarning("Invalid face data. Cannot retrieve associated face texture.");
        return new Texture2D(Screen.width, Screen.height);
    }
}

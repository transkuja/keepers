using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture2DUtils : MonoBehaviour {

    [Header("Die faces textures")]
    public Texture2D attackX1;
    public Texture2D attackX2;
    public Texture2D attackX3;
    public Texture2D attackX4;
    public Texture2D attackX5;
    public Texture2D attackX6;
    public Texture2D defenseX1;
    public Texture2D defenseX2;
    public Texture2D defenseX3;
    public Texture2D defenseX4;
    public Texture2D defenseX5;
    public Texture2D defenseX6;
    public Texture2D magicX1;
    public Texture2D magicX2;
    public Texture2D magicX3;
    public Texture2D magicX4;
    public Texture2D magicX5;
    public Texture2D magicX6;

    [Header("Cursors")]
    public Texture2D iconeMouse;
    public Texture2D iconeMouseClicked;
    public Texture2D attackCursor;
    public Texture2D buffCursor;
    public Texture2D magicCursor;

    [Header("Credits")]
    public Texture2D creditsEN;
    public Texture2D creditsFR;


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
            if (faceData.Value == 4)
                return attackX4;
            if (faceData.Value == 5)
                return attackX5;
            if (faceData.Value == 6)
                return attackX6;
        }

        if (faceData.Type == FaceType.Magical)
        {
            if (faceData.Value == 1)
                return magicX1;
            if (faceData.Value == 2)
                return magicX2;
            if (faceData.Value == 3)
                return magicX3;
            if (faceData.Value == 4)
                return magicX4;
            if (faceData.Value == 5)
                return magicX5;
            if (faceData.Value == 6)
                return magicX6;
        }

        if (faceData.Type == FaceType.Defensive)
        {
            if (faceData.Value == 1)
                return defenseX1;
            if (faceData.Value == 2)
                return defenseX2;
            if (faceData.Value == 3)
                return defenseX3;
            if (faceData.Value == 4)
                return defenseX4;
            if (faceData.Value == 5)
                return defenseX5;
            if (faceData.Value == 6)
                return defenseX6;
        }

        Debug.LogWarning("Invalid face data. Cannot retrieve associated face texture.");
        return new Texture2D(Screen.width, Screen.height);
    }
}

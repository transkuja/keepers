using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Utils {

	public static Direction GetOppositeDirection(Direction from)
    {
        switch (from)
        {
            case Direction.North:
                return Direction.South;
            case Direction.North_East:
                return Direction.South_West;
            case Direction.North_West:
                return Direction.South_East;
            case Direction.South:
                return Direction.North;
            case Direction.South_East:
                return Direction.North_West;
            case Direction.South_West:
                return Direction.North_East;
            default:
                return from;
        }
    }

    public static void TakeScreenShot()
    {
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        Camera.main.targetTexture = rt;
        Camera.main.Render();
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();
        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        GameObject.Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        Object.Destroy(screenShot);

        File.WriteAllBytes(Application.dataPath + "/Textures/CardLevel/SavedScreen" + SceneManager.GetActiveScene().buildIndex + ".png", bytes);
    }
}


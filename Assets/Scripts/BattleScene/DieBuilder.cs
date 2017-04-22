using UnityEngine;

public class DieBuilder : MonoBehaviour {

	public static GameObject BuildDie(Die dieToBuild, Tile tileConcerned, Vector3 position)
    {
        Debug.Log(dieToBuild);
        Debug.Log(dieToBuild.Faces.Length);
        GameObject dieInstance = Instantiate(GameManager.Instance.PrefabUtils.die, tileConcerned.transform);
        float emissionColor = 0.75f;
        Transform currentFace = dieInstance.transform.GetChild(0);
        Material currentFaceMaterial = currentFace.GetComponent<Renderer>().material;
        currentFaceMaterial.mainTexture = GameManager.Instance.Texture2DUtils.attackX1;
        currentFaceMaterial.SetTexture("_EmissionMap", GameManager.Instance.Texture2DUtils.attackX1);
        currentFaceMaterial.SetColor("_EmissionColor", new Color(emissionColor, emissionColor, emissionColor));
        currentFaceMaterial.EnableKeyword("_EMISSION");

        for (int i = 1; i <= 6; i++)
        {
            currentFace = dieInstance.transform.GetChild(i);
            currentFaceMaterial = currentFace.GetComponent<Renderer>().material;
            currentFaceMaterial.mainTexture = GameManager.Instance.Texture2DUtils.GetTextureFromFaceData(dieToBuild.Faces[i - 1]);
            currentFaceMaterial.SetTexture("_EmissionMap", GameManager.Instance.Texture2DUtils.GetTextureFromFaceData(dieToBuild.Faces[i - 1]));
            currentFaceMaterial.SetColor("_EmissionColor", new Color(emissionColor, emissionColor, emissionColor));
            currentFaceMaterial.EnableKeyword("_EMISSION");

            Debug.Log(i);
            currentFace.GetComponent<FaceComponent>().FaceData = dieToBuild.Faces[i - 1];
        }

        

        dieInstance.transform.position = GameManager.Instance.ActiveTile.transform.position + position;
        return dieInstance;
    }
}

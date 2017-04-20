using UnityEngine;

public class DieBuilder : MonoBehaviour {

	public static GameObject BuildDie(Die dieToBuild, Tile tileConcerned, Vector3 position)
    {
        GameObject dieInstance = Instantiate(GameManager.Instance.PrefabUtils.die, tileConcerned.transform);
        for (int i = 1; i <= 6; i++)
        {
            float emissionColor = 0.5f;

            Transform currentFace = dieInstance.transform.GetChild(i);
            Material currentFaceMaterial = currentFace.GetComponent<Renderer>().material;
            currentFaceMaterial.mainTexture = GameManager.Instance.Texture2DUtils.GetTextureFromFaceData(dieToBuild.Faces[i - 1]);
            currentFaceMaterial.SetTexture("_EmissionMap", GameManager.Instance.Texture2DUtils.GetTextureFromFaceData(dieToBuild.Faces[i - 1]));
            currentFaceMaterial.SetColor("_EmissionColor", new Color(emissionColor, emissionColor, emissionColor));
            currentFace.GetComponent<FaceComponent>().FaceData = dieToBuild.Faces[i - 1];
        }
        
        dieInstance.transform.position = GameManager.Instance.ActiveTile.transform.position + position;
        return dieInstance;
    }
}

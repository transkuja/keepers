using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBuilder : MonoBehaviour {

	public static void BuildDie(Die dieToBuild)
    {
        if (dieToBuild.NbrOfFaces == 6)
        {
            GameObject dieInstance = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject[] faceColliders = new GameObject[dieToBuild.NbrOfFaces];
            for (int i = 0; i < dieToBuild.NbrOfFaces; i++)
            {
                faceColliders[i].transform.SetParent(dieInstance.transform);
                faceColliders[i].AddComponent<SphereCollider>();
                faceColliders[i].AddComponent<FaceComponent>();
                faceColliders[i].GetComponent<FaceComponent>().FaceData = dieToBuild.Faces[i];
            }
            Bounds bounds = dieInstance.GetComponent<BoxCollider>().bounds;

            faceColliders[0].GetComponent<SphereCollider>().center += new Vector3(bounds.extents.x, 0, 0);
            faceColliders[1].GetComponent<SphereCollider>().center += new Vector3(-bounds.extents.x, 0, 0);
            faceColliders[2].GetComponent<SphereCollider>().center += new Vector3(bounds.extents.y, 0, 0);
            faceColliders[3].GetComponent<SphereCollider>().center += new Vector3(-bounds.extents.y, 0, 0);
            faceColliders[4].GetComponent<SphereCollider>().center += new Vector3(bounds.extents.z, 0, 0);
            faceColliders[5].GetComponent<SphereCollider>().center += new Vector3(-bounds.extents.z, 0, 0);
        }
    }
}

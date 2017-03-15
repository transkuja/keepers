using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaterialChanger : MonoBehaviour {
    public Shader GreyedShader;
    private Material[] BaseMaterials;
    private Material GreyedMaterial;

    void CreateMaterials()
    {
        /*if (GreyedMaterial == null)
        {
            GreyedMaterial = new Material(GreyedShader);
            GreyedMaterial.hideFlags = HideFlags.HideAndDontSave;
        }*/

        if (BaseMaterials == null)
        {
            MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
            BaseMaterials = new Material[mrs.Length];
            for(int i = 0; i < mrs.Length; i++)
            {
                BaseMaterials[i] = mrs[i].material;
            }
            Debug.Log(BaseMaterials[0].name);
        }
    }

    // Use this for initialization
    void Start () {
        CreateMaterials();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

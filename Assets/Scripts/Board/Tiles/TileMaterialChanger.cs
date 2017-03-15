using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaterialChanger : MonoBehaviour {
    public Shader GreyedShader;
    private Material[] BaseMaterials;
    private Material[] GreyedMaterials;
    private Tile tile;

    void CreateMaterials()
    {
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

        if (GreyedMaterials == null)
        {
            GreyedMaterials = new Material[BaseMaterials.Length];
            for(int i = 0; i < GreyedMaterials.Length; i++)
            {
                GreyedMaterials[i] = new Material(GreyedShader);
                GreyedMaterials[i].hideFlags = HideFlags.HideAndDontSave;
                GreyedMaterials[i].SetTexture("_MainTex", BaseMaterials[i].GetTexture("_MainTex"));
            }
            
        }
    }

    public void SetAsGreyedModel()
    {
        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mrs.Length; i++)
        {
            mrs[i].material = GreyedMaterials[i];
        }
    }

    public void SetAsBaseModel()
    {
        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mrs.Length; i++)
        {
            mrs[i].material = BaseMaterials[i];
        }
    }

    void UpdateModel()
    {
        if (tile.State == TileState.Greyed)
            SetAsGreyedModel();
        else if (tile.State == TileState.Discovered)
            SetAsBaseModel();
    }

    // Use this for initialization
    void Start () {
        CreateMaterials();
        tile = GetComponentInParent<Tile>();
        if(tile.State == TileState.Greyed)
        {
            SetAsGreyedModel();
        }
        tile.StateChanged += UpdateModel;
	}
	
    void OnDestroy()
    {
        tile.StateChanged -= UpdateModel;
    }
	// Update is called once per frame
	void Update () {
		
	}
}

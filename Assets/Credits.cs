using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour {

    private Material mat;
	void OnEnable () {
        mat = GetComponent<MeshRenderer>().material;
        mat.mainTexture = Translater.CreditsTexture();
        mat.SetTexture("_EmissionMap", Translater.CreditsTexture());
    }

}

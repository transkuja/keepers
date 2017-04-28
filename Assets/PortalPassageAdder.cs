using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PortalPassageAdder : MonoBehaviour {
    public GameObject portalPassagesPrefab;
    public bool activate = false;
	// Use this for initialization
	void Start () {
        activate = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (activate)
            Activate();
	}

    void Activate()
    {
        activate = false;
        GameObject[] portals = GameObject.FindGameObjectsWithTag("NorthTrigger");
        int l = portals.Length;
        for(int i = 0; i < l; i++)
        {
            GameObject go = Instantiate(portalPassagesPrefab, portals[i].transform.parent.parent);
            int sibIndex = portals[i].transform.parent.GetSiblingIndex();
            DestroyImmediate(portals[i].transform.parent.gameObject);
            go.transform.SetSiblingIndex(sibIndex);
            go.transform.localPosition = Vector3.zero;
            go.name = "PortalPassages";
        }

    }
}

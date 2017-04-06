using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceComponent : MonoBehaviour {
    Face faceData;

    Face scoredFace;

    public Face FaceData
    {
        get
        {
            return faceData;
        }

        set
        {
            faceData = value;
        }
    }

    public Face ScoredFace
    {
        get
        {
            return scoredFace;
        }

        set
        {
            scoredFace = value;
        }
    }

    private void Start()
    {
        scoredFace = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponentInParent<Rigidbody>().velocity.magnitude <= 0)
        {
            if (transform.GetSiblingIndex() % 2 == 0)
                scoredFace = transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<FaceComponent>().faceData;
            else
                scoredFace = transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<FaceComponent>().faceData;
        }
    }
}

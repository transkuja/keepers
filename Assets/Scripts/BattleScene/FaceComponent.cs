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

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponentInParent<Rigidbody>().velocity.magnitude <= 0)
        {
            // TODO save scored face, this should be the face opposite
            // scoredFace = 
        }
    }
}

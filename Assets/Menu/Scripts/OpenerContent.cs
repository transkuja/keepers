using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenerContent : MonoBehaviour {

    public struct trContent{
        public Vector3 v3Pos;
        public Quaternion quatRot;
    }

    public trContent trStart;
    public trContent trEnd;

    Opener openerParent;

    public float fSpeed = 1;

    float fLerp = 0;

    float fWay = -1;
    bool bIsMoving = false;

    Collider col;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider>();
        openerParent = transform.parent.GetComponent<Opener>();
	}
	
	// Update is called once per frame
	void Update () {
        if (bIsMoving == true)
        {
            UpdateLerp();
        }
	}

    public void Toogle()
    {
        fWay *= -1;
        bIsMoving = true;
        if(fLerp != 0 && fLerp != 1)
        {
            col.enabled = false;
        }
    }

    void UpdateLerp()
    {
        fLerp += Time.unscaledDeltaTime * fSpeed * fWay;

        if(fWay > 0 && fLerp > 1)
        {
            fLerp = 1;
            bIsMoving = false;
            col.enabled = true;
        }
        else if(fWay < 0 && fLerp < 0){
            fLerp = 0;
            bIsMoving = false;
            col.enabled = true;
        }

        transform.localPosition = Vector3.Slerp(trStart.v3Pos, trEnd.v3Pos, fLerp);
        //transform.localRotation = Quaternion.Slerp(trStart.quatRot, trEnd.quatRot, fLerp);
    }
}

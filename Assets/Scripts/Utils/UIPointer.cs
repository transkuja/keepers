using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPointer : MonoBehaviour {

    RectTransform rt;
    float fSlerp = 0;
    public float fSpeed = 2;
    public float fMagnitude = 20;
    Vector3 v3StartPos;
    Vector3 v3TargetPos;
    bool bBackward = true;

	// Use this for initialization
	void Start () {

        rt = GetComponent<RectTransform>();

        v3StartPos = rt.position;

        v3TargetPos = v3StartPos - new Vector3(0, 1 * fMagnitude, 1);

        Quaternion rotation = rt.rotation;
        Vector3 translateVector = v3TargetPos - v3StartPos;

        v3TargetPos = v3StartPos - (rotation * translateVector);
	}
	
	// Update is called once per frame
	void Update () {
        if (bBackward)
        {
            fSlerp += Mathf.Min(Time.unscaledDeltaTime * fSpeed, 0.03f);
        }
        else
        {
            fSlerp -= Time.unscaledDeltaTime * fSpeed;
        }

        if (bBackward)
        {
            if(fSlerp > 1)
            {
                fSlerp = 1;
                bBackward = false;
            }
        }
        else
        {
            if(fSlerp < 0)
            {
                fSlerp = 0;
                bBackward = true;
            }
        }

        rt.position = Vector3.Slerp(v3StartPos, v3TargetPos, fSlerp);

        Debug.Log(rt.anchoredPosition);
    }
}

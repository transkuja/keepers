using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniature : MonoBehaviour {

    enum State
    {
        shrank,
        growing = 1,
        grown,
        shrinking = -1,
    }
   
    public AnimationCurve curve;

    public bool bStartHidden = true;

    public bool bNeedForce = false;
    public bool bNeedShow = false;
    public bool bNeedHide = false;
    public float fRotateSpeed;
    public float fScaleSpeed;

    float fLerp = 1;
    Vector3 v3ScaleRef;
    State state = State.grown;

    // Use this for initialization
    void Start () {
        v3ScaleRef = transform.localScale;
        if (bStartHidden)
        {
            transform.localScale = Vector3.zero;
            state = State.shrank;
            fLerp = 0;
        }
	}

	// Update is called once per frame
	void Update () {

        if(bNeedShow)
        {
            Show(bNeedForce);
            bNeedShow = false;
        }
        if(bNeedHide)
        {
            Hide(bNeedForce);
            bNeedHide = false;
        }
        if (bNeedForce)
        {
            bNeedForce = false;
        }


        if (state != State.shrank)
        {
            updateRotation();

            if (state == State.growing || state == State.shrinking)
            {
                updateScale();
            }
        }
	}

    public void Show(bool force = false)
    {
        if (force)
        {
            fLerp = 0;
        }

        state = State.growing;
    }

    public void Hide(bool force = false)
    {
        if (force)
        {
            fLerp = 1;
        }

        state = State.shrinking;
    }

    void updateScale()
    {
        fLerp += Time.unscaledDeltaTime * fScaleSpeed * (int)state;

        if(state == State.growing && fLerp > 1)
        {
            fLerp = 1;
            state = State.grown;
        }
        else if (state == State.shrinking && fLerp < 0)
        {
            fLerp = 0;
            state = State.shrank;
        }

        transform.localScale = Vector3.Lerp(Vector3.zero, v3ScaleRef * 1.1f, curve.Evaluate(fLerp));
    }

    void updateRotation()
    {
        transform.localRotation *= Quaternion.Euler(new Vector3(0,1.0f,0) * fRotateSpeed);
    }
}

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
   
    [SerializeField] bool bNeedShow = false;  // FOR TESTS PURPOSES
    [SerializeField] bool bNeedHide = false;  // FOR TESTS PURPOSES

    [SerializeField] AnimationCurve curve;
    [SerializeField] float fScaleSpeed;
    [SerializeField] float fRotateSpeed;
    float fLerp = 0;
    Vector3 v3ScaleRef;
    State state = State.shrank;

    // Use this for initialization
    void Start () {
        v3ScaleRef = transform.localScale;
        transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

        if(bNeedShow)
        {
            Show();
            bNeedShow = false;
        }
        if(bNeedHide)
        {
            Hide();
            bNeedHide = false;
        }

        if(state != State.shrank)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displayer : MonoBehaviour {

    bool bIsOver = false;
    bool bIsShown = false;
    bool bIsMoving = false;
    float fLerp = 0;

    public float fShowDelay = 1;
    public float fSpeed = 1;

    [SerializeField] Transform trShowspot;

    Vector3 v3StartPos, v3TargetPos;

    public Opener openerParent;

    public List<Displayer> listDisplayerSiblings;

    [SerializeField] LayerMask maskToCheck;

	// Use this for initialization
	void Start () {
        Init();
	}
	
    public void Init()
    {
        v3StartPos = transform.position;
        v3TargetPos = trShowspot.position;

        if(transform.parent != null)
        {
            openerParent = GetComponentInParent<Opener>();
        }

        listDisplayerSiblings = new List<Displayer>();
        for(int i = 0; i< transform.parent.childCount; i++)
        {
            Displayer newDisplayer = transform.parent.GetChild(i).GetComponent<Displayer>();
            if(newDisplayer != null && newDisplayer != this)
            {
                listDisplayerSiblings.Add(newDisplayer);
            }
        }
    }

	// Update is called once per frame
	void Update () {
		if(bIsShown)
        {
            UpdateShow();

            if (Input.GetMouseButtonDown(0))
            {
                Hide();
            }
        }

        if (bIsMoving)
        {
            UpdatePosition();
        }
	}

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (bIsShown)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    void UpdateShow()
    {
        if (Input.GetMouseButtonDown(0) && bIsShown)
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, maskToCheck);
            if(hit.transform == null){
                Hide();
                openerParent.bDontClose = false;
            }
        }
    }

    void Show()
    {
        if (openerParent != null)
        {
            openerParent.bDontClose = true;
        }

        if (bIsMoving && !bIsShown)
        {
            fLerp = 1 - fLerp;
        }

        HideSiblings();

        bIsShown = true;
        bIsMoving = true;
    }

    public void Hide()
    {
        if (bIsShown)
        {
            if (bIsMoving)
            {
                fLerp = 1 - fLerp;
            }
            else
            {
                fLerp = 1;
            }
        }

        bIsShown = false;
        bIsMoving = true;
    }

    void UpdatePosition()
    {
        if (bIsShown)
        {
            fLerp += Time.unscaledDeltaTime * fSpeed;

            if(fLerp > 1)
            {
                fLerp = 1;
                bIsMoving = false;
            }
        }
        else
        {
            fLerp -= Time.unscaledDeltaTime * fSpeed;

            if (fLerp < 0)
            {
                fLerp = 0;
                bIsMoving = false;
            }
        }

        transform.position = Vector3.Lerp(v3StartPos, v3TargetPos, fLerp);
    }

    public void HideSiblings()
    {
        for (int i = 0; i< listDisplayerSiblings.Count; i++)
        {
            if(listDisplayerSiblings[i] != this)
            listDisplayerSiblings[i].Hide();
        }
    }
}

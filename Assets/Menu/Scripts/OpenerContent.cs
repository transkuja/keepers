using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenerContent : MonoBehaviour {

    public class keyPose{
        public Vector3 v3Pos;
        public Quaternion quatRot;

        public keyPose(Vector3 _v3, Quaternion _quat)
        {
            v3Pos = _v3;
            quatRot = _quat;
        }
    }

    public List<keyPose> listKeyPose = new List<keyPose>();
    int iKeyPoseTarget = 0;

    Opener openerParent;

    public float fSpeed;

    float fLerp = 0;

    public int iWay = -1;
    bool bIsMoving = false;

    public MeshCollider col;
    public Renderer rd;

    public bool bNeedCompute = false;
    public bool bNeedShow = false;
    public bool bNeedHide = false;
    public bool bKill = false;

    Vector3 size;

	// Use this for initialization
	void Start () {
        Init();
    }
	
    public void Init()
    {
        LoadParent();

        col = GetComponent<MeshCollider>();
        rd = GetComponent<Renderer>();
        if(col != null)
        {
            col.enabled = false;
        }
        if (rd != null)
        {
            rd.enabled = false;
        }
    }

    public void LoadParent()
    {
        if (transform.parent != null)
        {
            openerParent = transform.parent.GetComponent<Opener>();
        }
    }

	// Update is called once per frame
	void Update () {

        if (bNeedShow)
        {
            Show();
            bNeedShow = false;
        }

        if (bNeedHide)
        {
            Hide();
            bNeedHide = false;
        }

        if (bIsMoving == true)
        {
            UpdatePosition();
        }
	}

    public void Toogle()
    {
        iWay *= -1;
        iKeyPoseTarget += iWay;

        if (bIsMoving)
        {
            fLerp = 1 - fLerp;
        }else
        {
            if(iWay == 1)
            {
                if(rd != null)
                {
                    rd.enabled = true;
                }
            }else
            {
                if(col != null)
                {
                    col.enabled = false;
                }
            }
            fLerp = 0;
        }

        bIsMoving = true;
    }

    public void Show(bool force = false)
    {
        if(!bIsMoving || force)
        {
            iKeyPoseTarget = 1;
            iWay = 1;
            bIsMoving = true;
            if(rd != null)
            {
                rd.enabled = true;
            }
        }
        else
        {
            if(iWay == -1)
            {
                iWay = 1;
                iKeyPoseTarget += 1;
                fLerp = 1 - fLerp;
                if (rd != null)
                {
                    rd.enabled = true;
                }
            }
        }
    }

    public void Hide(bool force = false)
    {
        if (force && rd != null)
        {
            rd.enabled = true;
        }
        if (!bIsMoving || force)
        {
            iKeyPoseTarget = (listKeyPose.Count - 2) >= 0 ? listKeyPose.Count - 2 : 0;
            iWay = -1;
            bIsMoving = true; 
            if (col != null)
            {
                col.enabled = false;
            }
        }
        else
        {
            if (iWay == 1)
            {
                iWay = -1;
                iKeyPoseTarget -= 1;
                fLerp = 1 - fLerp;
                if (col != null)
                {
                    col.enabled = false;
                }
            }
        }

        Displayer displayer = GetComponent<Displayer>();
        if(displayer != null)
        {
            displayer.Hide();
        }
    }

    void UpdatePosition()
    {
        fLerp += Time.unscaledDeltaTime * fSpeed;

        if(fLerp > 1)
        {
            fLerp = 1;
        }

        int iIndexOrigin = iKeyPoseTarget + (-iWay);

        transform.localPosition = Vector3.Lerp(listKeyPose[iIndexOrigin].v3Pos , listKeyPose[iKeyPoseTarget].v3Pos, fLerp);
        transform.localRotation = Quaternion.Lerp(listKeyPose[iIndexOrigin].quatRot, listKeyPose[iKeyPoseTarget].quatRot, fLerp);

        if(fLerp == 1)
        {
            if(iKeyPoseTarget + iWay >= listKeyPose.Count || iKeyPoseTarget + iWay < 0) // Si on est arrivé au bout de l'animation;
            {
                if(iWay == -1)
                {
                    if(rd != null)
                    {
                        rd.enabled = false;
                    }
                    if(openerParent != null)
                    {
                        openerParent.bOpened = false;
                        if (bNeedCompute)
                        {
                            openerParent.LoadChilds();
                            openerParent.ComputeContentPositions();
                            bNeedCompute = false;
                        }
                    }
                }else
                {
                    if (bKill)
                    {
                        GameObject.Destroy(gameObject);
                    }
                    if (col != null)
                    {
                        col.enabled = true;
                    }

                    Displayer d = GetComponent<Displayer>();
                    if(d != null)
                    {
                        d.Init();
                    }

                }
                bIsMoving = false;
            }else
            {
                iKeyPoseTarget += iWay;
            }
            fLerp = 0;
        }
    }

    public void AddKeyPose(Vector3 _v3Pos, Quaternion _quatRot)
    {
        listKeyPose.Add(new OpenerContent.keyPose(_v3Pos, _quatRot));
    }
}

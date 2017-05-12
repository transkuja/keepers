﻿using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Opener : MonoBehaviour {

    [HideInInspector] public List<Opener> listOpenerSiblings;
    [HideInInspector] public List<OpenerContent> listChilds;
    /*[HideInInspector]*/ public bool bOpened = false;
    [HideInInspector] public bool bDontClose = false;
    [HideInInspector] public bool bNeedReload = false;

    [HideInInspector] public float fOverTimer = 0;

    LayerMask layerToCheck;

    public List<Transform> listTrSpots;
    public bool bIsLast;
    public bool bOverMode = false;
    public float fOverTime = 0;
    public float fOffsetX = .1f;
    public float fOffsetZ = .1f;

    void Start () {

        Init();
        layerToCheck = GameObject.Find("Menu").GetComponent<MenuControlsQ>().layerToCheck;

    }

    void Init()

    {

        LoadSiblings();

        LoadChilds();

        ComputeContentPositions();

    }

    public void Reset()
    {
        Fold();
        LoadChilds();

    }

    void LoadSiblings()
    {
        if (transform.parent != null)
        {
            Opener openerTemp;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                openerTemp = transform.parent.GetChild(i).gameObject.GetComponent<Opener>();
     
                if (openerTemp != null && openerTemp != this)
                {
                    if (listOpenerSiblings == null)
                    {
                        listOpenerSiblings = new List<Opener>();
                    }
                    listOpenerSiblings.Add(openerTemp);
                }
            }
        }
    }

    public void LoadChilds()
    {
        listChilds = new List<OpenerContent>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "OpenerContent")
            {
                listChilds.Add(transform.GetChild(i).gameObject.GetComponent<OpenerContent>());
                if (transform.GetChild(i).GetComponent<Displayer>() != null)
                {
                    transform.GetChild(i).GetComponent<Displayer>().LoadSiblings();
                }
            }
        }
    }

    public void ComputeContentPositions()

    {

        Vector3 size = Vector3.zero;

        if (listChilds.Count > 0)

        {

            size = GetComponentInChildren<MeshRenderer>().bounds.size;

        }



        for(int i = 0; i< listChilds.Count; i++)

        {

            listChilds[i].listKeyPose.Clear();



            listChilds[i].AddKeyPose(Vector3.zero, Quaternion.identity);



            float fOrigin = (listChilds.Count % 2 == 1) ? -((listChilds.Count / 2) * (size.x + fOffsetX)) : -((((listChilds.Count / 2) -1) * (size.x + fOffsetX)) + (size.x + fOffsetX) / 2.0f);

            float fIncrement = size.x + fOffsetX;



            for (int j = 0; j < listTrSpots.Count; j++)

            {

                if (j + 1 < listTrSpots.Count)

                {

                    listChilds[i].AddKeyPose(listTrSpots[j].position - transform.position, listTrSpots[j].rotation);

                }

                else

                {

                    Vector3 pos = listTrSpots[j].position - transform.position; ;



                    pos += (listTrSpots[j].rotation * new Vector3(fOrigin + i * fIncrement,0,0));



                    listChilds[i].AddKeyPose(pos, listTrSpots[j].rotation);

                }

            }

        }

    }

    void Update()
    {
        if (bOpened == true && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, layerToCheck);
            if(hit.transform == null)
            {
                Fold();
            }
        }

        if (bOverMode && bOpened)
        {
            if(fOverTimer > 0)
            {
                fOverTimer -= Time.unscaledDeltaTime;
            }

            if(fOverTimer < 0 && !bDontClose )
            {
                fOverTimer = 0;
                Fold();
            }
        }
    }

    #region OverMode



    void OnMouseEnter()
    {
        if (bOverMode) {
            //if (!bOpened){
                Unfold();
                CloseSiblings();
            //}
            fOverTimer = 0;
        }
    }



    void OnMouseExit()
    {
        if (bOverMode)
        {
            if (bDontClose)
            {
                fOverTimer = 0;
            }
            else
            {
                fOverTimer = fOverTime;
            }
        }
    }

    #endregion

    #region NormalMode

    void OnMouseDown()
    {
        if (!bOverMode)
        {
            if (bOpened)
            {
                Fold();
            }
            else
            {
                Unfold();
                CloseSiblings();
            }
        }
    }

    #endregion

    public void Unfold(bool force = false) // Depliage du contenu
    {
        if (bNeedReload)
        {
            LoadChilds();
            ComputeContentPositions();
            bNeedReload = false;
        }

        for (int i = 0; i < listChilds.Count; i++)
        {
            listChilds[i].Show(force);
        }

        bOpened = true;

        GameObject.FindObjectOfType<MenuManagerQ>().SetActiveChatBoxes(false);
    }

    public void Fold(bool force = false) // Rangement du contenu
    {
        bDontClose = false;

        if (bNeedReload)
        {
            LoadChilds();
            ComputeContentPositions();
            bNeedReload = false;
        }

        for (int i = 0; /*(bOpened || force) &&*/ i < listChilds.Count; i++)
        {
            listChilds[i].Hide(force);
            if (!bIsLast)
            {
                if (listChilds[i].GetComponent<Opener>() == null) Debug.Log("MERDE");
                listChilds[i].GetComponent<Opener>().Fold(force);
            }
        }

        if(this != null && transform.parent != null && transform.parent.GetComponent<Opener>() == null)
        {
            GameObject.FindObjectOfType<MenuManagerQ>().SetActiveChatBoxes(true);
        }
    }

    void CloseSiblings()

    {
        for (int i = 0; listOpenerSiblings != null && i < listOpenerSiblings.Count; i++)

        {
            if (listOpenerSiblings[i].bOpened == true)
            {
                listOpenerSiblings[i].Fold();
            }

        }

    }
}


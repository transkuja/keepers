using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour {

    List<Opener> listOpenerSiblings;

    LayerMask layerToCheck;

    [HideInInspector] public bool bOpened = false;
    [HideInInspector] public List<OpenerContent> listChilds;

    public bool bDontClose = false;

    public bool bIsLast;
    public bool bOverMode = false;
    float fOverTimer = 0;
    public float fOverTime = 0;
    public float fOffsetX = .1f;
    public float fOffsetZ = .1f;

    public List<Transform> listTrSpots;

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

    void LoadChilds()
    {
        listChilds = new List<OpenerContent>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "OpenerContent")
            {
                listChilds.Add(transform.GetChild(i).gameObject.GetComponent<OpenerContent>());
            }
        }
    }

    void ComputeContentPositions()
    {
        Vector3 size = GetComponentInChildren<MeshRenderer>().bounds.size;

        for(int i = 0; i< listChilds.Count; i++)
        {
            listChilds[i].AddKeyPose(Vector3.zero, Quaternion.identity);

            float fOrigin = (listChilds.Count % 2 == 1) ? -((listChilds.Count / 2) * (size.x + fOffsetX)) : -((listChilds.Count / 2) * (size.x / 2.0f + fOffsetX / 2.0f));
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
        if(bOpened == true && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, layerToCheck);
            if(hit.transform == null)
            {
                Fold();
            }
        }

        if (bOverMode)
        {
            if(fOverTimer > 0)
            {
                fOverTimer -= Time.unscaledDeltaTime;
            }

            if(fOverTimer < 0)
            {
                Fold();
            }
        }
    }

    #region OverMode

    void OnMouseEnter()
    {
        if (bOverMode && !bOpened)
        {
            Unfold();
            CloseSiblings();
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


    void Unfold() // Depliage du contenu
    {
        for (int i = 0; i < listChilds.Count; i++)
        {
            listChilds[i].Show();
        }
        bOpened = true;
    }

    void Fold() // Rangement du contenu
    {
        for (int i = 0; bOpened && i < listChilds.Count; i++)
        {
            listChilds[i].Hide();
            if (!bIsLast)
            {
                listChilds[i].GetComponent<Opener>().Fold();
            }
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

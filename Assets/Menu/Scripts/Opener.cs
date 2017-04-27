using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour {

    enum ContentNature
    {
        Deck,
        Card
    }

    enum eState
    {
        closed,     // tout est bien rangé
        going,      // le conteneur se déplace
        unfolding,  // on déplie le contenu
        open,       // tout est bien ouvert
        folding,    // on replie le contenu
        coming,     // le conteneur revien à sa place
    }

    eState state = eState.closed;
    public bool bIsLast;


    public bool bOverMode = false;
    public bool bAlwaysAllowed = true;
    public Transform trOffset = null;
    public float fOffsetX = .1f;
    public float fOffsetZ = .1f;

    bool bAllowed = true;

    Vector3 v3Size;
    Vector3 v3Offset = Vector3.zero;

    public List<OpenerContent> listChilds = new List<OpenerContent>();
    List<Opener> listOpenerSiblings;

	void Start () {
        Init();
    }

    void Init()
    {

        if (transform.parent != null)   // Récupération des Opener au même niveau de hierarchie
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
                    openerTemp.enabled = false;
                }
            }
        }

        if (trOffset != null)
        {
            v3Offset = trOffset.position - transform.position;
        }

        v3Size = GetComponentInChildren<MeshRenderer>().bounds.size;

        for (int i = 0; i < transform.childCount; i++)  // Récupération du contenu de l'Opener
        {
            if (transform.GetChild(i).tag == "OpenerContent")
            {
                listChilds.Add(transform.GetChild(i).gameObject.GetComponent<OpenerContent>());

                listChilds[listChilds.Count - 1].trStart.v3Pos = Vector3.zero;

                float x = -((listChilds.Count / 2.0f) * (v3Size.x + v3Offset.x)) + i * (v3Size.x + v3Offset.x);
                listChilds[listChilds.Count - 1].trEnd.v3Pos = new Vector3(x, 0, 0);



                //listChilds[listChilds.Count - 1].GetComponent<MeshCollider>().enabled = false;
                //transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        UpdateState();
    }

    #region OverMode

    void OnMouseEnter()
    {
        if (bOverMode)
        {
            Open();
        }
    }

    void OnMouseExit()
    {
        if (bOverMode)
        {
            Close();
        }
    }

    #endregion

    #region NormalMode

    void OnMouseDown()
    {
        if (!bOverMode)
        {
            if (bAllowed || bAlwaysAllowed)
            {
                switch (state)
                {
                    case eState.closed:
                        for(int i =0; listOpenerSiblings != null && i < listOpenerSiblings.Count; i++)
                        {
                            listOpenerSiblings[i].Close();
                        }
                        Open();
                        break;
                    case eState.going:
                        Close();
                        break;
                    case eState.unfolding:
                        break;
                    case eState.open:
                        break;
                    case eState.folding:
                        break;
                    case eState.coming:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #endregion


    void Open()
    {
        for (int i = 0; i < listChilds.Count; i++)
        {

            //float x = -((listGoCards.Count / 2.0f) * (v3Size.x + fOffsetX)) + ((listGoCards.Count % 2 == 0)? v3Size.x / 2.0f : v3Size.x) + i * (v3Size.x + fOffsetX);

            //listGoCards[i].transform.localPosition = new Vector3(x, 0, (trOffset == null ? (v3Size.z + fOffsetZ) : v3Size.z / 2.0f)) + v3Offset;

            listChilds[i].gameObject.SetActive(true);


            listChilds[i].GetComponent<Animator>().SetBool("bIsOpen", true);
        }

        for (int i = 0; !bAlwaysAllowed && listOpenerSiblings != null && i < listOpenerSiblings.Count; i++)
        {
            listOpenerSiblings[i].bAllowed = false;
        }

        state = eState.going;
    }

    void Close()
    {
        Fold();
        for (int i = 0; i < listChilds.Count; i++)
        {
            //listChilds[i].GetComponent<Animator>().SetBool("bIsOpen", false);
            //listGoCards[i].transform.localPosition = Vector3.zero;
            listChilds[i].GetComponent<MeshCollider>().enabled = false;
        }
        for (int i = 0; /*!bAlwaysAllowed &&*/ listOpenerSiblings != null && i < listOpenerSiblings.Count; i++)
        {
            listOpenerSiblings[i].bAllowed = true;
        }
    }

    void Unfold()
    {
        for (int i = 0; i < listChilds.Count; i++)
        {
            listChilds[i].Toogle();
        }
    }

    void Fold()
    {
        for (int i = 0; i < listChilds.Count; i++)
        {
            listChilds[i].Toogle();
        }
    }

    public void UpdateState()
    {

        switch (state)
        {
            case eState.closed:
                break;
            case eState.going:
                if (listChilds[0].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle_02"))
                {
                    Unfold();
                    state = eState.unfolding;
                }
                break;
            case eState.unfolding:
                break;
            case eState.open:
                break;
            case eState.folding:
                break;
            case eState.coming:
                break;
            default:
                break;
        }
    }
}

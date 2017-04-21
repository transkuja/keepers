using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour {

    public bool bAlwaysAllowed = true;
    public Transform trOffset = null;

    bool bAllowed = true;
    bool bIsOpen = false;

    Vector3 v3Offset = Vector3.zero;

    List<GameObject> listGoCards = new List<GameObject>();
    List<Opener> listOpenerSiblings;

	// Use this for initialization
	void Start () {
        Init();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (bAllowed || bAlwaysAllowed)
        {
            if (!bIsOpen)
            {
                for(int i =0; i < listOpenerSiblings.Count; i++)
                {
                    listOpenerSiblings[i].Close();
                }
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    void Init()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "OpenerContent")
            {
                listGoCards.Add(transform.GetChild(i).gameObject);
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

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

        if (trOffset != null)
        {
            v3Offset = trOffset.position - transform.position;
        }
    }

    void Open()
    {
        for (int i = 0; i < listGoCards.Count; i++)
        {
            float x = -((listGoCards.Count / 2.0f) * 1.1f) + i * 1.1f;

            listGoCards[i].transform.localPosition = new Vector3(x, 0, 1.2f) + v3Offset;

            listGoCards[i].SetActive(true);
        }
        bIsOpen = true;
        for (int i = 0; !bAlwaysAllowed && listOpenerSiblings != null && i < listOpenerSiblings.Count; i++)
        {
            listOpenerSiblings[i].bAllowed = false;
        }
    }

    void Close()
    {
        for (int i = 0; i < listGoCards.Count; i++)
        {
            listGoCards[i].transform.localPosition = Vector3.zero;
            listGoCards[i].SetActive(false);
        }
        bIsOpen = false;
        for (int i = 0; listOpenerSiblings != null && i < listOpenerSiblings.Count; i++)
        {
            listOpenerSiblings[i].bAllowed = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour {


    bool bAllowed = true;
    bool bIsOpen = false;

    public Transform trOffset = null;
    Vector3 v3Offset = Vector3.zero;

    List<GameObject> listGoCards = new List<GameObject>();
    List<Opener> listOpenerSiblings;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < transform.childCount; i++)
        {
            listGoCards.Add(transform.GetChild(i).gameObject);
        }

        if(transform.parent != null)
        {
            Opener openerTemp;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                openerTemp = transform.parent.GetChild(i).gameObject.GetComponent<Opener>();
                if(openerTemp != null && openerTemp != this)
                {
                    if(listOpenerSiblings == null)
                    {
                        listOpenerSiblings = new List<Opener>();
                    }
                    listOpenerSiblings.Add(openerTemp);
                }
            }
        }

        if(trOffset != null)
        {
            v3Offset = trOffset.position - transform.position;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (bAllowed)
        {
            if (!bIsOpen)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    void Open()
    {
        for (int i = 0; i < listGoCards.Count; i++)
        {
            float x = -((listGoCards.Count / 2) * 1.1f) + i * 1.1f;

            listGoCards[i].transform.localPosition = new Vector3(x, 0, 1.2f) + v3Offset;

            listGoCards[i].SetActive(true);
        }
        bIsOpen = true;
        for (int i = 0; listOpenerSiblings != null && i < listOpenerSiblings.Count; i++)
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

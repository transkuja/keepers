using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTranslation : MonoBehaviour {

    [SerializeField] List<Transform> listTrCubes = new List<Transform>();
    public float fSpeed;

    TranslationHandler th;

    
	// Use this for initialization
	void Start () {
        th = GetComponent<TranslationHandler>();
        th.Init();

        for (int i = 0; i < listTrCubes.Count; i++)
        {
            TranslationHandler.Translation t = new TranslationHandler.Translation(listTrCubes[i].name, fSpeed);
            t.AddKeyPose(new Vector3(0,2,0), Quaternion.identity);
            t.AddKeyPose(new Vector3(0,1.05f,0), listTrCubes[i].rotation, listTrCubes[i]);
            th.listTranslation.Add(t);
        }

        TranslationHandler.Translation t1 = new TranslationHandler.Translation("test", fSpeed);
        t1.AddKeyPose(new Vector3(-.5f, 2, 0), Quaternion.identity);
        for (int i = 0; i < listTrCubes.Count; i++)
        {
            t1.AddKeyPose(new Vector3(0, 1.05f, 0), listTrCubes[i].rotation, listTrCubes[i]);
        }
        t1.AddKeyPose(new Vector3(.5f, 2, 0), Quaternion.identity);
        th.listTranslation.Add(t1);

        th.SetCurrent("test");
        //Debug.Log(th.GetTranslationByName("test").listKeyPoses.Count);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            th.PlayCurrent(TranslationHandler.State.toEnd);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            th.PlayCurrent(TranslationHandler.State.toDebut);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            th.Invert();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            th.SetCurrent(th.BrowseTranslations());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            th.SetCurrent(th.BrowseTranslations(true));
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            if(hit.transform != null)
            {
                TranslationHandler.Translation trans = th.listTranslation.Find(t => t.name == hit.transform.name);
                if(trans != null)
                {
                    th.Play(trans.name, TranslationHandler.State.toEnd, true);
                }
            }
        }
    }
}

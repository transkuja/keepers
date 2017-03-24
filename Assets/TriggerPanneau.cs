using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerPanneau : MonoBehaviour {

    public GameObject prefabPanelPanneau;
    public string textPanneau;
    private GameObject goPanneau;

    void Awake()
    {
        goPanneau = Instantiate(prefabPanelPanneau, GameManager.Instance.Ui.basePanneauPanel.transform);
        goPanneau.transform.localPosition = Vector3.zero;
        goPanneau.transform.localScale = Vector3.one;
    }

    void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.layer == LayerMask.NameToLayer("KeeperInstance"))
        {
            KeeperInstance ki = other.gameObject.GetComponentInParent<KeeperInstance>();

            if (goPanneau == null) return;
            goPanneau.SetActive(true);
            // Si le personnage peut parler
            if ( ki.isAbleToImproveMoral)
            {
                Debug.Log("test");
                goPanneau.GetComponentInChildren<Text>().text = textPanneau;
            }
            else
            {
                goPanneau.GetComponentInChildren<Text>().text = "Ouaf Aou Ahouahou Wouf Wuf wuf Wouaf Wouaf Wuah Whaf Whouaf" ;
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("KeeperInstance"))
        {

            if (goPanneau == null) return;
            goPanneau.SetActive(false);
        }
    }
}

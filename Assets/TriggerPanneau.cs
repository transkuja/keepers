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
        goPanneau = Instantiate(GameManager.Instance.PrefabUtils.PrefabContentPanneauUI, GameManager.Instance.Ui.goContentPanneauParent.transform);
        goPanneau.transform.localPosition = Vector3.zero;
        goPanneau.transform.localScale = Vector3.one;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<KeeperInstance>() != null)
        {
            KeeperInstance ki = other.GetComponentInParent<KeeperInstance>();
            // On veut le mesh collider actif du perso
            foreach (MeshCollider mc in ki.gameObject.GetComponentsInChildren<MeshCollider>())
            {
                if (mc.enabled)
                {
                    GameManager.Instance.GoTarget = mc.gameObject;
                    break;
                }
            }
            InteractionImplementer InteractionImplementer = new InteractionImplementer();
            InteractionImplementer.Add(new Interaction(Look), 0, "Look", GameManager.Instance.SpriteUtils.spriteExplore, true);
            GameManager.Instance.Ui.UpdateActionPanelUIQ(InteractionImplementer);
        }
    }

    void Look(int _i)
    {
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {
            KeeperInstance ki = GameManager.Instance.ListOfSelectedKeepers[0];
            if (goPanneau == null) return;
            goPanneau.SetActive(true);
            // Si le personnage peut parler
            if (ki.isAbleToImproveMoral)
            {
                goPanneau.GetComponentInChildren<Text>().text = textPanneau;
            }
            else
            {
                goPanneau.GetComponentInChildren<Text>().text = "Ouaf Aou Ahouahou Wouf Wuf wuf Wouaf Wouaf Wuah Whaf Whouaf";
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<KeeperInstance>() != null)
        {
            if (goPanneau == null) return;
            goPanneau.SetActive(false);
        }
    }
}

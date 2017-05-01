using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerPanneau : MonoBehaviour {

    public string textPanneau;
    private GameObject goPanneau;

    void Start()
    {
        goPanneau = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabContentPanneauUI, GameManager.Instance.Ui.goContentPanneauParent.transform);
        goPanneau.transform.localPosition = Vector3.zero;
        goPanneau.transform.localScale = Vector3.one;

        GetComponent<Interactable>().Interactions.Add(new Interaction(Look), 0, "Look", GameManager.Instance.SpriteUtils.spriteExplore, true);
    }

    void Look(int _i)
    {
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {
            PawnInstance ki = GameManager.Instance.ListOfSelectedKeepers[0];
            if (goPanneau == null) return;
            goPanneau.SetActive(true);
            // Si le personnage peut parler
            if (ki.Data.Behaviours[(int)BehavioursEnum.CanSpeak])
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
        if (other.GetComponentInParent<Behaviour.Keeper>() != null)
        {
            if (goPanneau == null) return;
            goPanneau.SetActive(false);
        }
    }
}

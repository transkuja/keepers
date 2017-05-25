using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerPanneau : MonoBehaviour {

    public string textPanneau;
    private GameObject goPanneau;
    public string level;

    void Start()
    {
        goPanneau = Instantiate(GameManager.Instance.PrefabUIUtils.PrefabContentPanneauUI, GameManager.Instance.Ui.goContentPanneauParent.transform);
        goPanneau.transform.localPosition = Vector3.zero;
        goPanneau.transform.localScale = Vector3.one;

        GetComponent<Interactable>().Interactions.Add(new Interaction(Look), 0, "Look", GameManager.Instance.SpriteUtils.spriteExamine, true);
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
                goPanneau.GetComponentInChildren<Text>().text = Translater.PanelsText(level, 0, CharacterRace.Human);
            }
            else
            {
                if (ki.Data.PawnId == "lucky")
                {
                    goPanneau.GetComponentInChildren<Text>().text = Translater.PanelsText(level, 0, CharacterRace.Cat);
                }
                else
                {
                    goPanneau.GetComponentInChildren<Text>().text = Translater.PanelsText(level, 0, CharacterRace.Dog);
                }

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

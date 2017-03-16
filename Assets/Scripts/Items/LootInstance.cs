using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInstance : MonoBehaviour, IPickable {

    private InteractionImplementer interactionImplementer;

    public Sprite Loot;

    void Awake()
    {
        interactionImplementer = new InteractionImplementer();
        Interaction bit = new Interaction(Pick);
        Debug.Log(gameObject);
        //interactionImplementer.Add(new Interaction(Pick), "Pick", GameManager.Instance.Ui.spritePick);
    }


    public InteractionImplementer InteractionImplementer
    {
        get
        {
            return interactionImplementer;
        }

        set
        {
            interactionImplementer = value;
        }
    }

    public void Pick(int _i = 0)
    {
    
    }
    public void OnMouseOver()
    {
        GameManager.Instance.Ui.UiIconFeedBack.TriggerFeedback(Loot);
    }
    public void OnMouseExit()
    {
        GameManager.Instance.Ui.UiIconFeedBack.DisableFeedback();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInstance : MonoBehaviour, IPickable {

    private InteractionImplementer interactionImplementer;

    public int nbSlot = 6;

    public bool isInScene = false;

    public ItemContainer[] loot;

    void Awake()
    {
        interactionImplementer = new InteractionImplementer();
        interactionImplementer.Add(new Interaction(Pick), "Pick", GameManager.Instance.Ui.spritePick);
        if (isInScene)
        {
            Init(nbSlot);
        }
    }

    public void Init(int n)
    {
        loot = new ItemContainer[n];
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
        GameManager.Instance.Ui.UiIconFeedBack.TriggerFeedback(GameManager.Instance.Ui.spriteLoot);
    }
    public void OnMouseExit()
    {
        GameManager.Instance.Ui.UiIconFeedBack.DisableFeedback();
    }

}

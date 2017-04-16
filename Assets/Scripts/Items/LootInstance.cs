using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInstance : MonoBehaviour, IPickable {

    private List<string> Items;

    public int nbSlot;

    public bool isInScene = false;

    void Start()
    {
        InteractionImplementer.Add(new Interaction(Pick), 0, "Pick", GameManager.Instance.SpriteUtils.spritePick);
    }


    [System.Obsolete("Use interactable component instead")]
    public InteractionImplementer InteractionImplementer
    {
        get
        {
            return GetComponent<Interactable>().Interactions;
        }

        set
        {
            GetComponent<Interactable>().Interactions = value;
        }
    }

    public void Pick(int _i = 0)
    {
        GetComponent<Behaviour.Inventory>().InventoryPanel.SetActive(true);
    }
    public void OnMouseOver()
    {
        GameManager.Instance.Ui.UiIconFeedBack.TriggerFeedback(GameManager.Instance.SpriteUtils.spriteLoot);
    }
    public void OnMouseExit()
    {
        GameManager.Instance.Ui.UiIconFeedBack.DisableFeedback();
    }

    public void OnDestroy()
    {
        GlowController.UnregisterObject(GetComponent<GlowObjectCmd>());
    }

}

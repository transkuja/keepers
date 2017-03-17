using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInstance : MonoBehaviour, IPickable {

    private InteractionImplementer interactionImplementer;

    public GameObject lootPanel;

    void Awake()
    {
        interactionImplementer = new InteractionImplementer();
        interactionImplementer.Add(new Interaction(Pick), "Pick", GameManager.Instance.Ui.spritePick);
        lootPanel = GameManager.Instance.Ui.CreateInventoryPanel(this.gameObject);
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
        lootPanel.SetActive(true);
        GameManager.Instance.Ui.UpdateInventoryPanel(gameObject);
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

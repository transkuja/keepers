using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInstance : MonoBehaviour, IPickable {

    private InteractionImplementer interactionImplementer;

    private List<string> Items;

    public int nbSlot;

    public bool isInScene = false;

    void Awake()
    {
        interactionImplementer = new InteractionImplementer();

    }

    void Start()
    {
        interactionImplementer.Add(new Interaction(Pick), 0, "Pick", GameManager.Instance.SpriteUtils.spritePick);
        //if (isInScene)
        //{
        //    Init();
        //}
    }

    //public void Init()
    //{
        //lootPanel = GameManager.Instance.Ui.CreateInventoryPanel(gameObject);
    //}

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

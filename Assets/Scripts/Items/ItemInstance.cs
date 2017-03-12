using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemInstance : MonoBehaviour, IPickable
{
    private InteractionImplementer interactionImplementer;
    [SerializeField]
    private Item item = null;

    [SerializeField]
    int quantity = 1;

    [SerializeField]
    private bool isInScene = false;

    [SerializeField]
    private string idItem;

    void Awake()
    {
        if (isInScene)
        {
            foreach (Item it in GameManager.Instance.Database)
            {
                if (it.Id == idItem)
                {
                    item = it;
                    break;
                }
            }

        }
        interactionImplementer = new InteractionImplementer();
        interactionImplementer.Add(new Interaction(Pick), "Pick", GameManager.Instance.Ui.spritePick);
    }

    public Item Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;
        }
    }

    public int Quantity
    {
        get
        {
            return quantity;
        }

        set
        {
            if (item.GetType() == typeof(Equipment))
                value = 1;
            quantity = value;
        }
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
        ItemManager.AddItem(GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Inventory>().inventory, this);
        GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
        GameManager.Instance.Ui.UpdateKeeperInventoryPanel();
        Destroy(gameObject);
    }
}
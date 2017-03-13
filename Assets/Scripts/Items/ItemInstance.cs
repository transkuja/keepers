using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemInstance : MonoBehaviour, IPickable
{
    private InteractionImplementer interactionImplementer;

    [SerializeField]
    private ItemContainer itemContainer = null;

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
            Init(idItem, quantity);
        }
        interactionImplementer = new InteractionImplementer();
        interactionImplementer.Add(new Interaction(Pick), "Pick", GameManager.Instance.Ui.spritePick);
    }


    public void Init(string _IdItem, int _iNb)
    {
        idItem = _IdItem;
        itemContainer = new ItemContainer(GameManager.Instance.Database.getItemById(_IdItem), quantity);
    }

    public ItemContainer ItemContainer
    {
        get
        {
            return itemContainer;
        }

        set
        {
            itemContainer = value;
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
        bool isNoLeftOver = InventoryManager.AddItemToInventory(GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Inventory>().inventory, this.itemContainer);
        if (isNoLeftOver)
        {
            Destroy(gameObject);
        }
       
        GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
        GameManager.Instance.Ui.UpdateKeeperInventoryPanel();
    }
}
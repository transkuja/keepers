using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour
{
    public class Inventory : MonoBehaviour
    {
        CharacterInstance instance;

    int nbSlot;
    private ItemContainer[] items;
        private GameObject inventoryPanel;
    public ItemContainer[] Items
    {
        get
        {
            return items;
        }

        set
        {
            if(value != null)
            {
                nbSlot = value.Length;
            }
            else
            {
                nbSlot = 0;
            }
            
            items = value;
        }
    }

        public GameObject InventoryPanel
        {
            get
            {
                return inventoryPanel;
            }

            set
            {
                inventoryPanel = value;
            }
        }

    public void Init(int slotCount)
    {
        nbSlot = slotCount;
        items = new ItemContainer[slotCount];
    }

    public void Add(ItemContainer item)
    {
        ItemContainer[] temp = items;
        items = new ItemContainer[nbSlot];
        for (int i = 0; i < nbSlot; i++)
        {
            items[i] = temp[i];
        }
        items[nbSlot] = item;
        nbSlot++;
    }

    public void ComputeItems()
    {
        items = new ItemContainer[nbSlot];
        Item it = null;
        int i = 0;
        foreach (string _IdItem in possibleItems)
        {
            it = GameManager.Instance.Database.getItemById(_IdItem);
            if (Random.Range(0, 10) > it.Rarity)
            {
                items[i++] = new ItemContainer(it, 1);
            }
            if (i >= nbSlot)
                break;
        }

    }
        void Start()
        {
            instance = GetComponent<CharacterInstance>();
            instance.Interactions.Add(new Interaction(Trade), 0, "Trade", GameManager.Instance.MenuUi.spriteTrade);
            items = new List<ItemContainer>();
        }

        public void Trade(int _i = 0)
        {
            inventoryPanel.SetActive(true);
            GameManager.Instance.Ui.UpdateInventoryPanel(gameObject);
        }
    }
}
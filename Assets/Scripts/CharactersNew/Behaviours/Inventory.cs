using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<string> possibleItems;
    int nbSlot;
    private ItemContainer[] items;

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
}
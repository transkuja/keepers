using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemContainer {
    [SerializeField]
    private Item item;
    [SerializeField]
    private int quantity;

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

    public ItemContainer()
    {
        item = null;
        quantity = 0;
    }

    public ItemContainer(Item _item, int _nb)
    {
        item = ItemManager.getInstanciateItem(_item.Type);
        item = _item;
        quantity = _nb;
    }

    public ItemContainer(ItemContainer ic)
    {
        item = ItemManager.getInstanciateItem(ic.item.Type);
        item = ic.item;
        quantity = ic.Quantity;
    }

    public void UseItem(PawnInstance owner)
    {
        // TODO architecturez moi tout ça @Seb
        Quantity -= 1;
        item.UseItem(this, owner);
    }
}

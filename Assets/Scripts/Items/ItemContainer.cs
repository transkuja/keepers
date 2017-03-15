using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer {

    private Item item;
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
        Item = null;
        Quantity = 0;
    }

    public ItemContainer(Item _item, int _nb)
    {
        Item = ItemManager.getInstanciateItem(_item.Type);
        Item = _item;
        Quantity = _nb;
    }

    public ItemContainer(ItemContainer ic)
    {
        Item = ItemManager.getInstanciateItem(ic.item.Type);
        Item = ic.item;
        Quantity = ic.Quantity;
    }

    //public bool Add(Item _item, int iNb)
    //{
    //    if (Item == null)
    //    {
    //        Item = ItemManager.getInstanciateItem(_item.Type);
    //        //Item = _item;
    //        Quantity = iNb;
    //    }
    //    else
    //    {
    //        Quantity += iNb;
    //    }
    //    return true;
    //}

    //public bool Remove(int _iQty)
    //{
    //    if (Quantity == _iQty)
    //    {
    //        Item = null;
    //    }

    //    Quantity -= _iQty;

    //    return true;
    //}

    public void UseItem()
    {
        Quantity -= 1;
        item.UseItem(this);
    }
}

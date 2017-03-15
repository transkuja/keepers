using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

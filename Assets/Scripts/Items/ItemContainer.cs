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
        // TODO architecturez moi tout ça @Seb
        if (GameManager.Instance.ListOfSelectedKeepers[0].Keeper.GoListCharacterFollowing.Count == 0 
            || (item.GetType() == typeof(Ressource) && ((Ressource)item).ResourceUseIndex == ResourceFunctions.UpMentalHealth))
        {
            Quantity -= 1;
            item.UseItem(this);
        }
        else
        {
            if (Quantity >= 2)
            {
                Quantity -= 2;
                item.UseItem(this);
            }
            else
            {
                Quantity -= 1;
                item.UseItem(this, true);
            }
        }

    }
}

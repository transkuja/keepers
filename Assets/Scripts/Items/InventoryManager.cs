using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager {
    private const int maxItemsInSameSlot = 99;

    public static bool AddItemToInventory(List<ItemContainer> _inventory, int nbSlot,  ItemContainer _ic)
    {
        if (_ic.Item.IsStackable)
        {
            if (CheckIfItemTypeIsInInventory(_inventory, _ic))
            {
                for (int i = 0; i < _inventory.Count; i++)
                {
                    if (_inventory[i] != null && _inventory[i].Item.Id == _ic.Item.Id)
                    {
                        int quantityLeft = MergeStackables(_inventory[i], _ic);
                        if (quantityLeft > 0)
                        {
                            return AddItem(_inventory, nbSlot, _ic);
                        }
                    }

                }
            } else
            {
                return AddItem(_inventory, nbSlot, _ic);
            }
        }
        else
        {
            return AddItem(_inventory, nbSlot, _ic);
        }
        return true;
    }

    public static bool AddItem(List<ItemContainer> _inventory, int nbSlot,  ItemContainer _ic)
    {
        int freeIndex = FindFreeSlot(_inventory, nbSlot);
        if (freeIndex == -1)
        {
            return false;

        }

        if (freeIndex == _inventory.Count)
        {
            _inventory.Add(_ic);
        }
        else
        {
            _inventory[freeIndex] = _ic;
        }

        return true;
    }

    public static bool RemoveItem(List<ItemContainer> _inventory, ItemContainer _ic)
    {
        int index = GetInventoryItemIndex(_inventory, _ic);
        if (index == -1)
        {
            return false;
        }
        _inventory[index] = null;
        return true;
    }

    public static void SwapItemBeetweenInventories(List<ItemContainer> itemsFrom, int indexItemFrom, List<ItemContainer> itemsTo, int indexItemTo)
    {
        if (indexItemTo < itemsTo.Count)
        {
            ItemContainer temp = itemsFrom[indexItemFrom];
            itemsFrom[indexItemFrom] = itemsTo[indexItemTo];
            itemsTo[indexItemTo] = temp;
        }
        else
        {
            itemsTo.Add(itemsFrom[indexItemFrom]);
            itemsFrom[indexItemFrom] = null;
        }
    }

    public static void SwapItemInSameInventory(List<ItemContainer> items, int indexItemFrom, int indexItemTo)
    {
        if (indexItemTo < items.Count )
        {
            ItemContainer temp = items[indexItemFrom];
            items[indexItemFrom] = items[indexItemTo];
            items[indexItemTo] = temp;
        }
        else
        {
            items.Add(items[indexItemFrom]);
            items[indexItemFrom] = null;
        }
   
    }

    public static int MergeStackables(ItemContainer dest, ItemContainer src)
    {
        int newAmount = Mathf.Clamp(dest.Quantity + src.Quantity, 0, maxItemsInSameSlot);
        int usedAmount = newAmount - dest.Quantity;
        dest.Quantity = newAmount;
        src.Quantity -= usedAmount;

        if (src.Quantity > 0)
        {
            return src.Quantity;
        }


        return 0;
    }

    public static int FindFreeSlot(List<ItemContainer> items, int nbSlot)
    {
        int freeIndex = -1;
        for (int i = 0; i < nbSlot; i++)
        {
            if (i < items.Count)
            {
                if (items[i] == null)
                {
                    freeIndex = i;
                    break;
                }
            }

            freeIndex = i;
            break;
        }
        return freeIndex;
    }

    public static bool CheckIfItemTypeIsInInventory(List<ItemContainer> items, ItemContainer i) //Check if an item with the same ID already exists in the inventory
    {
        //return Array.Exists<ItemContainer>(items, x =>
        //{
        //    if (x != null)
        //    {
        //        return x.Item.Id == i.Item.Id;
        //    }

        //    return false;
        //});


        return items.Exists(x =>
        {
            if (x != null)
            {
                return x.Item.Id == i.Item.Id;
            }

            return false;
        });
    }

    public static int GetInventoryItemIndex(List<ItemContainer> items, ItemContainer i)
    {
        //    return Array.FindIndex<ItemContainer>(items, x => {
        //        if (x != null)
        //        {
        //            return x == i;
        //        }
        //        return false;
        //    });
        //}

        return items.FindIndex(x =>
        {
            if (x != null)
            {
                return x == i;
            }

            return false;
        });
    }
}

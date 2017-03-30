using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager {
    private const int maxItemsInSameSlot = 99;

    public static bool AddItemToInventory(ItemContainer[] _inventory,  ItemContainer _ic)
    {
        if (_ic.Item.IsStackable)
        {
            if (CheckIfItemTypeIsInInventory(_inventory, _ic))
            {
                for (int i = 0; i < _inventory.Length; i++)
                {
                    if (_inventory[i] != null && _inventory[i].Item.Id == _ic.Item.Id)
                    {
                        int quantityLeft = MergeStackables(_inventory[i], _ic);
                        if (quantityLeft > 0)
                        {
                            return AddItem(_inventory, _ic);
                        }
                    }

                }
            } else
            {
                return AddItem(_inventory, _ic);
            }
        }
        else
        {
            return AddItem(_inventory, _ic);
        }
        return true;
    }

    public static bool AddItem(ItemContainer[] _inventory,  ItemContainer _ic)
    {
        int freeIndex = FindFreeSlot(_inventory);
        if (freeIndex == -1)
        {
            return false;

        }
        _inventory[freeIndex] = _ic;

        return true;
    }

    public static bool RemoveItem(ItemContainer[] _inventory, ItemContainer _ic)
    {
        int index = GetInventoryItemIndex(_inventory, _ic);
        if (index == -1)
        {
            return false;
        }
        _inventory[index] = null;
        return true;
    }

    public static void SwapItemBeetweenInventories(ItemContainer[] itemsFrom, int indexItemFrom, ItemContainer[] itemsTo, int indexItemTo)
    {
        if (indexItemTo < itemsTo.Length)
        {
            ItemContainer temp = itemsFrom[indexItemFrom];
            itemsFrom[indexItemFrom] = itemsTo[indexItemTo];
            itemsTo[indexItemTo] = temp;
        }
    }

    public static void SwapItemInSameInventory(ItemContainer[] items, int indexItemFrom, int indexItemTo)
    {
        if (indexItemTo < items.Length )
        {
            ItemContainer temp = items[indexItemFrom];
            items[indexItemFrom] = items[indexItemTo];
            items[indexItemTo] = temp;
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

    public static int FindFreeSlot(ItemContainer[] items)
    {
        int freeIndex = -1;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                freeIndex = i;
                break;
            }
        }
        return freeIndex;
    }

    public static bool CheckIfItemTypeIsInInventory(ItemContainer[] items, ItemContainer i) //Check if an item with the same ID already exists in the inventory
    {
        return Array.Exists<ItemContainer>(items, x =>
        {
            if (x != null)
            {
                return x.Item.Id == i.Item.Id;
            }

            return false;
        });
    }

    public static int GetInventoryItemIndex(ItemContainer[] items, ItemContainer i)
    {
        return Array.FindIndex<ItemContainer>(items, x =>
        {
            if (x != null)
            {
                return x == i;
            }
            return false;
        });
     }
}

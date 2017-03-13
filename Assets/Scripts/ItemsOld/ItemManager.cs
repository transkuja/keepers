using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class ItemManager {

    private const int maxItemsInSameSlot = 99;

    public static void EquipItem(ItemContainer[] inventory, ItemContainer[] equipements, ItemContainer equipment)
    {
        if (!CheckIfItemIsInInventory(equipements, equipment))
        {
            Debug.Log("Can't find Item to equip in Inventory");
            return;
        }

        int index = GetInventoryItemIndex(equipements, equipment);

        // swap
        if (equipements[(int)((Equipment)equipment.Item).Constraint] != null)
        {
            ItemContainer temp = equipements[(int)((Equipment)equipment.Item).Constraint];
            equipements[(int)((Equipment)equipment.Item).Constraint] = equipment;
            inventory[index] = temp;
        }
        else
        {
            equipements[(int)((Equipment)equipment.Item).Constraint] = equipment;
            inventory[index] = null;
        }
        //PlayerUIController.UpdateEveryPanel();
        //StatsChanged.Invoke();
    }

    public static void AddItemOnTheGround(GameObject owner, ItemContainer itemInstance)
    {
        GameObject drop = GameObject.Instantiate(GameManager.Instance.prefabItemToDrop) as GameObject;


        drop.transform.SetParent(TileManager.Instance.GetTileFromKeeper[owner.GetComponent<KeeperInstance>()].transform);
        drop.transform.position = owner.transform.localPosition;

        if (drop.GetComponent<ItemContainer>() != null && drop.GetComponent<ItemContainer>().Item != null)
        {
            drop.GetComponent<ItemContainer>().Item = itemInstance.Item;
            drop.GetComponent<ItemContainer>().Quantity = itemInstance.Quantity;
        }

    }

    public static bool UnequipItem(ItemContainer[] items, EquipmentSlot equipSlot)
    {
        int index = FindFreeInventorySlot(items);
        if (index == -1)
        {
            return false;
        }

        items[index] = items[(int)equipSlot];
        items[(int)equipSlot] = null;

        return true;
    }

    public static bool MergeStackables(ItemContainer dest, ItemContainer src) //return true if there are still remaining items in the source
    {
        if (!dest.Item.IsStackable || !src.Item.IsStackable)
            return false;
        
        int newAmount = Mathf.Clamp(dest.Quantity + src.Quantity, 0, maxItemsInSameSlot);
        int usedAmount = newAmount - dest.Quantity;
        dest.Quantity = newAmount;
        src.Quantity -= usedAmount;
        if (src.Quantity <= 0)
            return false;
        return true;
    }

    public static int MergeStackables2(ItemContainer dest, ItemContainer src)
    {
        if (!dest.Item.IsStackable || !src.Item.IsStackable)
            return -1;

        int newAmount = Mathf.Clamp(dest.Quantity + src.Quantity, 0, maxItemsInSameSlot);
        int usedAmount = newAmount - dest.Quantity;
        dest.Quantity = newAmount;
        src.Quantity -= usedAmount;
        if (src.Quantity > 0)
            return src.Quantity;
        return 0;
    }

    public static void MoveItemToSlot(ItemContainer[] items, ItemContainer ic, int slot)
    {
        if (slot >= items.Length || slot < 0 || ic == null || !CheckIfItemIsInInventory(items, ic))
        {
            return;
        }
        int startIndex = GetInventoryItemIndex(items, ic);
        if (startIndex != slot)
        {
            if (items[slot] != null && items[slot].Item != null)
            {
                ItemContainer temp = items[startIndex];
                items[startIndex] = items[slot];
                items[slot] = temp;
            }
            else
            {
                items[slot] = items[startIndex];
                items[startIndex] = null;
            }
        }
    }

    public static void SwapItemBeetweenInventories (ItemContainer[] itemsFrom, int indexItemFrom, ItemContainer[] itemsTo, int indexItemTo)
    {
        ItemContainer temp = itemsFrom[indexItemFrom];
        itemsFrom[indexItemFrom] = itemsTo[indexItemTo];
        itemsTo[indexItemTo] = temp;
    }

    public static void SwapItemInSameInventory(ItemContainer[] items, int indexItemFrom, int indexItemTo)
    {
        ItemContainer temp = items[indexItemFrom];
        items[indexItemFrom] = items[indexItemTo];
        items[indexItemTo] = temp;
    }

    public static void TransferItemBetweenPanelsAtSlot(ItemContainer[] dest, ItemContainer[] src, int slotDest, int slotSrc)
    {
        if (dest[slotDest] != null)
        {
            ItemContainer temp = dest[slotDest];
            dest[slotDest] = src[slotSrc];
            src[slotSrc] = temp;
        }
        else
        {
            dest[slotDest] = src[slotSrc];
            src[slotSrc] = null;
        }
        //PlayerUIController.UpdateEveryPanel();
    }

    public static bool AddItem(ItemContainer[] items, ItemContainer item, bool stack = true)
    {
        bool add = true;
        if (stack)
        {
            if (item.GetType() == typeof(Ressource) && CheckIfItemTypeIsInInventory(items, item) )
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null && items[i].Item.Id == item.Item.Id)
                    {
                        add = MergeStackables(items[i], item);
                        if (!add)
                            break;
                    }
               
                }
            }
        }
        if (add)
        {
            int freeIndex = FindFreeInventorySlot(items);
            if (freeIndex != -1)
            {
                items[freeIndex] = item;
            }
            else
            {
                Debug.Log("No free slot");
                return false;
            }

        }
        return true;
    }

    public static int FindFreeInventorySlot(ItemContainer[] items)
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
                return x.Item.GetType() == i.Item.GetType();
            return false;
        });
    }

    public static bool CheckIfItemIsInInventory(ItemContainer[] items, ItemContainer i) //Check if the item itself is in the inventory
    {
        return Array.Exists<ItemContainer>(items, x =>
        {
            if (x!= null) {
                return x.Item == i.Item;
            }

            return false;
        });
    }


    public static int GetInventoryItemIndex(ItemContainer[] items, ItemContainer i)
    {
        return Array.FindIndex<ItemContainer>(items, x =>{
            if (x != null)
                return x.Item == i.Item;
            return false;
        });
    }

    //public int GetInventoryFirstItemTypeIndex(ItemContainer ic)
    //{
    //    ItemContainer[] inventory = GetComponent<KeeperInstance>().Inventory;
    //    return Array.FindIndex<ItemContainer>(inventory, x => x.item.nom == ic.item.nom);
    //}

    public static void RemoveItem(ItemContainer[] items, ItemContainer ic)
    {
        int index = GetInventoryItemIndex(items, ic);
        if (index == -1)
        {
            Debug.Log("Item does not exist in inventory");
            return;
        }
        items[index] = null;
    }
}

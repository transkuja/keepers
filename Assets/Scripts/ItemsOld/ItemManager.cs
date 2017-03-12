using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class ItemManager {

    private const int maxItemsInSameSlot = 99;

    public static void EquipItem(ItemInstance[] inventory, ItemInstance[] equipements, ItemInstance equipment)
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
            ItemInstance temp = equipements[(int)((Equipment)equipment.Item).Constraint];
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

    public static void AddItemOnTheGround(GameObject owner, ItemInstance itemInstance)
    {
        GameObject drop = GameObject.Instantiate(GameManager.Instance.prefabItemToDrop) as GameObject;


        drop.transform.SetParent(TileManager.Instance.GetTileFromKeeper[owner.GetComponent<KeeperInstance>()].transform);
        drop.transform.position = owner.transform.localPosition;

        if (drop.GetComponent<ItemInstance>() != null && drop.GetComponent<ItemInstance>().Item != null)
        {
            drop.GetComponent<ItemInstance>().Item = itemInstance.Item;
            drop.GetComponent<ItemInstance>().Quantity = itemInstance.Quantity;
        }

    }

    public static bool UnequipItem(ItemInstance[] items, EquipmentSlot equipSlot)
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

    public static bool MergeStackables(ItemInstance dest, ItemInstance src) //return true if there are still remaining items in the source
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

    public static int MergeStackables2(ItemInstance dest, ItemInstance src)
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

    public static void MoveItemToSlot(ItemInstance[] items, ItemInstance ic, int slot)
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
                ItemInstance temp = items[startIndex];
                items[startIndex] = items[slot];
                items[slot] = temp;
            }
            else
            {
                items[slot].Item = items[startIndex].Item;
                items[startIndex].Item = null;
            }
        }
    }

    public static void SwapItemBeetweenInventories (ItemInstance[] itemsFrom, int indexItemFrom, ItemInstance[] itemsTo, int indexItemTo)
    {
        ItemInstance temp = itemsFrom[indexItemFrom];
        itemsFrom[indexItemFrom] = itemsTo[indexItemTo];
        itemsTo[indexItemTo] = temp;
    }

    public static void SwapItemInSameInventory(ItemInstance[] items, int indexItemFrom, int indexItemTo)
    {
        ItemInstance temp = items[indexItemFrom];
        items[indexItemFrom] = items[indexItemTo];
        items[indexItemTo] = temp;
    }

    public static void TransferItemBetweenPanelsAtSlot(ItemInstance[] dest, ItemInstance[] src, int slotDest, int slotSrc)
    {
        if (dest[slotDest] != null)
        {
            ItemInstance temp = dest[slotDest];
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

    public static bool AddItem(ItemInstance[] items, ItemInstance item, bool stack = true)
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

    public static int FindFreeInventorySlot(ItemInstance[] items)
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

    public static bool CheckIfItemTypeIsInInventory(ItemInstance[] items, ItemInstance i) //Check if an item with the same ID already exists in the inventory
    {
        return Array.Exists<ItemInstance>(items, x =>
        {
            if (x.Item != null)
                return x.Item.GetType() == i.Item.GetType();
            return false;
        });
    }

    public static bool CheckIfItemIsInInventory(ItemInstance[] items, ItemInstance i) //Check if the item itself is in the inventory
    {
        return Array.Exists<ItemInstance>(items, x =>
        {
            if (x.Item != null)
            {
                Debug.Log(x.Item);
                Debug.Log(i.Item);
                Debug.Log(x.Item == i.Item);
                return x.Item == i.Item;
            }

            return false;
        });
    }


    public static int GetInventoryItemIndex(ItemInstance[] items, ItemInstance i)
    {
        return Array.FindIndex<ItemInstance>(items, x =>{
            if (x.Item != null)
                return x.Item == i.Item;
            return false;
        });
    }

    //public int GetInventoryFirstItemTypeIndex(ItemContainer ic)
    //{
    //    ItemContainer[] inventory = GetComponent<KeeperInstance>().Inventory;
    //    return Array.FindIndex<ItemContainer>(inventory, x => x.item.nom == ic.item.nom);
    //}

    public static void RemoveItem(ItemInstance[] items, ItemInstance ic)
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

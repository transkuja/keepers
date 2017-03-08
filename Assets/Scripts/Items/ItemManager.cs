using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public enum TypeItem { Equipement, Consummable };
public enum TypeEquipement { Arme, Def, Other };

public static class ItemManager {

    static Dictionary<TypeItem, System.Type> dicTypeEquipement = new Dictionary<TypeItem, System.Type>{
        { TypeItem.Equipement ,  typeof(Equipement)},
        { TypeItem.Consummable ,  typeof(Consummable)}
    };

    private const int maxItemsInSameSlot = 99;

    //public bool UseItem(Item item)
    //{
    //    if (item.GetType() == typeof(ItemEquipment))
    //    {
    //        EquipItem((ItemEquipment)item);
    //    }
    //    else if (item.GetType() == typeof(ItemConsumable))
    //    {
    //        ItemConsumable cons = (ItemConsumable)item;
    //        switch (cons.Action)
    //        {
    //            case ItemConsumable.UseAction.HEAL:
    //                Heal(cons.Value);
    //                break;
    //            case ItemConsumable.UseAction.DAMAGE:
    //                GetHit(cons.Value);
    //                break;
    //            case ItemConsumable.UseAction.ADD_MANA:
    //                AddMana(cons.Value);
    //                break;
    //            case ItemConsumable.UseAction.USE_MANA:
    //                UseMana(cons.Value);
    //                break;
    //        }
    //        bool destroy = false;
    //        if (item.Stackable)
    //        {
    //            item.StackedAmount--;
    //            if (item.StackedAmount <= 0)
    //                destroy = true;
    //        }
    //        else
    //        {
    //            destroy = true;
    //        }

    //        if (destroy)
    //        {
    //            if (CheckIfItemIsInInventory(item))
    //            {
    //                RemoveItem(item);
    //            }
    //            else if (CheckIfItemIsInActionBar(item))
    //            {
    //                RemoveActionBarItem(item);
    //            }
    //        }
    //        return destroy;
    //    }
    //    else if (item.GetType() == typeof(ItemMaterial))
    //    {
    //    }
    //    return false;
    //}

    public static void EquipItem(Item[] inventory, Item[] equipements, Equipement equipement)
    {
        if (!CheckIfItemIsInInventory(equipements, (Item)equipement))
        {
            Debug.Log("Can't find Item to equip in Inventory");
            return;
        }

        int index = GetInventoryItemIndex(equipements, equipement);

        // swap
        if (equipements[(int)equipement.type] != null)
        {
            Item temp = equipements[(int)equipement.type];
            equipements[(int)equipement.type] = equipement;
            inventory[index] = temp;
        }
        else
        {
            equipements[(int)equipement.type] = equipement;
            inventory[index] = null;
        }
        //PlayerUIController.UpdateEveryPanel();
        //StatsChanged.Invoke();
    }

    public static void AddItemOnTheGround(GameObject owner, Item item)
    {
        GameObject drop = GameObject.Instantiate(GameManager.Instance.prefabItemToDrop) as GameObject;


        drop.transform.SetParent(TileManager.Instance.GetTileFromKeeper[owner.GetComponent<KeeperInstance>()].transform);
        drop.transform.position = owner.transform.localPosition;
        if (item.GetType() == typeof(Equipement))
        {

            drop.GetComponent<ItemInstance>().typeItem = TypeItem.Equipement;
            drop.GetComponent<ItemInstance>().spriteToCopy = item.sprite;
            drop.GetComponent<ItemInstance>().Init();
        }
        else if (item.GetType() == typeof(Consummable))
        {
            drop.GetComponent<ItemInstance>().quantite = ((Consummable)item).quantite;
            drop.GetComponent<ItemInstance>().typeItem = TypeItem.Consummable;
            drop.GetComponent<ItemInstance>().spriteToCopy = item.sprite;
            drop.GetComponent<ItemInstance>().Init();
        }

    }

    public static bool UnequipItem(Item[] items, TypeEquipement equipSlot)
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

    public static bool MergeStackables(Consummable dest, Consummable src) //return true if there are still remaining items in the source
    {
        int newAmount = Mathf.Clamp(dest.quantite + src.quantite, 0, maxItemsInSameSlot);
        int usedAmount = newAmount - dest.quantite;
        dest.quantite = newAmount;
        src.quantite -= usedAmount;
        if (src.quantite <= 0)
            return false;
        return true;
    }

    public static int MergeStackables2(Consummable dest, Consummable src)
    {
        int newAmount = Mathf.Clamp(dest.quantite + src.quantite, 0, maxItemsInSameSlot);
        int usedAmount = newAmount - dest.quantite;
        dest.quantite = newAmount;
        src.quantite -= usedAmount;
        if (src.quantite > 0)
            return src.quantite;
        return 0;
    }

    public static void MoveItemToSlot(Item[] items, Item ic, int slot)
    {
        if (slot >= items.Length || slot < 0 || ic == null || !CheckIfItemIsInInventory(items, ic))
        {
            return;
        }

        int startIndex = GetInventoryItemIndex(items, ic);
        if (startIndex != slot)
        {
            if (items[slot] != null)
            {
                Item temp = items[startIndex];
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

    public static void SwapItemBeetweenInventories (Item[] itemsFrom, int indexItemFrom, Item[] itemsTo, int indexItemTo)
    {
        Item temp = itemsFrom[indexItemFrom];
        itemsFrom[indexItemFrom] = itemsTo[indexItemTo];
        itemsTo[indexItemTo] = temp;
    }

    public static void SwapItemInSameInventory(Item[] items, int indexItemFrom, int indexItemTo)
    {
        Item temp = items[indexItemFrom];
        items[indexItemFrom] = items[indexItemTo];
        items[indexItemTo] = temp;
    }

    public static void TransferItemBetweenPanelsAtSlot(Item[] dest, Item[] src, int slotDest, int slotSrc)
    {
        if (dest[slotDest] != null)
        {
            Item temp = dest[slotDest];
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

    public static bool AddItem(Item[] items, Item item, bool stack = true)
    {
        bool add = true;
        if (stack)
        {
            if (item.GetType() == typeof(Consummable) && CheckIfItemTypeIsInInventory(items, item) )
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null && items[i].sprite.name == item.sprite.name)
                    {
                        add = MergeStackables((Consummable)items[i], (Consummable)item);
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

    public static int FindFreeInventorySlot(Item[] items)
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

    public static bool CheckIfItemTypeIsInInventory(Item[] items, Item i) //Check if an item with the same ID already exists in the inventory
    {
        return Array.Exists<Item>(items, x =>
        {
            if (x != null)
                return x.GetType() == i.GetType();
            return false;
        });
    }

    public static bool CheckIfItemIsInInventory(Item[] items, Item i) //Check if the item itself is in the inventory
    {
        return Array.Exists<Item>(items, x =>
        {
            if (x != null)
                return x == i;
            return false;
        });
    }


    public static int GetInventoryItemIndex(Item[] items, Item i)
    {
        return Array.FindIndex<Item>(items, x =>{
            if (x != null)
                return x == i;
            return false;
        });
    }

    //public int GetInventoryFirstItemTypeIndex(ItemContainer ic)
    //{
    //    ItemContainer[] inventory = GetComponent<KeeperInstance>().Inventory;
    //    return Array.FindIndex<ItemContainer>(inventory, x => x.item.nom == ic.item.nom);
    //}

    public static void RemoveItem(Item[] items, Item ic)
    {
        int index = GetInventoryItemIndex(items, ic);
        if (index == -1)
        {
            Debug.Log("Item does not exist in inventory");
            return;
        }
        items[index] = null;
    }

    public static Item getInstanciateItem(TypeItem type)
    {
        System.Type value = dicTypeEquipement[type];
        // Les paramètres sont les paramètres du construction correspondant a la value ( qui est une classe ici sans paramètre) 
        ConstructorInfo cI = value.GetConstructor(new System.Type[0]);
        return (Item)cI.Invoke(new System.Type[0]);
    }
}

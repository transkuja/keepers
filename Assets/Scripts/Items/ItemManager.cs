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

    public static void EquipItem(KeeperInstance ki, Equipement equipement)
    {
        if (!CheckIfItemIsInInventory(ki ,(Item)equipement))
        {
            Debug.Log("Can't find Item to equip in Inventory");
            return;
        }

        int index = GetInventoryItemIndex(ki, equipement);

        Item[] inventory = ki.Inventory;
        Item[] equipements = ki.Equipment;

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

    public static bool UnequipItem(KeeperInstance ki, TypeEquipement equipSlot)
    {
        int index = FindFreeInventorySlot(ki);
        if (index == -1)
        {
            return false;
        }


        Item[] equipements = ki.Equipment;
        ki.Inventory[index] = equipements[(int)equipSlot];
        equipements[(int)equipSlot] = null;
        //PlayerUIController.InventoryNeedUpdate = true;
        //PlayerUIController.CharacterNeedUpdate = true;
        //StatsChanged.Invoke();

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

    public static void MoveItemToSlot(KeeperInstance ki, Item ic, int slot)
    {
        Item[] equipements = ki.Equipment;
        Item[] inventory = ki.Inventory;

        if (slot >= ki.Keeper.MaxInventorySlots || slot < 0 || ic == null || !CheckIfItemIsInInventory(ki, ic))
        {
            return;
        }

        int startIndex = GetInventoryItemIndex(ki, ic);
        if (startIndex != slot)
        {
            if (inventory[slot] != null)
            {
                Item temp = ki.Inventory[startIndex];
                inventory[startIndex] = inventory[slot];
                inventory[slot] = temp;
            }
            else
            {
                inventory[slot] = inventory[startIndex];
                inventory[startIndex] = null;
            }
        }

        /*if (InventoryChanged != null)
            InventoryChanged.Invoke();*/
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

    public static bool AddItem(KeeperInstance ki, Item item, bool stack = true)
    {
        Item[] inventory = ki.Inventory;

        bool add = true;
        if (stack)
        {
            if (item.GetType() == typeof(Consummable) && CheckIfItemTypeIsInInventory(ki, item) )
            {
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] != null && inventory[i].sprite.name == item.sprite.name)
                    {
                        add = MergeStackables((Consummable)inventory[i], (Consummable)item);
                        if (!add)
                            break;
                    }
               
                }
            }
        }
        if (add)
        {
            int freeIndex = FindFreeInventorySlot(ki);
            if (freeIndex != -1)
            {
                inventory[freeIndex] = item;
            }
            else
            {
                Debug.Log("No free slot");
                return false;
            }

        }
        return true;
    }

    public static int FindFreeInventorySlot(KeeperInstance ki)
    {
        Item[] inventory = ki.Inventory;
        int freeIndex = -1;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                freeIndex = i;
                break;
            }
        }

        return freeIndex;
    }

    public static bool CheckIfItemTypeIsInInventory(KeeperInstance ki, Item i) //Check if an item with the same ID already exists in the inventory
    {

        Item[] inventory = ki.Inventory;
        return Array.Exists<Item>(inventory, x =>
        {
            if (x != null)
                return x.GetType() == i.GetType();
            return false;
        });
    }

    public static bool CheckIfItemIsInInventory(KeeperInstance ki, Item i) //Check if the item itself is in the inventory
    {
        Item[] inventory = ki.Inventory;
        return Array.Exists<Item>(inventory, x =>
        {
            if (x != null)
                return x == i;
            return false;
        });
    }


    public static int GetInventoryItemIndex(KeeperInstance ki, Item i)
    {
        Item[] inventory = ki.Inventory;
        return Array.FindIndex<Item>(inventory, x =>{
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

    public static void RemoveItem(KeeperInstance ki, Item ic)
    {
        Item[] inventory = ki.Inventory;
        int index = GetInventoryItemIndex(ki, ic);
        if (index == -1)
        {
            Debug.Log("Item does not exist in inventory");
            return;
        }
        inventory[index] = null;
    }

    public static Item getInstanciateItem(TypeItem type)
    {
        System.Type value = dicTypeEquipement[type];
        // Les paramètres sont les paramètres du construction correspondant a la value ( qui est une classe ici sans paramètre) 
        ConstructorInfo cI = value.GetConstructor(new System.Type[0]);
        return (Item)cI.Invoke(new System.Type[0]);
    }
}

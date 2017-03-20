using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipementManager {
    public static void EquipItem(List<ItemContainer> inventory, ItemContainer[]  equipements, ItemContainer equipment)
    {
        int index = InventoryManager.GetInventoryItemIndex(inventory, equipment);

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
    }


    public static bool UnequipItem(List<ItemContainer> inventory, int nbSlot, ItemContainer[] equipements, EquipmentSlot equipSlot)
    {
        int index = InventoryManager.FindFreeSlot(inventory, nbSlot);
        if (index == -1)
        {
            return false;
        }

        if (index == inventory.Count)
        {
            inventory.Add(equipements[(int)equipSlot]);
            equipements[(int)equipSlot] = null;
        }
        else
        {
            inventory[index] = equipements[(int)equipSlot];
            equipements[(int)equipSlot] = null;
        }

        return true;
    }


    public static bool CheckIfItemTypeIsInEquipement(ItemContainer[] equipements, ItemContainer i) //Check if an item with the same constrainte is equiped
    {
        return Array.Exists(equipements , x =>
        {
            if (x != null)
            {
                return ((Equipment) x.Item).Constraint == ((Equipment)i.Item).Constraint;
            }

            return false;
        });
    }
}

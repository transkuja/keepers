using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipementManager {
    public static void EquipItem(ItemContainer[] inventory, ItemContainer[] equipements, ItemContainer equipment)
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


    public static bool UnequipItem(ItemContainer[] equipements, ItemContainer[] items, EquipmentSlot equipSlot)
    {
        int index = InventoryManager.FindFreeSlot(items);
        if (index == -1)
        {
            return false;
        }

        items[index] = items[(int)equipSlot];
        equipements[(int)equipSlot] = null;

        return true;
    }
}

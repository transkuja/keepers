using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class ItemManager {

    static Dictionary<string, System.Type> dicTypeEquipement = new Dictionary<string, System.Type>{
        { "Equipment" ,  typeof(Equipment)},
        { "Ressource" ,  typeof(Ressource)}
    };

    public static void AddItemOnTheGround(Tile tileWhere, Transform where, ItemContainer[] loot)
    {
        if (loot != null && loot.Length > 0)
        {
            GameObject dropedItem = GameObject.Instantiate(GameManager.Instance.PrefabUtils.prefabItemToDrop) as GameObject;
            Behaviour.Inventory dropInventory = dropedItem.GetComponent<Behaviour.Inventory>();
            dropedItem.transform.SetParent(tileWhere.transform);
            dropedItem.transform.position = where.position;

            int j = 0;
            for (int i = 0; i < loot.Length; i++)
            {
                if (loot[i] != null)
                {
                    j++;
                }
            }
            dropInventory.Data.NbSlot = j;
            dropInventory.InitUI();

            Array.Copy(loot, dropInventory.Items, dropInventory.Items.Length);
            dropInventory.UpdateInventories();
        }
    }
    public static Item getInstanciateItem(string type)
    {
        System.Type value = dicTypeEquipement[type];
        // Les paramètres sont les paramètres du construction correspondant a la value ( qui est une classe ici sans paramètre) 
        ConstructorInfo cI = value.GetConstructor(new System.Type[0]);
        return (Item)cI.Invoke(new System.Type[0]);
    }
}


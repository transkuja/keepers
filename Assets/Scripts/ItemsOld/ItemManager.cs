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
            GameObject drop = GameObject.Instantiate(GameManager.Instance.PrefabUtils.prefabItemToDrop) as GameObject;

            drop.transform.SetParent(tileWhere.transform);
            drop.transform.position = where.position;

            drop.GetComponent<LootInstance>().nbSlot = loot.Length;
            Inventory dropInventory = drop.GetComponent<Inventory>();
            dropInventory.Init(loot.Length);
            Array.Copy(loot, dropInventory.Items, dropInventory.Items.Length);
            drop.GetComponent<LootInstance>().Init();
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


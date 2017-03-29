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

    public static void AddItemOnTheGround(Tile tileWhere, Transform where, List<ItemContainer> loot)
    {
        if (loot != null && loot.Count > 0)
        {
            GameObject drop = GameObject.Instantiate(GameManager.Instance.PrefabUtils.prefabItemToDrop) as GameObject;

            drop.transform.SetParent(tileWhere.transform);
            drop.transform.position = where.position;

            drop.GetComponent<LootInstance>().nbSlot = loot.Count;
            drop.GetComponent<Inventory>().List_inventaire = loot;
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


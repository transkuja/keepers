using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class ItemManager {

    static Dictionary<string, System.Type> dicTypeEquipement = new Dictionary<string, System.Type>{
        { "Equipement" ,  typeof(Equipment)},
        { "Ressource" ,  typeof(Ressource)}
    };

    public static void AddItemOnTheGround(GameObject owner, ItemContainer itemContainer)
    {
        GameObject drop = GameObject.Instantiate(GameManager.Instance.prefabItemToDrop) as GameObject;


        drop.transform.SetParent(TileManager.Instance.GetTileFromKeeper[owner.GetComponent<KeeperInstance>()].transform);
        drop.transform.position = owner.transform.localPosition;

        if (itemContainer != null && itemContainer.Item != null)
        {
            drop.GetComponent<ItemInstance>().ItemContainer = itemContainer;
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


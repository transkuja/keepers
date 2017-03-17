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

    public static void AddItemOnTheGround(GameObject owner, List<ItemContainer> loot)
    {
        if (loot != null && loot.Count > 0)
        {
            // Deja fait par dragHandler ?
            //bool canRemove = InventoryManager.RemoveItem(GameManager.Instance.ListOfSelectedKeepers[0].GetComponent<Inventory>().inventory, itemContainer);

            GameObject drop = GameObject.Instantiate(GameManager.Instance.prefabItemToDrop) as GameObject;

            drop.transform.SetParent(TileManager.Instance.GetTileFromKeeper[owner.GetComponent<KeeperInstance>()].transform);
            drop.transform.position = owner.transform.localPosition;

            drop.GetComponent<Inventory>().List_inventaire = loot;
        }

        //GameManager.Instance.Ui.UpdateInventoryPanel(GameManager.Instance.ListOfSelectedKeepers[0].gameObject);
        //GameManager.Instance.Ui.UpdateSelectedKeeperPanel();

    }
    public static Item getInstanciateItem(string type)
    {
        System.Type value = dicTypeEquipement[type];
        // Les paramètres sont les paramètres du construction correspondant a la value ( qui est une classe ici sans paramètre) 
        ConstructorInfo cI = value.GetConstructor(new System.Type[0]);
        return (Item)cI.Invoke(new System.Type[0]);
    }
}


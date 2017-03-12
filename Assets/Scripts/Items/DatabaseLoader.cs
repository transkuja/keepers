using System;
using System.Collections.Generic;
using UnityEngine;
using Boomlagoon.JSON;

public class DatabaseLoader
{
    private class ItemElement
    {
        public string type;
        public Item item;
    }

    [SerializeField]
    private List<Item> itemsList;

    public List<Item> getItemsList()
    {
        return itemsList;
    }

    public bool Import(JSONObject source)
    {
        // Recuperation du tableau de items
        itemsList = new List<Item>();

        JSONArray array = source["items"].Array;

        foreach (JSONValue value in array)
        {
            ItemElement invItem = new ItemElement();
            foreach (KeyValuePair<string, JSONValue> itemEntry in value.Obj)
            {
                if (itemEntry.Key == "type")
                {
                    switch (itemEntry.Value.Str)
                    {
                        case "Ressource":
                            invItem.type = "Ressource";
                            break;
                        case "Equipment":
                            invItem.type = "Equipment";
                            break;
                        default:
                            invItem.type = "None";
                            break;
                    }
                }
                Item item;
                switch (invItem.type)
                {
                    case "Ressource":
                        item = new Ressource();
                        item.Type = "Ressource";
                        invItem.type = "Ressource";
                        break;
                    default:
                        item = new Equipment();
                        item.Type = "Equipment";
                        invItem.type = "Equipment";
                        break;
                }

                if (itemEntry.Key == "item")
                {
                    foreach (KeyValuePair<string, JSONValue> currentItem in itemEntry.Value.Obj)
                    {
                        if (currentItem.Key == "id") { item.Id = currentItem.Value.Str; }
                        if (currentItem.Key == "itemName") { item.ItemName = currentItem.Value.Str; }
                        if (currentItem.Key == "description") { item.Description = currentItem.Value.Str; }
                        if (currentItem.Key == "inventorySprite") { item.InventorySprite = GameManager.Instance.dictSprites[currentItem.Value.Str]; }
                        if (currentItem.Key == "ingameVisual") { } // TODO //item.IngameVisual = itemEntry.Value.Str; }

                        if (item.Type == "Equipment")
                        {
                            if (currentItem.Key == "slot")
                            {
                                if (!Enum.IsDefined(typeof(EquipmentSlot), currentItem.Value.Str))
                                    return false;
                                ((Equipment)item).Constraint = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), currentItem.Value.Str, true);
                            }

                            if (currentItem.Key == "stat")
                            {
                                if (!Enum.IsDefined(typeof(Stat), currentItem.Value.Str))
                                    return false;
                                ((Equipment)item).Stat = (Stat)Enum.Parse(typeof(Stat), currentItem.Value.Str, true);
                            }

                            if (currentItem.Key == "bonusStat") { ((Equipment)item).BonusToStat = (short)currentItem.Value.Number; }
                        }

                        if (item.Type == "Ressource")
                        {
                            if (currentItem.Key == "use")
                            {
                                if (!Enum.IsDefined(typeof(ResourceFunctions), currentItem.Value.Str))
                                    return false;
                                ((Ressource)item).ResourceUseIndex = (ResourceFunctions)Enum.Parse(typeof(ResourceFunctions), currentItem.Value.Str, true);
                            }

                            if (currentItem.Key == "value") { ((Ressource)item).Value = (int)currentItem.Value.Number; }
                        }
                    }
                }
                invItem.item = item;

            }
            itemsList.Add(invItem.item);
        }
        return true;
    }
}

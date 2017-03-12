using System;
using System.Collections.Generic;
using UnityEngine;
using Boomlagoon.JSON;

public class DatabaseLoader {
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

            foreach (KeyValuePair<string, JSONValue> itemEntry in value.Obj)
            {
                //Item item = null;

                //if (itemEntry.Key == "type")
                //{
                //    switch (itemEntry.Value.Str)
                //    {
                //        case "Ressource":
                //            item = new Ressource();
                //            item.Type = "Ressource";
                //            break;
                //        case "Equipment":
                //            item = new Equipment();
                //            item.Type = "Equipment";
                //            break;
                //        default:
                //            return false;
                //    }
                //}
                Item invItem = new Item();

                if (itemEntry.Key == "type")
                {
                    switch (itemEntry.Value.Str)
                    {
                        case "Ressource":
                            invItem.Type = "Ressource";
                            break;
                        case "Equipment":
                            invItem.Type = "Equipment";
                            break;
                        default:
                            invItem.Type = "None";
                            break;
                    }
                }
                Item item;
                switch (invItem.Type)
                {
                    case "Ressource":
                        item = new Ressource();
                        break;
                    default:
                        item = new Equipment();
                        break;
                }

                if (itemEntry.Key == "id") { item.Id = itemEntry.Value.Str; }
                if (itemEntry.Key == "itemName") { item.ItemName = itemEntry.Value.Str; }
                if (itemEntry.Key == "description") { item.Description = itemEntry.Value.Str; }
                if (itemEntry.Key == "inventorySprite") { item.InventorySprite = GameManager.Instance.dictSprites[itemEntry.Value.Str]; }
                if (itemEntry.Key == "ingameVisual") { } // TODO //item.IngameVisual = itemEntry.Value.Str; }

                if (item.Type == "Equipment")
                {
                    if (itemEntry.Key == "slot")
                    {
                        if (!Enum.IsDefined(typeof(EquipmentSlot), itemEntry.Value.Str))
                            return false;
                        ((Equipment)item).Constraint = (EquipmentSlot)Enum.Parse(typeof(EquipmentSlot), itemEntry.Value.Str, true);
                    }

                    if (itemEntry.Key == "stat")
                    {
                        if (!Enum.IsDefined(typeof(Stat), itemEntry.Value.Str))
                            return false;
                        ((Equipment)item).Stat = (Stat)Enum.Parse(typeof(Stat), itemEntry.Value.Str, true);
                    }

                    if (itemEntry.Key == "bonusStat") { ((Equipment)item).BonusToStat = (short)itemEntry.Value.Number; }
                }

                if (item.Type == "Ressource")
                {
                    if (itemEntry.Key == "use")
                    {
                        if (!Enum.IsDefined(typeof(ResourceFunctions), itemEntry.Value.Str))
                            return false;
                        ((Ressource)item).ResourceUseIndex = (ResourceFunctions)Enum.Parse(typeof(ResourceFunctions), itemEntry.Value.Str, true);
                    }

                    if (itemEntry.Key == "use") { ((Ressource)item).Value = (int)itemEntry.Value.Number; }

                }

                itemsList.Add(item);
            }
        }
        return true;
    }
}

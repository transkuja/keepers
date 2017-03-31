using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Boomlagoon.JSON;

public class PawnDatabase {

    public class PawnDataContainer
    {
        public PawnData pawnData;
        public Dictionary<string, ComponentData> dicComponentData;

        public PawnDataContainer()
        {
            pawnData = new PawnData();
            dicComponentData = new Dictionary<string, ComponentData>();
        }
    }

    Dictionary<string, PawnDataContainer> dicPawnDataContainer;

    public PawnDatabase()
    {
        dicPawnDataContainer = new Dictionary<string, PawnDataContainer>();
    }

    public void Init()
    {
        string pathBase = Application.dataPath + "/../Data";

        string fileContent = File.ReadAllText(pathBase + "/pawns.json");
        JSONObject json = JSONObject.Parse(fileContent);

        JSONArray pawnArray = json["Pawns"].Array;

        foreach(JSONValue value in pawnArray)
        {
            PawnDataContainer newPawnDataContainer = new PawnDataContainer();
            foreach(KeyValuePair<string, JSONValue> pawnEntry in value.Obj)
            {
                switch (pawnEntry.Key)
                {
                    // ROOT DATA
                    case "id":
                        newPawnDataContainer.pawnData.PawnId = pawnEntry.Value.Str;
                        break;
                    case "name":
                        newPawnDataContainer.pawnData.PawnName = pawnEntry.Value.Str;
                        break;
                    case "sprite":
                        newPawnDataContainer.pawnData.AssociatedSprite = Resources.Load(pawnEntry.Value.Str) as Sprite;
                        break;
                    // COMPONENTS DATA
                    case "AnimatedPawn":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.AnimatedPawn).ToString(), null);
                        break;
                    case "Escortable":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Escortable).ToString(), null);
                        break;
                    case "Fighter":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Fighter).ToString(), null);
                        break;
                    case "HungerHandler":
                        Behaviour.HungerHandler.HungerHandlerData newHungerHandlerData = new Behaviour.HungerHandler.HungerHandlerData();
                        foreach (KeyValuePair<string, JSONValue> hungerHandlerlEntry in pawnEntry.Value.Obj)
                        {
                            switch (hungerHandlerlEntry.Key)
                            {
                                case "maxHunger":
                                    newHungerHandlerData.MaxHunger = (int)hungerHandlerlEntry.Value.Number;
                                    break;
                            }
                        }
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.HungerHandler).ToString(), newHungerHandlerData);
                        break;
                    case "Inventory":
                        Behaviour.Inventory.InventoryData newInventoryData = new Behaviour.Inventory.InventoryData();
                        foreach (KeyValuePair<string, JSONValue> inventoryEntry in pawnEntry.Value.Obj)
                        {
                            switch (inventoryEntry.Key)
                            {
                                case "nbSlot":
                                    newInventoryData.NbSlot = (int)inventoryEntry.Value.Number;
                                    break;
                            }
                        }
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Inventory).ToString(), newInventoryData);
                        break;
                    case "Keeper":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Keeper).ToString(), null);
                        break;
                    case "MentalHealthHandler":
                        Behaviour.MentalHealthHandler.MentalHealthHandlerData newMentalHealthHandlerData = new Behaviour.MentalHealthHandler.MentalHealthHandlerData();
                        foreach (KeyValuePair<string, JSONValue> mentalHealthHandlerEntry in pawnEntry.Value.Obj)
                        {
                            switch (mentalHealthHandlerEntry.Key)
                            {
                                case "maxMentalHealth":
                                    newMentalHealthHandlerData.MaxMentalHealth = (int)mentalHealthHandlerEntry.Value.Number;
                                    break;
                            }
                        }
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Inventory).ToString(), newMentalHealthHandlerData);
                        break;
                    case "Monster":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Monster).ToString(), null);
                        break;
                    case "Mortal":
                        Behaviour.Mortal.MortalData newMortalData = new Behaviour.Mortal.MortalData();
                        foreach(KeyValuePair<string, JSONValue> mortalEntry in pawnEntry.Value.Obj)
                        {
                            switch (mortalEntry.Key)
                            {
                                case "maxHp":
                                    newMortalData.MaxHp = (int)mortalEntry.Value.Number;
                                    break;
                            }
                        }
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Mortal).ToString(), newMortalData);
                        break;
                    case "PathBlocker":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.PathBlocker).ToString(), null);
                        break;
                    case "Prisoner":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Prisoner).ToString(), null);
                        break;
                    case "QuestDealer":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.QuestDealer).ToString(), null);
                        break;
                }
            }
            dicPawnDataContainer.Add(newPawnDataContainer.pawnData.PawnId, newPawnDataContainer);
        }
    }

}

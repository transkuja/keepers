using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Boomlagoon.JSON;


public class PawnDataContainer
{
    public PawnData pawnData;
    public Dictionary<Type, ComponentData> dicComponentData;

    public PawnDataContainer()
    {
        pawnData = new PawnData();
        dicComponentData = new Dictionary<Type, ComponentData>();
    }
}

public class PawnDatabase {

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
                        newPawnDataContainer.pawnData.AssociatedSprite = Resources.Load<Sprite>(pawnEntry.Value.Str) as Sprite;
                        break;
                    case "canSpeak":
                        switch (pawnEntry.Value.Str)
                        {
                            case "true":
                                newPawnDataContainer.pawnData.Behaviours[(int)BehavioursEnum.CanSpeak] = true;
                                    break;
                            case "false":
                                newPawnDataContainer.pawnData.Behaviours[(int)BehavioursEnum.CanSpeak] = false;
                                break;
                        }
                        break;
                    // COMPONENTS DATA
                    case "Fighter":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Fighter), null);
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
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.HungerHandler), newHungerHandlerData);
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
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Inventory), newInventoryData);
                        break;
                    case "Keeper":
                        Behaviour.Keeper.KeeperData newKeeperData = new Behaviour.Keeper.KeeperData();
                        foreach (KeyValuePair<string, JSONValue> keeperEntry in pawnEntry.Value.Obj)
                        {
                            switch (keeperEntry.Key)
                            {
                                case "minMoralBuff":
                                    newKeeperData.MinMoralBuff = (int)keeperEntry.Value.Number;
                                    break;
                                case "maxMoralBuff":
                                    newKeeperData.MaxMoralBuff = (int)keeperEntry.Value.Number;
                                    break;
                                case "maxActionPoint":
                                    newKeeperData.MaxActionPoint = (int)keeperEntry.Value.Number;
                                    break;
                            }
                        }
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Keeper), newKeeperData);
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
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.MentalHealthHandler), newMentalHealthHandlerData);
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
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Mortal), newMortalData);
                        break;
                }
            }
            dicPawnDataContainer.Add(newPawnDataContainer.pawnData.PawnId, newPawnDataContainer);
        }
    }

    public GameObject CreatePawn(string idPawn, Vector3 v3Position, Quaternion quatRotation, Transform trParent)
    {
        GameObject goPawn = GameObject.Instantiate(GameManager.Instance.PrefabUtils.getPawnPrefabById(idPawn), v3Position, quatRotation);
        if (goPawn == null) { Debug.Log("Couldn't find the corresponding prefab in prefabUtils"); return null; }
        goPawn.transform.SetParent(trParent, false);
        goPawn.GetComponent<PawnInstance>().Data = dicPawnDataContainer[idPawn].pawnData;

        InitPawn(goPawn.GetComponent<PawnInstance>());
        return goPawn;
    }

    public void InitPawn(PawnInstance pi)
    {
        GameObject goPawn = pi.gameObject;
        string idPawn = pi.Data.PawnId;

        foreach(KeyValuePair<Type, ComponentData> pdc in dicPawnDataContainer[idPawn].dicComponentData)
        {
            goPawn.AddComponent(pdc.Key);

            switch (pdc.Key.ToString())
            {
                case "Behaviour.Fighter":
                    break;
                case "Behaviour.HungerHandler":
                    goPawn.GetComponent<Behaviour.HungerHandler>().Data = (Behaviour.HungerHandler.HungerHandlerData)pdc.Value;
                    break;
                case "Behaviour.Inventory":
                    goPawn.GetComponent<Behaviour.Inventory>().Data = (Behaviour.Inventory.InventoryData)pdc.Value;
                    break;
                case "Behaviour.Keeper":
                    goPawn.GetComponent<Behaviour.Keeper>().Data = (Behaviour.Keeper.KeeperData)pdc.Value;
                    break;
                case "Behaviour.MentalHealthHandler":
                    goPawn.GetComponent<Behaviour.MentalHealthHandler>().Data = (Behaviour.MentalHealthHandler.MentalHealthHandlerData)pdc.Value;
                    break;
                case "Behaviour.Mortal":
                    goPawn.GetComponent<Behaviour.Mortal>().Data = (Behaviour.Mortal.MortalData)pdc.Value;
                    break;
            }
        }
    }

    #region Accessors
    public Dictionary<string, PawnDataContainer> DicPawnDataContainer
    {
        get
        {
            return dicPawnDataContainer;
        }

        set
        {
            dicPawnDataContainer = value;
        }
    }
    #endregion
}

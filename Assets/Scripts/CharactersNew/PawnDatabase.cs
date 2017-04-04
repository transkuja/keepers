using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Boomlagoon.JSON;

public class PawnDatabase {

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

    [SerializeField] private GameObject goPawnBase;

    RuntimeAnimatorController pawnAnimationController;

    Dictionary<string, PawnDataContainer> dicPawnDataContainer;

    public PawnDatabase()
    {
        dicPawnDataContainer = new Dictionary<string, PawnDataContainer>();
    }

    public void Init()
    {
        string pathBase = Application.dataPath + "/../Data";

        goPawnBase = Resources.Load("Prefabs/Pawns/PawnBase") as GameObject;

        pawnAnimationController = Resources.Load("Animations/PawnAnimationController") as RuntimeAnimatorController;

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
                    case "inGameVisual":
                        newPawnDataContainer.pawnData.goInGameVisual = Resources.Load(pawnEntry.Value.Str) as GameObject;
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
                    case "AnimatedPawn":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.AnimatedPawn), null);
                        break;
                    case "Escortable":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Escortable), null);
                        break;
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
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Keeper), null);
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
                    case "Monster":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Monster), null);
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
                    case "PathBlocker":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.PathBlocker), null);
                        break;
                    case "Prisoner":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.Prisoner), null);
                        break;
                    case "QuestDealer":
                        newPawnDataContainer.dicComponentData.Add(typeof(Behaviour.QuestDealer), null);
                        break;
                }
            }
            dicPawnDataContainer.Add(newPawnDataContainer.pawnData.PawnId, newPawnDataContainer);
        }
    }

    public GameObject CreatePawn(string idPawn, Vector3 v3Position, Quaternion quatRotation ,Transform trParent)
    {
        GameObject goNew = GameObject.Instantiate(goPawnBase, v3Position, quatRotation, trParent);

        goNew.GetComponent<PawnInstance>().Data = dicPawnDataContainer[idPawn].pawnData;

        Animator anim = GameObject.Instantiate(dicPawnDataContainer[idPawn].pawnData.goInGameVisual, goNew.transform).GetComponent<Animator>();
        anim.runtimeAnimatorController = pawnAnimationController;

        foreach(KeyValuePair<Type, ComponentData> pdc in dicPawnDataContainer[idPawn].dicComponentData)
        {
            goNew.AddComponent(pdc.Key);

            switch (pdc.Key.ToString())
            {
                case "Behaviour.AnimatedPawn":
                    break;
                case "Behaviour.Escortable":
                    break;
                case "Behaviour.Fighter":
                    break;
                case "Behaviour.HungerHandler":
                    goNew.GetComponent<Behaviour.HungerHandler>().Data = (Behaviour.HungerHandler.HungerHandlerData)pdc.Value;
                    break;
                case "Behaviour.Inventory":
                    goNew.GetComponent<Behaviour.Inventory>().Data = (Behaviour.Inventory.InventoryData)pdc.Value;
                    break;
                case "Behaviour.Keeper":
                    break;
                case "Behaviour.MentalHealthHandler":
                    goNew.GetComponent<Behaviour.MentalHealthHandler>().Data = (Behaviour.MentalHealthHandler.MentalHealthHandlerData)pdc.Value;
                    break;
                case "Behaviour.Monster":
                    break;
                case "Behaviour.Mortal":
                    goNew.GetComponent<Behaviour.Mortal>().Data = (Behaviour.Mortal.MortalData)pdc.Value;
                    break;
                case "Behaviour.PathBlocker":
                    break;
                case "Behaviour.Prisoner":
                    break;
                case "Behaviour.QuestDealer":
                    break;
            }
        }
        return goNew;
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

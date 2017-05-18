using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using QuestSystem;
using Behaviour;

public class CharactersInitializer : MonoBehaviour {

    // Init keepers and call next initialization
    public void InitKeepers(Transform[] beginPositionsKeepers)
    {
        GameManager.Instance.AllKeepersList.Clear();

        for (int i = 0; i < GameManager.Instance.AllKeeperListId.Count; i++)
        {
            string curKeeperStr = GameManager.Instance.AllKeeperListId[i];
            GameObject curKeeper = GameManager.Instance.PawnDataBase.CreatePawn(curKeeperStr, beginPositionsKeepers[i].position, Quaternion.identity, null);
            curKeeper.transform.localScale = Vector3.one;
            curKeeper.transform.GetComponent<NavMeshAgent>().enabled = true;
            PawnInstance curKeeperPI = curKeeper.transform.GetComponent<PawnInstance>();
            if (curKeeperPI.Data.Behaviours[(int)BehavioursEnum.Archer] == true)
            {
                GameManager.Instance.ArcherInstance = curKeeperPI;
            }

            GameManager.Instance.AllKeepersList.Add(curKeeperPI);
            InitCharacterUI(curKeeperPI);
            GameManager.Instance.RegisterKeeperPosition(curKeeperPI);
            GlowController.RegisterObject(curKeeperPI.GetComponent<GlowObjectCmd>());
        }

        // Next step, init quests
        // TODO: init quests

        // Next step, init NPCs
        // TODO: init quests and call this properly
        if (TutoManager.s_instance != null)
            TutoManager.s_instance.InitDataTuto();
        InitNPCs();
        if (TutoManager.s_instance != null)
            TutoManager.s_instance.InitTuto();

        EventManager.HandleWeather();
    }

    private void InitNPCs()
    {
        foreach (Quest quest in GameManager.Instance.QuestManager.Quests)
        {
            if (quest.Identifier.SourceID != string.Empty)
            {
                if (GameManager.Instance.QuestSources == null)
                {
                    break;
                }
                QuestSource source = GameManager.Instance.QuestSources.FindSourceByID(quest.Identifier.SourceID);
                if (source == null)
                {
                    Debug.Log("Can't spawn NPC \"" + quest.Identifier.SourceID + "\". No QuestSource with this ID found in the scene.");
                }
                else
                {
                    if (source.needsToBeSpawned)
                    {
                        GameObject spawnedPawn = GameManager.Instance.PawnDataBase.CreatePawn(source.ID, source.Transform.position, source.Transform.rotation, null);
                        // TODO: NPCs must be registered in Tile Manager and ComputeItems called in character initializer
                        if (spawnedPawn.GetComponent<PawnInstance>() != null && spawnedPawn.GetComponent<Keeper>() == null)
                        {
                            if (spawnedPawn.GetComponent<Inventory>() != null && spawnedPawn.GetComponent<Inventory>().PossibleItems != null && spawnedPawn.GetComponent<Inventory>().PossibleItems.Count > 0)
                                spawnedPawn.GetComponent<Inventory>().ComputeItems();
                        }
                        spawnedPawn.transform.SetParent(source.Tile.transform);
                        spawnedPawn.transform.SetAsLastSibling();
                        spawnedPawn.GetComponent<PawnInstance>().CurrentTile = source.Tile;
                        spawnedPawn.GetComponent<Behaviour.QuestDealer>().QuestToGive = quest;
                        spawnedPawn.GetComponent<Behaviour.QuestDealer>().Init();
                        InitCharacterUI(spawnedPawn.GetComponent<PawnInstance>());
                        if (source.Tile.State != TileState.Discovered)
                        {
                            spawnedPawn.SetActive(false);
                        }

                    }
                    else
                    {
                        source.Transform.GetComponent<Behaviour.QuestDealer>().QuestToGive = quest;
                        source.Transform.GetComponent<Behaviour.QuestDealer>().Init();
                    }
                }
            }
        }

        foreach (Tile t in TileManager.Instance.Tiles.GetComponentsInChildren<Tile>())
        {
            if (GetComponentsInChildren<Trader>() != null)
            {
                for (int i = 0; i < t.transform.childCount; i++)
                {
                    if (t.transform.GetChild(i).GetComponent<Trader>() != null)
                    {
                        Trader ta = t.transform.GetChild(i).GetComponent<Trader>();
                        if (ta.GetComponent<Inventory>() != null && ta.GetComponent<Inventory>().PossibleItems != null && ta.GetComponent<Inventory>().PossibleItems.Count > 0)
                            ta.GetComponent<Inventory>().ComputeItems();
             
                        ta.GetComponent<PawnInstance>().CurrentTile = t;
                        InitCharacterUI(ta.GetComponent<PawnInstance>());
                    }
                }
            }

            if (GetComponentsInChildren<HintWoman>() != null)
            {
                for (int i = 0; i < t.transform.childCount; i++)
                {
                    if (t.transform.GetChild(i).GetComponent<HintWoman>() != null)
                    {
                        HintWoman ta = t.transform.GetChild(i).GetComponent<HintWoman>();
                        ta.GetComponent<PawnInstance>().CurrentTile = t;
                    }
                }
            }
        }

        // TODO this should not be handled like, especially if there is more prisoner in scene
        GameObject prisoner = GameManager.Instance.PawnDataBase.CreatePawn("ashley", TileManager.Instance.BeginTile.transform.position, Quaternion.identity, null);
        GlowController.RegisterObject(prisoner.GetComponent<GlowObjectCmd>()); // TODO: Inutile maintenant ? 

        InitCharacterUI(prisoner.GetComponent<PawnInstance>());
        GameManager.Instance.PrisonerInstance = prisoner.GetComponent<PawnInstance>();
        if(GameManager.Instance.QuestManager.CurrentQuestDeck != null)
        {
            switch (GameManager.Instance.QuestManager.CurrentQuestDeck.LevelId)
            {
                case "tuto":
                    GameManager.Instance.QuestSources.GetComponent<QuestInitializer_Level1>().InitializeQuests();
                    break;
                case "level1":
                    GameManager.Instance.QuestSources.GetComponent<QuestInitializer_Level1>().InitializeQuests();
                    break;
                case "level2":
                    GameManager.Instance.QuestSources.GetComponent<QuestInitializer_Level2>().InitializeQuests();
                    break;
                case "level3":
                    GameManager.Instance.QuestSources.GetComponent<QuestInitializer_Level3>().InitializeQuests();
                    break;
                case "level4":
                    GameManager.Instance.QuestSources.GetComponent<QuestInitializer_Level4>().InitializeQuests();
                    break;
                default:
                    break;
            }
            //GameManager.Instance.QuestSources.GetComponent<QuestInitializer>().InitializeQuests();
            GameManager.Instance.QuestManager.MainQuest.OnQuestComplete += EndGameQuest;
        }
        else
        {
            Debug.Log("No deck loaded");
        } 
    }

    public void EndGameQuest()
    {
        GameManager.Instance.QuestManager.MainQuest.OnQuestComplete -= EndGameQuest;
        GameManager.Instance.Win();
    }

    public void InitCharacterUI(PawnInstance newCharacter)
    {
        if (newCharacter.GetComponent<Behaviour.Keeper>() != null)
        {
            newCharacter.GetComponent<Behaviour.Keeper>().InitUI();
        }
        else if (newCharacter.GetComponent<Behaviour.Escortable>() != null)
        {
            newCharacter.GetComponent<Behaviour.Escortable>().InitUI();
        }
        else if (newCharacter.GetComponent<Behaviour.QuestDealer>() != null)
        {
            // Do nothing yet but has to reach init for inventory
        }
        else if (newCharacter.GetComponent<Trader>() != null)
        {
            // Do nothing yet but has to reach init for inventory
        }
        else
        {
            Debug.LogWarning("Trying to initialize UI on a pawn that is not a Keeper or an Escortable. Pawn name: " + newCharacter.Data.PawnName);
            return;
        }

        if (newCharacter.GetComponent<Behaviour.Mortal>() != null)
            newCharacter.GetComponent<Behaviour.Mortal>().InitUI();

        if (newCharacter.GetComponent<Behaviour.HungerHandler>() != null)
            newCharacter.GetComponent<Behaviour.HungerHandler>().InitUI();

        if (newCharacter.GetComponent<Behaviour.MentalHealthHandler>() != null)
            newCharacter.GetComponent<Behaviour.MentalHealthHandler>().InitUI();

        if (newCharacter.GetComponent<Behaviour.Inventory>() != null)
            newCharacter.GetComponent<Behaviour.Inventory>().InitUI();

        if (newCharacter.GetComponent<Interactable>() != null)
            newCharacter.GetComponent<Interactable>().InitUI();


    }
}

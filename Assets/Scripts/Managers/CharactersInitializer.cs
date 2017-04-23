using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using QuestSystem;

public class CharactersInitializer : MonoBehaviour {

    public void Init(Transform[] beginPositionsKeepers, GameObject beginPositionPrisonnier)
    {
        for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            GameManager.Instance.AllKeepersList[i].transform.position = beginPositionsKeepers[i].position;
            GameManager.Instance.AllKeepersList[i].transform.SetParent(null);
            GameManager.Instance.AllKeepersList[i].transform.rotation = Quaternion.identity;
            GameManager.Instance.AllKeepersList[i].transform.localScale = Vector3.one;
            GameManager.Instance.AllKeepersList[i].transform.GetComponent<NavMeshAgent>().enabled = true;

            //GlowController.RegisterObject(GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>());
        }

        /*
        GameManager.Instance.PrisonerInstance.transform.position = beginPositionPrisonnier.transform.position;
        GameManager.Instance.PrisonerInstance.transform.SetParent(null);
        GameManager.Instance.PrisonerInstance.transform.rotation = Quaternion.identity;
        GameManager.Instance.PrisonerInstance.transform.localScale = Vector3.one;
        GameManager.Instance.PrisonerInstance.transform.GetComponent<NavMeshAgent>().enabled = true;
        */
        // TODO : faire une meilleure caméra d'entrée de niveau

        // Pour que ça marche il me faut l'instanciation du tile Manager
        //GameManager.Instance.CameraManager.UpdateCameraPosition();
    }

    // Init keepers and call next initialization
    public void InitKeepers(Transform[] beginPositionsKeepers)
    {
        for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            GameManager.Instance.AllKeepersList[i].transform.position = beginPositionsKeepers[i].position;
            GameManager.Instance.AllKeepersList[i].transform.SetParent(null);
            GameManager.Instance.AllKeepersList[i].transform.rotation = Quaternion.identity;
            GameManager.Instance.AllKeepersList[i].transform.localScale = Vector3.one;
            GameManager.Instance.AllKeepersList[i].transform.GetComponent<NavMeshAgent>().enabled = true;

            InitCharacterUI(GameManager.Instance.AllKeepersList[i]);
            GameManager.Instance.RegisterKeeperPosition(GameManager.Instance.AllKeepersList[i]);
            GlowController.RegisterObject(GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>());
        }

        // Next step, init quests
        // TODO: init quests

        // Next step, init NPCs
        // TODO: init quests and call this properly
        InitNPCs();
    }

    private void InitNPCs()
    {
        foreach (Quest quest in GameManager.Instance.QuestManager.Quests)
        {
            if (quest.Identifier.SourceID != string.Empty && 
                quest.Identifier.SourceID.Contains("pnj"))
            {
                if(GameManager.Instance.QuestSources == null)
                {

                }
                QuestSource source = GameManager.Instance.QuestSources.FindSourceByID(quest.Identifier.SourceID);
                if(source == null)
                {
                    Debug.Log("Can't spawn NPC \"" + quest.Identifier.SourceID + "\". No QuestSource with this ID found in the scene.");
                }
                else
                {
                    if (source.needsToBeSpawned)
                    {
                        GameObject spawnedPawn = GameManager.Instance.PawnDataBase.CreatePawn(source.ID, source.Transform.position, source.Transform.rotation, null);
                        spawnedPawn.transform.SetParent(source.Tile.transform);
                        spawnedPawn.transform.SetAsLastSibling();
                        spawnedPawn.GetComponent<PawnInstance>().CurrentTile = source.Tile;
                        spawnedPawn.GetComponent<Behaviour.QuestDealer>().questToGive = quest;
                        spawnedPawn.GetComponent<Behaviour.QuestDealer>().Init();
                        InitCharacterUI(spawnedPawn.GetComponent<PawnInstance>());
                        if (source.Tile.State != TileState.Discovered)
                        {
                            spawnedPawn.SetActive(false);
                        }

                    }
                }
            }
        }
        // TODO this should not be handled like, especially if there is more prisoner in scene
        GameObject prisoner = GameManager.Instance.PawnDataBase.CreatePawn("ashley", TileManager.Instance.BeginTile.transform.position, Quaternion.identity, GameManager.Instance.transform);
        GlowController.RegisterObject(prisoner.GetComponent<GlowObjectCmd>());

        InitCharacterUI(prisoner.GetComponent<PawnInstance>());
        GameManager.Instance.PrisonerInstance = prisoner.GetComponent<PawnInstance>();
        GameManager.Instance.QuestSources.GetComponent<QuestInitializer>().InitializeQuests();
        GameManager.Instance.QuestManager.MainQuest.OnQuestComplete += EndGameQuest;
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

    }
}

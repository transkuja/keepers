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

            //GlowController.RegisterObject(GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>());

            InitCharacterUI(GameManager.Instance.AllKeepersList[i]);
            GameManager.Instance.RegisterKeeperPosition(GameManager.Instance.AllKeepersList[i]);
        }

        // Next step, init quests
        // TODO: init quests

        // Next step, init NPCs
        // TODO: init quests and call this properly
        InitNPCs(new QuestDeck());
    }

    private void InitNPCs(QuestSystem.QuestDeck _questDeck)
    {
        // TODO: init characters linked to quests

        // TODO this should not be handled like, especially if there is more prisoner in scene
        GameObject prisoner = GameManager.Instance.PawnDataBase.CreatePawn("ashley", TileManager.Instance.BeginTile.transform.position, Quaternion.identity, GameManager.Instance.transform);
        GameManager.Instance.PrisonerInstance = prisoner.GetComponent<PawnInstance>();

        // I NEED A QUEST INITIALIZER
        List<IQuestObjective> mainObjectives = new List<IQuestObjective>();
        mainObjectives.Add(new PrisonerEscortObjective("Until the end", "Bring Ashley to the end, and ALIVE.", GameObject.FindObjectOfType<Behaviour.Prisoner>().gameObject, TileManager.Instance.EndTile));
        GameManager.Instance.MainQuest = new Quest(new QuestIdentifier(0, gameObject), new QuestText("Main Quest: The last phoque licorne", "", "You're probably wondering why I gathered all of you here today. Well I'll be quick, I want you to bring this wonderful animal to my good and rich friend. Don't worry, you will be rewarded. His name is \"End\", you'll see his flag from pretty far away, head towards it. I'm counting on you, it is extremely important.", "Hint: Don't kill Ashley."), mainObjectives);
        GameManager.Instance.MainQuest.CheckAndComplete();
        GameManager.Instance.MainQuest.OnQuestComplete += EndGameQuest;

        InitCharacterUI(GameManager.Instance.PrisonerInstance);
    }

    void EndGameQuest()
    {
        GameManager.Instance.MainQuest.OnQuestComplete -= EndGameQuest;
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

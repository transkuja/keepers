using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        }

        // Next step, init quests
        // TODO: init quests

        // Next step, init NPCs
        // TODO: init quests and call this properly
        InitNPCs(new QuestSystem.QuestDeck());

        GameManager.Instance.CameraManager.UpdateCameraPosition();
        //NewGameManager.Instance.UpdateCameraPosition();
    }

    private void InitNPCs(QuestSystem.QuestDeck _questDeck)
    {
        // TODO: init characters linked to quests
    }
}

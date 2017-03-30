using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharactersInitializer : MonoBehaviour {

    public void Init(GameObject[] beginPositionsKeepers, GameObject beginPositionPrisonnier)
    {
        for (int i = 0; i < GameManager.Instance.AllKeepersListOld.Count; i++)
        {
            GameManager.Instance.AllKeepersListOld[i].transform.position = beginPositionsKeepers[i].transform.position;
            GameManager.Instance.AllKeepersListOld[i].transform.SetParent(null);
            GameManager.Instance.AllKeepersListOld[i].transform.rotation = Quaternion.identity;
            GameManager.Instance.AllKeepersListOld[i].transform.localScale = Vector3.one;
            GameManager.Instance.AllKeepersListOld[i].transform.GetComponent<NavMeshAgent>().enabled = true;

            //GlowController.RegisterObject(GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>());
        }


        GameManager.Instance.PrisonerInstance.transform.position = beginPositionPrisonnier.transform.position;
        GameManager.Instance.PrisonerInstance.transform.SetParent(null);
        GameManager.Instance.PrisonerInstance.transform.rotation = Quaternion.identity;
        GameManager.Instance.PrisonerInstance.transform.localScale = Vector3.one;
        GameManager.Instance.PrisonerInstance.transform.GetComponent<NavMeshAgent>().enabled = true;

        // TODO : faire une meilleure caméra d'entrée de niveau

        // Pour que ça marche il me faut l'instanciation du tile Manager
        //GameManager.Instance.CameraManager.UpdateCameraPosition();
    }
}

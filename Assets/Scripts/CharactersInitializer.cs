using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharactersInitializer : MonoBehaviour {

    [SerializeField]
    private GameObject beginPositions;

    [SerializeField]
    private PrisonerInstance beginPositionPrisonnier;

    void Awake()
    {
    
        for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            GameManager.Instance.AllKeepersList[i].transform.position = beginPositions.transform.GetChild(i).position;
            GameManager.Instance.AllKeepersList[i].transform.SetParent(null);
            GameManager.Instance.AllKeepersList[i].transform.GetComponent<NavMeshAgent>().enabled = true;

            GlowController.RegisterObject(GameManager.Instance.AllKeepersList[i].GetComponent<GlowObjectCmd>());
        }


        GameManager.Instance.PrisonerInstance = beginPositionPrisonnier;
    }
}

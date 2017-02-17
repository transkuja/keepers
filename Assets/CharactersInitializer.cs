using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharactersInitializer : MonoBehaviour {

    [SerializeField]
    private GameObject beginPositions;

    void Awake()
    {
        KeeperInstance[] keeperInstances = GameManager.Instance.GetComponentsInChildren<KeeperInstance>();
        for (int i = 0; i < keeperInstances.Length; i++)
        {
            keeperInstances[i].transform.position = beginPositions.transform.GetChild(i).position;
            keeperInstances[i].transform.SetParent(null);
            keeperInstances[i].transform.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}

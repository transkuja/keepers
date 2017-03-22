using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleLauncher : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MonsterInstance>())
        {
            GetComponentInParent<NavMeshAgent>().SetDestination(transform.position);
            BattleHandler.StartBattleProcess(TileManager.Instance.GetTileFromKeeper[GetComponentInParent<KeeperInstance>()]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleLauncher : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MonsterInstance>())
        {
            Tile tile;
            if (GetComponentInParent<KeeperInstance>() != null)
                tile = TileManager.Instance.GetTileFromKeeper[GetComponentInParent<KeeperInstance>()];
            else
                tile = TileManager.Instance.PrisonerTile;

            GetComponentInParent<NavMeshAgent>().SetDestination(transform.position);
            foreach (MonsterInstance mi in TileManager.Instance.MonstersOnTile[tile])
                mi.GetComponent<NavMeshAgent>().SetDestination(mi.transform.position);
            BattleHandler.StartBattleProcess(tile);
        }
    }
}

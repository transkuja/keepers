using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleLauncher : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Behaviour.Monster>() != null)
        {
            Tile tile;
            tile = GetComponentInParent<PawnInstance>().CurrentTile;

            GetComponentInParent<NavMeshAgent>().SetDestination(transform.position);
            foreach (PawnInstance mi in TileManager.Instance.MonstersOnTile[tile])
                mi.GetComponent<NavMeshAgent>().SetDestination(mi.transform.position);
            BattleHandler.StartBattleProcess(tile);

            GameManager.Instance.CameraManager.UpdateCameraPosition(GetComponentInParent<Behaviour.Keeper>().getPawnInstance);
        }
    }
}

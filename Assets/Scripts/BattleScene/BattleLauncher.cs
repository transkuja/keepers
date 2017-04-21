using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleLauncher : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Behaviour.Monster>() != null)
        {
            if (BattleHandler.IsABattleAlreadyInProcess())
                return;

            Tile tile = GetComponentInParent<PawnInstance>().CurrentTile;

            BattleHandler.StartBattleProcess(tile);

            GameManager.Instance.UpdateCameraPosition(GetComponentInParent<PawnInstance>());
        }
    }
}

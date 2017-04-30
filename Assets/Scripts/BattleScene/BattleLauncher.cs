using UnityEngine;
using Behaviour;

public class BattleLauncher : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.CurrentState == GameState.Normal)
        {
            if (other.gameObject.GetComponentInParent<Monster>() != null)
            {
                if (GetComponentInParent<Fighter>() != null && GetComponentInParent<Fighter>().IsTargetableByMonster == true)
                {
                    if (BattleHandler.IsABattleAlreadyInProcess())
                        return;

                    Tile tile = GetComponentInParent<PawnInstance>().CurrentTile;

                    BattleHandler.StartBattleProcess(tile);

                    GameManager.Instance.UpdateCameraPosition(GetComponentInParent<PawnInstance>());
                }
            }
        }
    }
}

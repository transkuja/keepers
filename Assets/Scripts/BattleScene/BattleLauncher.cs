using UnityEngine;
using Behaviour;

public class BattleLauncher : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.CurrentState == GameState.Normal)
        {
            if (other.gameObject.GetComponentInParent<Monster>() != null && other.gameObject.GetComponent<AggroBehaviour>() == null && other.gameObject.GetComponentInParent<Monster>().BattleOnCollision)
            {
                if (GetComponentInParent<Fighter>() != null && GetComponentInParent<Fighter>().IsTargetableByMonster == true)
                {
                    if (other.gameObject.GetComponentInParent<PawnInstance>().CurrentTile == GetComponentInParent<PawnInstance>().CurrentTile)
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
}

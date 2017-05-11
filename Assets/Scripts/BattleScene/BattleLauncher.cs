using UnityEngine;
using Behaviour;
using System.Collections.Generic;

public class BattleLauncher : MonoBehaviour {

    PawnInstance instance;

    void Start()
    {
        instance = GetComponentInParent<PawnInstance>();
    }

    void LaunchBattle(PawnInstance _aggroTarget)
    {
        if (GetComponentInParent<Monster>().BattleOnCollision && _aggroTarget.GetComponentInParent<Fighter>() != null && _aggroTarget.GetComponentInParent<Fighter>().IsTargetableByMonster == true)
        {
            BattleHandler.StartBattleProcess(instance.CurrentTile);
            GameManager.Instance.UpdateCameraPosition(_aggroTarget);
        }
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.Normal && !BattleHandler.IsABattleAlreadyInProcess())
        {
            if (instance == null) instance = GetComponentInParent<PawnInstance>();

            // If an error appears here, call the 0646440132, thanks
            if (TileManager.Instance.KeepersOnTile.ContainsKey(instance.CurrentTile))
            {
                List<PawnInstance> keepers = TileManager.Instance.KeepersOnTile[instance.CurrentTile];
                for (int i = 0; i < keepers.Count; i++)
                {
                    if (keepers[i].CurrentTile == instance.CurrentTile)
                    {
                        if (Vector3.Distance(keepers[i].transform.position, transform.position) < 0.5f)
                            LaunchBattle(keepers[i]);
                    }
                }
            }
        }
    }
}

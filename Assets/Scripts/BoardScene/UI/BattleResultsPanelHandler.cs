using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleResultsPanelHandler : MonoBehaviour {

    public void CloseBattleResultsPanel()
    {
        transform.GetChild((int)BattleResultScreenChildren.Loot).gameObject.SetActive(true);
        transform.GetChild((int)BattleResultScreenChildren.Lost).gameObject.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}

public enum BattleResultScreenChildren
{
    Header,
    Loot,
    Lost
}

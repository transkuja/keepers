using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleResultsPanelHandler : MonoBehaviour {

    public void CloseBattleResultsPanel()
    {
        //transform.GetChild((int)BattleResultScreenChildren.Loot).gameObject.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;

        GameManager.Instance.SelectedKeeperNeedUpdate = true;
        GameManager.Instance.ShortcutPanel_NeedUpdate = true;
    }
}

public enum BattleResultScreenChildren
{
    Header,
    Logger
}

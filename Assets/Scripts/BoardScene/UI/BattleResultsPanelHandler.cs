using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleResultsPanelHandler : MonoBehaviour {

    public void CloseBattleResultsPanel()
    {
        //transform.GetChild((int)BattleResultScreenChildren.Loot).gameObject.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;

        //GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
        //GameManager.Instance.Ui.UpdateShortcutPanel();
    }
}

public enum BattleResultScreenChildren
{
    Header,
    Background,
    Logger
}

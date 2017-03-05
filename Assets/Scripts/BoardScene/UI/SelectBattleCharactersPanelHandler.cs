using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBattleCharactersPanelHandler : MonoBehaviour {

    Tile activeTile;

    public Tile ActiveTile
    {
        get
        {
            return activeTile;
        }

        set
        {
            activeTile = value;
        }
    }

    public void CloseSelectBattleCharactersPanel()
    {
        List<KeeperInstance> selected = new List<KeeperInstance>();

        if (transform.GetChild((int)SelectBattleCharactersScreenChildren.FirstCharacter).GetChild(0) != null)
            selected.Add(transform.GetChild((int)SelectBattleCharactersScreenChildren.FirstCharacter).GetComponentInChildren<KeeperInstance>());

        if (transform.GetChild((int)SelectBattleCharactersScreenChildren.SecondCharacter).GetChild(0) != null)
            selected.Add(transform.GetChild((int)SelectBattleCharactersScreenChildren.SecondCharacter).GetComponentInChildren<KeeperInstance>());

        if (transform.GetChild((int)SelectBattleCharactersScreenChildren.ThirdCharacter).GetChild(0) != null)
            selected.Add(transform.GetChild((int)SelectBattleCharactersScreenChildren.ThirdCharacter).GetComponentInChildren<KeeperInstance>());

        BattleHandler.SetSelectedKeepers(selected);
        gameObject.SetActive(false);
    }

}

public enum SelectBattleCharactersScreenChildren
{
    Header,
    FirstCharacter,
    SecondCharacter,
    ThirdCharacter,
    Go,
    CharactersOnTile
}
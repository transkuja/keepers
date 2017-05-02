using UnityEngine;
using Behaviour;

public class BattleResultsPanelHandler : MonoBehaviour {

    public void CloseBattleResultsPanel()
    {
        //transform.GetChild((int)BattleResultScreenChildren.Loot).gameObject.SetActive(true);
        gameObject.SetActive(false);
        GameManager.Instance.CurrentState = GameState.Normal;

        GameManager.Instance.ClearListKeeperSelected();
        PawnInstance[] curFighters = GameManager.Instance.CurrentFighters;
        for (int i = 0; i < curFighters.Length; i++)
        {
            if (curFighters[i].GetComponent<Keeper>() != null && curFighters[i].GetComponent<Mortal>().CurrentHp > 0)
            {
                curFighters[i].GetComponent<Keeper>().IsSelected = true;
                GameManager.Instance.AddKeeperToSelectedList(curFighters[i]);
                break;
            }
        }

    }
}

public enum BattleResultScreenChildren
{
    Header,
    Background,
    Logger
}

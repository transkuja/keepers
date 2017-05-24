using UnityEngine;
using Behaviour;
using System.Collections;
using UnityEngine.SceneManagement;
public class BattleResultsPanelHandler : MonoBehaviour {
    public bool victory; 
    //public void CloseBattleResultsPanel()
    //{
    //    //transform.GetChild((int)BattleResultScreenChildren.Loot).gameObject.SetActive(true);
    //    gameObject.SetActive(false);
    //    GameManager.Instance.CurrentState = GameState.Normal;

    //    GameManager.Instance.ClearListKeeperSelected();
    //    PawnInstance[] curFighters = GameManager.Instance.CurrentFighters;
    //    for (int i = 0; i < curFighters.Length; i++)
    //    {
    //        if (curFighters[i].GetComponent<Keeper>() != null && curFighters[i].GetComponent<Mortal>().CurrentHp > 0)
    //        {
    //            curFighters[i].GetComponent<Keeper>().IsSelected = true;
    //            GameManager.Instance.AddKeeperToSelectedList(curFighters[i]);
    //            break;
    //        }
    //    }

    //}

    public void OnEnable()
    {
        StartCoroutine(HideMe());
    }

    public IEnumerator HideMe()
    {
        if(victory)
        {
            yield return new WaitForSeconds(4.0f);
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
        }
        //transform.GetChild((int)BattleResultScreenChildren.Loot).gameObject.SetActive(true);
        GameManager.Instance.BattleResultScreen.gameObject.SetActive(false);
        GameManager.Instance.CurrentState = GameState.Normal;
        AudioManager.Instance.StopBattleMusic();

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
        yield return null;
    }
}

public enum BattleResultScreenChildren
{
    Header,
    Background,
    Logger
}

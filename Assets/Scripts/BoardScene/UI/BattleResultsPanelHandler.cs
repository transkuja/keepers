using UnityEngine;
using Behaviour;
using System.Collections;
using UnityEngine.SceneManagement;
public class BattleResultsPanelHandler : MonoBehaviour {

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
        yield return new WaitForSeconds(3);
        //transform.GetChild((int)BattleResultScreenChildren.Loot).gameObject.SetActive(true);
        GameManager.Instance.BattleResultScreen.gameObject.SetActive(false);
        GameManager.Instance.CurrentState = GameState.Normal;
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                AudioManager.Instance.Fade(AudioManager.Instance.Scene1Clip);
                break;
            case 2:
                AudioManager.Instance.Fade(AudioManager.Instance.Scene2Clip);
                break;
            case 3:
                AudioManager.Instance.Fade(AudioManager.Instance.Scene3Clip);
                break;
            case 4:
                AudioManager.Instance.Fade(AudioManager.Instance.Scene4Clip);
                break;
        }
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

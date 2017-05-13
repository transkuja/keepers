using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Behaviour;

public class BuffFeedback : MonoBehaviour
{
    public List<BattleBoeuf> curBoeufs;
    int nextIndex = 0;
    float timerPop = 0.0f;
    bool isInitialized = false;

    private void Start()
    {
        curBoeufs = new List<BattleBoeuf>();
        nextIndex = 0;
    }

    public void UpdateCurrentBoeufsList(List<BattleBoeuf> _updatedList)
    {
        curBoeufs = _updatedList;
        nextIndex = 0;
    }

    public void ShowBuffs(bool _show)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!_show)
                isInitialized = false;

            if (!isInitialized && _show)
            {
                PrintBuff();
                isInitialized = true;
            }
            transform.GetChild(i).gameObject.SetActive(_show);
        }
    }

    void PrintBuff()
    {
        Image imgChild = transform.GetChild(1).GetComponent<Image>();
        if (curBoeufs[nextIndex].BoeufType == BoeufType.Damage)
        {
            if (curBoeufs[nextIndex].EffectValue < 0)
            {
                imgChild.sprite = GameManager.Instance.SpriteUtils.spriteAtkDesBoeufs;
            }
            else
            {
                imgChild.sprite = GameManager.Instance.SpriteUtils.spriteAtkBuff;
            }
        }
        else if (curBoeufs[nextIndex].BoeufType == BoeufType.Defense)
        {
            if (curBoeufs[nextIndex].EffectValue < 0)
            {
                imgChild.sprite = GameManager.Instance.SpriteUtils.spriteDefDesBoeufs;
            }
            else
            {
                imgChild.sprite = GameManager.Instance.SpriteUtils.spriteDefBuff;
            }
        }
        nextIndex++;
        nextIndex = nextIndex % curBoeufs.Count;
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.InBattle)
        {
            if (curBoeufs.Count == 0)
            {
                ShowBuffs(false);
                return;
            }

            ShowBuffs(true);
            timerPop -= Time.unscaledDeltaTime;

            if (timerPop < 0.0f)
            {
                timerPop = 2.0f;
                PrintBuff();
            }
        }
    }
}

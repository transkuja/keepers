using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Behaviour;

public class BuffFeedback : MonoBehaviour
{

    Fighter pawnFighter;
    public List<BattleBoeuf> curBoeufs;
    bool listUpdated = false;
    int nextIndex = 0;
    float timerPop = 0.0f;
    public Color positive;
    public Color negative;

    private void Start()
    {
        pawnFighter = GetComponentInParent<Fighter>();
        curBoeufs = new List<BattleBoeuf>();
        nextIndex = 0;
    }

    public void UpdateCurrentBoeufsList(List<BattleBoeuf> _updatedList)
    {
        curBoeufs = _updatedList;
        nextIndex = 0;
    }

    void ShowBuffs(bool _show)
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(_show);
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
                Text textChild = GetComponentInChildren<Text>();

                if (curBoeufs[nextIndex].EffectValue < 0)
                {
                    textChild.text = "_";
                    textChild.transform.localPosition = new Vector2(0.04f, -0.2f);
                    textChild.color = negative;
                }
                else
                {
                    textChild.text = "+";
                    textChild.transform.localPosition = new Vector2(0.07f, -0.25f);
                    textChild.color = positive;
                }

                Image imgChild = transform.GetChild(1).GetComponent<Image>();
                imgChild.color = textChild.color;
                if (curBoeufs[nextIndex].BoeufType == BoeufType.Damage)
                    imgChild.sprite = GameManager.Instance.SpriteUtils.spriteAtkBuff;
                else if (curBoeufs[nextIndex].BoeufType == BoeufType.Defense)
                    imgChild.sprite = GameManager.Instance.SpriteUtils.spriteDefBuff;

                nextIndex++;
                Debug.Log(nextIndex);
                nextIndex = nextIndex % curBoeufs.Count;
            }
        }
    }
}

﻿using UnityEngine;
using Behaviour;

public class SelectCharactersByClickingOnButton : MonoBehaviour {

    public void SelectCharacter()
    {
        UIBattleHandler uiBattleHandler = GetComponentInParent<UIBattleHandler>();
        Transform characterPanel = (name.Contains("Avatar")) ? transform.parent.parent : transform;
        PawnInstance pawnInstance = uiBattleHandler.GetCharacterFromPanelIndex(characterPanel);
        if (pawnInstance.GetComponent<Keeper>() != null)
        {
            if (pawnInstance.GetComponent<Fighter>() != null && !pawnInstance.GetComponent<Fighter>().HasPlayedThisTurn)
            {
                for (int i = 0; i < 3; i++)
                {
                    uiBattleHandler.SkillsPanels.transform.GetChild(i).gameObject.SetActive(false);
                }
                GameManager.Instance.ClearListKeeperSelected();
                GameManager.Instance.AddKeeperToSelectedList(pawnInstance);
                pawnInstance.GetComponent<Keeper>().IsSelected = true;
                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(pawnInstance).gameObject.SetActive(true);
            }
        }
    }
}

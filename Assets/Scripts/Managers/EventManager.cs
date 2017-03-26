using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    private static short actionPointsResetValue = 3;

    public static void EndTurnEvent()
    {
        DecreaseMentalHealth();
        IncreaseHunger();
        ApplyEndTurnHungerPenalty();
        ApplyEndTurnMentalHealthPenalty();

        ResetActionPointsForNextTurn();
        GameManager.Instance.ShortcutPanel_NeedUpdate = true;
        GameManager.Instance.SelectedKeeperNeedUpdate = true;
        GameManager.Instance.Ui.ClearActionPanel();
    }

    private static void DecreaseMentalHealth()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.IsAlive)
                ki.CurrentMentalHealth -= 10;
        }
    }

    private static void IncreaseHunger()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.IsAlive)
                ki.CurrentHunger -= 10;
        }
        if (GameManager.Instance.PrisonerInstance.IsAlive)
        {
            GameManager.Instance.PrisonerInstance.CurrentHunger -= 10;
        }
    }

    private static void ResetActionPointsForNextTurn()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.IsAlive)
                ki.ActionPoints = actionPointsResetValue;
        }
    }

    public static void ApplyEndTurnHungerPenalty()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.IsAlive && ki.IsStarving)
                ki.CurrentHp -= 20;
        }

        if (GameManager.Instance.PrisonerInstance.IsAlive && GameManager.Instance.PrisonerInstance.IsStarving)
        {
            GameManager.Instance.PrisonerInstance.CurrentHp -= 20;
        }
    }

    public static void ApplyEndTurnMentalHealthPenalty()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.IsAlive && ki.IsMentalHealthLow && !ki.isLowMentalHealthBuffApplied)
            {
                ki.Keeper.BonusDefense = (short)(ki.Keeper.BonusDefense - ki.Keeper.BaseDefense/2);
                ki.Keeper.BonusStrength = (short)(ki.Keeper.BonusStrength - ki.Keeper.BaseStrength / 2);
                ki.Keeper.BonusIntelligence = (short)(ki.Keeper.BonusIntelligence - ki.Keeper.BaseIntelligence / 2);
                ki.Keeper.BonusSpirit = (short)(ki.Keeper.BonusSpirit - ki.Keeper.BaseSpirit / 2);
                ki.isLowMentalHealthBuffApplied = true;
            }
        }
    }
}

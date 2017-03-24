using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    private static short actionPointsResetValue = 3;

    public static void EndTurnEvent()
    {
        DecreaseMentalHealth();
        IncreaseHunger();

        ResetActionPointsForNextTurn();
        GameManager.Instance.ShortcutPanel_NeedUpdate = true;
        GameManager.Instance.SelectedKeeperNeedUpdate = true;
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
                ki.CurrentHunger += 10;
        }
        if (GameManager.Instance.PrisonerInstance.IsAlive)
        {
            GameManager.Instance.PrisonerInstance.CurrentHunger += 10;
        }
    }

    private static void ResetActionPointsForNextTurn()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            ki.ActionPoints = actionPointsResetValue;
        }
    }

    public static void ApplyEndTurnHungerPenalty()
    {

    }

    public static void ApplyEndTurnMentalHealthPenalty()
    {

    }
}

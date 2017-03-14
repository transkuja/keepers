using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    private static short actionPointsResetValue = 3;

    public static void EndTurnEvent()
    {
        DecreaseMentalHealth();
        IncreaseHunger();
        GameManager.Instance.ShortcutPanel_NeedUpdate = true;

        ResetActionPointsForNextTurn();
        GameManager.Instance.SelectedKeeperNeedUpdate = true;
    }

    private static void DecreaseMentalHealth()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            ki.CurrentMentalHealth -= 10;
        }
    }

    private static void IncreaseHunger()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            ki.CurrentHunger += 10;
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

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
    }

    private static void DecreaseMentalHealth()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            ki.Keeper.ActualMentalHealth -= 10;
        }
    }

    private static void IncreaseHunger()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            ki.Keeper.ActualHunger += 10;
        }
    }

    private static void ResetActionPointsForNextTurn()
    {
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            ki.Keeper.ActionPoints = actionPointsResetValue;
        }
    }

    public static void ApplyEndTurnHungerPenalty()
    {

    }

    public static void ApplyEndTurnMentalHealthPenalty()
    {

    }
}

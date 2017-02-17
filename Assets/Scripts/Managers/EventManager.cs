using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public static void EndTurnEvent()
    {
        DecreaseMentalHealth();
        IncreaseHunger();
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

    public static void ApplyEndTurnHungerPenalty()
    {

    }

    public static void ApplyEndTurnMentalHealthPenalty()
    {

    }
}

using Behaviour;
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
        GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
        //GameManager.Instance.Ui.UpdateShortcutPanel();
        GameManager.Instance.Ui.ClearActionPanel();
    }

    private static void DecreaseMentalHealth()
    {
        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.GetComponent<Mortal>().IsAlive)
                ki.GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 10;
        }
    }

    private static void IncreaseHunger()
    {
        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.GetComponent<Mortal>().IsAlive)
                ki.GetComponent<HungerHandler>().CurrentHunger -= 10;
        }

        // TODO fix prisoner

        //if (GameManager.Instance.PrisonerInstanceOld.IsAlive)
        //{
        //    GameManager.Instance.PrisonerInstanceOld.CurrentHunger -= 10;
        //}
    }

    private static void ResetActionPointsForNextTurn()
    {
        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.GetComponent<Mortal>().IsAlive)
                ki.GetComponent<Keeper>().ActionPoints = actionPointsResetValue;
        }
    }

    public static void ApplyEndTurnHungerPenalty()
    {
        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.GetComponent<Mortal>().IsAlive && ki.GetComponent<HungerHandler>().IsStarving)
                ki.GetComponent<Mortal>().CurrentHp -= 20;
        }

        /*
        if (GameManager.Instance.PrisonerInstanceOld.IsAlive && GameManager.Instance.PrisonerInstanceOld.IsStarving)
        {
            GameManager.Instance.PrisonerInstanceOld.CurrentHp -= 20;
        }*/
    }

    public static void ApplyEndTurnMentalHealthPenalty()
    {
        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.GetComponent<Mortal>().IsAlive && ki.GetComponent<MentalHealthHandler>().IsDepressed && !ki.GetComponent<MentalHealthHandler>().IsLowMentalHealthBuffApplied)
            {
                //ki.Keeper.BonusDefense = (short)(ki.Keeper.BonusDefense - ki.Keeper.BaseDefense/2);
                //ki.Keeper.BonusStrength = (short)(ki.Keeper.BonusStrength - ki.Keeper.BaseStrength / 2);
                //ki.Keeper.BonusIntelligence = (short)(ki.Keeper.BonusIntelligence - ki.Keeper.BaseIntelligence / 2);
                //ki.Keeper.BonusSpirit = (short)(ki.Keeper.BonusSpirit - ki.Keeper.BaseSpirit / 2);
                ki.GetComponent<MentalHealthHandler>().IsLowMentalHealthBuffApplied = true;
            }
        }
    }
}

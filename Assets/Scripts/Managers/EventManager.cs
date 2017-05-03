using Behaviour;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

    // Lister tous les types d'évènements.
    // Les objets concernés invoqueront les delegate, avec une reférence vers eux même.
    // Ex: Jean-Louis le loup meurt -> Dans sa fonction de mort : Eventmanager.OnMonsterDie(this)

    public delegate void DefaultEvent();
    public delegate void MonsterEvent(Monster m);
    public delegate void KeeperEvent(Keeper k);
    public delegate void PrisonerEvent(Prisoner p);
    public delegate void AnimatedPawnEvent(AnimatedPawn ap);
    public delegate void PawnInstanceEvent(PawnInstance pi);
    public delegate void ItemEvent(ItemInstance ii);

    public static MonsterEvent OnMonsterDie;
    public static KeeperEvent OnKeeperDie;
    public static ItemEvent OnHarvest;
    public static ItemEvent OnPickUp;


    private static short actionPointsResetValue = 3;

    public static void EndTurnEvent()
    {
        if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto
            && TutoManager.s_instance.PlayingSequence == null && TutoManager.s_instance.GetComponent<SeqActionCostHunger>().AlreadyPlayed == false)
        {
            TutoManager.s_instance.GetComponent<SeqActionCostHunger>().hasPressedEndTurnButton = true;
        }


        IncreaseHunger();
        DecreaseMentalHealth();
        ApplyEndTurnHungerPenalty();
        ApplyEndTurnMentalHealthPenalty();

        ResetActionPointsForNextTurn();
        GameManager.Instance.Ui.ClearUiOnTurnEnding();
        HandleWeather();
    }

    private static void DecreaseMentalHealth()
    {
        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.GetComponent<Mortal>().IsAlive && ki.GetComponent<MentalHealthHandler>() != null)
                ki.GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 10;
                //ki.AddFeedBackToQueue(GameManager.Instance.SpriteUtils.spriteMoralDebuff, -10);
        }
    }

    private static void IncreaseHunger()
    {
        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.GetComponent<Mortal>().IsAlive)
                ki.GetComponent<HungerHandler>().CurrentHunger -= 10;
                //ki.AddFeedBackToQueue(GameManager.Instance.SpriteUtils.spriteHunger, -10);
        }

        if (GameManager.Instance.PrisonerInstance.GetComponent<Mortal>().IsAlive && GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>() != null)
            GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>().CurrentHunger -= 10;
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

        if (GameManager.Instance.PrisonerInstance.GetComponent<Mortal>().IsAlive && GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>() != null && GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>().IsStarving)
            GameManager.Instance.PrisonerInstance.GetComponent<Mortal>().CurrentHp -= 20;
    }

    public static void ApplyEndTurnMentalHealthPenalty()
    {
        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.GetComponent<Mortal>().IsAlive && ki.GetComponent<MentalHealthHandler>() != null && ki.GetComponent<MentalHealthHandler>().IsDepressed && !ki.GetComponent<MentalHealthHandler>().IsLowMentalHealthBuffApplied)
            {
                //ki.Keeper.BonusDefense = (short)(ki.Keeper.BonusDefense - ki.Keeper.BaseDefense/2);
                //ki.Keeper.BonusStrength = (short)(ki.Keeper.BonusStrength - ki.Keeper.BaseStrength / 2);
                //ki.Keeper.BonusIntelligence = (short)(ki.Keeper.BonusIntelligence - ki.Keeper.BaseIntelligence / 2);
                //ki.Keeper.BonusSpirit = (short)(ki.Keeper.BonusSpirit - ki.Keeper.BaseSpirit / 2);
                ki.GetComponent<MentalHealthHandler>().IsLowMentalHealthBuffApplied = true;
            }
        }
    }

    public static void HandleWeather()
    {
        Tile[] tiles = TileManager.Instance.Tiles.GetComponentsInChildren<Tile>();
        if (GameManager.Instance.ListEventSelected.Count > 0)
        {
            // id event 1
            if (GameManager.Instance.ListEventSelected.Contains("1")){
                if (GameManager.Instance.NbTurn >= 2 && GameManager.Instance.NbTurn <= 4)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile.Type == TileType.Snow && tile.State == TileState.Discovered)
                        {
                            if (tile.gameObject.GetComponentInChildren<Climat>() == null)
                            {
                                Debug.Log(tile.name + "n'a pas de climat en enfant");
                                return;
                            }
                            Climat climat = tile.gameObject.GetComponentInChildren<Climat>();
                            if (climat != null)
                            {
                                climat.TypeClimat = TypeClimat.Snow;
                            }

                        }
                    }
                }
                else if (GameManager.Instance.NbTurn > 5)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile.gameObject.GetComponentInChildren<Climat>() == null)
                        {
                            Debug.Log(tile.name + "n'a pas de climat en enfant");
                            return;
                        }
                        Climat climat = tile.gameObject.GetComponentInChildren<Climat>();
                        if (climat != null)
                        {
                            climat.TypeClimat = TypeClimat.None;
                        }
                    }
                }
            }

            if (GameManager.Instance.ListEventSelected.Contains("2"))
            {
                if (GameManager.Instance.NbTurn >= 1 && GameManager.Instance.NbTurn <= 3)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile.Type == TileType.Plain && tile.State == TileState.Discovered)
                        {
                            if (tile.gameObject.GetComponentInChildren<Climat>() == null)
                            {
                                Debug.Log(tile.name + "n'a pas de climat en enfant");
                                return;
                            }
                            Climat climat = tile.gameObject.GetComponentInChildren<Climat>();
                            if (climat != null)
                            {
                                climat.TypeClimat = TypeClimat.Butterfly;
                            }

                        }
                    }
                }
                else if (GameManager.Instance.NbTurn > 4)
                {
                    foreach (Tile tile in tiles)
                    {
                        if (tile.gameObject.GetComponentInChildren<Climat>() == null)
                        {
                            Debug.Log(tile.name + "n'a pas de climat en enfant");
                            return;
                        }
                        Climat climat = tile.gameObject.GetComponentInChildren<Climat>();
                        if (climat != null)
                        {
                            climat.TypeClimat = TypeClimat.None;
                        }
                    }
                }
            }
        }
    }
}

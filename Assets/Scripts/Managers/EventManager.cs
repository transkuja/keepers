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

    public delegate void PawnMoveOnTile(PawnInstance pi, Tile ti);




    public static MonsterEvent OnMonsterDie;



    public static KeeperEvent OnKeeperDie;



    public static ItemEvent OnHarvest;



    public static ItemEvent OnPickUp;

    public static PawnMoveOnTile OnPawnMove;






    public static int nbDayInWeek = 7;



    public static int nbDayInMonth = 20;







    private static float weatherFrequency = 50.0f;







    public static int hungerPenalty = 25;







    //private static short actionPointsResetValue = 3;







    public static void EndTurnEvent()


    {
        IncreaseHunger();

        DecreaseMentalHealth();

        ApplyEndTurnHungerPenalty();

        ApplyEndTurnMentalHealthPenalty();

        if (TutoManager.s_instance.PlayingSequence == null)
            HandleWeather();



        ResetActionPointsForNextTurn();
        GameManager.Instance.Ui.ClearUiOnTurnEnding();

    }



    private static void DecreaseMentalHealth()


    {


        foreach (PawnInstance pi in GameManager.Instance.AllKeepersList)


        {


            if (pi.GetComponent<Mortal>().IsAlive && pi.GetComponent<MentalHealthHandler>() != null)


            {


                bool moodModifier = false;


                if (GameManager.Instance.ListEventSelected.Count > 0)


                {


                    if (GameManager.Instance.ListEventSelected.Contains("1"))


                    {
                        if (pi.CurrentTile == null)
                        {
                            Debug.Log(pi.Data.PawnName + "is not on a tile");
                            break;
                        }

                        if (pi.CurrentTile.GetComponentInChildren<Climat>() != null && pi.CurrentTile.GetComponentInChildren<Climat>().TypeClimat == TypeClimat.Snow)


                        {


                            if (TutoManager.s_instance.PlayingSequence == null && TutoManager.s_instance.GetComponent<SeqMoraleExplained>().AlreadyPlayed == false)


                            {


                                TutoManager.s_instance.GetComponent<SeqMoraleExplained>().isLaunchedDuringASnowEvent = true;


                                GameManager.Instance.UpdateCameraPosition(pi.CurrentTile);


                                TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqMoraleExplained>());


                            }


                            moodModifier = true;


                        }


                    }


                }



                if (moodModifier)


                {


                    if (TutoManager.s_instance.PlayingSequence == null)


                        pi.GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 5;
                }



                if (pi.CurrentTile != null && TileManager.Instance.MonstersOnTile != null && TileManager.Instance.MonstersOnTile.ContainsKey(pi.CurrentTile) && TileManager.Instance.MonstersOnTile[pi.CurrentTile].Count > 0)


                {


                    int goodMonsters = 0;


                    foreach (PawnInstance monstersButNotReally in TileManager.Instance.MonstersOnTile[pi.CurrentTile])


                    {


                        if (monstersButNotReally.GetComponent<QuestDealer>() != null)


                            goodMonsters++;


                    }


                    if (goodMonsters != TileManager.Instance.MonstersOnTile[pi.CurrentTile].Count)


                        pi.GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 5;


                }



                if (pi.CurrentTile.GetComponent<Tile>().Friendliness == TileFriendliness.Scary)


                    pi.GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 10;



                if (pi.CurrentTile.GetComponent<Tile>().Friendliness == TileFriendliness.Friendly)


                    pi.GetComponent<MentalHealthHandler>().CurrentMentalHealth += 5;



                foreach (PawnInstance piOnTile in TileManager.Instance.KeepersOnTile[pi.CurrentTile])


                {


                    if (piOnTile.Data.Behaviours[(int)BehavioursEnum.Stinks])


                        pi.GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 20;


                }



            }



        }



    }







    private static void IncreaseHunger()



    {



        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)



        {



            if (ki.GetComponent<Mortal>().IsAlive && ki.GetComponent<HungerHandler>() != null)



            {



                bool hungerModifier = false;



                if (GameManager.Instance.ListEventSelected.Count > 0)



                {



                    if (GameManager.Instance.ListEventSelected.Contains("3"))



                    {



                        if (ki.CurrentTile.GetComponentInChildren<Climat>() != null && ki.CurrentTile.GetComponentInChildren<Climat>().TypeClimat == TypeClimat.HeatDistorsion)



                        {



                            hungerModifier = true;



                        }



                    }



                }







                if (hungerModifier)



                    ki.GetComponent<HungerHandler>().CurrentHunger -= 5;







                float hungerMultiplier = 1.0f;



                if (ki.GetComponent<PawnInstance>().Data.Behaviours[(int)BehavioursEnum.Morfale])



                    hungerMultiplier = 1.5f;



                ki.GetComponent<HungerHandler>().CurrentHunger -= (int)(10.0f* hungerMultiplier);



            }



         



                //ki.AddFeedBackToQueue(GameManager.Instance.SpriteUtils.spriteHunger, -10);



        }







        if (GameManager.Instance.PrisonerInstance.GetComponent<Mortal>().IsAlive && GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>() != null)



        {



            bool hungerModifier = false;



            if (GameManager.Instance.ListEventSelected.Count > 0)



            {



                if (GameManager.Instance.ListEventSelected.Contains("3"))



                {



                    if (GameManager.Instance.PrisonerInstance.CurrentTile.GetComponentInChildren<Climat>() != null && GameManager.Instance.PrisonerInstance.CurrentTile.GetComponentInChildren<Climat>().TypeClimat == TypeClimat.HeatDistorsion)



                    {



                        hungerModifier = true;



                    }



                }



            }







            if (hungerModifier)



                GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>().CurrentHunger -= 5;







            GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>().CurrentHunger -= 10;



        }







    }







    private static void ResetActionPointsForNextTurn()
    {
        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)
        {
            if (ki.GetComponent<LuckBased>() != null)
                ki.GetComponent<LuckBased>().HandleLuckForActionPoints();

            if (ki.GetComponent<Mortal>().IsAlive)
                ki.GetComponent<Keeper>().ActionPoints = ki.GetComponent<Keeper>().Data.MaxActionPoint;
        }
    }







    public static void ApplyEndTurnHungerPenalty()



    {



        foreach (PawnInstance ki in GameManager.Instance.AllKeepersList)



        {



            if (ki.GetComponent<Mortal>().IsAlive && ki.GetComponent<HungerHandler>().IsStarving)



            {



                if (ki.GetComponent<HungerHandler>().HasTakenHungerPenaltyThisTurn)



                    ki.GetComponent<HungerHandler>().HasTakenHungerPenaltyThisTurn = false;



                else



                    ki.GetComponent<Mortal>().CurrentHp -= hungerPenalty;



            }



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


                ki.GetComponent<MentalHealthHandler>().IsLowMentalHealthBuffApplied = true;


            }


        }


    }







    public static void HandleWeather()


    {


        if (GameManager.Instance.ListEventSelected.Count <= 0)


            return;



        weatherFrequency = 50.0f;



        Tile[] tiles = TileManager.Instance.Tiles.GetComponentsInChildren<Tile>();



        foreach (Tile tile in tiles)


        {


            HandleWeather(tile, false);


        }


    }







    public static void HandleWeather(Tile currentTile, bool wasJustDiscoveredByAKeeper)



    {



        if (GameManager.Instance.ListEventSelected.Count <= 0)



            return;







        // Snow events



        if (GameManager.Instance.ListEventSelected.Contains("1"))



        {



            if (GameManager.Instance.NbTurn % nbDayInMonth < 5)



            {



                if ((currentTile.Type == TileType.Snow || currentTile.Type == TileType.Forest) && currentTile.State == TileState.Discovered)



                {



                    if (currentTile.gameObject.GetComponentInChildren<Climat>() == null)



                    {



                        Debug.Log(currentTile.name + "n'a pas de climat en enfant");



                        return;



                    }



                    Climat climat = currentTile.gameObject.GetComponentInChildren<Climat>();



                    if (climat != null)



                    {



                        if (Random.Range(0, 100) <= weatherFrequency)



                        {



                            climat.TypeClimat = TypeClimat.Snow;



                            weatherFrequency /= 2.0f;



                        }



                        else



                        {



                            climat.TypeClimat = TypeClimat.None;



                            weatherFrequency += 20.0f;



                        }



                    }







                }



            }



        }



        else



        {



            if ((currentTile.Type == TileType.Snow || currentTile.Type == TileType.Forest) && currentTile.State == TileState.Discovered)



            {



                if (currentTile.gameObject.GetComponentInChildren<Climat>() == null)



                {



                    Debug.Log(currentTile.name + "n'a pas de climat en enfant");



                    return;



                }



                Climat climat = currentTile.gameObject.GetComponentInChildren<Climat>();



                if (climat != null)



                {



                    climat.TypeClimat = TypeClimat.None;



                }



            }



        }



        // HButterfly events



        if (GameManager.Instance.ListEventSelected.Contains("2"))



        {



            if (wasJustDiscoveredByAKeeper)



            {



                if (GameManager.Instance.NbTurn % nbDayInWeek == 5 || GameManager.Instance.NbTurn % nbDayInWeek == 6)



                {



                    if (currentTile.Type == TileType.Plain && currentTile.State == TileState.Discovered)



                    {



                        if (currentTile.gameObject.GetComponentInChildren<Climat>() == null)



                        {



                            Debug.Log(currentTile.name + "n'a pas de climat en enfant");



                            return;



                        }



                        Climat climat = currentTile.gameObject.GetComponentInChildren<Climat>();



                        if (climat != null)



                        {



                            climat.TypeClimat = TypeClimat.Butterfly;



                        }







                    }



                }



                else



                {



                    if (currentTile.Type == TileType.Plain && currentTile.State == TileState.Discovered)



                    {



                        if (currentTile.gameObject.GetComponentInChildren<Climat>() == null)



                        {



                            Debug.Log(currentTile.name + "n'a pas de climat en enfant");



                            return;



                        }



                        Climat climat = currentTile.gameObject.GetComponentInChildren<Climat>();



                        if (climat != null)



                        {



                            climat.TypeClimat = TypeClimat.None;



                        }



                    }



                }



            }



   



        }



        // HEat distorsion



        if (GameManager.Instance.ListEventSelected.Contains("3"))



        {



            if (GameManager.Instance.NbTurn % nbDayInWeek == 2 || GameManager.Instance.NbTurn % nbDayInWeek == 3)



            {



                if (currentTile.Type == TileType.Desert && currentTile.State == TileState.Discovered)



                {



                    if (currentTile.gameObject.GetComponentInChildren<Climat>() == null)



                    {



                        Debug.Log(currentTile.name + "n'a pas de climat en enfant");



                        return;



                    }



                    Climat climat = currentTile.gameObject.GetComponentInChildren<Climat>();



                    if (climat != null)



                    {



                        if (Random.Range(0, 100) <= weatherFrequency)



                        {



                            climat.TypeClimat = TypeClimat.HeatDistorsion;



                            weatherFrequency /= 2.0f;



                        }



                        else



                        {



                            climat.TypeClimat = TypeClimat.None;



                            weatherFrequency += 20.0f;



                        }



                    }



                }



            }



            else



            {



                if (currentTile.Type == TileType.Desert && currentTile.State == TileState.Discovered)



                {



                    if (currentTile.gameObject.GetComponentInChildren<Climat>() == null)



                    {



                        Debug.Log(currentTile.name + "n'a pas de climat en enfant");



                        return;



                    }



                    Climat climat = currentTile.gameObject.GetComponentInChildren<Climat>();



                    if (climat != null)



                    {



                        climat.TypeClimat = TypeClimat.None;



                    }



                }



            }



        }



    }



}




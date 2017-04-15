using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;

public class BattleHandler {
    private enum AttackType { Physical, Magical }
    // Current battle data
    private static Face[] lastThrowResult;
    // Is the prisoner on the tile where the battle is processed
    public static bool isPrisonerOnTile = false;
    private static Text battleLogger;
    // TODO: Reset these 3 @Anthony
    private static PawnInstance[] currentBattleMonsters;
    private static PawnInstance[] currentBattleKeepers;
    private static int turnId = 0;
    private static bool isVictorious;
    private static PawnInstance currentPawnTurn;
    private static PawnInstance currentTargetMonster;

    // Debug parameters
    private static bool isDebugModeActive = false;


    /// <summary>
    /// Autoselect keepers if there are not enough for a selection
    /// </summary>
    /// <param name="tile"></param>
    public static void StartBattleProcess(Tile tile)
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.battleSound, 0.5f);
        GameManager.Instance.CurrentState = GameState.InPause;
        if (currentBattleKeepers != null || currentBattleMonsters != null)
        {
            Debug.LogWarning("A battle is already in process.");
            return;
        }

        // Auto selection
        if (TileManager.Instance.KeepersOnTile[tile].Count <= 1)
        {
            List<PawnInstance> keepersForBattle = TileManager.Instance.KeepersOnTile[tile];
            if (TileManager.Instance.PrisonerTile == tile)
            {
                isPrisonerOnTile = true;
            }
            else
            {
                isPrisonerOnTile = false;
            }

            LaunchBattle(tile, keepersForBattle);
        }
        // Manual selection
        else
        {
            GameManager.Instance.OpenSelectBattleCharactersScreen(tile);
        }
    }


    /// <summary>
    /// Battle entry point, called to start the battle.
    /// </summary>
    /// <param name="tile">Tile where the battle happens</param>
    /// <param name="selectedKeepersForBattle">Keepers selected for the battle</param>
    public static void LaunchBattle(Tile tile, List<PawnInstance> selectedKeepersForBattle)
    {
        battleLogger = GameManager.Instance.BattleResultScreen.GetChild((int)BattleResultScreenChildren.Logger).GetComponentInChildren<Text>();
        battleLogger.text = string.Empty;

        currentBattleMonsters = new PawnInstance[TileManager.Instance.MonstersOnTile[tile].Count];
        for (int i = 0; i < TileManager.Instance.MonstersOnTile[tile].Count; i++)
            currentBattleMonsters[i] = TileManager.Instance.MonstersOnTile[tile][i];

        currentBattleKeepers = selectedKeepersForBattle.ToArray();

        ShiftTurn();
    }

    private static void HandleBattleEnding(Tile tile, List<PawnInstance> selectedKeepersForBattle)
    {
        bool isVictorious = true;
        if (isVictorious)
        {
            HandleBattleVictory(selectedKeepersForBattle, tile);
        }
        else
        {
            HandleBattleDefeat(selectedKeepersForBattle, TileManager.Instance.MonstersOnTile[tile]);
        }

        PrintResultsScreen(isVictorious);
        PostBattleCommonProcess(selectedKeepersForBattle, tile);
    }

    public static void ReceiveDiceThrowData(Face[] _result, ThrowType _throwType)
    {
        lastThrowResult = _result;
        switch (_throwType)
        {
            case ThrowType.Attack:
                int attackValue = 0;
                for (int i = 0; i < _result.Length; i++)
                {
                    switch (_result[i].Type)
                    {
                        case FaceType.Physical:
                            attackValue += _result[i].Value;
                            break;
                        case FaceType.Magical:
                        case FaceType.Defensive:
                        case FaceType.Support:
                            attackValue++;
                            break;
                    }
                    ResolveStandardAttack(attackValue);
                }
                break;
            case ThrowType.BeginTurn:
                for (int i = 0; i < _result.Length; i++)
                {
                    switch (_result[i].Type)
                    {
                        case FaceType.Physical:
                            currentPawnTurn.GetComponent<Fighter>().PhysicalSymbolStored += _result[i].Value;
                            break;
                        case FaceType.Magical:
                            currentPawnTurn.GetComponent<Fighter>().MagicalSymbolStored += _result[i].Value;
                            break;
                        case FaceType.Defensive:
                            currentPawnTurn.GetComponent<Fighter>().DefensiveSymbolStored += _result[i].Value;
                            break;
                        case FaceType.Support:
                            currentPawnTurn.GetComponent<Fighter>().SupportSymbolStored += _result[i].Value;
                            break;
                    }
                }
                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().ChangeState(UIBattleState.Actions);
                break;
            case ThrowType.Defense:
                // TODO: GUard button press behaviour
                break;
            case ThrowType.Special:
                // TODO: Attack button press behaviour
                break;
        }
    }

    private static void ResolveStandardAttack(int attackValue)
    {
        int damage = (int)(attackValue * ((float)attackValue / currentTargetMonster.GetComponent<Monster>().EffectiveDefense));
        currentTargetMonster.GetComponent<Mortal>().CurrentHp -= damage;
    }

    public static void ShiftTurn()
    {
        if (turnId > currentBattleKeepers.Length + currentBattleMonsters.Length)
            turnId = 0;

        if (turnId > currentBattleKeepers.Length)
        {
            currentPawnTurn = currentBattleMonsters[turnId];
            // TODO: resolve monster turn
        }
        else
        {
            currentPawnTurn = currentBattleKeepers[turnId];
            GameManager.Instance.GetBattleUI.SetActive(true);
        }

        turnId++;
    }

    private static bool BattleEndConditionsReached()
    {
        if (currentBattleMonsters.Length == 0)
        {
            isVictorious = true;
            return true;
        }

        if (currentBattleKeepers.Length == 0 || GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Mortal>().CurrentHp == 0)
        {
            isVictorious = false;
            return true;
        }

        return false;
    }

    /*
     * Auto resolve battle. Will later be replaced by EngageBattle. Returns true if the battle is won, else false. 
     */
    private static bool ResolveBattle(List<PawnInstance> keepers, Tile tile)
    { 
        List<PawnInstance> monsters = new List<PawnInstance>();
        monsters.AddRange(TileManager.Instance.MonstersOnTile[tile]);

        // TODO @Anthony temporary
        foreach (PawnInstance m in monsters)
            m.GetComponent<Behaviour.Mortal>().CurrentHp = 0;


        // Battle loop
        // -- Keepers turn
        // -- -- Keeper turn
        // -- -- -- Throw dices for special skills
        // -- -- -- -- Attack/Defend/Use special skill
        // -- -- -- -- -- if attack or defend -> throw dices again
        // -- -- -- -- -- for attack, apply result directly for offensive strike
        // -- -- -- -- -- for defense, add result to current defense stock, consumed first
        // -- -- -- -- -- if special skill, just resolve the special skill
        // -- Monsters turn
        // -- -- Monster turn
        // -- -- -- Same pattern? Fix set of skill battles with algorithm to determine which skill use?

        // General melee!
        //while (monsters.Count > 0 && keepers.Count > 0)
        //{
        //    // Keepers turn
        //    foreach (PawnInstance currentKeeper in keepers)
        //    {
        //        PawnInstance target;
        //        AttackType attackType = currentKeeper.Keeper.GetEffectiveStrength() > currentKeeper.Keeper.GetEffectiveIntelligence() ? AttackType.Physical : AttackType.Magical;

        //        target = GetTargetForAttack(monsters, attackType);
        //        int monsterIndexForDmgCalculation = 0;
        //        for (int i = 0; i < monsterNames.Length; i++)
        //        {
        //            if (target.Monster.CharacterName == monsterNames[i])
        //                monsterIndexForDmgCalculation = i;
        //        }
        //        // Inflict damage to target
        //        damageTakenByMonsters[monsterIndexForDmgCalculation] += KeeperDamageCalculation(currentKeeper, target, attackType);
        //        if (damageTakenByMonsters[monsterIndexForDmgCalculation] > monstersInitialHp[monsterIndexForDmgCalculation])
        //            damageTakenByMonsters[monsterIndexForDmgCalculation] = monstersInitialHp[monsterIndexForDmgCalculation];
        //        // Remove monster from list if dead
        //        if (target.CurrentHp <= 0)
        //        {
        //            BattleLog(target.Monster.CharacterName + " died.");
        //            monsters.Remove(target);
        //        }

        //        if (monsters.Count == 0)
        //            break;
        //    }

        //    // Monsters turn
        //    foreach (PawnInstance currentMonster in monsters)
        //    {
        //        bool isPrisonerTargeted = false;
        //        AttackType attackType = currentMonster.Monster.GetEffectiveStrength() > currentMonster.Monster.GetEffectiveIntelligence() ? AttackType.Physical : AttackType.Magical;

        //        if (isPrisonerOnTile)
        //        {
        //            float determineTarget = Random.Range(0, 100);
        //            if (determineTarget < ((100.0f / (keepers.Count + 2)) * 2))
        //            {
        //                isPrisonerTargeted = true;
        //            }
        //        }

        //        if (isPrisonerTargeted)
        //        {
        //            damageTaken[damageTaken.Length - 1] += MonsterDamageCalculation(currentMonster, null, attackType, true);
        //        }
        //        else
        //        {
        //            if (keepers.Count == 0)
        //            {
        //                break;
        //            }
        //            PawnInstance target = GetTargetForAttack(keepers);
        //            int keeperIndexForDmgCalculation = 0;
        //            for (int i = 0; i < keeperNames.Length; i++)
        //            {
        //                if (target.Keeper.CharacterName == keeperNames[i])
        //                    keeperIndexForDmgCalculation = i;
        //            }
        //            damageTaken[keeperIndexForDmgCalculation] += MonsterDamageCalculation(currentMonster, target, attackType);

        //            if (target.CurrentHp <= 0)
        //            {
        //                BattleLog(target.Keeper.CharacterName + " died.");
        //                keepers.Remove(target);
        //            }

        //        }
        //    }

        //    totalDamageTaken = damageTaken[0];
        //    for (int i = 1; i < damageTaken.Length; i++)
        //        totalDamageTaken += damageTaken[i];

        //    if (totalDamageTaken >= 50)
        //    {
        //        BattleLog("Over 50hp lost. Battle ends.");
        //        break;
        //    }
        //    else if (GameManager.Instance.PrisonerInstanceOld.CurrentHp <= 0)
        //    {
        //        BattleLog("Prisoner died. Battle ends.");
        //        break;
        //    }
        //    else if (keepers.Count == 0)
        //    {
        //        BattleLog("All keepers died. Battle ends.");
        //        break;
        //    }
        //}      

        //for (int i = 0; i < damageTaken.Length - 1; i++)
        //    BattleLog(keeperNames[i] + " lost " + damageTaken[i] + " health.");
        //if (isPrisonerOnTile)
        //    BattleLog("Prisoner lost " + damageTaken[damageTaken.Length - 1] + " health.");
        //for (int i = 0; i < damageTakenByMonsters.Length; i++)
        //{
        //    if (damageTakenByMonsters[i] > 0)
        //        BattleLog(monsterNames[i] + " lost " + damageTakenByMonsters[i] + " health.");
        //    BattleLog(monsterNames[i] + " has " + (monstersInitialHp[i] - damageTakenByMonsters[i]) + " health left.");
        //}
        
        //// Battle result
        //if (monsters.Count == 0)
        //{
        //    BattleLog("All monsters defeated.");
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}*/
        return true;
    }
    /*
    private static PawnInstance GetTargetForAttack(List<PawnInstance> monsters, AttackType attackType)
    {
        List<PawnInstance> subMonstersList = new List<PawnInstance>();
        foreach (PawnInstance mi in monsters)
        {
            if (mi.CurrentHp == 0)
            {
                continue;
            }

            if (attackType == AttackType.Physical)
            {
                if (mi.Monster.GetEffectiveSpirit() > mi.Monster.GetEffectiveDefense())
                    subMonstersList.Add(mi);
            }
            else
            {
                if (mi.Monster.GetEffectiveSpirit() < mi.Monster.GetEffectiveDefense())
                    subMonstersList.Add(mi);
            }
        }

        if (subMonstersList.Count == 0)
        {
            subMonstersList.AddRange(monsters);
        }

        PawnInstance target = null;
        int tmpHp = 100;

        foreach (PawnInstance mi in subMonstersList)
        {
            if (mi.CurrentHp <= tmpHp)
            {
                target = mi;
                tmpHp = mi.CurrentHp;
            }
        }

        return target;
    }

    private static PawnInstance GetTargetForAttack(List<PawnInstance> keepers)
    {
        return keepers[Random.Range(0, keepers.Count)];
    }

    private static int KeeperDamageCalculation(PawnInstance attacker, PawnInstance targetMonster, AttackType attackType)
    {
        int damage = 0;
        if (attackType == AttackType.Physical)
        {
            damage = Mathf.RoundToInt(Mathf.Pow(attacker.Keeper.GetEffectiveStrength(), 2) / targetMonster.Monster.GetEffectiveDefense());
        }
        else
        {
            damage = Mathf.RoundToInt(Mathf.Pow(attacker.Keeper.GetEffectiveIntelligence(), 2) / targetMonster.Monster.GetEffectiveSpirit());
        }

        damage = Mathf.RoundToInt(damage * Random.Range(0.75f, 1.25f));

        targetMonster.CurrentHp -= damage;
        //Debug.Log(attacker.Keeper.CharacterName + " deals " + damage + " damage to " + targetMonster.Monster.CharacterName + ".\n");
        //Debug.Log(targetMonster.Monster.CharacterName + " has " + targetMonster.CurrentHp + " left.\n");
        return damage;
    }

    private static int MonsterDamageCalculation(PawnInstance attacker, PawnInstance targetKeeper, AttackType attackType, bool prisonerTargeted = false)
    {
        int damage = 0;
        if (attackType == AttackType.Physical)
        {
            if (!prisonerTargeted)
                damage = Mathf.RoundToInt(attacker.Monster.GetEffectiveStrength() / targetKeeper.Keeper.GetEffectiveDefense());
            else
                damage = Mathf.RoundToInt(attacker.Monster.GetEffectiveStrength() / GameManager.Instance.PrisonerInstanceOld.Prisoner.GetEffectiveDefense());
        }
        else
        {
            if (!prisonerTargeted)
                damage = Mathf.RoundToInt(attacker.Monster.GetEffectiveIntelligence() / targetKeeper.Keeper.GetEffectiveSpirit());
            else
                damage = Mathf.RoundToInt(attacker.Monster.GetEffectiveIntelligence() / GameManager.Instance.PrisonerInstanceOld.Prisoner.GetEffectiveSpirit());
        }

        damage = Mathf.RoundToInt(damage * Random.Range(0.75f, 1.25f));

        if (prisonerTargeted)
        {
            GameManager.Instance.PrisonerInstanceOld.CurrentHp -= damage;
            //Debug.Log(attacker.Monster.CharacterName + " deals " + damage + " damage to prisoner.\n");
            //Debug.Log("Prisoner has " + GameManager.Instance.PrisonerInstance.CurrentHp + " left.\n");
        }
        else
        {
            targetKeeper.CurrentHp -= damage;
            //Debug.Log(attacker.Monster.CharacterName + " deals " + damage + " damage to " + targetKeeper.Keeper.CharacterName + ".\n");
            //Debug.Log(targetKeeper.Keeper.CharacterName + " has " + targetKeeper.CurrentHp + " left.\n");
        }

        return damage;
    }
    */
    /*
     * Process everything that needs to be processed after a victory
     */
    private static void HandleBattleVictory(List<PawnInstance> keepers, Tile tile)
    {
        foreach (PawnInstance ki in keepers)
        {
            ki.GetComponent<Behaviour.MentalHealthHandler>().CurrentMentalHealth += 10;
            ki.GetComponent<Behaviour.HungerHandler>().CurrentHunger -= 5;
            //BattleLog(ki.Keeper.CharacterName + " won 10 mental health and lost 5 hunger due to victory.");
        }

        /*
        if (isPrisonerOnTile)
        {
            GameManager.Instance.PrisonerInstanceOld.CurrentMentalHealth += 10;
            GameManager.Instance.PrisonerInstanceOld.CurrentHunger -= 5;
            //BattleLog("Prisoner won 10 mental health and lost 5 hunger due to victory.");
        }*/
    }

    /*
     * Process everything that needs to be processed after a defeat
     */
    private static void HandleBattleDefeat(List<PawnInstance> keepers, List<PawnInstance> monsters)
    {
        /*
        foreach (PawnInstance ki in keepers)
        {
            if (ki.IsAlive)
            {
                ki.CurrentMentalHealth -= 10;
                ki.CurrentHunger -= 5;

                ki.CurrentHp -= 10;
                //  BattleLog(ki.Keeper.CharacterName + " lost 10 mental health, 5 hunger, 10HP due to defeat.");
            }
            if (isPrisonerOnTile && GameManager.Instance.PrisonerInstanceOld.IsAlive)
            {
                GameManager.Instance.PrisonerInstanceOld.CurrentMentalHealth -= 10;
                GameManager.Instance.PrisonerInstanceOld.CurrentHunger -= 5;
                GameManager.Instance.PrisonerInstanceOld.CurrentHp -= 10;
               // BattleLog("Prisoner lost 10 mental health, 5 hunger, 10HP due to defeat.");
            }
        }

        foreach (PawnInstance ki in GameManager.Instance.ListOfSelectedKeepersOld)
        {
            if (ki.IsAlive)
            {
                ki.transform.position = ki.transform.position - ki.transform.forward * 0.5f;
            }
        }

        foreach (PawnInstance mi in monsters)
        {
            mi.RestAfterBattle();
        }*/
    }

    private static void PostBattleCommonProcess(List<PawnInstance> keepers, Tile tile)
    {
        TileManager.Instance.RemoveDefeatedMonsters(tile);
    }

    private static void PrintResultsScreen(bool isVictorious)
    {
        GameManager.Instance.BattleResultScreen.gameObject.SetActive(true);
        Transform header = GameManager.Instance.BattleResultScreen.GetChild((int)BattleResultScreenChildren.Header);

        header.GetComponentInChildren<Image>().color = isVictorious ? Color.green : Color.red;
        header.GetComponentInChildren<Text>().text = isVictorious ? "Victory!" : "Defeat";

        // Freeze time until close button is pressed
        GameManager.Instance.ClearListKeeperSelected();
        GameManager.Instance.CurrentState = GameState.InPause;
    }

    private static void BattleLog(string log)
    {
        string tmp = battleLogger.text;

        if (tmp.Length > 1500 && !isDebugModeActive)
        {
            int indexOfFirstEndline = tmp.IndexOf("\n") + 1;
            tmp = tmp.Substring(indexOfFirstEndline);
        }
        tmp += log + '\n';
        battleLogger.text = tmp;
    }

    public static PawnInstance CurrentPawnTurn
    {
        get
        {
            return currentPawnTurn;
        }

        set
        {
            currentPawnTurn = value;
        }
    }
}

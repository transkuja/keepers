using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;

public class BattleHandler {

    private enum AttackType { Physical, Magical }
    private static bool isProcessingABattle = false;

    // Current battle data
    private static Dictionary<PawnInstance, Face[]> lastThrowResult;
    // Is the prisoner on the tile where the battle is processed
    public static bool isPrisonerOnTile = false;
    private static Text battleLogger;
    // TODO: Reset these 3 @Anthony
    private static PawnInstance[] currentBattleMonsters;
    private static PawnInstance[] currentBattleKeepers;
    private static int nbTurn = 0;
    private static int nextMonsterIndex = 0;
    private static bool isVictorious;
    private static PawnInstance currentTargetMonster;
    // Default value is false because ShiftTurn is called at the beginning of battle
    private static bool isKeepersTurn = false;
    private static Die[] currentTurnDice;
    private static Dictionary<PawnInstance, List<GameObject>> currentTurnDiceInstance;
    private static bool hasDiceBeenThrown = false;

    private static List<GameObject> enabledLifeBars = new List<GameObject>();
    private static bool wasTheLastToPlay = false;

    // Debug parameters
    private static bool isDebugModeActive = false;

    public static bool IsABattleAlreadyInProcess()
    {
        return isProcessingABattle;
    }

    /// <summary>
    /// Autoselect keepers if there are not enough for a selection
    /// </summary>
    /// <param name="tile"></param>
    public static void StartBattleProcess(Tile tile)
    {
        isProcessingABattle = true;
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.battleSound, 0.5f);
        GameManager.Instance.CurrentState = GameState.InPause;
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

        GameManager.Instance.SetStateToInBattle(AllCurrentFighters());

        // Move pawns to battle positions and initialize ui info panel
        int offsetIndex = 0;
        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().OccupiedCharacterPanelIndex = new bool[3];

        if (isPrisonerOnTile)
        {
            Transform newTransform = TileManager.Instance.BattlePositions.GetChild(offsetIndex);
            GameManager.Instance.PrisonerInstance.GetComponent<AnimatedPawn>().StartMoveToBattlePositionAnimation(newTransform.localPosition, newTransform.localRotation);
            offsetIndex = 1;
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().CharacterPanelInit(GameManager.Instance.PrisonerInstance);
        }

        int keeperIndex = 0;
        for (int i = offsetIndex; i < offsetIndex + currentBattleKeepers.Length; i++)
        {
            Transform newTransform = TileManager.Instance.BattlePositions.GetChild(i);
            currentBattleKeepers[keeperIndex].GetComponent<AnimatedPawn>().StartMoveToBattlePositionAnimation(newTransform.localPosition, newTransform.localRotation);
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().CharacterPanelInit(currentBattleKeepers[keeperIndex]);
            keeperIndex++;
        }

        int monsterIndex = 0;
        for (int i = 3; i < 3 + currentBattleMonsters.Length; i++)
        {
            Transform newTransform = TileManager.Instance.BattlePositions.GetChild(i);
            currentBattleMonsters[monsterIndex].GetComponent<AnimatedPawn>().StartMoveToBattlePositionAnimation(newTransform.localPosition, newTransform.localRotation);
            monsterIndex++;
        }

        GameManager.Instance.GetBattleUI.SetActive(true);

        ShiftTurn();
    }

    private static PawnInstance[] AllCurrentFighters()
    {
        PawnInstance[] currentFighters = new PawnInstance[currentBattleKeepers.Length + currentBattleMonsters.Length + ((isPrisonerOnTile) ? 1 : 0)];
        for (int i = 0; i < currentBattleKeepers.Length; i++)
            currentFighters[i] = currentBattleKeepers[i];
        for (int i = 0; i < currentBattleMonsters.Length; i++)
            currentFighters[i + currentBattleKeepers.Length] = currentBattleMonsters[i];
        if (isPrisonerOnTile)
            currentFighters[currentBattleKeepers.Length + currentBattleMonsters.Length] = GameManager.Instance.PrisonerInstance;

        return currentFighters;
    }

    public static void ReceiveDiceThrowData(Dictionary<PawnInstance, Face[]> _result, Dictionary<PawnInstance, List<GameObject>> _diceInstances)
    {
        lastThrowResult = _result;
        currentTurnDiceInstance = _diceInstances;

        foreach (PawnInstance pi in lastThrowResult.Keys)
        {
            for (int i = 0; i < lastThrowResult[pi].Length; i++)
            {
                switch (lastThrowResult[pi][i].Type)
                {
                    case FaceType.Physical:
                        pi.GetComponent<Fighter>().PhysicalSymbolStored += lastThrowResult[pi][i].Value;
                        break;
                    case FaceType.Magical:
                        pi.GetComponent<Fighter>().MagicalSymbolStored += lastThrowResult[pi][i].Value;
                        break;
                    case FaceType.Defensive:
                        pi.GetComponent<Fighter>().DefensiveSymbolStored += lastThrowResult[pi][i].Value;
                        break;
                    case FaceType.Support:
                        pi.GetComponent<Fighter>().SupportSymbolStored += lastThrowResult[pi][i].Value;
                        break;
                }
            }
            pi.GetComponent<Fighter>().LastThrowResult = lastThrowResult[pi];
            pi.GetComponent<Fighter>().LastThrowDiceInstance = currentTurnDiceInstance[pi];
        }

        HasDiceBeenThrown = true;
    }

    private static void ClearDiceForNextThrow()
    {
        if (currentTurnDiceInstance != null)
        {
            foreach (PawnInstance pi in currentTurnDiceInstance.Keys)
            {
                for (int i = 0; i < currentTurnDiceInstance[pi].Count; i++)
                    GameObject.Destroy(currentTurnDiceInstance[pi][i]);
            }
        }
    }

    private static void ResolveStandardAttack(int attackValue)
    {
        int damage = (int)(attackValue * ((float)attackValue / currentTargetMonster.GetComponent<Monster>().EffectiveDefense));
        currentTargetMonster.GetComponent<Mortal>().CurrentHp -= damage;
    }

    public static void CheckTurnStatus()
    {
        if (BattleEndConditionsReached())
        {
            HandleBattleVictory(GameManager.Instance.ActiveTile);
            return;
        }
        
        bool mustShiftTurn = true;
        for (int i = 0; i < CurrentBattleKeepers.Length; i++)
        {
            if (!CurrentBattleKeepers[i].GetComponent<Fighter>().HasPlayedThisTurn)
            {
                mustShiftTurn = false;
            }            
        }
        wasTheLastToPlay = mustShiftTurn;

        if (mustShiftTurn)
        {
            ShiftTurn();
        }
    }

    public static void ShiftTurn()
    {
        isKeepersTurn = !isKeepersTurn;

        if (isKeepersTurn)
        {
            nbTurn++;
            hasDiceBeenThrown = false;

            // Initialization for keepers turn
            for (int i = 0; i < currentBattleKeepers.Length; i++)
            {
                currentBattleKeepers[i].GetComponent<Fighter>().HasPlayedThisTurn = false;
            }
            ClearDiceForNextThrow();
        }
        else
        {
            // Resolve turn for each monster then shift turn to keepers'
            MonsterTurn(0);
        }
    }

    public static void ShiftToNextMonsterTurn()
    {
        if (nextMonsterIndex + 1 < currentBattleMonsters.Length)
            MonsterTurn(nextMonsterIndex + 1);
        else
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().ChangeState(UIBattleState.WaitForDiceThrow);
    }

    private static void MonsterTurn(int _nextMonsterIndex)
    {
        for (int i = _nextMonsterIndex; i < currentBattleMonsters.Length; i++)
        {
            if (currentBattleMonsters[_nextMonsterIndex] != null && currentBattleMonsters[_nextMonsterIndex].GetComponent<Mortal>().CurrentHp <= 0)
            {
                nextMonsterIndex = i;
                break;
            }
        }

        if (nextMonsterIndex == currentBattleMonsters.Length - 1)
            ShiftTurn();

        PawnInstance target = GetTargetForAttack();
        Fighter monsterBattleInfo = currentBattleMonsters[nextMonsterIndex].GetComponent<Fighter>();
        SkillBattle skillUsed = monsterBattleInfo.BattleSkills[Random.Range(0, currentBattleMonsters[nextMonsterIndex].GetComponent<Fighter>().BattleSkills.Count)];
        skillUsed.UseSkill(currentBattleMonsters[nextMonsterIndex].GetComponent<Fighter>(), target);
    }

    private static PawnInstance GetTargetForAttack()
    {
        if (isPrisonerOnTile)
        {
            float determineTarget = Random.Range(0, 100);
            if (determineTarget < ((100.0f / (currentBattleKeepers.Length + 2)) * 2))
            {
                return GameManager.Instance.PrisonerInstance;
            }
        }

        return currentBattleKeepers[Random.Range(0, currentBattleKeepers.Length)];
    }

    private static bool BattleEndConditionsReached()
    {
        for (int i = 0; i < currentBattleMonsters.Length; i++)
        {
            if (currentBattleMonsters[i] != null && currentBattleMonsters[i].GetComponent<Mortal>().CurrentHp > 0)
                return false;
        }

        return true;
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
    private static void HandleBattleVictory(Tile tile)
    {
        for (int i = 0; i < currentBattleKeepers.Length; i++)
        {
            if (currentBattleKeepers[i] != null)
            {
                currentBattleKeepers[i].GetComponent<MentalHealthHandler>().CurrentMentalHealth += 10;
                currentBattleKeepers[i].GetComponent<HungerHandler>().CurrentHunger -= 5;
            }
        }

        if (isPrisonerOnTile)
        {
            GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>().CurrentHunger -= 5;
            //BattleLog("Prisoner won 10 mental health and lost 5 hunger due to victory.");
        }

        PrintResultsScreen(true);
        PostBattleCommonProcess();
    }

    /*
     * Process everything that needs to be processed after a defeat
     */
    public static void HandleBattleDefeat()
    {
        for (int i = 0; i < currentBattleKeepers.Length; i++)
        {
            if (currentBattleKeepers[i] != null)
            {
                currentBattleKeepers[i].GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 10;
                currentBattleKeepers[i].GetComponent<HungerHandler>().CurrentHunger -= 5;
                //  BattleLog(ki.Keeper.CharacterName + " lost 10 mental health, 5 hunger, 10HP due to defeat.");
            }
            if (isPrisonerOnTile)
            {
                GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>().CurrentHunger -= 5;
                // BattleLog("Prisoner lost 10 mental health, 5 hunger, 10HP due to defeat.");
            }
        }

        PrintResultsScreen(false);
        PostBattleCommonProcess();
    }

    public static void PostBattleCommonProcess()
    {
        for (int i = 0; i < currentBattleKeepers.Length; i++)
        {
            currentBattleKeepers[i].GetComponent<Fighter>().ResetValuesAfterBattle();
            currentBattleKeepers[i].GetComponent<AnimatedPawn>().StartMoveFromBattlePositionAnimation();
        }

        if (isPrisonerOnTile)
            GameManager.Instance.PrisonerInstance.GetComponent<AnimatedPawn>().StartMoveFromBattlePositionAnimation();

        for (int i = 0; i < currentBattleMonsters.Length; i++)
        {
            // TODO: test death cases
            if (currentBattleMonsters[i] != null)
            {
                if (currentBattleMonsters[i].GetComponent<Mortal>().CurrentHp > 0)
                {
                    currentBattleMonsters[i].GetComponent<AnimatedPawn>().StartMoveFromBattlePositionAnimation();
                    currentBattleMonsters[i].GetComponent<Fighter>().HasRecentlyBattled = true;
                    currentBattleMonsters[i].transform.GetChild(1).gameObject.SetActive(true);
                    GlowController.UnregisterObject(currentBattleMonsters[i].GetComponent<GlowObjectCmd>());
                }
            }
        }

        GameManager.Instance.GetBattleUI.gameObject.SetActive(false);
        ResetBattleHandler();
    }

    private static void ResetBattleHandler()
    {
        ClearDiceForNextThrow();
        isProcessingABattle = false;
        if (lastThrowResult != null)
            lastThrowResult.Clear();
        isPrisonerOnTile = false;
        battleLogger.text = "";
        currentBattleMonsters = null;
        currentBattleKeepers = null;
        nbTurn = 0;
        nextMonsterIndex = 0;
        isVictorious = false;
        currentTargetMonster = null;
        isKeepersTurn = false;
        currentTurnDice = null;
        hasDiceBeenThrown = false;
        wasTheLastToPlay = false;
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

    public static void EnableMonstersLifeBars()
    {
        if (currentBattleMonsters == null)
        {
            Debug.LogWarning("No monsters for current battle.");
            return;
        }
        for (int i = 0; i < currentBattleMonsters.Length; i++)
        {
            foreach (Transform child in currentBattleMonsters[i].transform)
            {
                if (child.CompareTag("FeedbackTransform"))
                {
                    GameObject lifeBar = child.GetChild(0).GetChild(1).gameObject;
                    Image lifeBarImg = lifeBar.transform.GetChild((int)LifeBarChildren.Remaining).GetComponent<Image>();
                    lifeBarImg.fillAmount = currentBattleMonsters[i].GetComponent<Mortal>().CurrentHp / (float)currentBattleMonsters[i].GetComponent<Mortal>().MaxHp;
                    if (lifeBarImg.fillAmount < 0.33f)
                    {
                        lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spriteOrangeLifeBar;
                    }
                    else
                    {
                        lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spriteGreenLifeBar;
                    }
                    lifeBar.SetActive(true);
                    enabledLifeBars.Add(lifeBar);
                    break;
                }
            }
        }
    }

    public static void DisableMonstersLifeBars()
    {
        foreach (GameObject lifeBar in enabledLifeBars)
            lifeBar.SetActive(false);
        enabledLifeBars.Clear();
    }

    public static void ActivateFeedbackSelection(bool _activateOnKeepers, bool _activateOnMonsters)
    {
        if (currentBattleKeepers == null || currentBattleMonsters == null)
        {
            return;
        }

        if (_activateOnKeepers)
        {
            for (int i = 0; i < currentBattleKeepers.Length; i++)
            {
                if (currentBattleKeepers[i] != null && currentBattleKeepers[i].GetComponent<Mortal>().CurrentHp > 0)
                {
                    if (!currentBattleKeepers[i].GetComponent<Fighter>().HasPlayedThisTurn)
                    {
                        currentBattleKeepers[i].GetComponent<Keeper>().FeedbackSelection.SetActive(true);
                        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateAvatar(currentBattleKeepers[i], true);
                    }
                    else
                    {
                        currentBattleKeepers[i].GetComponent<Keeper>().FeedbackSelection.SetActive(false);
                        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateAvatar(currentBattleKeepers[i], false);
                    }
                }
            }

        }
        // Activate on monsters
        if (_activateOnMonsters)
        {
            for (int i = 0; i < currentBattleMonsters.Length; i++)
            {
                if (currentBattleMonsters[i] != null && currentBattleMonsters[i].GetComponent<Mortal>().CurrentHp > 0)
                {
                    currentBattleMonsters[i].GetComponent<Monster>().PointerFeedback.SetActive(true);
                }
                else if (currentBattleMonsters[i] != null)
                {
                    currentBattleMonsters[i].GetComponent<Monster>().PointerFeedback.SetActive(false);
                }
            }
        }
    }

    public static void DeactivateFeedbackSelection(bool _deactivateOnKeepers, bool _deactivateOnMonsters)
    {
        if (currentBattleKeepers == null || currentBattleMonsters == null)
        {
            return;
        }

        if (_deactivateOnKeepers)
        {
            for (int i = 0; i < currentBattleKeepers.Length; i++)
            {
                if (currentBattleKeepers[i] != null && currentBattleKeepers[i].GetComponent<Mortal>().CurrentHp > 0)
                {
                    currentBattleKeepers[i].GetComponent<Keeper>().FeedbackSelection.SetActive(false);
                    GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateAvatar(currentBattleKeepers[i], false);
                }
            }
        }

        // Deactivate on monsters
        if (_deactivateOnMonsters)
        {
            for (int i = 0; i < currentBattleMonsters.Length; i++)
            {
                if (currentBattleMonsters[i] != null && currentBattleMonsters[i].GetComponent<Mortal>().CurrentHp > 0)
                {
                    currentBattleMonsters[i].GetComponent<Monster>().PointerFeedback.SetActive(false);
                }
            }
        }
    }

    public static bool IsKeepersTurn
    {
        get
        {
            return isKeepersTurn;
        }

        set
        {
            isKeepersTurn = value;
        }
    }

    public static PawnInstance[] CurrentBattleKeepers
    {
        get
        {
            return currentBattleKeepers;
        }

        set
        {
            currentBattleKeepers = value;
        }
    }

    public static bool HasDiceBeenThrown
    {
        get
        {
            return hasDiceBeenThrown;
        }

        private set
        {
            hasDiceBeenThrown = value;
            if (hasDiceBeenThrown == true)
            {
                for (int i = 0; i < currentBattleKeepers.Length; i++)
                {
                    ActivateFeedbackSelection(true, false);
                }
            }
        }

    }

    public static Dictionary<PawnInstance, Face[]> LastThrowResult
    {
        get
        {
            return lastThrowResult;
        }
    }

    public static bool WasTheLastToPlay
    {
        get
        {
            return wasTheLastToPlay;
        }

        set
        {
            wasTheLastToPlay = value;
        }
    }
}

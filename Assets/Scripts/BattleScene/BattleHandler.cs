using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;
using System.Collections;

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
    //private static Die[] currentTurnDice;
    private static Dictionary<PawnInstance, List<GameObject>> currentTurnDiceInstance;
    private static bool hasDiceBeenThrown = false;

    private static List<GameObject> enabledLifeBars = new List<GameObject>();
    private static bool wasTheLastToPlay = false;
    private static SkillBattle pendingSkill;
    private static bool isWaitingForSkillEnd = false;
    private static bool wasLaunchedDuringKeepersTurn;

    private static bool battleEndConditionsReached = false;

    // Debug parameters
    private static bool isDebugModeActive = false;

    private static int expectedAnswers = 0;
    private static int answersReceived = 0;
    private static List<ItemContainer> currentBattleLoot = new List<ItemContainer>();
    private static Tile archerPreviousTile;
    public static float CurrentSkillAnimDuration = 0.0f;

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
        battleEndConditionsReached = false;
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.battleSound, 0.5f);
        GameManager.Instance.CurrentState = GameState.InPause;
        // Auto selection

        if (TutoManager.s_instance && TutoManager.s_instance.enableTuto && TutoManager.s_instance.PlayingSequence == null
            && TutoManager.s_instance.GetComponent<SeqTutoCombat>().AlreadyPlayed == false)
        {
            List<PawnInstance> keepersForBattle = new List<PawnInstance>();
            // Only one keeper for tuto, TODO: ptet mettre celui qui déclenche le combat
            keepersForBattle.Add(TileManager.Instance.KeepersOnTile[tile][0]);
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
        else
        {
            if (TileManager.Instance.KeepersOnTile[tile].Count <= 1)
            {
                if (GameManager.Instance.ArcherInstance != null)
                {
                    Tile archerTile = GameManager.Instance.ArcherInstance.CurrentTile;
                    if (archerTile != null)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            if (archerTile == tile.Neighbors[i])
                            {
                                archerPreviousTile = archerTile;
                                GameManager.Instance.ArcherInstance.CurrentTile = tile;
                                GameManager.Instance.OpenSelectBattleCharactersScreen(tile);
                                return;
                            }
                        }
                    }
                }

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
                if (GameManager.Instance.ArcherInstance != null)
                {
                    Tile archerTile = GameManager.Instance.ArcherInstance.CurrentTile;
                    if (archerTile != null)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            if (archerTile == tile.Neighbors[i])
                            {
                                archerPreviousTile = archerTile;
                                GameManager.Instance.ArcherInstance.CurrentTile = tile;
                            }
                        }
                    }
                }
                GameManager.Instance.OpenSelectBattleCharactersScreen(tile);
            }
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
        MonsterType monsterType = MonsterType.Common;

        for (int i = 0; i < TileManager.Instance.MonstersOnTile[tile].Count && i < 3; i++)
        {
            currentBattleMonsters[i] = TileManager.Instance.MonstersOnTile[tile][i];
            if (currentBattleMonsters[i].GetComponent<Monster>().GetMType == MonsterType.Miniboss && monsterType != MonsterType.Epic)
            {
                monsterType = MonsterType.Miniboss;
            }
            else if (currentBattleMonsters[i].GetComponent<Monster>().GetMType == MonsterType.Epic)
            {
                monsterType = MonsterType.Epic;
            }
            if (currentBattleMonsters[i].GetComponent<QuestDealerFeedbackUpdater>() != null)
                currentBattleMonsters[i].GetComponent<QuestDealerFeedbackUpdater>().feedbackContainer.SetActive(false);
        }

        currentBattleKeepers = selectedKeepersForBattle.ToArray();

        GameManager.Instance.SetStateToInBattle(AllCurrentFighters());
        
        // Move pawns to battle positions and initialize ui info panel
        int offsetIndex = 0;
        GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().OccupiedCharacterPanelIndex = new bool[3];
        bool isBeachTile = GameManager.Instance.ActiveTile.Type == TileType.Beach;

        if (isPrisonerOnTile)
        {
            Transform newTransform = TileManager.Instance.BattlePositions.GetChild(offsetIndex);
            GameManager.Instance.PrisonerInstance.GetComponent<AnimatedPawn>().StartMoveToBattlePositionAnimation(newTransform.localPosition + ((isBeachTile) ? (-Vector3.up *0.11f) : Vector3.zero), newTransform.localRotation);
            offsetIndex = 1;
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().CharacterPanelInit(GameManager.Instance.PrisonerInstance);
        }

        int keeperIndex = 0;
        for (int i = offsetIndex; i < offsetIndex + currentBattleKeepers.Length; i++)
        {
            Transform newTransform = TileManager.Instance.BattlePositions.GetChild(i);
            currentBattleKeepers[keeperIndex].GetComponent<AnimatedPawn>().StartMoveToBattlePositionAnimation(newTransform.localPosition + ((isBeachTile) ? (-Vector3.up * 0.11f) : Vector3.zero), newTransform.localRotation);
            GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().CharacterPanelInit(currentBattleKeepers[keeperIndex]);
            keeperIndex++;
        }
        
        int monsterIndex = 0;
        for (int i = 3; i < 3 + currentBattleMonsters.Length; i++)
        {
            Transform newTransform = TileManager.Instance.BattlePositions.GetChild(i);
            currentBattleMonsters[monsterIndex].GetComponent<AnimatedPawn>().StartMoveToBattlePositionAnimation(newTransform.localPosition + ((isBeachTile) ? (-Vector3.up * 0.11f) : Vector3.zero), newTransform.localRotation);
            monsterIndex++;
        }

        AudioManager.Instance.PlayBattleMusic(monsterType);

        GameManager.Instance.GetBattleUI.SetActive(true);
        DeactivateFeedbackSelection(true, false);

        ShiftTurn();

        if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto && TutoManager.s_instance.GetComponent<SeqTutoCombat>().AlreadyPlayed == false)
        {
            TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqTutoCombat>());
        }
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

    public static void CheckTurnStatus()
    {
        if (BattleEndConditionsReached())
        {
            if (isVictorious)
                HandleBattleVictory(GameManager.Instance.ActiveTile);
            else
                HandleBattleDefeat();
            return;
        }
        
        bool mustShiftTurn = true;
        for (int i = 0; i < CurrentBattleKeepers.Length; i++)
        {
            if (!CurrentBattleKeepers[i].GetComponent<Fighter>().HasPlayedThisTurn && CurrentBattleKeepers[i].GetComponent<Mortal>().CurrentHp > 0)
            {
                mustShiftTurn = false;
            }            
        }
        if (isPrisonerOnTile && !GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().HasPlayedThisTurn)
            mustShiftTurn = false;

        wasTheLastToPlay = mustShiftTurn;

        if (mustShiftTurn)
        {
            DeactivateFeedbackSelection(true, false);
            ShiftTurn();
        }
        else
        {
            ActivateFeedbackSelection(true, false);
            DeactivateFeedbackSelection(false, true);
        }
    }

    public static void ShiftTurn()
    {
        isKeepersTurn = !isKeepersTurn;
        if (isKeepersTurn)
        {
            NbTurn++;
            hasDiceBeenThrown = false;

            // Initialization for keepers turn
            for (int i = 0; i < currentBattleKeepers.Length; i++)
            {
                currentBattleKeepers[i].GetComponent<Fighter>().HasPlayedThisTurn = false;
                if (isPrisonerOnTile) GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().HasPlayedThisTurn = false;
            }
            ClearDiceForNextThrow();
            if (GameManager.Instance.CurrentState != GameState.InTuto)
                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().ChangeState(UIBattleState.WaitForDiceThrow);
        }
        else
        {
            // Resolve turn for each monster then shift turn to keepers'
            nextMonsterIndex = 0;
            ShiftToNextMonsterTurn();
        }
    }

    private static void ShiftToNextMonsterTurn()
    {
        while (currentBattleMonsters[nextMonsterIndex].GetComponent<Mortal>().CurrentHp <= 0)
        {
            nextMonsterIndex++;
            if (nextMonsterIndex >= currentBattleMonsters.Length)
                break;
        }

        if (nextMonsterIndex >= currentBattleMonsters.Length)
        {
            if (TutoManager.s_instance != null && TutoManager.s_instance.PlayingSequence != null && TutoManager.s_instance.PlayingSequence.CurrentState == SequenceState.WaitingForExternalEvent)
            {
                TutoManager.s_instance.PlayingSequence.Play();
            }
            ShiftTurn();
            return;
        }

        PawnInstance target = GetTargetForAttack();
        Fighter monsterBattleInfo = currentBattleMonsters[nextMonsterIndex].GetComponent<Fighter>();
        monsterBattleInfo.UseSkill(target);
        nextMonsterIndex++;
    }

    public static void ResetCurrentBattleMonsters()
    {
        if (currentBattleMonsters.Length <= 1)
            return;

        int deathNbr = 0;
        for (int i = 0; i < currentBattleMonsters.Length; i++)
        {
            if (currentBattleMonsters[i] == null)
                deathNbr++;
        }
        PawnInstance[] newCurrentMonsters = new PawnInstance[currentBattleMonsters.Length - deathNbr];

        int j = 0;
        for (int i = 0; i < currentBattleMonsters.Length - deathNbr; i++)
        {
            if (currentBattleMonsters[i + j] == null)
                j++;
            newCurrentMonsters[i] = currentBattleMonsters[i + j];
        }

        currentBattleMonsters = null;
        currentBattleMonsters = newCurrentMonsters;
    }


    private static PawnInstance GetTargetForAttack()
    {
        if (isPrisonerOnTile)
        {
            for (int i = 0; i < currentBattleKeepers.Length; i++)
            {
                // Gérer les aggro ici, Ashley a 50 d'aggro (50%) de base
            }
            float determineTarget = Random.Range(0, 100);
            if (determineTarget < ((100.0f / (currentBattleKeepers.Length + 2)) * 2))
            {
                return GameManager.Instance.PrisonerInstance;
            }
        }

        int deathNbr = 0;
        for (int i = 0; i < currentBattleKeepers.Length; i++)
        {
            if (currentBattleKeepers[i].GetComponent<Mortal>().CurrentHp <= 0)
                deathNbr++;
        }

        if (deathNbr == currentBattleKeepers.Length)
            return GameManager.Instance.PrisonerInstance;

        int[] alreadyPickedIndex = new int[currentBattleKeepers.Length];
        for (int i = 0; i < alreadyPickedIndex.Length; i++)
            alreadyPickedIndex[i] = -1;

        PawnInstance target = null;
        int drawIndex = 0;

        int newIndex = 0;
        bool newIndexHasAlreadyBeenPicked = true;
        while (target == null)
        {
            while (newIndexHasAlreadyBeenPicked)
            {
                newIndex = Random.Range(0, currentBattleKeepers.Length);
                for (int i = 0; i < alreadyPickedIndex.Length; i++)
                {
                    if (newIndex == alreadyPickedIndex[i])
                    {
                        newIndexHasAlreadyBeenPicked = true;
                        break;
                    }
                    else
                    {
                        if (currentBattleKeepers[newIndex].GetComponent<Mortal>().CurrentHp <= 0)
                        {
                            alreadyPickedIndex[drawIndex] = newIndex;
                            newIndexHasAlreadyBeenPicked = true;
                            break;
                        }
                    }
                    newIndexHasAlreadyBeenPicked = false;
                }
            }

            target = currentBattleKeepers[newIndex];

            // Safety: Remove when a lot of tests have been performed
            if (drawIndex >= alreadyPickedIndex.Length)
            {
                Debug.LogError("GetTargetForAttack Loop safety error!");
                for (int i = 0; i < currentBattleKeepers.Length; i++)
                {
                    if (currentBattleKeepers[i].GetComponent<Mortal>().CurrentHp > 0)
                        target = currentBattleKeepers[i];
                }
            }
        }

        return target;
    }

    private static bool BattleEndConditionsReached()
    {
        bool isWinningConditionReached = false;
        bool isLosingConditionReached = false;

        int nbDeadMonsters = 0;
        for (int i = 0; i < currentBattleMonsters.Length; i++)
        {
            if (currentBattleMonsters[i] != null && currentBattleMonsters[i].GetComponent<Mortal>().CurrentHp <= 0)
            {
                nbDeadMonsters++;
            }
        }
        isWinningConditionReached = (nbDeadMonsters == currentBattleMonsters.Length);

        int nbDeadKeepers = 0;
        for (int i = 0; i < currentBattleKeepers.Length; i++)
        {
            if (currentBattleKeepers[i] != null && currentBattleKeepers[i].GetComponent<Mortal>().CurrentHp <= 0)
            {
                nbDeadKeepers++;
            }
        }
        isLosingConditionReached = ((isPrisonerOnTile && GameManager.Instance.PrisonerInstance.GetComponent<Mortal>().CurrentHp <= 0) || (nbDeadKeepers == currentBattleKeepers.Length));

        isVictorious = isWinningConditionReached && !isLosingConditionReached;

        battleEndConditionsReached = (isWinningConditionReached || isLosingConditionReached);
        return battleEndConditionsReached;
    }

    /*
     * Process everything that needs to be processed after a victory
     */
    private static void HandleBattleVictory(Tile tile)
    {
        for (int i = 0; i < currentBattleKeepers.Length; i++)
        {
            if (currentBattleKeepers[i] != null)
            {
                currentBattleKeepers[i].GetComponent<AnimatedPawn>().Anim.SetTrigger("dance");
            }
        }

        if (isPrisonerOnTile)
        {
            GameManager.Instance.PrisonerInstance.GetComponent<AnimatedPawn>().Anim.SetTrigger("dance");
        }
        
        PrintResultsScreen(true);
        PostBattleCommonProcess();
    }

    /*
     * Process everything that needs to be processed after a defeat
     */
    public static void HandleBattleDefeat()
    {
        PrintResultsScreen(false);
        PostBattleCommonProcess();
    }

    public static void PostBattleCommonProcess()
    {
        for (int i = 0; i < currentBattleKeepers.Length; i++)
        {
            currentBattleKeepers[i].GetComponent<Fighter>().ResetValuesAfterBattle();
            if (GameManager.Instance.ArcherInstance != null)
            {
                if (archerPreviousTile != null)
                    GameManager.Instance.ArcherInstance.CurrentTile = archerPreviousTile;
            }
            currentBattleKeepers[i].GetComponent<AnimatedPawn>().StartMoveFromBattlePositionAnimation();
        }

        if (isPrisonerOnTile)
        {
            GameManager.Instance.PrisonerInstance.GetComponent<AnimatedPawn>().StartMoveFromBattlePositionAnimation();
            GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().ResetValuesAfterBattle();
        }

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
                    currentBattleMonsters[i].GetComponent<Fighter>().EffectiveBoeufs.Clear();
                    currentBattleMonsters[i].GetComponent<Fighter>().UpdateActiveBoeufs();
                    currentBattleMonsters[i].GetComponentInChildren<BuffFeedback>().ShowBuffs(false);
                    if (currentBattleMonsters[i].GetComponent<QuestDealerFeedbackUpdater>() != null)
                        currentBattleMonsters[i].GetComponent<QuestDealerFeedbackUpdater>().feedbackContainer.SetActive(true);
                }
                else
                {
                    GameObject.Destroy(currentBattleMonsters[i].gameObject, 0.5f);
                }
            }
        }

        GameManager.Instance.GetBattleUI.gameObject.SetActive(false);
        GameManager.Instance.ClearListKeeperSelected();
        for (int i = 0; i < currentBattleKeepers.Length; i++)
        {
            if (currentBattleKeepers[i].GetComponent<Mortal>().CurrentHp > 0)
            {
                GameManager.Instance.AddKeeperToSelectedList(currentBattleKeepers[i]);
                currentBattleKeepers[i].GetComponent<Keeper>().IsSelected = true;
                currentBattleKeepers[i].GetComponent<Fighter>().EffectiveBoeufs.Clear();
                currentBattleKeepers[i].GetComponent<Fighter>().UpdateActiveBoeufs();
                currentBattleKeepers[i].GetComponentInChildren<BuffFeedback>().ShowBuffs(false);
            }
        }

        if (isPrisonerOnTile)
        {
            GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().EffectiveBoeufs.Clear();
            GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().UpdateActiveBoeufs();
            GameManager.Instance.PrisonerInstance.GetComponentInChildren<BuffFeedback>().ShowBuffs(false);
        }

        ItemManager.AddItemOnTheGround(GameManager.Instance.ActiveTile, GameManager.Instance.ActiveTile.transform, currentBattleLoot.ToArray());
        
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
        //currentTurnDice = null;
        hasDiceBeenThrown = false;
        wasTheLastToPlay = false;
        PendingSkill = null;
        isWaitingForSkillEnd = false;
        battleEndConditionsReached = false;
        currentBattleLoot.Clear();
        archerPreviousTile = null;
    }

    public static void ResetBattleHandlerForTuto()
    {
        nbTurn = 0;
        isKeepersTurn = false;
        PendingSkill = null;
        currentBattleLoot.Clear();
        ShiftTurn();
    }

    private static void PrintResultsScreen(bool isVictorious)
    {
        GameManager.Instance.BattleResultScreen.gameObject.SetActive(true);
        Transform header = GameManager.Instance.BattleResultScreen.GetChild((int)BattleResultScreenChildren.Header);

        header.GetComponentInChildren<Text>().color = isVictorious ? Color.green : new Color(0.75f, 0,0,1);
        header.GetComponentInChildren<Text>().text = isVictorious ? "Victory!" : "Defeat";
        if (isVictorious)
            AudioManager.Instance.Fade(AudioManager.Instance.winningMusic, 0.2f);

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
                    lifeBarImg.sprite = GameManager.Instance.SpriteUtils.spriteMonsterGreenLifeBar;
                    if (lifeBarImg.fillAmount < 0.33f)
                    {
                        // TODO: wait for white lifebars
                        lifeBarImg.color = Color.white;
                    }
                    else
                    {
                        lifeBarImg.color = Color.white;
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
            if(lifeBar != null)
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
            if (isPrisonerOnTile)
            {
                PawnInstance pi = GameManager.Instance.PrisonerInstance;
                if (!pi.GetComponent<Fighter>().HasPlayedThisTurn)
                {
                    pi.GetComponent<Prisoner>().FeedbackSelection.SetActive(true);
                    GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateAvatar(pi, true);
                }
                else
                {
                    pi.GetComponent<Prisoner>().FeedbackSelection.SetActive(false);
                    GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateAvatar(pi, false);
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
            if (isPrisonerOnTile)
            {
                GameManager.Instance.PrisonerInstance.GetComponent<Prisoner>().FeedbackSelection.SetActive(false);
                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UpdateAvatar(GameManager.Instance.PrisonerInstance, false);
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

    public static void WaitForSkillConfirmation(SkillBattle _skillData)
    {
        if (_skillData.SkillUser.GetComponent<LuckBased>() != null)
            PendingSkill = _skillData.SkillUser.GetComponent<LuckBased>().HandleLuckForSkills(_skillData);
        else
            PendingSkill = _skillData;
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
                if (currentBattleKeepers.Length == 1 && GameManager.Instance.CurrentState != GameState.InTuto)
                {
                    GameManager.Instance.ClearListKeeperSelected();
                    GameManager.Instance.AddKeeperToSelectedList(currentBattleKeepers[0]);
                    currentBattleKeepers[0].GetComponent<Keeper>().IsSelected = true;
                    GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(currentBattleKeepers[0]).gameObject.SetActive(true);
                }
                else
                {
                    for (int i = 0; i < currentBattleKeepers.Length; i++)
                    {
                        ActivateFeedbackSelection(true, false);
                    }
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

    public static SkillBattle PendingSkill
    {
        get
        {
            return pendingSkill;
        }

        set
        {
            if (value != null)
                pendingSkill = new SkillBattle(value);
            else
                pendingSkill = null;
        }
    }

    public static bool IsWaitingForSkillEnd
    {
        get
        {
            return isWaitingForSkillEnd;
        }

        set
        {

            //if (isWaitingForSkillEnd == false && value == false)
            //    return;

            isWaitingForSkillEnd = value;

            if (isWaitingForSkillEnd == false)
            {
                answersReceived++;
                if (answersReceived != expectedAnswers)
                    return;

                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().SkillName.SetActive(false);
                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().UnlockCharactersPanelButtons();
                if (pendingSkill != null && pendingSkill.SkillUser != null && (pendingSkill.SkillUser.GetComponent<Keeper>() != null || pendingSkill.SkillUser.GetComponent<Prisoner>() != null))
                {
                    pendingSkill.SkillUser.HasPlayedThisTurn = true;
                    pendingSkill = null;
                    return;
                }

                if (wasLaunchedDuringKeepersTurn)
                {
                    ActivateFeedbackSelection(true, false);
                }
                else
                {
                    if (BattleEndConditionsReached())
                    {
                        if (isVictorious)
                            HandleBattleVictory(GameManager.Instance.ActiveTile);
                        else
                            HandleBattleDefeat();

                        pendingSkill = null;
                        return;
                    }

                    if (nextMonsterIndex == currentBattleMonsters.Length)
                    {
                        pendingSkill = null;
                        if (TutoManager.s_instance != null && TutoManager.s_instance.PlayingSequence != null && TutoManager.s_instance.PlayingSequence.CurrentState == SequenceState.WaitingForExternalEvent)
                        {
                            TutoManager.s_instance.PlayingSequence.Play();
                        }
                        ShiftTurn();
                    }
                    else
                    {
                        pendingSkill = null;
                        ShiftToNextMonsterTurn();
                    }

                }
            }
            else
            {
                wasLaunchedDuringKeepersTurn = IsKeepersTurn;
                DeactivateFeedbackSelection(true, true);
            }
        }
    }

    public static PawnInstance[] CurrentBattleMonsters
    {
        get
        {
            return currentBattleMonsters;
        }

        set
        {
            currentBattleMonsters = value;
        }
    }

    public static int NbTurn
    {
        get
        {
            return nbTurn;
        }

        set
        {
            nbTurn = value;
            for (int i = 0; i < currentBattleKeepers.Length; i++)
            {
                if (currentBattleKeepers[i].GetComponent<Mortal>().IsAlive)
                    currentBattleKeepers[i].GetComponent<Fighter>().UpdateActiveBoeufs();
            }
            if (isPrisonerOnTile)
                GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().UpdateActiveBoeufs();
            for (int i = 0; i < currentBattleMonsters.Length; i++)
            {
                if (currentBattleMonsters[i].GetComponent<Mortal>().IsAlive)
                    currentBattleMonsters[i].GetComponent<Fighter>().UpdateActiveBoeufs();
            }
        }
    }

    public static int ExpectedAnswers
    {
        get
        {
            return expectedAnswers;
        }

        set
        {
            expectedAnswers = value;
            answersReceived = 0;
        }
    }

    public static List<ItemContainer> CurrentBattleLoot
    {
        get
        {
            return currentBattleLoot;
        }

        set
        {
            currentBattleLoot = value;
        }
    }

    public static Tile ArcherPreviousTile
    {
        get
        {
            return archerPreviousTile;
        }

        set
        {
            archerPreviousTile = value;
        }
    }
}

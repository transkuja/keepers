using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler {

    /*
     * Battle entry point, called to start the battle.
     */
    public static void LaunchBattle(Tile tile)
    {
        List<KeeperInstance> keepersUsedForBattle = SelectKeepersForBattle(tile);

        if (ResolveBattle(keepersUsedForBattle, tile))
        {
            HandleBattleVictory(keepersUsedForBattle, tile);
        }
        else
        {
            HandleBattleDefeat(keepersUsedForBattle);
        }
    }

    private static List<KeeperInstance> SelectKeepersForBattle(Tile tile)
    {
        List<KeeperInstance> keepers = TileManager.Instance.KeepersOnTile[tile];

        // TODO: @ Anthony, implement selection from list
        List<KeeperInstance> keepersUsedForBattle = new List<KeeperInstance>();

        for (int i = 0; i < 3 && i < keepers.Count; i++)
        {
            keepersUsedForBattle.Add(keepers[i]);
        }

        return keepersUsedForBattle;
    }

    /*
     * Auto resolve battle. Will later be replaced by EngageBattle. Returns true if the battle is won, else false. 
     */
    private static bool ResolveBattle(List<KeeperInstance> keepers, Tile tile)
    {
        List<MonsterInstance> monsters = TileManager.Instance.MonstersOnTile[tile];

        // General melee!

        // Mock

        if (keepers[0].Keeper.CharacterName == "MockBattle")
        {
            Debug.Log("Battle lost");
            return false;
        }

        Debug.Log("Battle won! Yippi!");
        return true;
    }


    /*
     * Process everything that needs to be processed after a victory
     */
    private static void HandleBattleVictory(List<KeeperInstance> keepers, Tile tile)
    {
        foreach (KeeperInstance ki in keepers)
        {
            ki.Keeper.ActualMentalHealth += 10;
            ki.Keeper.ActualHunger += 5;
        }

        TileManager.Instance.RemoveDefeatedMonsters(tile);
    }

    /*
     * Process everything that needs to be processed after a defeat
     */
    private static void HandleBattleDefeat(List<KeeperInstance> keepers)
    {
        foreach (KeeperInstance ki in keepers)
        {
            ki.Keeper.ActualMentalHealth -= 10;
            ki.Keeper.ActualHunger += 5;

            // TODO: @Anthony refactor Character to have base<Stat>s, bonusTo<Stat>s, currentMP, currentHP
            ki.Keeper.Hp -= 10;
        }
    }
}

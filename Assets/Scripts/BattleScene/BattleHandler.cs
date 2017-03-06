using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler {

    /*
     * Battle entry point, called to start the battle.
     */
    public static void LaunchBattle(Tile tile, List<KeeperInstance> selectedKeepersForBattle)
    {
        if (ResolveBattle(selectedKeepersForBattle, tile))
        {
            HandleBattleVictory(selectedKeepersForBattle, tile);
        }
        else
        {
            HandleBattleDefeat(selectedKeepersForBattle);
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
        PrintVictoryScreen();
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

            ki.Keeper.CurrentHp -= 10;
        }

        foreach (KeeperInstance ki in GameManager.Instance.ListOfSelectedKeepers)
        {
            ki.transform.position = ki.transform.position - ki.transform.forward * 0.5f;
        }

        PrintDefeatScreen(10, 5, 10);
    }

    private static void PrintVictoryScreen()
    {
        GameManager.Instance.BattleResultScreen.gameObject.SetActive(true);

        Transform header = GameManager.Instance.BattleResultScreen.GetChild((int)BattleResultScreenChildren.Header);
        Transform loot = GameManager.Instance.BattleResultScreen.GetChild((int)BattleResultScreenChildren.Loot);

        header.GetComponent<Text>().text = "Victory!";
        GameManager.Instance.BattleResultScreen.GetChild((int)BattleResultScreenChildren.Lost).gameObject.SetActive(false);

        loot.GetComponent<Text>().text = "Legendary Meat!\nPaper Sword!";

    }

    private static void PrintDefeatScreen(int moraleLost, int hungerIncreased, int hpLost)
    {
        GameManager.Instance.BattleResultScreen.gameObject.SetActive(true);

        Transform header = GameManager.Instance.BattleResultScreen.GetChild((int)BattleResultScreenChildren.Header);
        Transform lost = GameManager.Instance.BattleResultScreen.GetChild((int)BattleResultScreenChildren.Lost);

        header.GetComponent<Text>().text = "Defeat";
        GameManager.Instance.BattleResultScreen.GetChild((int)BattleResultScreenChildren.Loot).gameObject.SetActive(false);

        lost.GetComponent<Text>().text = hpLost + " HP lost\n"
                                        + moraleLost + " morale lost\n"
                                        + "Hunger increased by " + hungerIncreased;

        // Freeze time until close button is pressed
        GameManager.Instance.ClearListKeeperSelected();
        Time.timeScale = 0.0f;
    }

}

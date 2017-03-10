using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler {

    // Is the prisoner on the tile where the battle is processed
    public static bool isPrisonerOnTile = false;

    /// <summary>
    /// Autoselect keepers if there are not enough for a selection
    /// </summary>
    /// <param name="tile"></param>
    public static void StartBattleProcess(Tile tile)
    {
        // Auto selection
        if (TileManager.Instance.KeepersOnTile[tile].Count <= 1)
        {
            List<KeeperInstance> keepersForBattle = TileManager.Instance.KeepersOnTile[tile];
            if (TileManager.Instance.PrisonerTile == tile)
            {
                isPrisonerOnTile = true;
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
            ki.CurrentMentalHealth += 10;
            ki.CurrentHunger += 5;

            if (isPrisonerOnTile)
            {
                GameManager.Instance.PrisonerInstance.CurrentMentalHealth += 10;
                GameManager.Instance.PrisonerInstance.CurrentHunger += 5;
            }
        }

        PrintVictoryScreen();
        Item[] loot = ComputeTotalLoot(tile);
        TileManager.Instance.RemoveDefeatedMonsters(tile);

        // TODO: @Remi Open loot window
        IngameScreens.Instance.goInventoryLoot.GetComponentInParent<Inventory>().inventory = loot;
        IngameScreens.Instance.UpdateLootInterface();
    }

    /*
     * Process everything that needs to be processed after a defeat
     */
    private static void HandleBattleDefeat(List<KeeperInstance> keepers)
    {
        foreach (KeeperInstance ki in keepers)
        {
            ki.CurrentMentalHealth -= 10;
            ki.CurrentHunger += 5;

            ki.CurrentHp -= 10;

            if (isPrisonerOnTile)
            {
                GameManager.Instance.PrisonerInstance.CurrentMentalHealth -= 10;
                GameManager.Instance.PrisonerInstance.CurrentHunger += 5;
                GameManager.Instance.PrisonerInstance.CurrentHp -= 10;
            }
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

        header.GetComponent<Text>().text = "Victory!";
        GameManager.Instance.BattleResultScreen.GetChild((int)BattleResultScreenChildren.Lost).gameObject.SetActive(false);

        IngameScreens.Instance.CreateLootInterface();
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

    private static Item[] ComputeTotalLoot(Tile tile)
    {
        List<Item> lootList = new List<Item>();
        foreach (MonsterInstance mi in TileManager.Instance.MonstersOnTile[tile])
        {
            lootList.AddRange(mi.GetComponent<Loot>().ComputeLoot());
        }

        return lootList.ToArray();
    }
}

using UnityEngine;
using UnityEngine.UI;
using Behaviour;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DebugControls : MonoBehaviour {

    public static bool isDebugModeActive = false;
    bool isUnlimitedActionPointsModeActive = false;
    bool isMapUncovered = false;

    [SerializeField]
    GameObject monsterToPopPrefab;
    [SerializeField]
    [Range(1, 99)] int itemsToPopAmount = 1;
    [SerializeField]
    GameObject debugCanvas;

    Dictionary<Tile, TileState> oldTileStates = new Dictionary<Tile, TileState>();

    void Start () {
        isDebugModeActive = false;
        isUnlimitedActionPointsModeActive = false;
        debugCanvas.SetActive(false);
    }
	
	void Update () {
		if (Input.GetKey(KeyCode.LeftControl)
            && Input.GetKeyDown(KeyCode.RightControl))
        {
            isDebugModeActive = !isDebugModeActive;
            debugCanvas.SetActive(isDebugModeActive);
            Debug.Log("Debug mode now active.");
        }

        if (isDebugModeActive)
        {
            // Help window
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                debugCanvas.SetActive(!debugCanvas.activeInHierarchy);
            }

            // Unlimited action points
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                if (GameManager.Instance.CurrentState == GameState.InBattle)
                {
                    for (int i = 0; i < BattleHandler.CurrentBattleKeepers.Length; i++)
                    {
                        BattleHandler.CurrentBattleKeepers[i].GetComponent<Fighter>().PhysicalSymbolStored = 9;
                        BattleHandler.CurrentBattleKeepers[i].GetComponent<Fighter>().DefensiveSymbolStored = 9;
                        BattleHandler.CurrentBattleKeepers[i].GetComponent<Fighter>().MagicalSymbolStored = 9;
                    }

                    if (BattleHandler.isPrisonerOnTile)
                    {
                        GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().PhysicalSymbolStored = 9;
                        GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().DefensiveSymbolStored = 9;
                        GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().MagicalSymbolStored = 9;
                    }
                    
                }
                else
                {
                    if (isUnlimitedActionPointsModeActive)
                    {
                        Debug.Log("Deactivate unlimited action points mode.");
                        foreach (PawnInstance pi in GameManager.Instance.AllKeepersList)
                        {
                            pi.GetComponent<Keeper>().MaxActionPoints = 3;
                            pi.GetComponent<Keeper>().ActionPoints = 3;
                        }
                    }
                    else
                    {
                        Debug.Log("Activate unlimited action points mode.");
                        foreach (PawnInstance pi in GameManager.Instance.AllKeepersList)
                        {
                            pi.GetComponent<Keeper>().MaxActionPoints = 99;
                            pi.GetComponent<Keeper>().ActionPoints = 99;
                        }
                    }
                    isUnlimitedActionPointsModeActive = !isUnlimitedActionPointsModeActive;
                }            
            }

            // Discover all tiles
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (isMapUncovered)
                    {
                        if (oldTileStates != null && oldTileStates.Count > 0)
                        {
                            foreach (Tile tile in oldTileStates.Keys)
                            {
                                tile.State = oldTileStates[tile];
                            }
                        }
                    }
                    isMapUncovered = false;
                }
                else
                {
                    if (!isMapUncovered)
                    {
                        oldTileStates.Clear();
                        foreach (Tile tile in TileManager.Instance.Tiles.GetComponentsInChildren<Tile>())
                        {
                            oldTileStates.Add(tile, tile.State);
                            tile.State = TileState.Discovered;
                        }
                    }
                    isMapUncovered = true;
                }
            }

            // Decrease food Ashley
            if (Input.GetKey(KeyCode.CapsLock) && Input.GetKey(KeyCode.Alpha2))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>().CurrentHunger++;
                }
                else
                {
                    GameManager.Instance.PrisonerInstance.GetComponent<HungerHandler>().CurrentHunger--;
                }
            }

            if (GameManager.Instance.ListOfSelectedKeepers == null || GameManager.Instance.ListOfSelectedKeepers.Count == 0)
            {
                return;
            }

            // Kill selected character
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Killed " + GameManager.Instance.GetFirstSelectedKeeper().Data.PawnName + " using debug tools.");
                GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Mortal>().CurrentHp = 0;
            }

            // Decrease food
            if (Input.GetKey(KeyCode.Alpha2) && !Input.GetKey(KeyCode.CapsLock))
            {
                if (GameManager.Instance.CurrentState == GameState.InBattle)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().PhysicalSymbolStored++;
                    else
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().PhysicalSymbolStored--;
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<HungerHandler>().CurrentHunger++;
                    else
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<HungerHandler>().CurrentHunger--;
                }
            }

            // Decrease mental health
            if (Input.GetKey(KeyCode.Alpha3))
            {
                if (GameManager.Instance.CurrentState == GameState.InBattle)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().DefensiveSymbolStored++;
                    else
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().DefensiveSymbolStored--;
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<MentalHealthHandler>().CurrentMentalHealth++;
                    else
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<MentalHealthHandler>().CurrentMentalHealth--;
                }
            }

            // Decrease HP
            if (Input.GetKey(KeyCode.Alpha4))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Mortal>().CurrentHp++;
                else
                    GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Mortal>().CurrentHp--;
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (GameManager.Instance.CurrentState == GameState.InBattle)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().MagicalSymbolStored++;
                    else
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().MagicalSymbolStored--;
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Keeper>().ActionPoints++;
                    else
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Keeper>().ActionPoints--;
                }
            }

            // Pop a monster
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                if (GameManager.Instance.CurrentState == GameState.InBattle)
                {
                    GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().EscapeBattle();
                }
                else
                {
                    Instantiate(monsterToPopPrefab,
                        GameManager.Instance.GetFirstSelectedKeeper().CurrentTile.transform.position,
                        Quaternion.identity, GameManager.Instance.GetFirstSelectedKeeper().CurrentTile.transform);
                }
            }

            // Pop an item on the ground
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                ItemContainer[] itemToSpawn = new ItemContainer[1];
                itemToSpawn[0] = new ItemContainer(GameManager.Instance.ItemDataBase.ItemsList[Random.Range(0, GameManager.Instance.ItemDataBase.ItemsList.Count)], itemsToPopAmount);
                ItemManager.AddItemOnTheGround(GameManager.Instance.GetFirstSelectedKeeper().CurrentTile, GameManager.Instance.GetFirstSelectedKeeper().CurrentTile.transform, itemToSpawn);
            }

            // TP selected keeper
            if (Input.GetKey(KeyCode.T))
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    RaycastHit hitInfo;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) == true)
                    {
                        if (hitInfo.transform.GetComponentInParent<Tile>() != null)
                        {
                            Tile destinationTile = hitInfo.transform.GetComponentInParent<Tile>();
                            PawnInstance selectedKeeper = GameManager.Instance.GetFirstSelectedKeeper();
                            TileManager.Instance.RemoveKeeperFromTile(selectedKeeper.CurrentTile, selectedKeeper);
                            TileManager.Instance.AddKeeperOnTile(destinationTile, selectedKeeper);

                            // Physical movement
                            selectedKeeper.GetComponent<AnimatedPawn>().StartBetweenTilesAnimation(destinationTile.transform.position);

                            //selectedKeeper.transform.position = destinationTile.transform.position;
                            GameObject currentCharacter;
                            Keeper keeperComponent = selectedKeeper.GetComponent<Keeper>();
                            for (int i = 0; i < keeperComponent.GoListCharacterFollowing.Count; i++)
                            {
                                currentCharacter = keeperComponent.GoListCharacterFollowing[i];

                                if (currentCharacter.GetComponent<Escortable>() != null)
                                {
                                    currentCharacter.GetComponent<PawnInstance>().CurrentTile = destinationTile;
                                    currentCharacter.GetComponent<AnimatedPawn>().StartBetweenTilesAnimation(destinationTile.transform.position + Vector3.right * 0.2f);
                                }

                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.ResetGameData();
                GameManager.Instance.CurrentState = GameState.Normal;
                AudioManager.Instance.Fade(AudioManager.Instance.menuMusic);
                GameManager.Instance.Ui.GoActionPanelQ.transform.parent.SetParent(GameManager.Instance.Ui.transform);
                SceneManager.LoadScene(0);
            }
        }
    }
}

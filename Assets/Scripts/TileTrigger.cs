using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileTrigger : MonoBehaviour {
    
    public List<KeeperInstance> ki;

    int actionCostExplore = 3;
    int actionCostMove = 2;

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<KeeperInstance>() != null)
        {
            HandleTrigger(other.GetComponentInParent<KeeperInstance>());
            
        }
   
    }

    public void HandleTrigger(KeeperInstance k)
    {
        ki.Add(k);
        InteractionImplementer InteractionImplementer = new InteractionImplementer();
        Direction eTrigger = Direction.None;

        string strTag = tag;

        switch (strTag)
        {
            case "NorthTrigger":
                eTrigger = Direction.North;
                break;
            case "NorthEastTrigger":
                eTrigger = Direction.North_East;
                break;
            case "SouthEastTrigger":
                eTrigger = Direction.South_East;
                break;
            case "SouthTrigger":
                eTrigger = Direction.South;
                break;
            case "SouthWestTrigger":
                eTrigger = Direction.South_West;
                break;
            case "NorthWestTrigger":
                eTrigger = Direction.North_West;
                break;
            default:
                eTrigger = Direction.None;
                break;
        }

        if (eTrigger != Direction.None && GetComponentInParent<Tile>().Neighbors[(int)eTrigger] != null)
        {
            if (k.ActionPoints > 0)
            {
                if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                {
                    // On veut le mesh collider actif du perso
                    foreach (MeshCollider mc in GameManager.Instance.ListOfSelectedKeepers[0].gameObject.GetComponentsInChildren<MeshCollider>())
                    {
                        if (mc.enabled)
                        {
                            GameManager.Instance.GoTarget = mc.gameObject;
                            break;
                        }
                    }

                    IngameUI ui = GameObject.Find("IngameUI").GetComponent<IngameUI>();

                    if (GetComponentInParent<Tile>().Neighbors[(int)eTrigger].State == TileState.Discovered)
                    {
                        InteractionImplementer.Add(new Interaction(Move), actionCostMove, "Move", GameManager.Instance.Ui.spriteMove, true, (int)eTrigger);
                        ui.UpdateActionPanelUIQ(InteractionImplementer);
                    }
                    if (GetComponentInParent<Tile>().Neighbors[(int)eTrigger].State == TileState.Greyed)
                    {
                        InteractionImplementer.Add(new Interaction(Explore), actionCostExplore, "Explore", GameManager.Instance.Ui.spriteExplore, true, (int)eTrigger);
                        ui.UpdateActionPanelUIQ(InteractionImplementer);
                    }

                }
                else
                {
                    // TODO : reflechir
                    Debug.Log("Cas non géré (last selected keeper");
                }

            }
            else
            {
                GameManager.Instance.Ui.ZeroActionTextAnimation();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<KeeperInstance>() != null)
        {
            KeeperInstance leaving = other.GetComponentInParent<KeeperInstance>();
            if (ki.Contains(leaving))
            {
                ki.Remove(leaving);
            }
        }
        Debug.Log("Exit");
    }

    void Move(int _i)
    {
        
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {
            KeeperInstance toMove = GameManager.Instance.ListOfSelectedKeepers[0];
            if (toMove.ActionPoints >= actionCostMove)
            {
                Tile currentTile = TileManager.Instance.GetTileFromKeeper[toMove];

                // Confirmation Panel
                // TODO : refaire en mieux ? 
                if (toMove.Keeper.GoListCharacterFollowing.Count == 0
                    && currentTile == TileManager.Instance.PrisonerTile)
                {
                    bool isAshleyNotAlone = false;
                    foreach (KeeperInstance kip in GameManager.Instance.AllKeepersList)
                    {
                        if (kip.IsAlive)
                        {
                            if (kip != toMove && TileManager.Instance.PrisonerTile == TileManager.Instance.GetTileFromKeeper[kip])
                            {
                                isAshleyNotAlone = true;
                                break;
                            }
                        }
                    }
                    if (isAshleyNotAlone)
                    {
                        MoveWithoutConfirmation(_i);
                    }
                    else
                    {
                        GameManager.Instance.Ui.goConfirmationPanel.SetActive(true);
                        int n = _i;
                        GameManager.Instance.Ui.goConfirmationPanel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                        GameManager.Instance.Ui.goConfirmationPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => MoveWithoutConfirmation(n));
                        GameManager.Instance.Ui.goConfirmationPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.Ui.goConfirmationPanel.SetActive(false));
                    }
                }
                else
                {
                    MoveWithoutConfirmation(_i);
                }
            }
            else
            {
                GameManager.Instance.Ui.ZeroActionTextAnimation();
            }
        }
    }

    void Explore(int _i)
    {
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {

            KeeperInstance toMove = GameManager.Instance.ListOfSelectedKeepers[0];
            if (toMove.ActionPoints >= actionCostExplore)
            {
                Tile currentTile = TileManager.Instance.GetTileFromKeeper[toMove];

                // Confirmation Panel
                // TODO : refaire en mieux ? 
                if (toMove.Keeper.GoListCharacterFollowing.Count == 0
                    && currentTile == TileManager.Instance.PrisonerTile)
                {
                    bool isAshleyNotAlone = false;
                    foreach (KeeperInstance kip in GameManager.Instance.AllKeepersList)
                    {
                        if (kip.IsAlive)
                        {
                            if (kip != toMove && TileManager.Instance.PrisonerTile == TileManager.Instance.GetTileFromKeeper[kip])
                            {
                                isAshleyNotAlone = true;
                                break;
                            }
                        }
                    }
                    if (isAshleyNotAlone)
                    {
                        ExploreWithoutConfirmation(_i);
                    }
                    else
                    {
                        GameManager.Instance.Ui.goConfirmationPanel.SetActive(true);
                        int n = _i;
                        GameManager.Instance.Ui.goConfirmationPanel.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                        GameManager.Instance.Ui.goConfirmationPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => ExploreWithoutConfirmation(n));
                        GameManager.Instance.Ui.goConfirmationPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.Ui.goConfirmationPanel.SetActive(false));
                    }
                }
                else
                {
                    ExploreWithoutConfirmation(_i);
                }
            }
            else
            {
                GameManager.Instance.Ui.ZeroActionTextAnimation();
            }
        }
    }

    private void MoveWithoutConfirmation(int _i)
    {
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {
            KeeperInstance toMove = GameManager.Instance.ListOfSelectedKeepers[0];
            //Check if the prisoner is following
            PrisonerInstance prisoner = null;
            if (toMove.Keeper.GoListCharacterFollowing.Count > 0 && toMove.Keeper.GoListCharacterFollowing[0].GetComponent<PrisonerInstance>())
            {
                prisoner = toMove.Keeper.GoListCharacterFollowing[0].GetComponent<PrisonerInstance>();
            }

            //int costAction = interactionImplementer.Get("Move").costAction;
            TileManager.Instance.MoveKeeper(toMove, TileManager.Instance.GetTileFromKeeper[toMove], (Direction)_i, actionCostMove);
            GameManager.Instance.SelectedKeeperNeedUpdate = true;
            GameManager.Instance.ShortcutPanel_NeedUpdate = true;
            GameManager.Instance.Ui.HideInventoryPanels();

            toMove.IsTargetableByMonster = false;
            // TODO Trigger then translate instead of tp
            toMove.GetComponentInChildren<Animator>().SetTrigger("moveBetweenTiles");
            if (prisoner != null)
            {
                prisoner.GetComponentInChildren<Animator>().SetTrigger("moveBetweenTiles");
            }
        }
        else
        {
            Debug.Log("No keeper selected");
        }
    }

    private void ExploreWithoutConfirmation(int _i)
    {
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {
            KeeperInstance toMove = GameManager.Instance.ListOfSelectedKeepers[0];
            //Check if the prisoner is following
            PrisonerInstance prisoner = null;
            if (toMove.Keeper.GoListCharacterFollowing.Count > 0 && toMove.Keeper.GoListCharacterFollowing[0].GetComponent<PrisonerInstance>())
            {
                prisoner = toMove.Keeper.GoListCharacterFollowing[0].GetComponent<PrisonerInstance>();
            }

            // Move to explored tile
            //int costAction = interactionImplementer.Get("Explore").costAction;
            TileManager.Instance.MoveKeeper(toMove, TileManager.Instance.GetTileFromKeeper[toMove], (Direction)_i, actionCostExplore);
            // Tell the tile it has been discovered (and watch it panic)
            Tile exploredTile = TileManager.Instance.GetTileFromKeeper[toMove];
            exploredTile.State = TileState.Discovered;
            foreach (Tile t in exploredTile.Neighbors)
            {
                if (t != null && t.State == TileState.Hidden)
                {
                    t.State = TileState.Greyed;
                }

            }

            // Apply exploration costs
            toMove.CurrentHunger += 5;
            GameManager.Instance.Ui.BuffActionTextAnimation(GameManager.Instance.Ui.goHungerBuffOnStatPanel, -5);
            GameManager.Instance.ShortcutPanel_NeedUpdate = true;
            GameManager.Instance.SelectedKeeperNeedUpdate = true;
            //TODO: Apply this only when the discovered tile is unfriendly
            toMove.CurrentMentalHealth -= 5;
            GameManager.Instance.Ui.BuffActionTextAnimation(GameManager.Instance.Ui.goMentalHeathBuffOnStatPanel, -5);
            GameManager.Instance.ShortcutPanel_NeedUpdate = true;
            GameManager.Instance.SelectedKeeperNeedUpdate = true;
            // If the player is exploring with the prisoner following, apply costs to him too
            if (prisoner != null)
            {
                prisoner.CurrentHunger += 5;
                //TODO: Apply this only when the discovered tile is unfriendly
                prisoner.CurrentMentalHealth -= 5;
            }

            // Apply bad effects if monsters are discovered
            if (TileManager.Instance.MonstersOnTile.ContainsKey(exploredTile)
                && TileManager.Instance.MonstersOnTile[exploredTile] != null
                && TileManager.Instance.MonstersOnTile[exploredTile].Count > 0)
            {
                toMove.CurrentHp -= 5;
                toMove.CurrentMentalHealth -= 5;
                if (prisoner != null)
                {
                    prisoner.CurrentHp -= 5;
                    prisoner.CurrentMentalHealth -= 5;
                }
            }
            GameManager.Instance.SelectedKeeperNeedUpdate = true;
            GameManager.Instance.ShortcutPanel_NeedUpdate = true;
            GameManager.Instance.Ui.HideInventoryPanels();

            toMove.IsTargetableByMonster = false;

            // TODO Trigger then translate instead of tp
            toMove.GetComponentInChildren<Animator>().SetTrigger("moveBetweenTiles");
            if (prisoner != null)
            {
                prisoner.GetComponentInChildren<Animator>().SetTrigger("moveBetweenTiles");
            }

        }
        else
        {
            Debug.Log("No keeper selected");
        }
    }
}

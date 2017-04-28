using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Behaviour;

public class TileTrigger : MonoBehaviour {
    
    public List<PawnInstance> piList;

    int actionCostExplore = 3;
    int actionCostMove = 2;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Keeper>() != null && other.isTrigger)
        {
            HandleTrigger(other.GetComponentInParent<PawnInstance>());
            //if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto && TutoManager.s_instance.GetComponent<SeqFirstMove>().AlreadyPlayed == false)
            //{
            //    if (tag == "NorthEastTrigger")
            //        TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqFirstMove>());
            //}
        }
    }

    public void HandleTrigger(PawnInstance k)
    {
        piList.Add(k);
        GetComponent<Interactable>().Interactions = new InteractionImplementer();
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
            if (k.GetComponent<AnimatedPawn>().ArrivingTrigger == eTrigger)
            {
                k.GetComponent<AnimatedPawn>().ArrivingTrigger = Direction.None;
            }
            else
            {
                k.GetComponent<AnimatedPawn>().ArrivingTrigger = Utils.GetOppositeDirection(eTrigger);

                if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                {
                    // On veut le mesh collider actif du perso
                    GameManager.Instance.GoTarget = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Interactable>();

                    if (GetComponentInParent<Tile>().Neighbors[(int)eTrigger].State == TileState.Discovered)
                    {
                        GetComponent<Interactable>().Interactions.Add(new Interaction(Move), actionCostMove, "Move", GameManager.Instance.SpriteUtils.spriteMove, true, (int)eTrigger);
                        GameManager.Instance.Ui.UpdateActionPanelUIQ(GetComponent<Interactable>().Interactions);
                    }
                    if (GetComponentInParent<Tile>().Neighbors[(int)eTrigger].State == TileState.Greyed)
                    {
                        GetComponent<Interactable>().Interactions.Add(new Interaction(Explore), actionCostExplore, "Explore", GameManager.Instance.SpriteUtils.spriteExplore, true, (int)eTrigger);
                        GameManager.Instance.Ui.UpdateActionPanelUIQ(GetComponent<Interactable>().Interactions);
                    }
                }
                else
                {
                    // TODO : reflechir
                    Debug.Log("Cas non géré (last selected keeper)");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Keeper>() != null)
        {
            PawnInstance leaving = other.GetComponentInParent<PawnInstance>();
            if (piList.Contains(leaving))
            {
                piList.Remove(leaving);
            }
        }
    }

    void Move(int _i)
    {
        
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {
            PawnInstance toMove = GameManager.Instance.GetFirstSelectedKeeper();
            if (toMove.GetComponent<Keeper>() != null
                && toMove.GetComponent<Keeper>().ActionPoints >= actionCostMove)
            {
                Tile currentTile = toMove.CurrentTile;

                // Confirmation Panel
                // TODO : refaire en mieux ? 
                if (toMove.GetComponent<Keeper>().GoListCharacterFollowing.Count == 0
                    && currentTile == GameManager.Instance.PrisonerInstance.CurrentTile)
                {                    
                    if (!toMove.GetComponent<Keeper>().IsTheLastKeeperOnTheTile())
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
                GameManager.Instance.Ui.ZeroActionTextAnimation(GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>());
            }
        }
    }

    void Explore(int _i)
    {
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {

            PawnInstance toMove = GameManager.Instance.GetFirstSelectedKeeper();
            if (toMove.GetComponent<Keeper>() != null
                && toMove.GetComponent<Keeper>().ActionPoints >= actionCostExplore)
            {
                Tile currentTile = toMove.CurrentTile;

                // Confirmation Panel
                // TODO : refaire en mieux ? 
                if (toMove.GetComponent<Keeper>().GoListCharacterFollowing.Count == 0
                    && currentTile == GameManager.Instance.PrisonerInstance.CurrentTile)
                {
                    if (!toMove.GetComponent<Keeper>().IsTheLastKeeperOnTheTile())
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
                GameManager.Instance.Ui.ZeroActionTextAnimation(GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>());
            }
        }
    }

    private void MoveWithoutConfirmation(int _i)
    {
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {
            PawnInstance toMove = GameManager.Instance.GetFirstSelectedKeeper();

            TileManager.Instance.MoveKeeper(toMove, toMove.CurrentTile, (Direction)_i, actionCostMove);

            GameManager.Instance.Ui.HideInventoryPanels();

            if (toMove.GetComponent<Fighter>() != null)
                toMove.GetComponent<Fighter>().IsTargetableByMonster = false;
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
            PawnInstance toMove = GameManager.Instance.GetFirstSelectedKeeper();

            // Move to explored tile
            TileManager.Instance.MoveKeeper(toMove, toMove.CurrentTile, (Direction)_i, actionCostExplore);

            // Tell the tile it has been discovered (and watch it panic)
            Tile exploredTile = toMove.CurrentTile;
            exploredTile.State = TileState.Discovered;
            foreach (Tile t in exploredTile.Neighbors)
            {
                if (t != null && t.State == TileState.Hidden)
                {
                    t.State = TileState.Greyed;
                }

            }

            // Apply exploration costs
            if (toMove.GetComponent<HungerHandler>() != null)
                toMove.GetComponent<HungerHandler>().CurrentHunger -= 5;
            //GameManager.Instance.Ui.BuffActionTextAnimation(GameManager.Instance.Ui.goHungerBuffOnStatPanel, -5);
            //Moral is affected by the friendliness of the discovered tile
            if (toMove.GetComponent<MentalHealthHandler>() != null)
            {
                if (exploredTile.Friendliness == TileFriendliness.Scary)
                {

                    toMove.GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 5;
                }
                else if (exploredTile.Friendliness == TileFriendliness.Friendly)
                {
                    toMove.GetComponent<MentalHealthHandler>().CurrentMentalHealth += 5;
                }
            }   
            
            //GameManager.Instance.Ui.BuffActionTextAnimation(GameManager.Instance.Ui.goMentalHeathBuffOnStatPanel, -5);

            // If the player is exploring with the prisoner following, apply costs to him too
            if (toMove.GetComponent<Keeper>() != null && toMove.GetComponent<Keeper>().GoListCharacterFollowing.Count > 0)
            {
                foreach(GameObject follower in toMove.GetComponent<Keeper>().GoListCharacterFollowing)
                {
                    if (follower.GetComponent<HungerHandler>() != null)
                        follower.GetComponent<HungerHandler>().CurrentHunger -= 5;


                    if (follower.GetComponent<MentalHealthHandler>() != null)
                    {
                        if (exploredTile.Friendliness == TileFriendliness.Scary)
                        {
                            follower.GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 5;
                        }
                        else if (exploredTile.Friendliness == TileFriendliness.Friendly)
                        {
                            follower.GetComponent<MentalHealthHandler>().CurrentMentalHealth += 5;
                        }
                    }

                    //TODO: Apply this only when the discovered tile is unfriendly
                    if (follower.GetComponent<MentalHealthHandler>() != null)
                        follower.GetComponent<MentalHealthHandler>().CurrentMentalHealth -= 5;
                }
            }

            GameManager.Instance.Ui.HideInventoryPanels();

            if (toMove.GetComponent<Fighter>() != null)
                toMove.GetComponent<Fighter>().IsTargetableByMonster = false;
        }
        else
        {
            Debug.Log("No keeper selected");
        }
    }
}

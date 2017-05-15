using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using Behaviour;

using UnityEngine.UI;

using UnityEngine.AI;

using UnityEngine.SceneManagement;

public enum PassageStatus
{
    // On: Both tiles are connected and discovered 
    // Off: Both tile are connected but one is greyed 
    // Disabled: The path is blocked
    On, Off, Disabled  
}

public class TilePassage : MonoBehaviour {

    int actionCostExplore = 3;

    int actionCostMove = 2;

    public Direction dir;

    private PassageStatus status;

    Tile parentTile;

    public PassageStatus Status
    {
        get
        {
            return status;
        }

        set
        {
            if(SceneManager.GetActiveScene().buildIndex != 1)
            switch(value)
            {
                case PassageStatus.On:
                    transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                    break;
                case PassageStatus.Off:
                    transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                    transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                    break;
                case PassageStatus.Disabled:
                    transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                    break;
            }
            status = value;
        }
    }


    // Use this for initialization

    void Start () {
        string strTag = tag;
        parentTile = GetComponentInParent<Tile>();
        switch (strTag)
        {
            case "NorthTrigger":
                dir = Direction.North;
                break;
            case "NorthEastTrigger":
                dir = Direction.North_East;
                break;
            case "SouthEastTrigger":
                dir = Direction.South_East;
                break;
            case "SouthTrigger":
                dir = Direction.South;
                break;
            case "SouthWestTrigger":
                dir = Direction.South_West;
                break;
            case "NorthWestTrigger":
                dir = Direction.North_West;
                break;
            default:
                dir = Direction.None;
                break;
        }

        if (parentTile.Neighbors[(int)dir] == null)
        {
            gameObject.SetActive(false);
        }
        else if(parentTile.Neighbors[(int)dir].State == TileState.Discovered && parentTile.State == TileState.Discovered)
        {
            Status = PassageStatus.On;
        }
        else
        {
            Status = PassageStatus.Off;
        }
    }





    public void HandleClick()

    {
        if (Status == PassageStatus.Disabled)
            return;

        PawnInstance k = GameManager.Instance.ListOfSelectedKeepers[0];

        GetComponent<Interactable>().Interactions = new InteractionImplementer();

        Direction dir = Direction.None;



        string strTag = tag;



        switch (strTag)

        {

            case "NorthTrigger":

                dir = Direction.North;

                break;

            case "NorthEastTrigger":

                dir = Direction.North_East;

                break;

            case "SouthEastTrigger":

                dir = Direction.South_East;

                break;

            case "SouthTrigger":

                dir = Direction.South;

                break;

            case "SouthWestTrigger":

                dir = Direction.South_West;

                break;

            case "NorthWestTrigger":

                dir = Direction.North_West;

                break;

            default:

                dir = Direction.None;

                break;

        }



        if (dir != Direction.None && GetComponentInParent<Tile>().Neighbors[(int)dir] != null)

        {

            if (k.GetComponent<AnimatedPawn>().ArrivingTrigger == dir)

            {

                k.GetComponent<AnimatedPawn>().ArrivingTrigger = Direction.None;

            }

            else

            {

                k.GetComponent<AnimatedPawn>().ArrivingTrigger = Utils.GetOppositeDirection(dir);



                if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)

                {

                    // On veut le mesh collider actif du perso

                    GameManager.Instance.GoTarget = GetComponent<Interactable>();

                    if (GetComponentInParent<Tile>().Neighbors[(int)dir].State == TileState.Discovered)

                    {

                        GetComponent<Interactable>().Interactions.Add(new Interaction(Move), actionCostMove, "Move", GameManager.Instance.SpriteUtils.spriteMove, true, (int)dir);

                        GameManager.Instance.Ui.UpdateActionPanelUIQ(GetComponent<Interactable>().Interactions);

                    }

                    if (GetComponentInParent<Tile>().Neighbors[(int)dir].State == TileState.Greyed)

                    {



                        GetComponent<Interactable>().Interactions.Add(new Interaction(Explore), actionCostExplore, "Explore", GameManager.Instance.SpriteUtils.spriteExplore, true, (int)dir);

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

                        if (GameManager.Instance.CurrentState != GameState.InTuto || GameManager.Instance.CurrentState != GameState.InBattle)
                            GameManager.Instance.CurrentState = GameState.InPause;

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
            if (toMove.GetComponent<Keeper>() == null) return;

            int actionCostExploreTmp = actionCostExplore;
            if (toMove.Data.Behaviours[(int)BehavioursEnum.Explorateur] == true)
            {
                actionCostExploreTmp -= 1;
            }

            if ( toMove.GetComponent<Keeper>().ActionPoints >= actionCostExploreTmp)

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

                        if (GameManager.Instance.CurrentState != GameState.InTuto || GameManager.Instance.CurrentState != GameState.InBattle)
                            GameManager.Instance.CurrentState = GameState.InPause;

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

        if (GameManager.Instance.CurrentState != GameState.InTuto && GameManager.Instance.CurrentState != GameState.InBattle)
            GameManager.Instance.CurrentState = GameState.Normal;

        PawnInstance toMove = GameManager.Instance.GetFirstSelectedKeeper();
        AnimatedPawn toMoveAnimatedPawn = toMove.GetComponent<AnimatedPawn>();

        toMoveAnimatedPawn.CmdMove = true;
        toMoveAnimatedPawn.WhereMove = _i;

        for (int i = 0; i < GameManager.Instance.ListOfSelectedKeepers.Count; i++)
        {
            GameManager.Instance.ListOfSelectedKeepers[i].GetComponent<AnimatedPawn>().TriggerRotation(transform.position);
        }
    }



    private void ExploreWithoutConfirmation(int _i)
    {
        if (GameManager.Instance.CurrentState != GameState.InTuto && GameManager.Instance.CurrentState != GameState.InBattle)
            GameManager.Instance.CurrentState = GameState.Normal;

        PawnInstance toMove = GameManager.Instance.GetFirstSelectedKeeper();
        AnimatedPawn toMoveAnimatedPawn = toMove.GetComponent<AnimatedPawn>();

        toMoveAnimatedPawn.CmdExplore = true;
        toMoveAnimatedPawn.WhereMove = _i;

        for (int i = 0; i < GameManager.Instance.ListOfSelectedKeepers.Count; i++)
        {
            GameManager.Instance.ListOfSelectedKeepers[i].GetComponent<AnimatedPawn>().TriggerRotation(transform.position);
        }
    }



    public void OnTriggerStay(Collider other)

    {
        if (other.GetComponentInParent<Keeper>() != null && other.GetComponentInParent<AnimatedPawn>().WhereMove == (int)dir)

        {
            if (other.GetComponentInParent<AnimatedPawn>().CmdExplore == true)

            {
                if (GameManager.Instance.GoTarget != null && GameManager.Instance.GoTarget.GetComponent<TilePassage>() == this && other.GetComponentInParent<NavMeshAgent>().remainingDistance <= 0.001f)

                {
                    other.GetComponentInParent<NavMeshAgent>().Stop();

                    other.GetComponentInParent<AnimatedPawn>().CmdExplore = false;

                    PawnInstance toMove = other.GetComponentInParent<PawnInstance>();

                    Direction dirToMove = (Direction)other.GetComponentInParent<AnimatedPawn>().WhereMove;
                    //toMove.CurrentTile.GetPassage(dirToMove).Status = PassageStatus.On;
                    // Move to explored tile
                    int actionCostExploreTmp = actionCostExplore;
                    if (toMove.Data.Behaviours[(int)BehavioursEnum.Explorateur] == true)
                    {
                        actionCostExploreTmp -= 1;
                    }
                    
                    TileManager.Instance.MoveKeeper(toMove, toMove.CurrentTile, (Direction)other.GetComponentInParent<AnimatedPawn>().WhereMove, actionCostExploreTmp);

                    // Tell the tile it has been discovered (and watch it panic)

                    Tile exploredTile = toMove.CurrentTile;

                    exploredTile.State = TileState.Discovered;
                    //exploredTile.GetPassage(Utils.GetOppositeDirection(dirToMove)).Status = PassageStatus.On;
                    //Status = PassageStatus.On;

                    foreach (Tile t in exploredTile.Neighbors)
                    {

                        if (t != null && t.State == TileState.Hidden)

                        {

                            t.State = TileState.Greyed;

                        }
                    }

                    GameManager.Instance.Ui.HideInventoryPanels();



                    if (toMove.GetComponent<Fighter>() != null)

                        toMove.GetComponent<Fighter>().IsTargetableByMonster = false;

                }



            }

            else if (other.GetComponentInParent<AnimatedPawn>().CmdMove == true)
            {
                if (other.GetComponentInParent<NavMeshAgent>().isActiveAndEnabled && other.GetComponentInParent<NavMeshAgent>().remainingDistance <= 0.001f)
                {
                    other.GetComponentInParent<NavMeshAgent>().Stop();
                    other.GetComponentInParent<AnimatedPawn>().CmdMove = false;

                    PawnInstance toMove = other.GetComponentInParent<PawnInstance>();



                    TileManager.Instance.MoveKeeper(toMove, toMove.CurrentTile, (Direction)other.GetComponentInParent<AnimatedPawn>().WhereMove, actionCostMove);



                    GameManager.Instance.Ui.HideInventoryPanels();

                    //if (toMove.GetComponent<Keeper>() != null && toMove.GetComponent<Keeper>().GoListCharacterFollowing.Count > 0)

                    //{

                    //    foreach (GameObject follower in toMove.GetComponent<Keeper>().GoListCharacterFollowing)

                    //    {

                    //        PawnInstance followerInstance = follower.GetComponent<PawnInstance>();

                    //        TileManager.Instance.MoveEscortable(followerInstance, followerInstance.CurrentTile, (Direction)other.GetComponentInParent<AnimatedPawn>().WhereMove);

                    //    }

                    //}



                    if (toMove.GetComponent<Fighter>() != null)

                        toMove.GetComponent<Fighter>().IsTargetableByMonster = false;

                }

            }

        }

    }

}


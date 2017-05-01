using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Behaviour;
public class ControlsManager : MonoBehaviour
{
    public GameObject goPreviousLeftclicked;
    float fTimerDoubleClick;
    [SerializeField]
    private float fDoubleClickCoolDown = 0.3f;
    public LayerMask layerMask;
    void Start()
    {
        goPreviousLeftclicked = null;
        Cursor.SetCursor(GameManager.Instance.Texture2DUtils.iconeMouse, Vector2.zero, CursorMode.Auto);
        fTimerDoubleClick = 0;
    }
    void Update()
    {
        SelectionControls();
        ChangeSelectedKeeper();
        UpdateDoubleCick();
        ShortcutMenuControls();
    }
    private void SelectionControls()
    {
        if (GameManager.Instance.CurrentState == GameState.Normal)
        {
            NormalStateControls();
        }
        else if (GameManager.Instance.CurrentState == GameState.InBattle)
        {
            InBattleControls();
        }
        
    }
    private void NormalStateControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, ~layerMask) == true)
                {
                    GameManager.Instance.Ui.ClearActionPanel();
                    if (hitInfo.transform.gameObject.GetComponentInParent<Keeper>() != null)
                    {
                        Keeper clickedKeeper = hitInfo.transform.gameObject.GetComponentInParent<Keeper>();
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            if (GameManager.Instance.ListOfSelectedKeepers.Contains(clickedKeeper.getPawnInstance))
                            {
                                GameManager.Instance.ListOfSelectedKeepers.Remove(clickedKeeper.getPawnInstance);
                                clickedKeeper.IsSelected = false;
                            }
                            else
                            {
                                GameManager.Instance.AddKeeperToSelectedList(clickedKeeper.getPawnInstance);
                                clickedKeeper.IsSelected = true;
                            }
                        }
                        else
                        {
                            GameManager.Instance.ClearListKeeperSelected();
                            GameManager.Instance.AddKeeperToSelectedList(clickedKeeper.getPawnInstance);
                            GameManager.Instance.Ui.HideInventoryPanels();
                            clickedKeeper.IsSelected = true;
                        }
                        if (fTimerDoubleClick > 0 && goPreviousLeftclicked == hitInfo.transform.gameObject)
                        {
                            Camera.main.GetComponent<CameraManager>().UpdateCameraPosition(clickedKeeper.getPawnInstance);
                            goPreviousLeftclicked = null;
                            fTimerDoubleClick = 0;
                        }
                        else
                        {
                            fTimerDoubleClick = fDoubleClickCoolDown;
                            goPreviousLeftclicked = hitInfo.transform.gameObject;
                        }
                    }
                    else
                    {
                        GameManager.Instance.ClearListKeeperSelected();
                        GameManager.Instance.Ui.HideInventoryPanels();
                    }

                    if (hitInfo.transform.gameObject.GetComponentInParent<Keeper>() == null)
                    {
                        GameManager.Instance.ClearListKeeperSelected();
                        GameManager.Instance.Ui.HideInventoryPanels();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                {
                    RaycastHit hitInfo;
                    // LayerMask layermask = 1 << LayerMask.NameToLayer("TilePortal");
                    // layermask = ~layermask;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, ~layerMask) == true)
                    {
                        IngameUI ui = GameManager.Instance.Ui;

                        Cursor.SetCursor(GameManager.Instance.Texture2DUtils.iconeMouseClicked, Vector2.zero, CursorMode.Auto);
                        Invoke("CursorNormalState", 0.5f);

                        Tile tileHit = hitInfo.collider.gameObject.GetComponentInParent<Tile>();
                        Tile keeperSelectedTile = GameManager.Instance.GetFirstSelectedKeeper().CurrentTile;
                        GameObject clickTarget = hitInfo.collider.gameObject;
                        // Handle click on a ItemInstance
                        if (clickTarget.GetComponent<ItemInstance>() != null)
                        {
                            if (tileHit == keeperSelectedTile)
                            {
                                //To add if we want the keepers to move to the clicked object
                                GameManager.Instance.GoTarget = hitInfo.collider.gameObject.GetComponent<Interactable>();
                                ui.UpdateActionPanelUIQ(hitInfo.collider.gameObject.GetComponent<ItemInstance>().InteractionImplementer);
                            }
                        }
                        // Handle click on a ItemInstance
                        else if (clickTarget.GetComponent<LootInstance>() != null)
                        {
                            if (tileHit == keeperSelectedTile)
                            {
                                GameManager.Instance.GoTarget = hitInfo.collider.gameObject.GetComponent<Interactable>();
                                ui.UpdateActionPanelUIQ(hitInfo.collider.gameObject.GetComponent<LootInstance>().InteractionImplementer);
                            }
                        }
                        else if (clickTarget.GetComponent<TriggerPanneau>() != null)
                        {
                            if (tileHit == keeperSelectedTile)
                            {
                                GameManager.Instance.GoTarget = clickTarget.GetComponent<Interactable>();
                                ui.UpdateActionPanelUIQ(clickTarget.GetComponent<Interactable>().Interactions);
                            }
                        }
                        // Handle click on a pawn
                        else if (clickTarget.GetComponentInParent<PawnInstance>() != null)
                        {
                            tileHit = clickTarget.GetComponentInParent<PawnInstance>().CurrentTile;
                            if (tileHit == keeperSelectedTile)
                            {
                                // If click on same keeper, do nothing
                                if (clickTarget.GetComponentInParent<Keeper>() != null)
                                {
                                    if (clickTarget.GetComponentInParent<PawnInstance>() == GameManager.Instance.GetFirstSelectedKeeper())
                                    {
                                        return;
                                    }
                                }
                                if (clickTarget.GetComponentInParent<Escortable>() != null)
                                {
                                    clickTarget.GetComponentInParent<Escortable>().UpdateEscortableInteractions();
                                    if (TutoManager.s_instance != null && TutoManager.s_instance.enableTuto && TutoManager.s_instance.PlayingSequence == null
                                        && TutoManager.s_instance.GetComponent<SeqAshleyEscort>().AlreadyPlayed == false && TutoManager.s_instance.GetComponent<SeqMultiCharacters>().AlreadyPlayed == true)
                                    {
                                        TutoManager.s_instance.playSequence(TutoManager.s_instance.GetComponent<SeqAshleyEscort>());
                                    }
                                }
                                if (clickTarget.GetComponentInParent<QuestDealer>() != null)
                                {
                                    //clickTarget.GetComponentInParent<QuestDealer>().;
                                }
                                GameManager.Instance.GoTarget = clickTarget.GetComponentInParent<Interactable>();
                                if (clickTarget.GetComponentInParent<Monster>() != null)
                                {
                                    NavMeshAgent agent = GameManager.Instance.GetFirstSelectedKeeper().GetComponent<NavMeshAgent>();
                                    if (agent != null && agent.isActiveAndEnabled)
                                        agent.SetDestination(clickTarget.transform.position);
                                }
                                else
                                {
                                    ui.UpdateActionPanelUIQ(clickTarget.GetComponentInParent<PawnInstance>().Interactions);
                                }
                            }
                        }
                        else if (hitInfo.collider.gameObject.GetComponent<Arrival>() != null)
                        {
                            if (tileHit == keeperSelectedTile)
                            {
                                GameManager.Instance.GoTarget = clickTarget.GetComponentInParent<Interactable>();
                                ui.UpdateActionPanelUIQ(clickTarget.GetComponent<Arrival>().InterationImplementer);
                            }
                        }
                        else if (hitInfo.collider.gameObject.GetComponent<TilePassage>() != null)
                        {
                            ui.ClearActionPanel();
                            TilePassage tp = hitInfo.collider.gameObject.GetComponent<TilePassage>();
                            if(GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                            {
                                if(tp.GetComponentInParent<Tile>() == GameManager.Instance.ListOfSelectedKeepers[0].CurrentTile)
                                    tp.HandleClick();
                            }
                        }
                        else
                        { 
                            ui.ClearActionPanel();
                            if (tileHit != null)
                            {
                                Tile currentKeeperTile = GameManager.Instance.GetFirstSelectedKeeper().CurrentTile;
                                foreach (PawnInstance pi in GameManager.Instance.GetKeepersOnTile(currentKeeperTile))
                                    pi.GetComponent<Fighter>().IsTargetableByMonster = true;
                                if (GameManager.Instance.PrisonerInstance.CurrentTile == currentKeeperTile)
                                {
                                    if (GameManager.Instance.PrisonerInstance.GetComponent<Fighter>() != null)
                                        GameManager.Instance.PrisonerInstance.GetComponent<Fighter>().IsTargetableByMonster = true;
                                    else
                                        Debug.LogWarning("Missing Fighter component on Prisoner.");
                                }
                            }
                            if (tileHit == keeperSelectedTile)
                            {
                                // Move the keeper
                                for (int i = 0; i < GameManager.Instance.ListOfSelectedKeepers.Count; i++)
                                {
                                    if (GameManager.Instance.ListOfSelectedKeepers[i].GetComponent<Mortal>().IsAlive && !GameManager.Instance.ListOfSelectedKeepers[i].GetComponent<AnimatedPawn>().IsMovingBetweenTiles)
                                    {
                                        GameManager.Instance.ListOfSelectedKeepers[i].GetComponent<AnimatedPawn>().TriggerRotation(hitInfo.point);
                                    }
                                }
                            }
                            else
                            {
                                //TODO: Change this to show the button BEFORE moving
                                if (Array.Exists(GameManager.Instance.GetFirstSelectedKeeper().CurrentTile.Neighbors, x => x == tileHit))
                                {
                                    int neighbourIndex = Array.FindIndex(GameManager.Instance.GetFirstSelectedKeeper().CurrentTile.Neighbors, x => x == tileHit);
                                    Tile currentTile = GameManager.Instance.GetFirstSelectedKeeper().CurrentTile;
                                    TileTrigger tt = currentTile.transform.GetChild(0).GetChild(1).GetChild(neighbourIndex).GetComponent<TileTrigger>();
                                    TilePassage tp = currentTile.transform.GetChild(0).GetChild(1).GetChild(neighbourIndex).GetComponent<TilePassage>();
                                    Vector3 movePosition = Vector3.zero;
                                    if (tt != null)
                                        movePosition = tt.transform.position;
                                    if(tp != null)
                                        movePosition = tp.transform.position;
                                    // Move the keeper

                                    if(tp != null)
                                        tp.HandleClick();
                                }
                            }
                        }
                    }
                }
                else
                {
                    GameManager.Instance.Ui.ClearActionPanel();
                }
            }
        }
    }
    private void InBattleControls()
    {
        if (BattleHandler.IsABattleAlreadyInProcess() && BattleHandler.IsKeepersTurn && BattleHandler.HasDiceBeenThrown && !BattleHandler.IsWaitingForSkillEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.Instance.ListOfSelectedKeepers != null && GameManager.Instance.ListOfSelectedKeepers.Count > 0
                        && (GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().HasClickedOnAttack
                        || BattleHandler.PendingSkill != null))
                {
                    HandleActionValidationDuringBattle();
                }
                else
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        RaycastHit hitInfo;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) == true)
                        {
                            GameObject clickTarget = hitInfo.collider.gameObject;
                            if (GameManager.Instance.ListOfSelectedKeepers != null && GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                            {
                                // TODO: find another way
                                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(GameManager.Instance.GetFirstSelectedKeeper()).gameObject.SetActive(false);
                            }

                            if (clickTarget.GetComponentInParent<PawnInstance>() != null)
                            {
                                if (clickTarget.GetComponentInParent<Keeper>() != null)
                                {
                                    if (clickTarget.GetComponentInParent<Fighter>() != null && !clickTarget.GetComponentInParent<Fighter>().HasPlayedThisTurn)
                                    {
                                        Keeper clickedKeeper = clickTarget.GetComponentInParent<Keeper>();
                                        GameManager.Instance.ClearListKeeperSelected();
                                        GameManager.Instance.AddKeeperToSelectedList(clickedKeeper.getPawnInstance);
                                        clickedKeeper.IsSelected = true;
                                        GameManager.Instance.Ui.UpdateActionPanelUIForBattle(clickedKeeper.GetComponent<Fighter>());
                                    }
                                }
                                else if (clickTarget.GetComponentInParent<Monster>() != null)
                                {
                                    // TODO: Show monster informations (pv, name, etc.)
                                }
                            }
                            else
                            {
                                if (GameManager.Instance.ListOfSelectedKeepers != null && GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                                {
                                    // TODO: find another way
                                    GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(GameManager.Instance.GetFirstSelectedKeeper()).gameObject.SetActive(false);
                                }
                                GameManager.Instance.ClearListKeeperSelected();
                                GameManager.Instance.Ui.mouseFollower.SetActive(false);
                                BattleHandler.ActivateFeedbackSelection(true, false);
                                BattleHandler.DeactivateFeedbackSelection(false, true);
                            }
                        }
                        else
                        {
                            if (GameManager.Instance.ListOfSelectedKeepers != null && GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                            {
                                // TODO: find another way
                                GameManager.Instance.GetBattleUI.GetComponent<UIBattleHandler>().GetSkillsPanelIndex(GameManager.Instance.GetFirstSelectedKeeper()).gameObject.SetActive(false);
                            }
                            GameManager.Instance.ClearListKeeperSelected();
                            GameManager.Instance.Ui.mouseFollower.SetActive(false);
                            BattleHandler.ActivateFeedbackSelection(true, false);
                            BattleHandler.DeactivateFeedbackSelection(false, true);
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (GameManager.Instance.ListOfSelectedKeepers != null && GameManager.Instance.ListOfSelectedKeepers.Count > 0
                        && (GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().HasClickedOnAttack
                        || BattleHandler.PendingSkill != null))
                {
                    HandleActionValidationDuringBattle();
                }
                else if (GameManager.Instance.ListOfSelectedKeepers == null || GameManager.Instance.ListOfSelectedKeepers.Count == 0)
                {
                    BattleHandler.ActivateFeedbackSelection(true, false);
                }
            }
        }
    }

    // TODO: some things are in double for left click
    private void HandleActionValidationDuringBattle()
    {
        GameManager.Instance.Ui.mouseFollower.SetActive(false);
        BattleHandler.DeactivateFeedbackSelection(false, true);

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) == true)
            {
                GameObject clickTarget = hitInfo.collider.gameObject;
                if (clickTarget.GetComponentInParent<PawnInstance>() != null)
                {
                    if (GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().HasClickedOnAttack)
                        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().AttackProcess(clickTarget.GetComponentInParent<Fighter>());
                    else
                    {
                        BattleHandler.PendingSkill.UseSkill(clickTarget.GetComponentInParent<PawnInstance>());
                    }
                }
                else
                {
                    GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().HasClickedOnAttack = false;
                    BattleHandler.PendingSkill = null;
                    GameManager.Instance.ClearListKeeperSelected();
                }
            }
            else
            {
                GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().HasClickedOnAttack = false;
                BattleHandler.PendingSkill = null;
                GameManager.Instance.ClearListKeeperSelected();
            }
        }

        if (!BattleHandler.WasTheLastToPlay && !BattleHandler.IsWaitingForSkillEnd)
            BattleHandler.ActivateFeedbackSelection(true, false);        
    }

    private void ChangeSelectedKeeper()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.Instance.CurrentState == GameState.Normal)
            {
                if (GameManager.Instance.ListOfSelectedKeepers != null && GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                {
                    if(GameManager.Instance.CameraManagerReference.state == CameraManager.CameraState.Far)
                    {
                        // Get first selected
                        PawnInstance currentKeeperSelected = GameManager.Instance.ListOfSelectedKeepers[0];
                        // Get next on tile
                        List<PawnInstance> keepers = GameManager.Instance.AllKeepersList;
                        int currentKeeperSelectedIndex = keepers.FindIndex(x => x == currentKeeperSelected);
                        // Next keeper on the same tile is now active
                        GameManager.Instance.ClearListKeeperSelected();
                        PawnInstance nextKeeper = keepers[(currentKeeperSelectedIndex + 1) % keepers.Count];
                        GameManager.Instance.AddKeeperToSelectedList(nextKeeper);
                        Vector3 pos = GameManager.Instance.CameraManagerReference.transform.position;
                        nextKeeper.GetComponent<Keeper>().IsSelected = true;
                        //GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
                        GameManager.Instance.Ui.HideInventoryPanels();
                    }
                    else
                    {
                        // Get first selected
                        PawnInstance currentKeeperSelected = GameManager.Instance.ListOfSelectedKeepers[0];
                        // Get his tile
                        Tile currentKeeperTile = TileManager.Instance.GetTileFromKeeper[currentKeeperSelected];
                        // Get next on tile
                        List<PawnInstance> keepersOnTile = TileManager.Instance.KeepersOnTile[currentKeeperTile];
                        int currentKeeperSelectedIndex = keepersOnTile.FindIndex(x => x == currentKeeperSelected);
                        // Next keeper on the same tile is now active
                        GameManager.Instance.ClearListKeeperSelected();
                        PawnInstance nextKeeper = keepersOnTile[(currentKeeperSelectedIndex + 1) % keepersOnTile.Count];
                        GameManager.Instance.AddKeeperToSelectedList(nextKeeper);
                        nextKeeper.GetComponent<Keeper>().IsSelected = true;
                        //GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
                        GameManager.Instance.Ui.HideInventoryPanels();
                    }
                }
            }
            if (GameManager.Instance.CurrentState == GameState.InBattle)
            {
                if (GameManager.Instance.ListOfSelectedKeepers != null && GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                {
                    // Get first selected
                    PawnInstance currentKeeperSelected = GameManager.Instance.GetFirstSelectedKeeper();
                    // Get next in battle
                    List<PawnInstance> keepersInBattle = new List<PawnInstance>();
                    keepersInBattle.AddRange(BattleHandler.CurrentBattleKeepers);
                    int currentKeeperSelectedIndex = keepersInBattle.FindIndex(x => x == currentKeeperSelected);
                    // Next keeper is now active
                    GameManager.Instance.ClearListKeeperSelected();
                    PawnInstance nextKeeper;
                    int nbIterations = 0;
                    do
                    {
                        nextKeeper = keepersInBattle[(currentKeeperSelectedIndex + nbIterations + 1) % keepersInBattle.Count];
                        nbIterations++;
                    } while (nextKeeper.GetComponent<Fighter>().HasPlayedThisTurn && nbIterations < 3);
                    GameManager.Instance.AddKeeperToSelectedList(nextKeeper);
                    nextKeeper.GetComponent<Keeper>().IsSelected = true;
                }
                else
                {
                    GameManager.Instance.ClearListKeeperSelected();
                    PawnInstance nextKeeper = BattleHandler.CurrentBattleKeepers[0];
                    GameManager.Instance.AddKeeperToSelectedList(nextKeeper);
                    nextKeeper.GetComponent<Keeper>().IsSelected = true;
                }
            }
        }
    }
    private void UpdateDoubleCick()
    {
        if (fTimerDoubleClick > 0)
        {
            fTimerDoubleClick -= Time.unscaledDeltaTime;
        }
        else
        {
            goPreviousLeftclicked = null;
        }
    }
    private void ShortcutMenuControls()
    {
        if (GameManager.Instance.CurrentState == GameState.Normal)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                GameManager.Instance.Ui.ToggleShortcutPanel();
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                GameManager.Instance.Ui.EndTurn();
            }
        }
    }

    private void CursorNormalState()
    {
        Cursor.SetCursor(GameManager.Instance.Texture2DUtils.iconeMouse, Vector2.zero, CursorMode.Auto);
    }
}

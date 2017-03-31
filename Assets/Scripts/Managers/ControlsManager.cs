using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Behaviour;

public class ControlsManager : MonoBehaviour {

    public GameObject goPreviousLeftclicked;
    float fTimerDoubleClick;
    [SerializeField] private float fDoubleClickCoolDown = 0.3f;

    public LayerMask layerMask;

    void Start () {
        goPreviousLeftclicked = null;
        fTimerDoubleClick = 0;
    }
	
	void Update () {
        SelectionControls();
        ChangeSelectedKeeper();
        UpdateDoubleCick();
        ShortcutMenuControls();
        //CameraControls();
    }

    private void SelectionControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) == true)
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
                        GameManager.Instance.Ui.HideSelectedKeeperPanel();
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
                        Tile tileHit = hitInfo.collider.gameObject.GetComponentInParent<Tile>();
                        Tile keeperSelectedTile = GameManager.Instance.GetFirstSelectedKeeper().CurrentTile;
                        GameObject clickTarget = hitInfo.collider.gameObject;

                        // Handle click on a ItemInstance
                        if (clickTarget.GetComponent<ItemInstance>() != null)
                        {
                            if (tileHit == keeperSelectedTile)
                            {
                                GameManager.Instance.GoTarget = hitInfo.collider.gameObject;
                                ui.UpdateActionPanelUIQ(hitInfo.collider.gameObject.GetComponent<ItemInstance>().InteractionImplementer);
                            }
                        }
                        // Handle click on a ItemInstance
                        else if (clickTarget.GetComponent<LootInstance>() != null)
                        {
                            if (tileHit == keeperSelectedTile)
                            {
                                GameManager.Instance.GoTarget = hitInfo.collider.gameObject;
                                ui.UpdateActionPanelUIQ(hitInfo.collider.gameObject.GetComponent<LootInstance>().InteractionImplementer);
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

                                GameManager.Instance.GoTarget = clickTarget;
                                ui.UpdateActionPanelUIQ(clickTarget.GetComponentInParent<PawnInstance>().Interactions);
                            }
                        }
                        else if (hitInfo.collider.gameObject.GetComponent<Arrival>() != null)
                        {
                            if (tileHit == keeperSelectedTile)
                            {
                                GameManager.Instance.GoTarget = clickTarget;
                                ui.UpdateActionPanelUIQ(clickTarget.GetComponent<Arrival>().InterationImplementer);
                            }
                        }
                        else
                        {
                            ui.ClearActionPanel();
                            if (tileHit != null)
                            {
                                GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Fighter>().IsTargetableByMonster = true;
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

                                    if (tt.piList.Contains(GameManager.Instance.GetFirstSelectedKeeper()))
                                    {
                                        tt.HandleTrigger(GameManager.Instance.GetFirstSelectedKeeper());
                                    }
                                    else
                                    {
                                        Vector3 movePosition = tt.transform.position;
                                        // Move the keeper
                                        for (int i = 0; i < GameManager.Instance.ListOfSelectedKeepers.Count; i++)
                                        {
                                            GameManager.Instance.ListOfSelectedKeepers[i].GetComponent<AnimatedPawn>().TriggerRotation(movePosition);
                                        }
                                    }
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

    private void ChangeSelectedKeeper()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.Instance.ListOfSelectedKeepers != null && GameManager.Instance.ListOfSelectedKeepers.Count > 0)
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
                nextKeeper.GetComponent<Behaviour.Keeper>().IsSelected = true;

                //GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
                GameManager.Instance.Ui.HideInventoryPanels();
            }
        }
    }

    private void UpdateDoubleCick()
    {
        if(fTimerDoubleClick > 0)
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.Ui.ToggleShortcutPanel();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameManager.Instance.Ui.EndTurn();
        }
    }

    /*private void CameraControls()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {

        }

        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
            {
                float fov = Camera.main.fieldOfView;
                fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
                fov = Mathf.Clamp(fov, minFov, maxFov);
                Camera.main.fieldOfView = fov;
            }
            return;
        }

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 distance = Camera.main.transform.forward * 3.0f;
            Vector3 point = Camera.main.transform.position + distance;
            Vector3 perp = new Vector3(pos.y, -pos.x, 0);
            Vector3 rotateAroundAxis = perp.x * Camera.main.transform.right + perp.y * Vector3.up;
            float rotateDegrees = Mathf.Rad2Deg * Mathf.Atan(pos.magnitude / distance.magnitude);
            // TODO: Clamp
            Camera.main.transform.RotateAround(point, rotateAroundAxis, rotateDegrees);
        }
        else
        {
            //Vector3 move = new Vector3(-pos.x, -pos.y, 0);
            //Camera.main.transform.Translate(move.normalized * dragSpeed, Space.Self);
            Vector3 move = new Vector3(-pos.x, 0 ,-pos.y);
            Camera.main.transform.Translate(move.normalized * dragSpeed, Space.World);
        }

        dragOrigin = Input.mousePosition;
    }*/
}

/*float fHorizontalAxis = Input.GetAxisRaw("Horizontal"), fVerticalAxis = Input.GetAxisRaw("Vertical");
fHorizontalAxis = (fHorizontalAxis != 0.0f) ? fHorizontalAxis : 0.0f;
fVerticalAxis = (fVerticalAxis != 0.0f) ? fVerticalAxis : 0.0f;
Vector3 v3InputDirection = new Vector3(fHorizontalAxis, 0.0f ,fVerticalAxis).normalized;
if (v3InputDirection.magnitude > 0.0f)
{
    currentCharacter.transform.position += v3InputDirection * fMoveSpeed * Time.unscaledDeltaTime;


    quatPreviousRotation = currentCharacter.transform.rotation;
    quatTargetRotation.SetLookRotation(v3InputDirection);

    if(Quaternion.Angle(quatPreviousRotation, quatTargetRotation) > 10.0f){
        bIsRotating = true;
    }
    else
    {
        bIsRotating = false;
    }
}*/

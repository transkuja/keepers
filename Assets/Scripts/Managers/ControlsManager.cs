using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlsManager : MonoBehaviour {

    public GameObject goPreviousLeftclicked;
    float fTimerDoubleClick;
    [SerializeField] private float fDoubleClickCoolDown = 0.3f;

    public LayerMask layerMask;
    //public List<Character> listCharacters;

    /*public Character currentCharacter = null;

    public int iIdCurrentCharacter = 0;

    private float fMoveSpeed = 1.0f;
    private float fRotateSpeed = 1.1f;
    private float fLerpRotation = 0.0f;

    Quaternion quatPreviousRotation, quatTargetRotation;
    bool bIsRotating = false;*/

    // Camera parameters
    /*[Header("Camera Controls")]
    [SerializeField]
    float dragSpeed = 2;
    Vector3 dragOrigin;

    float minFov = 15f;
    float maxFov = 90f;
    float sensitivity = 10f;*/


    // Use this for initialization
    void Start () {
        //currentCharacter = listCharacters[iIdCurrentCharacter];
        goPreviousLeftclicked = null;
        fTimerDoubleClick = 0;
    }
	
	// Update is called once per frame
	void Update () {
        SelectionControls();
        ChangeSelectedKeeper();
        UpdateDoubleCick();
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
                    if (hitInfo.transform.gameObject.GetComponentInParent<KeeperInstance>() != null)
                    {
                        KeeperInstance c = hitInfo.transform.gameObject.GetComponentInParent<KeeperInstance>();
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            if (GameManager.Instance.ListOfSelectedKeepers.Contains(c))
                            {
                                GameManager.Instance.ListOfSelectedKeepers.Remove(c);
                                c.IsSelected = false;
                            }
                            else
                            {
                                GameManager.Instance.ListOfSelectedKeepers.Add(c);
                                c.IsSelected = true;
                            }
                        }
                        else
                        {
      
                            GameManager.Instance.ClearListKeeperSelected();
                            GameManager.Instance.ListOfSelectedKeepers.Add(c);

                            GameManager.Instance.Ui.ShowSelectedKeeperPanel();
                            GameManager.Instance.Ui.UpdateSelectedKeeperPanel();

                            GameManager.Instance.Ui.HideInventoryPanels();

                            GameManager.Instance.Ui.UpdateActionText();
                            c.IsSelected = true;
                        }

                        if (fTimerDoubleClick > 0 && goPreviousLeftclicked == hitInfo.transform.gameObject)
                        {
                            Camera.main.GetComponent<CameraManager>().UpdateCameraPosition(c);
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

                        // Handle click on a ItemInstance
                        if (hitInfo.collider.gameObject.GetComponent<ItemInstance>() != null)
                        {
                            if (tileHit == TileManager.Instance.GetTileFromKeeper[GameManager.Instance.ListOfSelectedKeepers[0]])
                            {
                                GameManager.Instance.GoTarget = hitInfo.collider.gameObject;
                                ui.UpdateActionPanelUIQ(hitInfo.collider.gameObject.GetComponent<ItemInstance>().InteractionImplementer);
                            }
                        }
                        // Handle click on a ItemInstance
                        else if (hitInfo.collider.gameObject.GetComponent<LootInstance>() != null)
                        {
                            if (tileHit == TileManager.Instance.GetTileFromKeeper[GameManager.Instance.ListOfSelectedKeepers[0]])
                            {
                                GameManager.Instance.GoTarget = hitInfo.collider.gameObject;
                                ui.UpdateActionPanelUIQ(hitInfo.collider.gameObject.GetComponent<LootInstance>().InteractionImplementer);
                            }
                        }
                        // Handle click on a PNJInstance
                        else if (hitInfo.collider.gameObject.GetComponent<PNJInstance>() != null)
                        {
                            if (tileHit == TileManager.Instance.GetTileFromKeeper[GameManager.Instance.ListOfSelectedKeepers[0]])
                            {
                                GameManager.Instance.GoTarget = hitInfo.collider.gameObject;
                                ui.UpdateActionPanelUIQ(hitInfo.collider.gameObject.GetComponent<PNJInstance>().InteractionImplementer);
                            }
                        }
                        // Handle click on a PrisonerInstance
                        else if (hitInfo.collider.gameObject.GetComponentInParent<PrisonerInstance>() != null)
                        {
                            if (TileManager.Instance.PrisonerTile == TileManager.Instance.GetTileFromKeeper[GameManager.Instance.ListOfSelectedKeepers[0]])
                            {
                                GameManager.Instance.GoTarget = hitInfo.collider.gameObject;
                                ui.UpdateActionPanelUIQ(hitInfo.collider.gameObject.GetComponentInParent<PrisonerInstance>().InteractionImplementer);
                            }
                        }
                        else if (hitInfo.collider.gameObject.GetComponentInParent<KeeperInstance>() != null)
                        {
                            tileHit = TileManager.Instance.GetTileFromKeeper[hitInfo.collider.gameObject.GetComponentInParent<KeeperInstance>()];
                            if (hitInfo.collider.gameObject.GetComponentInParent<KeeperInstance>() != GameManager.Instance.ListOfSelectedKeepers[0]
                                && tileHit == TileManager.Instance.GetTileFromKeeper[GameManager.Instance.ListOfSelectedKeepers[0]])
                            {
                                GameManager.Instance.GoTarget = hitInfo.collider.gameObject;
                                ui.UpdateActionPanelUIQ(hitInfo.collider.gameObject.GetComponentInParent<KeeperInstance>().InteractionImplementer);
                            }
                     
                        }
                        else if(hitInfo.collider.gameObject.GetComponent<Arrival>() != null)                            
                        {
                            if (tileHit == TileManager.Instance.GetTileFromKeeper[GameManager.Instance.ListOfSelectedKeepers[0]])
                            {
                                GameManager.Instance.GoTarget = hitInfo.collider.gameObject;
                                ui.UpdateActionPanelUIQ(hitInfo.collider.gameObject.GetComponent<Arrival>().InterationImplementer);
                            }
                        }
                        else
                        {                           
                            ui.ClearActionPanel();
                            if (tileHit == TileManager.Instance.GetTileFromKeeper[GameManager.Instance.ListOfSelectedKeepers[0]])
                            {
                                // Move the keeper
                                for (int i = 0; i < GameManager.Instance.ListOfSelectedKeepers.Count; i++)
                                {

                                    GameManager.Instance.ListOfSelectedKeepers[i].TriggerRotation(hitInfo.point);

                                }
                            }
                            else
                            {
                                //TODO: Change this to show the button BEFORE moving
                                if (Array.Exists(TileManager.Instance.GetTileFromKeeper[GameManager.Instance.ListOfSelectedKeepers[0]].Neighbors, x => x == tileHit))
                                {
                                    int neighbourIndex = Array.FindIndex(TileManager.Instance.GetTileFromKeeper[GameManager.Instance.ListOfSelectedKeepers[0]].Neighbors, x => x == tileHit);
                                    Tile currentTile = TileManager.Instance.GetTileFromKeeper[GameManager.Instance.ListOfSelectedKeepers[0]];
                                    Vector3 movePosition = currentTile.transform.GetChild(0).GetChild(1).GetChild(neighbourIndex).position;
                                    // Move the keeper
                                    for (int i = 0; i < GameManager.Instance.ListOfSelectedKeepers.Count; i++)
                                    {

                                        GameManager.Instance.ListOfSelectedKeepers[i].TriggerRotation(movePosition);

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
                KeeperInstance currentKeeperSelected = GameManager.Instance.ListOfSelectedKeepers[0];

                // Get his tile
                Tile currentKeeperTile = TileManager.Instance.GetTileFromKeeper[currentKeeperSelected];

                // Get next on tile
                List<KeeperInstance> keepersOnTile = TileManager.Instance.KeepersOnTile[currentKeeperTile];
                int currentKeeperSelectedIndex = keepersOnTile.FindIndex(x => x == currentKeeperSelected);

                // Next keeper on the same tile is now active
                GameManager.Instance.ClearListKeeperSelected();
                KeeperInstance nextKeeper = keepersOnTile[(currentKeeperSelectedIndex + 1) % keepersOnTile.Count];
                GameManager.Instance.ListOfSelectedKeepers.Add(nextKeeper);
                nextKeeper.IsSelected = true;

                Camera.main.GetComponent<CameraManager>().UpdateCameraPosition(nextKeeper);

                GameManager.Instance.SelectedKeeperNeedUpdate = true;
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

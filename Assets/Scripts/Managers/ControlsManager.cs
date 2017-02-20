using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlsManager : MonoBehaviour {

    //public List<Character> listCharacters;

    /*public Character currentCharacter = null;

    public int iIdCurrentCharacter = 0;

    private float fMoveSpeed = 1.0f;
    private float fRotateSpeed = 1.1f;
    private float fLerpRotation = 0.0f;

    Quaternion quatPreviousRotation, quatTargetRotation;
    bool bIsRotating = false;*/

    // Camera parameters
    [Header("Camera Controls")]
    [SerializeField]
    float dragSpeed = 2;
    Vector3 dragOrigin;

    float minFov = 15f;
    float maxFov = 90f;
    float sensitivity = 10f;


    // Use this for initialization
    void Start () {
        //currentCharacter = listCharacters[iIdCurrentCharacter];
    }
	
	// Update is called once per frame
	void Update () {
        SelectionControls();
        CameraControls();
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
                    if (hitInfo.transform.gameObject.GetComponent<KeeperInstance>() != null)
                    {
                        KeeperInstance c = hitInfo.transform.gameObject.GetComponent<KeeperInstance>();
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
                            c.IsSelected = true;
                        }
                    }
                    else
                    {
                        GameManager.Instance.ClearListKeeperSelected();
                    }
                }
                // Handle click on a actionnable
                GameManager.Instance.listOfActions.Clear();

                // Handle click on anything else
                GameManager.Instance.ActionPanelNeedUpdate = true;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
                {
                    RaycastHit hitInfo;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) == true)
                    {
                        // Handle click on a actionnable
                        GameManager.Instance.listOfActions.Clear();
                        if (hitInfo.collider.GetComponent<Actionable>() != null)
                        {
                            GameManager.Instance.GoTarget = hitInfo.collider.gameObject;
                            HandleClickOnActionable(hitInfo.collider.GetComponent<Actionable>());
                        }
                        else
                        {
                            // Move the keeper
                            for (int i = 0; i < GameManager.Instance.ListOfSelectedKeepers.Count; i++)
                            {
                                //GameManager.Instance.ListOfSelectedKeepers[i].gameObject.GetComponent<NavMeshAgent>().enabled = true;
                                //GameManager.Instance.ListOfSelectedKeepers[i].gameObject.GetComponent<NavMeshAgent>().destination = hitInfo.point;

                                GameManager.Instance.ListOfSelectedKeepers[i].TriggerRotation(hitInfo.point);

                            }
                        }
                    }

                    // Handle click on anything else
                    GameManager.Instance.ActionPanelNeedUpdate = true;
                }
            }
        }                  

    }

    private void CameraControls()
    {

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
            Vector3 move = new Vector3(-pos.x, -pos.y, 0);
            Camera.main.transform.Translate(move.normalized * dragSpeed, Space.Self);
        }

        dragOrigin = Input.mousePosition;
    }

    private void HandleClickOnActionable(Actionable a)
    {
        GameManager.Instance.listOfActions.AddRange(a.listOfActions);
    }
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

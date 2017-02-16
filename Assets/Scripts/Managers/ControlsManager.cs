using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    // TODO: suppress warnings by implementing full camera controls
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
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) == true)
            {
                KeeperInstance c = null;
                if ((c = hitInfo.transform.gameObject.GetComponent<KeeperInstance>()) != null)
                {
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
                    Debug.Log("deselected");
                    GameManager.Instance.ClearListKeeperSelected();
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) == true)
                {
                    for (int i = 0; i < GameManager.Instance.ListOfSelectedKeepers.Count; i++)
                    {
                        GameManager.Instance.ListOfSelectedKeepers[i].gameObject.GetComponent<NavMeshAgent>().destination = hitInfo.point;
                    }
                }
            }
        }
    }

    // TODO: finish camera controls (yaw pitch)
    private void CameraControls()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
        {
            float fov = Camera.main.fieldOfView;
            fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Camera.main.fieldOfView = fov;
        }

        ///if (Input.GetMouseButtonDown(0))
        ///{
        ///    dragOrigin = Input.mousePosition;
        ///    return;
        ///}

        ///if (!Input.GetMouseButton(0)) return;

        ///Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        ///Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

        ///transform.Translate(move, Space.World);
    }

    /*void RotateCharater(Character character)
    {
        if(fLerpRotation >= 1.0f)
        {
            character.transform.rotation = quatTargetRotation;
            fLerpRotation = 0.0f;
            bIsRotating = false;
        }
        else
        {
            fLerpRotation += fRotateSpeed * Time.unscaledDeltaTime;
            character.transform.rotation = Quaternion.Lerp(quatPreviousRotation, quatTargetRotation, 0.2f);
        }
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
}

if (bIsRotating)
{
    RotateCharater(currentCharacter);
}

if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
{
    if (Input.GetKeyDown(KeyCode.J)){
        iIdCurrentCharacter = (iIdCurrentCharacter - 1 + listCharacters.Count) % listCharacters.Count;
    }
    else if (Input.GetKeyDown(KeyCode.K))
    {
        iIdCurrentCharacter = (iIdCurrentCharacter + 1) % listCharacters.Count;
    }
    currentCharacter.gameObject.GetComponentInChildren<MeshRenderer>().material.shader = Shader.Find("Diffuse");
    currentCharacter = listCharacters[iIdCurrentCharacter];
    currentCharacter.gameObject.GetComponentInChildren<MeshRenderer>().material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
}*/

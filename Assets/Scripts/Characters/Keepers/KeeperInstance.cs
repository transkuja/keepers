using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KeeperInstance : MonoBehaviour {

    [Header("Keeper Info")]
    [SerializeField]
    private Keeper keeper = null;
    private bool isSelected = false;

    [SerializeField]
    private GameObject goSelectionAura;

    // Used only in menu. Handles selection in main menu.
    [SerializeField]
    private bool isSelectedInMenu = false;
    public MeshRenderer meshToHighlight;


    // Update variables
    NavMeshAgent agent;

    Vector3 v3AgentDirectionTemp;

    // Rotations
    float fLerpRotation = 0.666f;
    Quaternion quatTargetRotation;
    Quaternion quatPreviousRotation;
    bool bIsRotating = false;
    [SerializeField]
    float fRotateSpeed = 1.0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fRotateSpeed = 5.0f;
    }

    private void Update()
    {
        Vector3 currentPosition;
        if (agent.isOnOffMeshLink)
        {
            currentPosition = transform.position;
            if (Input.GetKeyDown(KeyCode.A))
            {
                agent.CompleteOffMeshLink();
                TileManager.Instance.MoveKeeper(this, TileManager.Instance.GetTileFromKeeper[this], Direction.North_East);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {

                agent.CompleteOffMeshLink();
                agent.Warp(currentPosition);
            }
        }

        /*GameObject goDestinationTemp = gameObject;
        for(int i=0; i < keeper.GoListCharacterFollowing.Count; goDestinationTemp = keeper.GoListCharacterFollowing[i], i++)
        {
            keeper.GoListCharacterFollowing[i].GetComponent<NavMeshAgent>().destination = goDestinationTemp.transform.position;
        }*/

        if (bIsRotating)
        {
            Rotate();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<MonsterInstance>())
        {
            agent.Stop();
            agent.ResetPath();
            BattleHandler.LaunchBattle(TileManager.Instance.GetTileFromKeeper[this]);
            agent.Resume();
        }
    }


    private void ToggleHighlightOnMesh(bool isSelected)
    {
        if (meshToHighlight != null)
        {
            if (isSelected)
            {
                meshToHighlight.material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
                meshToHighlight.material.SetColor("_OutlineColor", Color.blue);
            }
            else
            {
                meshToHighlight.material.shader = Shader.Find("Diffuse");
            }
        }
    }

    public KeeperInstance(KeeperInstance from)
    {
        keeper = from.keeper;
        goSelectionAura = from.goSelectionAura;

    }

    /* ------------------------------------------------------------------------------------ */
    /* ------------------------------------- Accessors ------------------------------------ */
    /* ------------------------------------------------------------------------------------ */

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }

        set
        {
            isSelected = value;
            GoSelectionAura.SetActive(value);
        }
    }



    public bool IsSelectedInMenu
    {
        get
        {
            return isSelectedInMenu;
        }

        set
        {
            isSelectedInMenu = value;
            ToggleHighlightOnMesh(isSelectedInMenu);
        }
    }

    public Keeper Keeper
    {
        get
        {
            return keeper;
        }

        set
        {
            keeper = value;
        }
    }

    public GameObject GoSelectionAura
    {
        get
        {
            return goSelectionAura;
        }

        set
        {
            goSelectionAura = value;
        }
    }

    public void TriggerRotation(Vector3 v3Direction)
    {
        agent.angularSpeed = 0.0f;

        quatPreviousRotation = transform.rotation;

        quatTargetRotation.SetLookRotation(v3Direction - transform.position);

        bIsRotating = true;

        agent.enabled = false;

        v3AgentDirectionTemp = v3Direction;

        fLerpRotation = 0.0f;
    }

    void Rotate()
    {
        if(fLerpRotation >= 1.0f)
        {
            transform.rotation = quatTargetRotation;
            bIsRotating = false;
            agent.enabled = true;
            fLerpRotation = 0.0f;

            agent.destination = v3AgentDirectionTemp;
            agent.angularSpeed = 100.0f;
        }
        else
        {
            fLerpRotation += fRotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(quatPreviousRotation, quatTargetRotation, fLerpRotation);
        }
    }
}

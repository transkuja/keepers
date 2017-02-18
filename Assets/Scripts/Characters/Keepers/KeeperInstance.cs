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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
            }

            if (Input.GetKeyDown(KeyCode.B))
            {

                agent.CompleteOffMeshLink();
                agent.Warp(currentPosition);
            }
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

    public void OnTriggerEnter(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.tag == "TilePath")
        {
            // TODO Trigger UI Change Tile
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
}

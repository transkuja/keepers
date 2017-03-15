using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJInstance : MonoBehaviour {

    [Header("PNJ Info")]
    [SerializeField]
    private PNJ pnj = null;

    public GameObject pnjInventoryPanel;

    // Actions
    [Header("Actions")]

    private InteractionImplementer interactionImplementer;

    // Mouvement
    [Header("Mouvement")]
    // Update variables
    UnityEngine.AI.NavMeshAgent agent;
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
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        fRotateSpeed = 5.0f;

        InteractionImplementer = new InteractionImplementer();
        InteractionImplementer.Add(new Interaction(Trade), "Trade", GameManager.Instance.Ui.spriteTrade);

        pnjInventoryPanel = GameManager.Instance.Ui.CreatePNJInventoryPanels(this);
    }

    public PNJInstance(PNJInstance from)
    {
        pnj = from.pnj;
    }

    /* ------------------------------------------------------------------------------------ */
    /* ------------------------------------- Accessors ------------------------------------ */
    /* ------------------------------------------------------------------------------------ */



    public PNJ Pnj
    {
        get
        {
            return pnj;
        }

        set
        {
            pnj = value;
        }
    }

    public InteractionImplementer InteractionImplementer
    {
        get
        {
            return interactionImplementer;
        }

        set
        {
            interactionImplementer = value;
        }
    }

    public void Trade(int _i = 0)
    {
        pnjInventoryPanel.SetActive(true);
        GameManager.Instance.Ui.UpdateInventoryPanel(gameObject);
    }
}

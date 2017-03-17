using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJInstance : MonoBehaviour {

    [Header("PNJ Info")]
    [SerializeField]
    private PNJ pnj = null;

    public GameObject pnjInventoryPanel;

    private InteractionImplementer interactionImplementer;

    private void Start()
    {

        InteractionImplementer = new InteractionImplementer();
        InteractionImplementer.Add(new Interaction(Trade), "Trade", GameManager.Instance.Ui.spriteTrade);

        pnjInventoryPanel = GameManager.Instance.Ui.CreateInventoryPanel(this.gameObject);
    }

    public PNJInstance(PNJInstance from)
    {
        pnj = from.pnj;
    }


    #region Accessors

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
    #endregion

    public void Trade(int _i = 0)
    {
        pnjInventoryPanel.SetActive(true);
        GameManager.Instance.Ui.UpdateInventoryPanel(gameObject);
    }
}

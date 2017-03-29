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
        InteractionImplementer.Add(new Interaction(Trade), 0, "Trade", GameManager.Instance.SpriteUtils.spriteTrade);

        GetComponent<Inventory>().List_inventaire = ComputeItems();
        pnjInventoryPanel = GameManager.Instance.Ui.CreateInventoryPanel(this.gameObject);

        if (GetComponent<QuestDealer>() != null)
        {
            InteractionImplementer.Add(new Interaction(Quest), 1, "Quest", GameManager.Instance.SpriteUtils.spriteQuest);
        }
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

    public void Quest(int _i = 0)
    {
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {
            int costAction = interactionImplementer.Get("Quest").costAction;
            if (GameManager.Instance.ListOfSelectedKeepers[0].ActionPoints >= costAction)
            {
                GameManager.Instance.ListOfSelectedKeepers[0].ActionPoints -= (short)costAction;
                GetComponent<QuestDealer>().goQuest.SetActive(true);
            }
            else
            {
                GameManager.Instance.Ui.ZeroActionTextAnimation();
            }
        }
    }

    public List<ItemContainer> ComputeItems()
    {
        List<ItemContainer> tmpList = new List<ItemContainer>();
        Item it = null;
        foreach (string _IdItem in pnj.PossibleItems)
        {
            it = GameManager.Instance.Database.getItemById(_IdItem);
            if (Random.Range(0, 10) > it.Rarity)
            {
                tmpList.Add(new ItemContainer(it, 1));
            }
        }

        return tmpList;
    }
}

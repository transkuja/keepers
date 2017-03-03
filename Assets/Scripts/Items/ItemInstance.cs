using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInstance : MonoBehaviour, IPickable {

    [SerializeField]
    public TypeItem typeItem;

    // Hide if not equipement
    [HideInInspector]
    public TypeEquipement equipType;

    [HideInInspector]
    public BonusStats bonusStats;

    // Hide if not consummable
    [HideInInspector]
    public int quantite;

    [SerializeField]
    public Item item;

    private InteractionImplementer interactionImplementer;

    // Use this for initialization
    void Awake() {
        Init();
    }

    public void Pick(int _i = 0)
    {
        ItemManager.AddItem(GameManager.Instance.ListOfSelectedKeepers[0], item);
        GameManager.Instance.Ui.UpdateSelectedKeeperPanel();
        Destroy(gameObject);
    }

    #region Constructors
    public void Init()
    {
        item = ItemManager.getInstanciateItem(typeItem);
        switch(typeItem)
        {
            case TypeItem.Equipement:
                ((Equipement)item).type = equipType;
                ((Equipement)item).stats = bonusStats;
                break;
            case TypeItem.Consummable:
                ((Consummable)item).quantite = quantite;
                break;
            default:
                break;

        }

        interactionImplementer = new InteractionImplementer();
        interactionImplementer.Add(new Interaction(Pick), "Pick", null);
    }
    #endregion

    #region Accessors
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
}

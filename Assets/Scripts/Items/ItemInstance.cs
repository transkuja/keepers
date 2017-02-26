using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance : MonoBehaviour, IPickable {

    Item item;

    private InteractionImplementer interactionImplementer;

    // Use this for initialization
    void Awake() {
        Init();
    }

    public void Pick()
    {
        Debug.Log("Picked");
    }

    #region Constructors
    public void Init()
    {
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

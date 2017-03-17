using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private List<ItemContainer> list_inventaire;

    public List<ItemContainer> List_inventaire
    {
        get
        {
            return list_inventaire;
        }

        set
        {
            list_inventaire = value;
        }
    }

    public void Awake()
    {
        list_inventaire = new List<ItemContainer>();
    }
}
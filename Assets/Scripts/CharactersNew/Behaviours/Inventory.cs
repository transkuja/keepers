using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private List<ItemContainer> items;

    public List<ItemContainer> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    public void Awake()
    {
        items = new List<ItemContainer>();
    }
}
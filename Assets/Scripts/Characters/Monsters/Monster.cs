using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster : Character
{
    [SerializeField]
    private List<ItemContainer> possibleDrops;

    public List<ItemContainer> PossibleDrops
    {
        get
        {
            return possibleDrops;
        }

        set
        {
            possibleDrops = value;
        }
    }
}

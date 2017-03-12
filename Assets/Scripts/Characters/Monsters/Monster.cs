using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster : Character
{
    [SerializeField]
    private List<ItemInstance> possibleDrops;

    public List<ItemInstance> PossibleDrops
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

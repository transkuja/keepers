using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Monster : Character
{
    [SerializeField]
    private List<string> possibleDrops;

    public List<string> PossibleDrops
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

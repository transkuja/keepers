using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterOld : Character
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

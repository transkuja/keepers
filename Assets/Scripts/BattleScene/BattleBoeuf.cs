using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;

public enum BoeufType { Defense, Damage, Aggro, CostReduction, IncreaseStocks }
[System.Serializable]
public class BattleBoeuf {

    [SerializeField]
    BoeufType boeufType;
    [SerializeField]
    int duration;
    [SerializeField]
    int effectValue;
    [SerializeField]
    FaceType[] symbolsAffected;

    public int Duration
    {
        get
        {
            return duration;
        }

        set
        {
            duration = value;
        }
    }

    public BoeufType BoeufType
    {
        get
        {
            return boeufType;
        }

        set
        {
            boeufType = value;
        }
    }

    public int EffectValue
    {
        get
        {
            return effectValue;
        }

        set
        {
            effectValue = value;
        }
    }

    public FaceType[] SymbolsAffected
    {
        get
        {
            return symbolsAffected;
        }

        set
        {
            symbolsAffected = value;
        }
    }
}

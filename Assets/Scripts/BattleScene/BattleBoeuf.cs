using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;

public enum BoeufType { Defense, Damage, Aggro, CostReduction, IncreaseStocks }
public enum BoeufTarget { SameAsAttack, Self }
[System.Serializable]
public class BattleBoeuf {

    [SerializeField]
    BoeufType boeufType;
    [SerializeField]
    BoeufTarget boeufTarget;
    [SerializeField]
    int duration;
    [SerializeField]
    int effectValue;
    [SerializeField]
    FaceType[] symbolsAffected;

    public BattleBoeuf()
    {

    }

    public BattleBoeuf(BattleBoeuf _origin)
    {
        boeufType = _origin.boeufType;
        boeufTarget = _origin.boeufTarget;
        duration = _origin.duration;
        effectValue = _origin.effectValue;
        symbolsAffected = _origin.symbolsAffected;
    }

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

    public BoeufTarget BoeufTarget
    {
        get
        {
            return boeufTarget;
        }

        set
        {
            boeufTarget = value;
        }
    }
}

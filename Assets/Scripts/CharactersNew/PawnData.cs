using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ComponentData
{
    public virtual void Init(ComponentData cd)
    {

    }
}

[System.Serializable]
public class PawnData {



    [SerializeField]
    private string pawnId;

    [SerializeField]
    private string pawnName = "Jean-Joel";

    [SerializeField]
    private bool[] behaviours;

    [Header("UI")]
    [SerializeField]
    private Sprite associatedSprite;
    [SerializeField]
    private Sprite associatedSpriteForShortcut;
    [SerializeField]
    private Sprite associatedSpriteForBattle;

    #region Ideas
    // int specialDicesUse = 3;
    // int numberOfDice = 4;
    // int diceMinValue = 1;
    // int diceMaxValue = 6;
    // DiceType diceType;
    // enum DiceType = { Offensive, Defensive, Support } => define DiceTypes + what they are used for
    #endregion

    public PawnData()
    {
        behaviours = new bool[(int)BehavioursEnum.Size];
    }

    #region Accessors
    public string PawnId
    {
        get
        {
            return pawnId;
        }

        set
        {
            pawnId = value;
        }
    }
    public string PawnName
    {
        get
        {
            return pawnName;
        }

        set
        {
            pawnName = value;
        }
    }

     public bool[] Behaviours
    {
        get
        {
            return behaviours;
        }

        set
        {
            behaviours = value;
        }
    }

    public Sprite AssociatedSprite
    {
        get
        {
            return associatedSprite;
        }

        set
        {
            associatedSprite = value;
        }
    }

    public Sprite AssociatedSpriteForShortcut
    {
        get
        {
            return associatedSpriteForShortcut;
        }

        set
        {
            associatedSpriteForShortcut = value;
        }
    }

    public Sprite AssociatedSpriteForBattle
    {
        get
        {
            return associatedSpriteForBattle;
        }

        set
        {
            associatedSpriteForBattle = value;
        }
    }


    #endregion


}

public enum BehavioursEnum
{
    Morfale,
    Gaga,
    Explorateur,
    Sensible,
    CanSpeak,
    Archer,
    Stinks,
    Healer,
    Size
};
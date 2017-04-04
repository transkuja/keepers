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

    // TODO Ce truc est en public car besoin a la fois eventuellement dans l'inspecteur, mais aussi en code
        //  Si c'est Pas ok n'hesitez pas a le mettre en private serialisable avec accesseurs.  Quentin
    public GameObject goInGameVisual;

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


    #endregion


}

public enum BehavioursEnum
{
    AnimatedPawn,
    Escortable,
    Fighter,
    HungerHandler,
    Inventory,
    Keeper,
    MentalHealthHandler,
    Monster,
    Mortal,
    PathBlocker,
    QuestDealer,
    CanSpeak,
    Size
};
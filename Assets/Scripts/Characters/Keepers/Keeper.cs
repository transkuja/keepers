using UnityEngine;
using System.Collections.Generic;
 
[System.Serializable]
public class Keeper : Character
{
    [SerializeField]
    private Sprite associatedSprite;

    [SerializeField]
    private Sprite deadSprite;

    [Header("Status")]    
    [SerializeField]
    [Range(50, 120)]
    private short maxHunger = 100;

    [SerializeField]
    private short maxMentalHealth = 100;


    [SerializeField]
    public int nbSlot = 4;



    [SerializeField]
    private short maxActionPoints = 3;


    private int maxInventorySlots = 4;


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

    public short MaxHunger
    {
        get
        {
            return maxHunger;
        }

        private set {}
    }

    public short MaxActionPoints
    {
        get
        {
            return maxActionPoints;
        }

        set
        {
            maxActionPoints = value;
        }
    }

    public short MaxMentalHealth
    {
        get
        {
            return maxMentalHealth;
        }
    }

    public int MaxInventorySlots
    {
        get
        {
            return maxInventorySlots;
        }

        set
        {
            maxInventorySlots = value;
        }
    }

    public Sprite DeadSprite
    {
        get
        {
            return deadSprite;
        }

        set
        {
            deadSprite = value;
        }
    }

    public Keeper()
    {
        
    }

    public Keeper(Keeper from)
    {
        associatedSprite = from.associatedSprite;
    }

}

using UnityEngine;
using System.Collections.Generic;
 
[System.Serializable]
public class Keeper : Character
{
    [SerializeField]
    private Sprite associatedSprite;

    [Header("Status")]

    [SerializeField]
    private short actualHunger = 0;
    [SerializeField]
    [Range(50, 120)]
    private short maxHunger = 100;

    [SerializeField]
    private short maxMentalHealth = 100;

    [SerializeField]
    private short actualMentalHealth;

    [SerializeField]
    private short actionPoints = 3;

    private bool isStarving = false;
    private bool isMentalHealthLow = false;
    private int maxInventorySlots = 4;

    public short ActualHunger {
        get { return actualHunger; }
        set {
            actualHunger = value;
            if (actualHunger > MaxHunger)
            {
                actualHunger = MaxHunger;
                isStarving = true;
            }
            else if (actualHunger < 0)
            {
                actualHunger = 0;
                isStarving = false;
            }
            else
            {
                isStarving = false;
            }

        }
    }

    public short ActualMentalHealth {
        get { return actualMentalHealth; }
        set {
            actualMentalHealth = value;
            if (actualMentalHealth < 0)
            {
                actualMentalHealth = 0;
                isMentalHealthLow = true;
            }
            else if (actualMentalHealth > MaxMentalHealth)
            {
                actualMentalHealth = MaxMentalHealth;
                isMentalHealthLow = false;
            }
            else
            {
                isMentalHealthLow = false;
            }
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

    public short MaxHunger
    {
        get
        {
            return maxHunger;
        }

        private set {}
    }

    public short ActionPoints
    {
        get
        {
            return actionPoints;
        }

        set
        {
            actionPoints = value;
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

    public Keeper()
    {
        actualMentalHealth = MaxMentalHealth;
    }

    public Keeper(Keeper from)
    {
        associatedSprite = from.associatedSprite;
    }

}

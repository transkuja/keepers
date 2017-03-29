using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData : PawnData {

    [SerializeField]
    private bool[] behaviours;

    [Header("UI")]
    [SerializeField]
    private Sprite associatedSprite;

    [SerializeField]
    private Sprite deadSprite;

    [SerializeField]
    private int inventorySlots = 4;

    [Header("Status")]
    [SerializeField]
    [Range(50, 120)]
    private short maxHunger = 100;

    [SerializeField]
    private short maxMentalHealth = 100;

    [SerializeField]
    private short maxActionPoints = 3;

    #region Accessors
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

    public int InventorySlots
    {
        get
        {
            return inventorySlots;
        }

        set
        {
            inventorySlots = value;
        }
    }

    public short MaxHunger
    {
        get
        {
            return maxHunger;
        }

        set
        {
            maxHunger = value;
        }
    }

    public short MaxMentalHealth
    {
        get
        {
            return maxMentalHealth;
        }

        set
        {
            maxMentalHealth = value;
        }
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
    #endregion
}

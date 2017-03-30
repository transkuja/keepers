using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrisonerOld : Character {

    [SerializeField]
    private Sprite associatedSprite;

    [Header("Status")]

    [SerializeField]
    [Range(50, 120)]
    private short maxHunger = 100;

    [SerializeField]
    private short maxMentalHealth = 100;

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

        private set { }
    }

    public short MaxMentalHealth
    {
        get
        {
            return maxMentalHealth;
        }
        private set { }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Prisoner : Character {

    [SerializeField]
    private GameObject associatedSprite;

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


    private bool isStarving = false;
    private bool isMentalHealthLow = false;

    public short ActualHunger
    {
        get { return actualHunger; }
        set
        {
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

    public short ActualMentalHealth
    {
        get { return actualMentalHealth; }
        set
        {
            actualMentalHealth = value;
            if (actualMentalHealth < 0)
            {
                actualMentalHealth = 0;
                isMentalHealthLow = true;
            }
            else if (actualMentalHealth > maxMentalHealth)
            {
                actualMentalHealth = maxMentalHealth;
                isMentalHealthLow = false;
            }
            else
            {
                isMentalHealthLow = false;
            }
        }
    }

    public GameObject AssociatedSprite
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

    public Prisoner()
    {
        actualMentalHealth = maxMentalHealth;
    }

    public Prisoner(Prisoner from)
    {
        associatedSprite = from.associatedSprite;
    }
}

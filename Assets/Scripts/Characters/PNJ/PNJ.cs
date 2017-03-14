﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PNJ{
    [SerializeField]
    private Sprite associatedSprite;
    [SerializeField]
    private string characterName = "CharacterBob";

    private int maxInventorySlots = 6;


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

    public string CharacterName
    {
        get
        {
            return characterName;
        }

        set
        {
            characterName = value;
        }
    }

    public PNJ()
    {

    }

    public PNJ(PNJ from)
    {
        associatedSprite = from.associatedSprite;
    }

}

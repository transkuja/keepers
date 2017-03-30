using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInstance : MonoBehaviour {

    PawnData data;
    InteractionImplementer interactions;

    // Need to be initialized in charactersInitializer and changed in moveCharacter
    Tile currentTile;

    void Start()
    {
        interactions = new InteractionImplementer();
    }

    #region Accessors

    public PawnData Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
        }
    }

    public InteractionImplementer Interactions
    {
        get
        {
            return interactions;
        }

        set
        {
            interactions = value;
        }
    }

    public Tile CurrentTile
    {
        get
        {
            return currentTile;
        }

        set
        {
            currentTile = value;
        }
    }
    #endregion
}

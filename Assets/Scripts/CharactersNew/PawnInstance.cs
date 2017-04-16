using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnInstance : MonoBehaviour {

    [SerializeField]
    PawnData data;

    // Need to be initialized in charactersInitializer and changed in moveCharacter
    Tile currentTile;

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

    [System.Obsolete("Use interaction component instead")]
    public InteractionImplementer Interactions
    {
        get
        {
            return GetComponent<Interactable>().Interactions;
        }

        set
        {
            GetComponent<Interactable>().Interactions = value;
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

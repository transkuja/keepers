using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstance : MonoBehaviour {

    InteractionImplementer interactions;

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

    void Start()
    {
        interactions = new InteractionImplementer();
    }
}

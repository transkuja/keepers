using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerInstance : MonoBehaviour {

    [Header("Prisoner Info")]
    [SerializeField]
    private Prisoner prisoner = null;

    public PrisonerInstance(PrisonerInstance from)
    {
        prisoner = from.prisoner;
    }

    /* ------------------------------------------------------------------------------------ */
    /* ------------------------------------- Accessors ------------------------------------ */
    /* ------------------------------------------------------------------------------------ */
    public Prisoner Prisoner
    {
        get
        {
            return prisoner;
        }

        set
        {
            prisoner = value;
        }
    }
}

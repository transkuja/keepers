using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeAction { Suivre };

[System.Serializable]
public class Action {

    [SerializeField]
    private string actionName = "";

    [SerializeField]
    private Sprite actionSprite;

    [SerializeField]
    private TypeAction typeAction;

    public delegate void Use();
    public Use action = null;

    public void suivre()
    {
        Debug.Log("Keeper que le Prisonier doit suivre : " + GameManager.Instance.ListOfSelectedKeepers[0]);
    }


    /* ------------------------------------------------------------------------------------ */
    /* ------------------------------------- Accessors ------------------------------------ */
    /* ------------------------------------------------------------------------------------ */

    public string ActionName
    {
        get
        {
            return actionName;
        }

        set
        {
            actionName = value;
        }
    }

    public Sprite ActionSprite
    {
        get
        {
            return actionSprite;
        }

        set
        {
            actionSprite = value;
        }
    }

    public TypeAction TypeAction
    {
        get
        {
            return typeAction;
        }

        set
        {
            typeAction = value;
            if (value == TypeAction.Suivre)
                action = suivre;
        }
    }
}
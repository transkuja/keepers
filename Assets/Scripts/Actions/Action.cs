using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

        GameManager.Instance.ListOfSelectedKeepers[0].Keeper.GoListCharacterFollowing.Add(GameManager.Instance.GoTarget);
        GameManager.Instance.GoTarget.GetComponent<NavMeshAgent>().stoppingDistance = 0.75f;
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
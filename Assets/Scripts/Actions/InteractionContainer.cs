using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Interaction();

[System.Serializable]
public class InteractionContainer {

    public bool isAllowed;

    public string strName;

    public Sprite sprite;

    public Interaction action;

    public InteractionContainer()
    {
        action = null;
        strName = "";
        sprite = null;
        isAllowed = true;
    }

    public InteractionContainer(Interaction _action, string _strName, Sprite _sprite, bool _isAllowed)
    {
        action = _action;
        strName = _strName;
        sprite = _sprite;
        isAllowed = _isAllowed;
    }
}

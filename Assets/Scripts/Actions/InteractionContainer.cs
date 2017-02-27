using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Interaction(int i = 0);

[System.Serializable]
public class InteractionContainer {

    public bool isAllowed;

    public string strName;

    public Sprite sprite;

    public Interaction action;

    public int iParam;

    public InteractionContainer()
    {
        action = null;
        strName = "";
        sprite = null;
        isAllowed = true;
        iParam = -1;
    }

    public InteractionContainer(Interaction _action, string _strName, Sprite _sprite, bool _isAllowed, int _iParam = -1)
    {
        action = _action;
        strName = _strName;
        sprite = _sprite;
        isAllowed = _isAllowed;
        iParam = _iParam;
    }
}

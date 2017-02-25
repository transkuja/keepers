using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ActionQ();

public class ActionContainer {

    public string strName;

    public Sprite sprite;

    public ActionQ action;

    public ActionContainer()
    {
        action = null;
        strName = "";
        sprite = null;
    }

    public ActionContainer(ActionQ _action, string _strName, Sprite _sprite)
    {
        action = _action;
        strName = _strName;
        sprite = _sprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionImplementer {

    public List<InteractionContainer> listActionContainers = new List<InteractionContainer>();

    public void Add(Interaction _action, int _costAction, string _strName, Sprite _sprite, bool _isAllowed = true, int _iParam = -1)
    {
        listActionContainers.Add(new InteractionContainer(_action, _costAction, _strName, _sprite, _isAllowed, _iParam));
    }

    public bool Remove(string strName)
    {
        for (int i = 0; i< listActionContainers.Count; i++)
        {
            if (listActionContainers[i].strName == strName)
            {
                listActionContainers.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public InteractionContainer Get(string strName)
    {
        for (int i = 0; i < listActionContainers.Count; i++)
        {
            if (listActionContainers[i].strName == strName)
            {
                return listActionContainers[i];
            }
        }
        return null;
    }
}

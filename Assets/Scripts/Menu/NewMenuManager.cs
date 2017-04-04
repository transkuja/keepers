using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMenuManager : MonoBehaviour {

    private int cardLevelSelected = -1;
    private List<PawnInstance> listeSelectedKeepers;

    void Start()
    {
        listeSelectedKeepers = new List<PawnInstance>();
    }

    #region Accessors
    public int CardLevelSelected
    {
        get
        {
            return cardLevelSelected;
        }

        set
        {
            cardLevelSelected = value;
        }
    }

    public void AddToSelectedKeepers(PawnInstance pi)
    {
        listeSelectedKeepers.Add(pi);
    }

    public void RemoveFromSelectedKeepers(PawnInstance pi)
    {
        listeSelectedKeepers.Remove(pi);
    }

    public bool ContainsSelectedKeepers(PawnInstance pi)
    {
        return listeSelectedKeepers.Contains(pi);
    }
    #endregion
}

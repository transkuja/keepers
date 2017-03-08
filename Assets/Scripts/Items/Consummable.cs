using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void Use(int i = 0);

public class Consummable : Item, IUse
{
    public Use action;

    public int value;

    public int quantite;

    public void Use(int _i = 0)
    {
        quantite -= 1;
        GameManager.Instance.ListOfSelectedKeepers[0].CurrentHp += value;
    }
}

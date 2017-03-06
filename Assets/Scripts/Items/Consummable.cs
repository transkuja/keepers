using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consummable : Item
{
    public enum UseAction { HEAL, DAMAGE, ADD_MANA, USE_MANA }
    UseAction action;
    private int value;
    
    public int quantite;

}

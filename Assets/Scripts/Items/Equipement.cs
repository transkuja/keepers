using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BonusStats
{
    [SerializeField]
    public int strength;
    [SerializeField]
    public int defense;
    [SerializeField]
    public int agility;
}

public class Equipement : Item {

    public TypeEquipement type;

    public BonusStats stats;

}

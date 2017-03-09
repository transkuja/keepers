using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public Sprite sprite;

    // 0 : common, 9 : rare
    [Range(0, 10)]
    public int rarity;
}

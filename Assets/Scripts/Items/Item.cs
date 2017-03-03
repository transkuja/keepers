using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField]
    public Sprite sprite;

    public override string ToString()
    {
        return GetType().ToString();
    }
}

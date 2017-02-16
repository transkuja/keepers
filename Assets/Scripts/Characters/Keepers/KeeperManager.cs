using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeperManager {

    public static List<Keeper> listKeepers = new List<Keeper>();

    public static void AddKeeper(Keeper k)
    {
        listKeepers.Add(k);
    }

    public static void RemoveKeeper(int index)
    {
        listKeepers.RemoveAt(index);
    }

    public static void RemoveKeeper(Keeper k)
    {
        listKeepers.Remove(k);
    }
}

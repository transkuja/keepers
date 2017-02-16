using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager {

    public static List<Monster> listMonsters = new List<Monster>();

    public static void AddMonster(Monster m)
    {
        listMonsters.Add(m);
    }

    public static void RemoveMonster(int index)
    {
        listMonsters.RemoveAt(index);
    }

    public static void RemoveMonster(Monster m)
    {
        listMonsters.Remove(m);
    }
}

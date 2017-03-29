using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager {

    public static List<MonsterOld> listMonsters = new List<MonsterOld>();

    public static void AddMonster(MonsterOld m)
    {
        listMonsters.Add(m);
    }

    public static void RemoveMonster(int index)
    {
        listMonsters.RemoveAt(index);
    }

    public static void RemoveMonster(MonsterOld m)
    {
        listMonsters.Remove(m);
    }
}

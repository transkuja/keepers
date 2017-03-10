using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {

	public List<Item> ComputeLoot()
    {
        List<Item> tmpList = new List<Item>();
        foreach (Item it in transform.GetComponent<MonsterInstance>().PossibleDrops)
        {
            if (Random.Range(0, 10) > it.rarity)
            {
                tmpList.Add(it);
            }
        }

        return tmpList;
    }
}

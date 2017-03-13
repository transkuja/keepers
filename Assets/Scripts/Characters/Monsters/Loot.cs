using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {

	public List<ItemContainer> ComputeLoot()
    {
        List<ItemContainer> tmpList = new List<ItemContainer>();
        foreach (ItemContainer it in transform.GetComponent<MonsterInstance>().Monster.PossibleDrops)
        {
            if (Random.Range(0, 10) > it.Item.Rarity)
            {
                tmpList.Add(it);
            }
        }

        return tmpList;
    }
}

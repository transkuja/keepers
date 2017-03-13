using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {

	public List<ItemContainer> ComputeLoot()
    {
        List<ItemContainer> tmpList = new List<ItemContainer>();
        Item it = null;
        foreach (string _IdItem in transform.GetComponent<MonsterInstance>().Monster.PossibleDrops)
        {
            it = GameManager.Instance.Database.getItemById(_IdItem);
            if (Random.Range(0, 10) > it.Rarity)
            {
                tmpList.Add(new ItemContainer(it, 1));
            }
        }

        return tmpList;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;

public class KillAllMonsterOnTile : MonoBehaviour {

    [SerializeField]
    public List<Behaviour.Monster> listMonstertoKill;

    public void OnEnable()
    {
        // FIx this shit @Rémi
        Invoke("KillAllMonster", 2.0f);

        Invoke("SetActiveFalse", 10.0f);
    }

    public void KillAllMonster()
    {
        if (listMonstertoKill.Count > 0)
        {
            foreach (Monster monster in listMonstertoKill)
            {
                if (monster.GetMType == MonsterType.Common)
                    monster.GetComponent<Mortal>().Die();
            }
        }

    }

    public void SetActiveFalse()
    {
        this.gameObject.SetActive(false);
    }
}

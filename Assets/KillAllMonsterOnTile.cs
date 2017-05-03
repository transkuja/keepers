using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour;

public class KillAllMonsterOnTile : MonoBehaviour {

    [SerializeField]
    public List<Behaviour.Monster> listMonstertoKill;

    public void OnEnable()
    {
        if (listMonstertoKill.Count > 0)
        {
            foreach(Monster monster in listMonstertoKill)
            {
                monster.GetComponent<Mortal>().Die();
            }
        }

        Invoke("SetActiveFalse", 10.0f);
    }

    public void SetActiveFalse()
    {
        this.gameObject.SetActive(false);
    }
}

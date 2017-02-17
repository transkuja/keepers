using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInstance : MonoBehaviour {
    [Header("Monster Info")]
    [SerializeField]
    private Monster monster = null;
    
    public Monster Monster
    {
        get
        {
            return monster;
        }

        set
        {
            monster = value;
        }
    }

    void Update () {
		
	}
}

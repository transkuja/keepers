using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInstance : MonoBehaviour {

    private Monster associatedMonster = null;

    [Header("Monster Info")]
    [SerializeField]
    private new string name = "Monster1";

    [Header("Stats")]

    [Range(1, 100)]
    [SerializeField]
    private short hpValue = 100;

    [Range(1, 100)]
    [SerializeField]
    private short mpValue = 50;

    [Range(1, 10)]
    [SerializeField]
    private short strengthValue = 5;

    [Range(1, 10)]
    [SerializeField]
    private short defenseValue = 5;

    [Range(1, 10)]
    [SerializeField]
    private short intelligenceValue = 5;

    [Range(1, 10)]
    [SerializeField]
    private short spiritValue = 5;


    void Start () {
        associatedMonster = new Monster();

        associatedMonster.Name = name;

        associatedMonster.Hp = hpValue;
        associatedMonster.Mp = mpValue;
        associatedMonster.Strength = strengthValue;
        associatedMonster.Defense = defenseValue;
        associatedMonster.Intelligence = intelligenceValue;
        associatedMonster.Spirit = spiritValue;
    }

    public Monster getAssociatedMonster()
    {
        return associatedMonster;
    }

    void Update () {
		
	}
}

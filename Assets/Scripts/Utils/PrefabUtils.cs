using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PawnPrefab
{
    [SerializeField]
    public string idPawn;
    [SerializeField]
    public GameObject prefabPawn;
}

public class PrefabUtils : MonoBehaviour {

    [Header("Pawn")]
    public List<PawnPrefab> listPawnPrefab;

    //[Header("QuestCards")]
    //public GameObject prefabQuestCard;
    [Header("Item")]
    public GameObject prefabItemToDrop;

    [Header("LevelCard")]
    public GameObject prefabLevelCard;

    [Header("Battle")]
    public GameObject die;
    public GameObject selectionPointer;

    [Header("Monsters")]
    public List<PawnPrefab> listMonstersPrefab;

    [Header("Battle Animation (IBattleAnimation)")]
    public GameObject baseAttackAnimation;
    public GameObject getPawnPrefabById(string id)
    {
        for(int i= 0; i <listPawnPrefab.Count; i++)
        {
            if(listPawnPrefab[i].idPawn == id)
            {
                return listPawnPrefab[i].prefabPawn;
            }
        }
        return null;
    }
    

    public GameObject getMonsterPrefabById(string id)
    {
        for (int i = 0; i < listMonstersPrefab.Count; i++)
        {
            if (listMonstersPrefab[i].idPawn == id)
            {
                return listMonstersPrefab[i].prefabPawn;
            }
        }
        return null;
    }
}

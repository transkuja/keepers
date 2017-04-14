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

    [Header("QuestDecks")]
    public GameObject prefabQuestDeck;
    //[Header("QuestCards")]
    //public GameObject prefabQuestCard;
    [Header("Item")]
    public GameObject prefabItemToDrop;

    [Header("LevelCard")]
    public GameObject prefabLevelCard;

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
}

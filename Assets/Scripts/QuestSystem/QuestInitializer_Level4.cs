using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestInitializer_Level4 : MonoBehaviour {
    // Here will be all the functions or things we need to do specific to a certain quest (activating a tile ...)
    [Header("Quest 1")]
    [SerializeField]
    Tile duckTile;
    PawnInstance[] ducklings;

    private Quest side1;

    void Start () {
	}

    public void InitializeQuests()
    {
        Quest main1 = GameManager.Instance.QuestManager.MainQuest;
        if (main1.Identifier.ID == "main_quest_01")
        {
            ((PrisonerEscortObjective)main1.Objectives[0]).prisoner = GameManager.Instance.PrisonerInstance.gameObject;
            ((PrisonerEscortObjective)main1.Objectives[0]).destination = TileManager.Instance.EndTile;
        }
        Debug.Log("here");
        side1 = GameManager.Instance.QuestManager.GetQuestByID("side_quest_ducklings_01");
        if (side1 != null)
        {
            Debug.Log("there");
            ((MultipleEscortObjective)side1.Objectives[0]).destination = duckTile;
        }

    }
}

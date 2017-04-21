using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestInitializer : MonoBehaviour {
    // Here will be all the functions or things we need to do specific to a certain quest (activating a tile ...)
    [Header("Quest 1")]
    [SerializeField]
    string id1;


    void Start () {
		
	}

    public void InitializeQuests()
    {
        Quest main1 = GameManager.Instance.QuestManager.GetQuestByID("main_quest_01");
        if (GameManager.Instance.QuestManager.AvailableQuests.Contains(main1))
        {
            ((PrisonerEscortObjective)main1.Objectives[0]).prisoner = GameManager.Instance.PrisonerInstance.gameObject;
            ((PrisonerEscortObjective)main1.Objectives[0]).destination = TileManager.Instance.EndTile;
        }
    }	
}

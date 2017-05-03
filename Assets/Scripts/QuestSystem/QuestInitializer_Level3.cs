using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestInitializer_Level3 : MonoBehaviour {
    // Here will be all the functions or things we need to do specific to a certain quest (activating a tile ...)
    [Header("Quest 1")]
    [SerializeField]
    Tile tileToActivate;

    private Quest side1;

    void Start () {
        if (tileToActivate != null)
        tileToActivate.gameObject.SetActive(false);
	}

    public void InitializeQuests()
    {
        Quest main1 = GameManager.Instance.QuestManager.MainQuest;
        if (main1.Identifier.ID == "main_quest_01")
        {
            ((PrisonerEscortObjective)main1.Objectives[0]).prisoner = GameManager.Instance.PrisonerInstance.gameObject;
            ((PrisonerEscortObjective)main1.Objectives[0]).destination = TileManager.Instance.EndTile;
        }
        side1 = GameManager.Instance.QuestManager.GetQuestByID("side_quest_01");
        if (GameManager.Instance.QuestManager.Quests.Contains(side1))
        {
            side1.OnQuestInit += side_quest_01_Init;
        }

    }

    void side_quest_01_Init()
    {
        side1.OnQuestInit -= side_quest_01_Init;
        tileToActivate.gameObject.SetActive(true);
        tileToActivate.South.North = tileToActivate;
        tileToActivate.South.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
    }
}

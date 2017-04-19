using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class QuestInitializer : MonoBehaviour {

    [Header("Quest 1")]
    [SerializeField]
    string id1;
    [SerializeField]
    string idPNJ1;
    [SerializeField]
    Transform transform1;
    [SerializeField]
    Tile tile1;
    void Start () {
		
	}
    
    void InitializeQuests()
    {
        if(QuestManager.Instance.AvailableQuests.Contains(QuestManager.Instance.GetQuestByID("side_quest_01")))
        {
            GameObject createdPNJ = GameManager.Instance.PawnDataBase.CreatePawn(idPNJ1, transform1.position, transform1.rotation, null);
            createdPNJ.GetComponent<Behaviour.QuestDealer>().questToGive = QuestManager.Instance.GetQuestByID("side_quest_01");
            createdPNJ.GetComponent<Behaviour.QuestDealer>().Init();
        }
    }	
}

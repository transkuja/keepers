using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class JoinTeamOnQuestComplete : MonoBehaviour {
    [SerializeField]
    string idKeeper;
    [SerializeField]
    string idQuest;

    private Quest toSubscribe;

	void Start () {
        toSubscribe = GameManager.Instance.QuestManager.GetQuestByID(idQuest);
        toSubscribe.OnQuestComplete += BecomeAKeeper;
	}

    void BecomeAKeeper()
    {
        toSubscribe.OnQuestComplete -= BecomeAKeeper;
        PawnInstance pawn = GameManager.Instance.PawnDataBase.CreatePawn(idKeeper, transform.position, transform.rotation, null).GetComponent<PawnInstance>();
        GameManager.Instance.PawnDataBase.InitPawn(pawn);
        GameManager.Instance.CharacterInitializer.InitCharacterUI(pawn);
        TileManager.Instance.AddKeeperOnTile(GetComponentInParent<Tile>(), pawn);
        GameManager.Instance.ClearListKeeperSelected();
        GameManager.Instance.AllKeepersList.Add(pawn);
        GlowController.RegisterObject(pawn.GetComponent<GlowObjectCmd>());
        pawn.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        Transform feed = GetComponent<Interactable>().Feedback;
        feed.GetChild(feed.childCount - 1).SetParent(GameManager.Instance.Ui.transform);
        GameManager.Instance.PersistenceLoader.SetPawnUnlocked(idKeeper, true);
        DestroyImmediate(gameObject);
    }
}

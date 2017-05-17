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
        if (GameManager.Instance.PersistenceLoader.Pd.dicPersistencePawns.ContainsKey(idKeeper) && GameManager.Instance.PersistenceLoader.Pd.dicPersistencePawns[idKeeper] == true)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            toSubscribe = GameManager.Instance.QuestManager.GetQuestByID(idQuest);
            toSubscribe.OnQuestComplete += BecomeAKeeper;
        }
	}

    void BecomeAKeeper()
    {
        toSubscribe.OnQuestComplete -= BecomeAKeeper;
        PawnInstance pawn = GameManager.Instance.PawnDataBase.CreatePawn(idKeeper, transform.position, transform.rotation, null).GetComponent<PawnInstance>();
        GameManager.Instance.PersistenceLoader.SetPawnUnlocked(idKeeper, true);
        if (GameManager.Instance.PersistenceLoader.Pd.dicPersistencePawns.ContainsKey(idKeeper)){
            GameManager.Instance.PersistenceLoader.Pd.dicPersistencePawns[idKeeper] = true;
        }
        GameManager.Instance.PawnDataBase.InitPawn(pawn);
        GameManager.Instance.AllKeepersList.Add(pawn);
        GameManager.Instance.CharacterInitializer.InitCharacterUI(pawn);
        TileManager.Instance.AddKeeperOnTile(GetComponentInParent<Tile>(), pawn);
        GameManager.Instance.GetFirstSelectedKeeper().GetComponent<Behaviour.Keeper>().IsSelected = false;
        GameManager.Instance.ClearListKeeperSelected();
        pawn.GetComponent<Behaviour.Keeper>().IsSelected = true;
        GameManager.Instance.AddKeeperToSelectedList(pawn);
        GlowController.RegisterObject(pawn.GetComponent<GlowObjectCmd>());
        pawn.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        Transform feed = GetComponent<Interactable>().Feedback;
        feed.GetChild(feed.childCount - 1).SetParent(GameManager.Instance.Ui.transform);
        GameManager.Instance.PersistenceLoader.SetPawnUnlocked(idKeeper, true);
        DestroyImmediate(gameObject);
    }
}

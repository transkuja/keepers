using System.Collections.Generic;
using UnityEngine;

public class SeqActionCostHunger : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;
    public bool hasPressedEndTurnButton = false;

    public override void Init()
    {
        base.Init();
        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f) + GameManager.Instance.ActiveTile.transform.position);

        Etapes = new List<Step>();
        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));

        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Doing manual actions like Harvest, Explore, Move, will increase your characters' hunger."));
    }

    public override void End()
    {
        base.End();
        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.PlayingSequence = null;
        TutoManager.s_instance.GetComponent<SeqActionCostHunger>().AlreadyPlayed = true;
    }
}

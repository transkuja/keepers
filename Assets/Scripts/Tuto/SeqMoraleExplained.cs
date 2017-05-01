using System.Collections.Generic;
using UnityEngine;

public class SeqMoraleExplained : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;

    public override void Init()
    {
        base.Init();
        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f) + GameManager.Instance.ActiveTile.transform.position);

        Etapes = new List<Step>();
        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));

        Etapes.Add(new TutoManager.Message(pawnMrResetti, "A lot of events can alter your characters morale."));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "For example, arriving on an unwelcoming area can lower your characters morale."));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "	The easiest way to up your characters' mood is by using the Chat action between them."));
    }

    public override void End()
    {
        base.End();
        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.PlayingSequence = null;
        TutoManager.s_instance.GetComponent<SeqMoraleExplained>().AlreadyPlayed = true;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SeqMoraleExplained : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;
    public bool isLaunchedDuringASnowEvent = false;

    public override void Init()
    {
        base.Init();
        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f) + GameManager.Instance.ActiveTile.transform.position);

        Etapes = new List<Step>();
        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));

        //Etapes.Add(new TutoManager.Message(pawnMrResetti, "A lot of events can alter your characters morale."));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Your characters' morale can be altered by certain things."));
        //Etapes.Add(new TutoManager.Message(pawnMrResetti, "For example, arriving on an unwelcoming area can lower your characters morale."));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Arriving at an unwelcoming area is one example, it can lower your characters' morale."));

        if (isLaunchedDuringASnowEvent)
        {
            //Etapes.Add(new TutoManager.Message(pawnMrResetti, "Or when your characters stay on an area where it's snowing."));
            Etapes.Add(new TutoManager.Message(pawnMrResetti, "As well as spending the night at a snowy place."));
            //Etapes.Add(new TutoManager.Message(pawnMrResetti, "Like right now."));
            Etapes.Add(new TutoManager.Message(pawnMrResetti, "Like... right now."));
        }

        //Etapes.Add(new TutoManager.Message(pawnMrResetti, "	The easiest way to up your characters' mood is by using the Chat action between them."));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "Characters can cheer each other up by using the chat action."));
    }

    public override void End()
    {
        base.End();
        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);

        for (int i = 0; i < GameManager.Instance.AllKeepersList.Count; i++)
        {
            GameManager.Instance.AllKeepersList[i].GetComponent<Behaviour.MentalHealthHandler>().SelectedMentalHealthUI.SetActive(true);
        }

        TutoManager.s_instance.GetComponent<SeqMoraleExplained>().AlreadyPlayed = true;
        TutoManager.s_instance.PlayingSequence = null;
    }
}

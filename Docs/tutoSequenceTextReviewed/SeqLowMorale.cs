using System.Collections.Generic;
using UnityEngine;

public class SeqLowMorale : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;
    public GameObject shortcutPanels;

    public class ShowCharactersShortcut : Step
    {
        string str;
        public ShowCharactersShortcut(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            if (!TutoManager.s_instance.GetComponent<SeqLowMorale>().shortcutPanels.activeInHierarchy)
                GameManager.Instance.Ui.ToggleShortcutPanel();

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            alreadyPlayed = false;
        }
    }

    public override void Init()
    {
        base.Init();
        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f) + GameManager.Instance.ActiveTile.transform.position);

        Etapes = new List<Step>();
        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));

        Etapes.Add(new ShowCharactersShortcut("Be careful, one of your characters' morale is very low."));
        //Etapes.Add(new TutoManager.Message(pawnMrResetti, "When morale reach 0, your character efficiency will be reduced!"));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, "When a character's morale becomes null, his efficiency is be reduced!"));
    }

    public override void End()
    {
        base.End();
        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.GetComponent<SeqLowMorale>().AlreadyPlayed = true;
        TutoManager.s_instance.PlayingSequence = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeqLowHunger : Sequence {
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

        Etapes.Add(new ShowCharactersShortcut(Translater.TutoText("SeqLowHunger", 0)));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, Translater.TutoText("SeqLowHunger", 1)));
        Etapes.Add(new TutoManager.Message(pawnMrResetti, Translater.TutoText("SeqLowHunger", 2)));
    }

    public override void End()
    {
        base.End();
        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.GetComponent<SeqLowHunger>().AlreadyPlayed = true;
        TutoManager.s_instance.PlayingSequence = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeqAshleyLowHunger : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;
    public GameObject shortcutPanels;
    public List<GameObject> feedbacks;

    public class ShowAshleyHunger : Step
    {
        string str;
        GameObject feedback;
        public ShowAshleyHunger(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            if (!TutoManager.s_instance.GetComponent<SeqAshleyLowHunger>().shortcutPanels.activeInHierarchy)
                GameManager.Instance.Ui.ToggleShortcutPanel();

            SeqAshleyLowHunger seqAshleyLowHunger = TutoManager.s_instance.GetComponent<SeqAshleyLowHunger>();
            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = seqAshleyLowHunger.shortcutPanels.transform.GetChild(0).position;
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 190.0f;
                feedback.GetComponent<FlecheQuiBouge>().speed = 12.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, -135);
            }

            if (!TutoManager.s_instance.GetComponent<SeqAshleyLowHunger>().shortcutPanels.activeInHierarchy)
                GameManager.Instance.Ui.ToggleShortcutPanel();

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            Destroy(feedback);
            alreadyPlayed = false;
        }
    }

    public class ShowFeedSlot : Step
    {
        string str;
        GameObject feedback;
        public ShowFeedSlot(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Inventory>().SelectedInventoryPanel.transform.position + Vector3.up * (50 * (Screen.height/1080.0f));
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 100.0f;
                feedback.GetComponent<FlecheQuiBouge>().speed = 12.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, -75);
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
            TutoManager.s_instance.waitForFeedSlotAppearance = false;
        }

        public override void Reverse()
        {
            Destroy(feedback);
            alreadyPlayed = false;
        }
    }

    public class WaitForNextSeq : Step
    {
        string str;
        public WaitForNextSeq(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
            TutoManager.s_instance.waitForFeedSlotAppearance = true;
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

        if (!TutoManager.s_instance.waitForFeedSlotAppearance)
            Etapes.Add(new ShowAshleyHunger(Translater.TutoText("SeqAshleyLowHunger", 0)));

        if (GameManager.Instance.PrisonerInstance.GetComponent<Behaviour.Inventory>().SelectedInventoryPanel.activeSelf)
            Etapes.Add(new ShowFeedSlot(((TutoManager.s_instance.waitForFeedSlotAppearance) ? Translater.TutoText("SeqAshleyLowHunger", 1) : Translater.TutoText("SeqAshleyLowHunger", 2))));
        else
            Etapes.Add(new WaitForNextSeq(Translater.TutoText("SeqAshleyLowHunger", 3)));
    }

    public override void End()
    {
        base.End();
        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        if (TutoManager.s_instance.waitForFeedSlotAppearance)
            TutoManager.s_instance.GetComponent<SeqAshleyLowHunger>().position = -1;
        TutoManager.s_instance.GetComponent<SeqAshleyLowHunger>().AlreadyPlayed = !TutoManager.s_instance.waitForFeedSlotAppearance;
        TutoManager.s_instance.PlayingSequence = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeqAshleyEscort : Sequence {
    private GameObject pawnMrResetti;
    public AnimationClip jumpAnimationClip;
    
    public class DeactivateButtons : Step
    {
        string str;
        public DeactivateButtons(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            foreach(Button b in GameManager.Instance.Ui.GoActionPanelQ.GetComponentsInChildren<Button>())
            {
                ColorBlock cb = b.colors;
                cb.disabledColor = Color.white;
                b.colors = cb;
                b.interactable = false;
            }

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            alreadyPlayed = false;
        }
    }

    public class ReactivateButtons : Step
    {
        string str;
        public ReactivateButtons(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            foreach (Button b in GameManager.Instance.Ui.GoActionPanelQ.GetComponentsInChildren<Button>())
            {
                b.interactable = true;
            }

            GameManager.Instance.Ui.ClearActionPanel();

            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            alreadyPlayed = false;
        }
    }

    public class ShowEscortAction : Step
    {
        string str;
        GameObject feedback;
        public ShowEscortAction(string _str)
        {
            stepFunction = Message_fct;
            str = _str;
        }

        public void Message_fct()
        {
            if (feedback == null)
            {
                feedback = Instantiate(TutoManager.s_instance.uiPointer, GameManager.Instance.Ui.transform.GetChild(0));
                feedback.GetComponent<FlecheQuiBouge>().PointToPoint = Camera.main.WorldToScreenPoint(GameManager.Instance.Ui.GoActionPanelQ.transform.GetChild(0).position);
                feedback.GetComponent<FlecheQuiBouge>().distanceOffset = 120.0f;

                feedback.transform.localEulerAngles = new Vector3(0, 0, -45);
            }
            TutoManager.s_instance.EcrireMessage(str);
            TutoManager.s_instance.PlayingSequence.CurrentState = SequenceState.Idle;
        }

        public override void Reverse()
        {
            Destroy(feedback);
            alreadyPlayed = false;
        }
    }

    public override void Init()
    {
        base.Init();
        if (GameManager.Instance.Ui.goConfirmationPanel.activeSelf)
        {
            foreach (Button b in GameManager.Instance.Ui.goConfirmationPanel.GetComponentsInChildren<Button>())
                b.interactable = false;
        }

        pawnMrResetti = TutoManager.s_instance.SpawnMmeResetti(new Vector3(0.0f, 0.15f, -0.7f) + GameManager.Instance.ActiveTile.transform.position);

        Etapes = new List<Step>();
        Etapes.Add(new TutoManager.Spawn(pawnMrResetti, jumpAnimationClip));

        Etapes.Add(new DeactivateButtons(Translater.TutoText("SeqAshleyEscort", 0)));
        if (GameManager.Instance.Ui.goConfirmationPanel.activeSelf)
        {
            foreach (Button b in GameManager.Instance.Ui.goConfirmationPanel.GetComponentsInChildren<Button>())
                b.interactable = false;
        }
        Etapes.Add(new ShowEscortAction(Translater.TutoText("SeqAshleyEscort", 1)));
        Etapes.Add(new ReactivateButtons(Translater.TutoText("SeqAshleyEscort", 2)));
    }

    public override void End()
    {
        base.End();
        if (pawnMrResetti != null)
            TutoManager.UnSpawn(pawnMrResetti);
        if (TutoManager.s_instance.TutoPanelInstance != null)
            Destroy(TutoManager.s_instance.TutoPanelInstance);
        TutoManager.s_instance.GetComponent<SeqAshleyEscort>().AlreadyPlayed = true;
        TutoManager.s_instance.PlayingSequence = null;
        if (GameManager.Instance.Ui.goConfirmationPanel.activeSelf)
        {
            foreach (Button b in GameManager.Instance.Ui.goConfirmationPanel.GetComponentsInChildren<Button>())
                b.interactable = true;
        }
    }
}
